// Decompiled with JetBrains decompiler
// Type: JSDU.DataIO
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;
using System.IO;
using System.Windows.Forms;

namespace JSDU
{
  public class DataIO
  {
    public void TXTSaveData(string path, double[] Data_x, double[] Data_y)
    {
      if (Data_x.Length == Data_y.Length)
      {
        if (File.Exists(path))
          File.Delete(path);
        string str1 = "";
        using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
          StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
          try
          {
            for (int index = 0; index < Data_x.Length; ++index)
            {
              str1 = "";
              string str2 = Data_x[index].ToString() + " " + Data_y[index].ToString() + "\r\n";
              streamWriter.Write(str2);
            }
            streamWriter.Close();
            fileStream.Close();
          }
          catch (IOException ex)
          {
            int num = (int) MessageBox.Show(ex.Message + "File Error !");
          }
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("文件写入错误！");
      }
    }

    public int TXTReadData(string path, ref double[] Data_x, ref double[] Data_y, bool IfSize)
    {
      int num1 = 0;
      if (IfSize)
      {
        try
        {
          StreamReader streamReader = new StreamReader(path, false);
          while (streamReader.ReadLine() != null)
            ++num1;
          streamReader.Close();
          return num1;
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show(ex.Message);
          return 0;
        }
      }
      else
      {
        try
        {
          int num2 = 0;
          int num3 = 0;
          StreamReader streamReader = new StreamReader(path, false);
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string[] strArray = str.ToString().Trim().Split(' ');
            Data_x[num2++] = double.Parse(strArray[0].ToString());
            Data_y[num3++] = double.Parse(strArray[1].ToString());
          }
          streamReader.Close();
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show(ex.Message, "Error");
        }
        return 0;
      }
    }

    public int TXTReadDatas(string path, out double[,] Data)
    {
      Data = (double[,]) null;
      string str1;
      try
      {
        int length1 = 0;
        int length2 = 0;
        str1 = "";
        string[] strArray = (string[]) null;
        StreamReader streamReader = new StreamReader(path, false);
        string str2;
        while ((str2 = streamReader.ReadLine()) != null)
        {
          if (length1 == 0)
          {
            string str3 = str2.ToString().Trim();
            if (str3.IndexOf('\t') > 0)
              strArray = str3.Split('\t');
            else if (str3.IndexOf(' ') > 0)
              strArray = str3.Split(' ');
            length2 = strArray == null ? 1 : strArray.Length;
          }
          ++length1;
        }
        Data = new double[length1, length2];
        streamReader.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error");
      }
      try
      {
        int index1 = 0;
        str1 = "";
        StreamReader streamReader = new StreamReader(path, false);
        string str2;
        while ((str2 = streamReader.ReadLine()) != null)
        {
          string str3 = str2.ToString().Trim();
          string[] strArray = (string[]) null;
          if (str3.IndexOf('\t') > 0)
            strArray = str3.Split('\t');
          else if (str3.IndexOf(' ') > 0)
            strArray = str3.Split(' ');
          if (strArray != null)
          {
            for (int index2 = 0; index2 < strArray.Length; ++index2)
              Data[index1, index2] = (double) float.Parse(strArray[index2].ToString());
          }
          else
            Data[index1, 0] = (double) float.Parse(str3.ToString());
          ++index1;
        }
        streamReader.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error");
      }
      return 0;
    }

    public void TXTWriteIn(string path, string Content)
    {
      try
      {
        using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write))
        {
          StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
          streamWriter.WriteLine(Content.ToString());
          streamWriter.Close();
          fileStream.Close();
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("文件写入错误，请重新测量");
      }
    }

    public void CreatTXT(string path)
    {
      try
      {
        using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
          fileStream.Close();
      }
      catch
      {
        int num = (int) MessageBox.Show("文件错误，请重新测量");
      }
    }

    public void ReadLineTXT(string path, out string[] Content)
    {
      Content = new string[1];
      try
      {
        int length = 0;
        string str1 = "";
        StreamReader streamReader1 = new StreamReader(path, false);
        while ((str1 = streamReader1.ReadLine()) != null)
          ++length;
        streamReader1.Close();
        StreamReader streamReader2 = new StreamReader(path, false);
        Content = new string[length];
        int index = 0;
        string str2;
        while ((str2 = streamReader2.ReadLine()) != null)
        {
          Content[index] = str2;
          ++index;
        }
        streamReader2.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error");
      }
    }

    public void CopyFolder(string sourceFolder, string destFolder)
    {
      if (!Directory.Exists(destFolder))
        Directory.CreateDirectory(destFolder);
      foreach (string file in Directory.GetFiles(sourceFolder))
      {
        string fileName = Path.GetFileName(file);
        string destFileName = Path.Combine(destFolder, fileName);
        File.Copy(file, destFileName, true);
      }
      foreach (string directory in Directory.GetDirectories(sourceFolder))
      {
        string fileName = Path.GetFileName(directory);
        string destFolder1 = Path.Combine(destFolder, fileName);
        this.CopyFolder(directory, destFolder1);
      }
    }

    public void DeleteFolder(string dir)
    {
      if (!Directory.Exists(dir))
        return;
      foreach (string fileSystemEntry in Directory.GetFileSystemEntries(dir))
      {
        if (File.Exists(fileSystemEntry))
        {
          FileInfo fileInfo = new FileInfo(fileSystemEntry);
          if (fileInfo.Attributes.ToString().IndexOf("Readonly") != 1)
            fileInfo.Attributes = FileAttributes.Normal;
          File.Delete(fileSystemEntry);
        }
        else
          this.DeleteFolder(fileSystemEntry);
      }
      Directory.Delete(dir);
    }

    public void SaveStr(string WriteStr, string Path)
    {
      Path = Spectrometer.ApplicationPath + "\\Setting";
      File.Delete(Path);
      using (FileStream fileStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
      {
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
        try
        {
          streamWriter.Write(WriteStr);
          streamWriter.Close();
          fileStream.Close();
        }
        catch (IOException ex)
        {
          int num = (int) MessageBox.Show(ex.Message + "File Error !");
        }
      }
    }
  }
}
