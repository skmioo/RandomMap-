using UnityEngine;
using UnityEditor;

public class MapInfo 
{
    private GameObject map;
    public int row;
    public int column;
    public float xlength = 0.0f;
    public float zlength = 0.0f;
    public MapInfo(GameObject map)
    {
        this.map = map;
        GetMapInfo();
    }


    private void GetMapInfo()
    {
        Vector3 length = map.GetComponent<BoxCollider>().bounds.size;
        Vector3 scale = map.transform.lossyScale;
        xlength = length.x * scale.x;
        zlength = length.z * scale.z;
        row = (int)Mathf.Floor(xlength / MapCellInfo.CellSize);
        column = (int)Mathf.Floor(zlength / MapCellInfo.CellSize);
    }


    public override string ToString()
    {
        return "地图大小： row:" + row + "  colume:" + column;
    }
}