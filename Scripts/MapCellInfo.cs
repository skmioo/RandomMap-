using UnityEngine;
using UnityEditor;

/// <summary>
/// 单个地图cell的信息
/// </summary>
public class MapCellInfo 
{
    //单个cell所占面积大小，这个很重要，影响到地图生成的格子的多少
    public static int CellSize = 40;
    public int PosX;
    public int PosY;
    public bool isPlace = false;
}