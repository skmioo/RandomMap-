using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
public class MapUtilEditor {
    private static MapInfo mapInfo;
    //更新海岛信息
    [MenuItem("Tools/UpdateIslandInfo")]
    static public void UpdateIslandInfo()
    {
        LoadMapInfo();
        GameObject[] arr = Resources.LoadAll<GameObject>("Prefabs/IslandItems");
        string islandInfo = "[";
        foreach (GameObject go in arr)
        {
            Vector3 length = go.GetComponent<BoxCollider>().size;
            Vector3 scale = go.transform.lossyScale;
            Island island = new Island();
            island.name = go.name;
            island.scaleX = scale.x;
            island.scaleZ = scale.z;
            island.xlength = length.x * scale.x;
            island.zlength = length.z * scale.z;
            island.mapItemRealRow = (int)Mathf.Ceil(island.xlength / mapInfo.cellSize);
            island.mapItemRealColumn = (int)Mathf.Ceil(island.zlength / mapInfo.cellSize);
            islandInfo += JsonConvert.SerializeObject(island) + ",";
        }
        islandInfo = islandInfo.Substring(0, islandInfo.Length - 1);
        islandInfo += "]";
        FileOperateUtil.CreateFile("Assets/Configs", "islandInfo.json", islandInfo);
    }

    static void LoadMapInfo()
    {
        string info = FileOperateUtil.ReadFileInfo("Assets/Configs", "mapInfo.json");
        mapInfo = JsonConvert.DeserializeObject<MapInfo>(info);
    }
}
