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
        int right1 = rc1.PosX + rc1.XLength;
        int bottom1 = rc1.PosY + rc1.YLength;
        int right2 = rc2.PosX + rc2.XLength;
        int bottom2 = rc2.PosY + rc2.YLength;
        ////先简单检测,如果未重叠直接返回，否则进行具体的重叠检测
        ////rc1在rc2的 左上角 右上角 左下角 右下角
        //if (right1 < rc2.PosX && bottom1 < rc2.PosY
        //    || rc1.PosX > right2 && bottom1 < rc2.PosY
        //    || right1 < rc2.PosX && rc1.PosY > bottom2
        //    || rc1.PosX > right2 && rc1.PosY > bottom2
        //    )
        //{
        //    return false;
        //}

        float centerOffsetX = Mathf.Abs((rc1.PosX + right1) * 0.5f - (rc2.PosX + right2) * 0.5f);
        float centerOffsetY = Mathf.Abs((rc1.PosY + bottom1) * 0.5f - (rc2.PosY + bottom2) * 0.5f);
        float addX = (rc1.XLength + rc2.XLength) * 0.5f;
        float addY = (rc1.YLength + rc2.YLength) * 0.5f;
        if (centerOffsetX < addX && centerOffsetY < addY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //public static bool JudgeMapItemOverLay(RectAngle rc1, RectAngle rc2)
    //{
    //    int left1 = rc1.PosX;
    //    int top1 = rc1.PosY;
    //    int right1 = rc1.PosX + rc1.XLength;
    //    int bottom1 = rc1.PosY + rc1.YLength;
    //    int left2 = rc2.PosX;
    //    int top2 = rc2.PosY;
    //    int right2 = rc2.PosX + rc2.XLength;
    //    int bottom2 = rc2.PosY + rc2.YLength;
    //    //左上角
    //    /*
    //     * (left1,top1)      (right1,top1)
    //     * 
    //     * 
    //     * (left1,bottom1)   (right1,bottom1)
    //     * 
    //     * 
    //        * (left1,top1)      (right1,top1)
    //        * 
    //        * 
    //        * (left1,bottom1)   (right1,bottom1)
    //     * 
    //     */


    //    if (left1 < left2 && top1 < top2)
    //    {
    //        if(right1 < left1)
    //    }

    //}


    /// <summary>
    ///  判断矩形是否超出边界
    /// </summary>
    /// <param name="rc"></param>
    /// <param name="xMax"> 边界x的最大值</param>
    /// <param name="yMax"> 边界y的最大值</param>
    /// <returns></returns>
    public static bool JudgeBeyondBoundary(RectAngle rc, int xMax,int yMax)
    {
        if (rc.PosX + rc.XLength <= xMax && rc.PosY + rc.YLength <= yMax)
        {
            return false;
        }
        return true;
    }
}