using UnityEngine;
using UnityEditor;

public abstract class MapCircleItem : MapItem
{
    public override MapItemType ItemType
    {
        get
        {
            return MapItemType.Circle;
        }
    }

}