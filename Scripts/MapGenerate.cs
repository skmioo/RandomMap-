using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour {

    public GameObject mapBase;
    public GameObject RandomObjs;
    public MapInfo mapInfo;
    [HideInInspector]
    private List<GameObject> itemPrefabs = new List<GameObject>();
    private string itemPrefabsPath = "Prefabs/IslandItems";
    private Dictionary<string, MapCellInfo> mapCells = new Dictionary<string, MapCellInfo>();
    private Dictionary<string, MapCellInfo> tempCellInfos = new Dictionary<string, MapCellInfo>();
    private GameObject currentSelectGo = null;
    private List<RectAngle> PlaceGosPos = new List<RectAngle>();
    void Start () {
        //地图信息
        mapInfo = new MapInfo(mapBase);
        Debug.Log(mapInfo);
        tempCellInfos.Clear();
        //加载默认海岛物体
        GameObject[] arr = Resources.LoadAll<GameObject>(itemPrefabsPath);
        itemPrefabs.Clear();
        foreach (GameObject go in arr)
        {
            GameObject tmp = Instantiate(go);
            itemPrefabs.Add(tmp);
            tmp.SetActive(false);
        }
        int minSize = GetMinSize();
        Debug.Log("min size:" + minSize);
        for (int i = 0; i < mapInfo.row - minSize + 1; i++)
        {
            for (int j = 0; j < mapInfo.column - minSize + 1; j++)
            {
                MapCellInfo cellInfo = new MapCellInfo();
                cellInfo.PosX = i;
                cellInfo.PosY = j;
                cellInfo.isPlace = false;
                mapCells.Add(i + "," + j, cellInfo);
            }
        }
        StartCoroutine(StartGenerateMap());
    }

    public IEnumerator StartGenerateMap()
    {
        currentSelectGo = GetRandomPrefab();
        GameObject goClone = Instantiate(currentSelectGo, Vector3.zero, Quaternion.identity, RandomObjs.transform);
        goClone.SetActive(true);
        GenerateMap(goClone);
        yield return null;
    }

    void GenerateMap(GameObject go)
    {
        string pos = RandomPosition();
        if (pos != null)
        {
            int posX = int.Parse(pos.Split(',')[0]);
            int posY = int.Parse(pos.Split(',')[1]);
            MapItem item = go.GetComponent<MapItem>();
            //矩形
            if(item as MapRectAngleItem)
            {
                MapRectAngleItem rcItem = item as MapRectAngleItem;
                rcItem.SetMapItemPos(posX, posY);
                if(MapItemTool.JudgeBeyondBoundary(rcItem.RealNeedPlace, mapInfo.row, mapInfo.column))
                {
                    //当前获取到的随机地址不可存放物体(超过地图限制)，重新获取
                    tempCellInfos.Add(pos, mapCells[pos]);
                    mapCells.Remove(pos);
                    GenerateMap(go);
                    return;
                }

               
                //查看是否与以放置的物体重叠
                foreach (RectAngle rcPlaceInfo in PlaceGosPos)
                {
                    //判断要放置的物体与以放置的物体是否重叠
                    if (MapItemTool.JudgeMapItemOverLay(rcItem.RealNeedPlace, rcPlaceInfo))
                    {
                        //当前获取到的随机地址不可存放物体(存放物体范围内已被占用)，重新获取
                        tempCellInfos.Add(pos, mapCells[pos]);
                        mapCells.Remove(pos);
                        GenerateMap(go);
                        return;
                    }
                }
                //说明获取到的地址ok，放置物体
                PlaceRectGo(rcItem);
                ResetCellInfos();
                RemoveRCItemCells(rcItem);
            }
            //其它形状。。。如圆形
            else if (item as MapCircleItem)
            {

            }
            currentSelectGo = GetRandomPrefab();
            GameObject goClone = Instantiate(currentSelectGo, Vector3.zero, Quaternion.identity, RandomObjs.transform);
            goClone.SetActive(true);
            GenerateMap(goClone);
        }
        else
        {
            //已经没有可以放置该物体的点了，去除该物体的预制体
            Debug.Log("已经没有可以放置该物体的点了" + currentSelectGo.name);
            itemPrefabs.Remove(currentSelectGo);
            go.SetActive(false);
            ResetCellInfos();
            currentSelectGo = GetRandomPrefab();
            if (currentSelectGo != null)
            {
                GameObject goClone = Instantiate(currentSelectGo, Vector3.zero, Quaternion.identity, RandomObjs.transform);
                goClone.SetActive(true);
                GenerateMap(goClone);
            }
        }
    }

    /// <summary>
    /// 重置地图点的信息,并放置下一个物体
    /// </summary>
    public void ResetCellInfos()
    {
        foreach (string key in tempCellInfos.Keys)
        {
            mapCells.Add(key, tempCellInfos[key]);
        }
        tempCellInfos.Clear();
    }

    void RemoveRCItemCells(MapRectAngleItem rcItem)
    {
        RectAngle rc = rcItem.RealNeedPlace;
        int posX = rc.PosX;
        int posY = rc.PosY;
        for (int i = posX; i < posX + rc.XLength; i++)
        {
            for (int j = posY; j < posY + rc.YLength; j++)
            {
                mapCells.Remove(i+","+j);
            }
        }
    }

    void PlaceRectGo(MapRectAngleItem rcItem)
    {
        PlaceGosPos.Add(rcItem.RealNeedPlace);
        GameObject go = rcItem.gameObject;
        int posX = rcItem.RealNeedPlace.PosX;
        int posY = rcItem.RealNeedPlace.PosY;
        //int posX = 0;
        //int posY = 0;
        float startX = -mapInfo.xlength * 0.5f;
        float startZ = mapInfo.zlength * 0.5f;
        float goPosX = startX + (posX )* MapCellInfo.CellSize + 0.5f * rcItem.xlength;
        float goPosZ = startZ - (posY )* MapCellInfo.CellSize - 0.5f * rcItem.zlength;
        go.transform.position = new Vector3(goPosX, 0.0f, goPosZ);
        Debug.Log("放置物体:" + go.name + rcItem + "  " + go.transform.position + " posX:" + posX + " posY" + posY);

    }

 

    /// <summary>
    /// 从还没占用的格子里取得一个随机位置
    /// </summary>
    /// <returns></returns>
    string RandomPosition()
    {
        System.Random random = new System.Random(DateTime.Now.Millisecond); 
        int index = random.Next(0, mapCells.Keys.Count);
        int i = 0;
        foreach (string key in mapCells.Keys)
        {
            if (i == index)
            {
                return key;    
            }
            i++;
        }
        return null;
    }


    /// <summary>
    ///  获取可以随机生成的prefab实体
    /// </summary>
    /// <returns></returns>
    GameObject GetRandomPrefab()
    {
        if (itemPrefabs.Count == 0)
        {
            return null;
        }
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        return itemPrefabs[random.Next(0, itemPrefabs.Count)];
    }


    int GetMinSize()
    {
        int minSize = 0;
        foreach (GameObject go in itemPrefabs)
        {
            if (minSize == 0)
            {
                minSize = go.GetComponent<MapItem>().MapItemRealRow;
            }
            else
            {
                if (minSize > go.GetComponent<MapItem>().MapItemRealRow)
                {
                    minSize = go.GetComponent<MapItem>().MapItemRealRow;
                }
                else if (minSize > go.GetComponent<MapItem>().MapItemRealColumn)
                {
                    minSize = go.GetComponent<MapItem>().MapItemRealColumn;
                }
            }

        }
        return minSize;
    }

}
