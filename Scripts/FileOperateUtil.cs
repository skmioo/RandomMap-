using UnityEngine;
using UnityEditor;
using System.IO;

public class FileOperateUtil 
{
    public static void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "/" + name);
        if (t.Exists)
        {
            t.Delete();
        }
        //如果此文件不存在则创建
        sw = t.CreateText();
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    public static void AppandFileData(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "/" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else {
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    public static string ReadFileInfo(string path, string name)
    {
        return File.ReadAllText(path + "/" + name);
    }


}