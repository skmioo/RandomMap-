using UnityEngine;
using UnityEditor;

/// <summary>
/// 摆放矩形的地图物品信息(格子值)
/// </summary>
public class RectAngle 
{
    //左上角X,Y位置信息
    public int PosX;
    public int PosY;
    //矩形所占的长宽值
    public int Row;
    public int Column;
 
    public RectAngle()
    {

    }

    public RectAngle(int posX, int posY,int row,int column)
    {
        PosX = posX;
        PosY = posY;
        Row = row;
        Column = column;
    }

    public void SetPosXY(int posX, int posY)
    {
        PosX = posX;
        PosY = posY;
    }

    public void SetLengthXY(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
