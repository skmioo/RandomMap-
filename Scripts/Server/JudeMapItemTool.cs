using UnityEngine;
using UnityEditor;

public class MapItemTool 
{
    /*
    * 矩形重叠
    if (rc1.x + rc1.width > rc2.x &&
    rc2.x + rc2.width > rc1.x &&
    rc1.y + rc1.height > rc2.y &&
    rc2.y + rc2.height > rc1.y
    )
    return true;
    else
    return false;
    */
    public static bool JudgeMapItemOverLay(RectAngle rc1, RectAngle rc2)
    {
        int right1 = rc1.PosX + rc1.Row;
        int bottom1 = rc1.PosY + rc1.Column;
        int right2 = rc2.PosX + rc2.Row;
        int bottom2 = rc2.PosY + rc2.Column;
        ////先简单检测,如果未重叠直接返回，否则进行具体的重叠检测

        float centerOffsetX = Mathf.Abs((rc1.PosX + right1) * 0.5f - (rc2.PosX + right2) * 0.5f);
        float centerOffsetY = Mathf.Abs((rc1.PosY + bottom1) * 0.5f - (rc2.PosY + bottom2) * 0.5f);
        float addX = (rc1.Row + rc2.Row) * 0.5f;
        float addY = (rc1.Column + rc2.Column) * 0.5f;
        if (centerOffsetX < addX && centerOffsetY < addY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///  判断矩形是否超出边界
    /// </summary>
    /// <param name="rc"></param>
    /// <param name="xMax"> 边界x的最大值</param>
    /// <param name="yMax"> 边界y的最大值</param>
    /// <returns></returns>
    public static bool JudgeBeyondBoundary(RectAngle rc, int xMax,int yMax)
    {
        if (rc.PosX + rc.Row <= xMax && rc.PosY + rc.Column <= yMax)
        {
            return false;
        }
        return true;
    }
}