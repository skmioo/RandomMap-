using UnityEngine;

/// <summary>
/// 海岛上存在物体的基类
/// </summary>
public abstract class MapItem : MonoBehaviour
{
    public abstract string Name { get; }

    //海岛上物体实际大小所占行
    [HideInInspector]
    public int MapItemRealRow;
    //海岛上物体实际大小所占列
    [HideInInspector]
    public int MapItemRealColumn;

    //设置物体起始所在位置
    public abstract void SetMapItemPos(int x, int y);

    //场景中物体所占长宽
    public float xlength = 0.0f;
    public float zlength = 0.0f;

    public void InitItemData(Island island)
    {
        xlength = island.xlength;
        zlength = island.zlength;
        MapItemRealRow = island.mapItemRealRow;
        MapItemRealColumn = island.mapItemRealColumn;
        SetItemLengthXY();
    }

    /// <summary>
    /// 设置位置信息的长宽值
    /// </summary>
    public virtual void SetItemLengthXY()
    {

    }
}
