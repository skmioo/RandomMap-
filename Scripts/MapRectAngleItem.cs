using UnityEngine;
using UnityEditor;

public abstract class MapRectAngleItem : MapItem
{
    public override MapItemType ItemType
    {
        get
        {
            return MapItemType.RectAngle;
        }
    }

    //实际所占大小(包括距离)
    public RectAngle RealNeedPlace = new RectAngle();

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