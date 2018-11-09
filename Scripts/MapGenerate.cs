using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoSingletion<MapGenerate> {
    private List<GameObject> itemPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> islandPrefabs = new Dictionary<string, GameObject>();
    public GameObject RandomObjs;
    private Dictionary<string, Island> islandInfos = new Dictionary<string, Island>();
    private MapInfo mapInfo;
    void Start () {
        //LoadIslandInfos();
        LoadMapInfo();

        //加载默认海岛物体
        GameObject[] arr = Resources.LoadAll<GameObject>("Prefabs/IslandItems");
        itemPrefabs.Clear();
        foreach (GameObject go in arr)
        {
            islandPrefabs.Add(go.name, go);
        }
        MapGenerateMgr.Instance.placeIslands += PlaceIslands;
        MapGenerateMgr.Instance.InitData();
      
    }

    public void PlaceIslands(List<IslandPlaceInfo> infos)
    {
        foreach (IslandPlaceInfo info in infos)
        {
            Island island = info.island;
            GameObject templet = islandPrefabs[island.name];
            GameObject go = Instantiate(templet, Vector3.zero, Quaternion.identity, RandomObjs.transform);
            RectAngle rc = info.rc;
            //Debug.Log("放置海岛:"+island.name + " x:" + rc.PosX + " y:" + rc.PosY);
            float startX = - mapInfo.mapXLength * 0.5f;
            float startZ = mapInfo.mapZLength * 0.5f;
            float goPosX = startX + (rc.PosX) * mapInfo.cellSize + 0.5f * island.xlength;
            float goPosZ = startZ - (rc.PosY) * mapInfo.cellSize - 0.5f * island.zlength;
            go.transform.position = new Vector3(goPosX, 0.0f, goPosZ);
            go.SetActive(true);
            go.name = templet.name;
        }
    }

    //public void LoadIslandInfos()
    //{
    //    string info = FileOperateUtil.ReadFileInfo("Assets/Configs", "islandInfo.json");
    //    Island[] datas = JsonConvert.DeserializeObject<Island[]>(info);
    //    for (int i = 0; i < datas.Length; i++)
    //    {
    //        Island data = datas[i];
    //        islandInfos.Add(data.name, data);
    //    }
    //}

    private void LoadMapInfo()
    {
        string info = FileOperateUtil.ReadFileInfo("Assets/Configs", "mapInfo.json");
        mapInfo = JsonConvert.DeserializeObject<MapInfo>(info);
    }


    public MapInfo GetMapInfo()
    {
        return mapInfo;            
    }
}
