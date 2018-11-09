using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class MapGenerateMgr : CSharpSingletion<MapGenerateMgr>
{
    private List<string> mapCells = new List<string>();
    private List<string> tempCellInfos = new List<string>();
    //所有海岛信息
    private List<Island> islandInfos = new List<Island>();
    //保存还可以用于生成的海岛实例
    private List<Island> islandCanPlace = new List<Island>();
    //已放置的海岛
    private List<IslandPlaceInfo> islandsPlaced = new List<IslandPlaceInfo>();
    private Dictionary<string, int> isLandCount = new Dictionary<string, int>();
    private const int defaultIsLandCount = 2;
    private Island currentIsland = null;

    public delegate void PlaceIslands(List<IslandPlaceInfo> infos);
    public PlaceIslands placeIslands;
    private MapInfo mapInfo;
    public void InitData()
    {
        LoadMapInfo();
        LoadIslandInfos();
        SortIsland();
        LoadIslandCount();
        int minSize = GetMinSize();
        //添加可以所有生成需要的格子
        for (int i = 0; i < mapInfo.mapRow - minSize + 1; i++)
        {
            for (int j = 0; j < mapInfo.mapColumn - minSize + 1; j++)
            {
                mapCells.Add(i + "," + j);
            }
        }
        //开始随机放置物品
        PlaceRandomIsland();
        if (placeIslands != null)
        {
            placeIslands(islandsPlaced);
        }
    }

    private void PlaceRandomIsland()
    {
        currentIsland = GetCanPlaceIsland();
        if (currentIsland != null)
        {
            GenerateMap(currentIsland);
        }
    }

    private void GenerateMap(Island island)
    {
        string pos = RandomPosition();
        if (pos != null)
        {
            int posX = int.Parse(pos.Split(',')[0]);
            int posY = int.Parse(pos.Split(',')[1]);
            
            RectAngle islandRc = new RectAngle(posX, posY,island.mapItemRealRow, island.mapItemRealColumn);
            IslandPlaceInfo info = new IslandPlaceInfo();
            info.rc = islandRc;
            info.island = island;
            if (MapItemTool.JudgeBeyondBoundary(islandRc, mapInfo.mapRow, mapInfo.mapColumn))
            {
                //当前获取到的随机地址不可存放物体(超过地图限制)，重新获取地点
                tempCellInfos.Add(pos);
                mapCells.Remove(pos);
                GenerateMap(island);
                return;
            }

            //查看是否与以放置的物体重叠
            foreach (IslandPlaceInfo placeInfo in islandsPlaced)
            {
                //判断要放置的物体与以放置的物体是否重叠
                if (MapItemTool.JudgeMapItemOverLay(info.rc, placeInfo.rc))
                {
                    //当前获取到的随机地址不可存放物体(存放物体范围内已被占用)，重新获取地点
                    tempCellInfos.Add(pos);
                    mapCells.Remove(pos);
                    GenerateMap(island);
                    return;
                }
            }
            //说明获取到的地址ok，放置物体
            islandsPlaced.Add(info);
            ResetCellInfos();
            RemoveRCItemCells(info);
            //放置海岛后，生成下一个海岛，继续放置
            PlaceRandomIsland();

        }
        else
        {
            //已经没有可以放置该物体的点了，去除该物体的预制体及可以存放个数
            Debug.Log("已经没有可以放置该物体的点了" + currentIsland.name);
            if (isLandCount.ContainsKey(currentIsland.name))
            {
                isLandCount.Remove(currentIsland.name);
            }
            islandCanPlace.Remove(currentIsland);
            ResetCellInfos();
            PlaceRandomIsland();
        }
    }


    /// <summary>
    /// 重置地图点的信息,并放置下一个物体
    /// </summary>
    private void ResetCellInfos()
    {
        foreach (string key in tempCellInfos)
        {
            mapCells.Add(key);
        }
        tempCellInfos.Clear();
    }

    private void RemoveRCItemCells(IslandPlaceInfo info)
    {
        RectAngle rc = info.rc;
        int posX = rc.PosX;
        int posY = rc.PosY;
        for (int i = posX; i < posX + rc.Row; i++)
        {
            for (int j = posY; j < posY + rc.Column; j++)
            {
                mapCells.Remove(i + "," + j);
            }
        }
    }

    /// <summary>
    ///  获取可以摆放的海岛
    /// </summary>
    /// <returns></returns>
    private Island GetCanPlaceIsland()
    {
        if (islandCanPlace.Count == 0)
        {
            return null;
        }
        Island island = null;
        List<Island> removeIslands = new List<Island>();
        foreach (Island tmp in islandCanPlace)
        {
            if (GetIslandCount(tmp.name) > 0)
            {
                island = tmp;
                ReduceIslandCount(tmp.name);
                break;
            }
            else
            {
                removeIslands.Add(tmp);
            }
        }
        if (removeIslands.Count > 0)
        {
            foreach (Island tmp in removeIslands)
            {
                islandCanPlace.Remove(tmp);
            }
        }

        return island;
    }


    //获取当前islandCount
    private int GetIslandCount(string island)
    {
        if (isLandCount.ContainsKey(island))
        {
            return isLandCount[island];
        }
        else
        {
            return 0;
        }
    }


    private void ReduceIslandCount(string island)
    {
        if (isLandCount.ContainsKey(island))
        {
            int count = isLandCount[island];
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    isLandCount.Remove(island);
                }
                else
                {
                    isLandCount[island] = count;
                }
            }
            else
            {
                isLandCount.Remove(island);
            }
        }
    }

    /// <summary>
    /// 从还没占用的格子里取得一个随机位置
    /// </summary>
    /// <returns></returns>
    private string RandomPosition()
    {
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        int index = random.Next(0, mapCells.Count);
        int i = 0;
        foreach (string key in mapCells)
        {
            if (i == index)
            {
                return key;
            }
            i++;
        }
        return null;
    }



    private void LoadIslandCount()
    {
        foreach (Island go in islandInfos)
        {
            isLandCount.Add(go.name, defaultIsLandCount);
        }
    }

    private void LoadIslandInfos()
    {
        string info = FileOperateUtil.ReadFileInfo("Assets/Configs", "islandInfo.json");
        Island[] datas = JsonConvert.DeserializeObject<Island[]>(info);
        for (int i = 0; i < datas.Length; i++)
        {
            Island data = datas[i];
            islandInfos.Add(data);
        }
    }

    private void LoadMapInfo()
    {
        string info = FileOperateUtil.ReadFileInfo("Assets/Configs", "mapInfo.json");
        mapInfo = JsonConvert.DeserializeObject<MapInfo>(info);
    }

    private void SortIsland()
    {
        islandInfos.Sort((left, right) => {
            float leftArea = left.xlength * left.scaleX * left.zlength * left.scaleZ;
            float rightArea = right.xlength * right.scaleX * right.zlength * right.scaleZ;
            if (leftArea < rightArea)
                return 1;
            else if (leftArea == rightArea)
                return 0;
            else
                return -1;
        });

        for (int i = 0; i < islandInfos.Count; i++)
        {
            Island data = islandInfos[i];
            islandCanPlace.Add(data);
        }
    }

    private int GetMinSize()
    {
        int minSize = 0;
        foreach (Island island in islandInfos)
        {
            if (minSize == 0)
            {
                minSize = island.mapItemRealRow;
            }
            else
            {
                if (minSize > island.mapItemRealRow)
                {
                    minSize = island.mapItemRealRow;
                }
                else if (minSize > island.mapItemRealColumn)
                {
                    minSize = island.mapItemRealColumn;
                }
            }

        }
        return minSize;
    }

}