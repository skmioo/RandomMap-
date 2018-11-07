using UnityEngine;
using UnityEditor;

/// <summary>
/// 矩形的地图物品形状
/// </summary>
public class RectAngle 
{
    //左上角X,Y
    public int PosX;
    public int PosY;
    public int XLength;
    public int YLength;
 
    public RectAngle()
    {

    }

    public RectAngle(int posX, int posY,int xLength,int yLength)
    {
        PosX = posX;
        PosY = posY;
        XLength = posY;
        YLength = yLength;
    }

    public void SetPosXY(int posX, int posY)
    {
        PosX = posX;
        PosY = posY;
    }

    public void SetLengthXY(int xLength, int yLength)
    {
        XLength = xLength;
        YLength = yLength;
    }



}