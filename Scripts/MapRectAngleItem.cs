using UnityEngine;
using UnityEditor;

public class MapRectAngleItem : MapItem
{

    //实际所占大小(包括距离)
    public RectAngle RealNeedPlace = new RectAngle();

    public override string Name
    {
        get
        {
           return gameObject.name;
        }
    }

    public override void SetItemLengthXY()
    {
        RealNeedPlace.SetLengthXY(MapItemRealRow, MapItemRealColumn);
    }

    public override void SetMapItemPos(int x,int y)
    {
        RealNeedPlace.SetPosXY(x,y);
    }

    public override string ToString()
    {
        return "PosX" + RealNeedPlace.PosX + "PosY" + RealNeedPlace.PosY + ": MapItemRow:" + MapItemRealRow + " MapItemColumn:" + MapItemRealColumn;
    }

}