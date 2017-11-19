// Decompiled with JetBrains decompiler
// Type: JSDU.DataHandling
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;

namespace JSDU
{
  internal class DataHandling
  {
    public double MeanValue(double[] array)
    {
      double num = 0.0;
      for (int index = 0; index < array.Length; ++index)
        num += array[index];
      return num / (double) array.Length;
    }

    public double StdError(double[] array)
    {
      double d = 0.0;
      double num = this.MeanValue(array);
      for (int index = 0; index < array.Length; ++index)
        d += (array[index] - num) * (array[index] - num);
      if (array.Length == 1)
        return Math.Sqrt(d);
      return Math.Sqrt(d / (double) (array.Length - 1));
    }

    public int MaxValueIndex(double[] array)
    {
      double num1 = 0.0;
      int num2 = 0;
      for (int index = 0; index < array.Length; ++index)
      {
        if (num1 < array[index])
        {
          num1 = array[index];
          num2 = index;
        }
      }
      return num2;
    }

    public int[] MaxValueIndex(double[] Array, int MaxNum, out double[] max)
    {
      double[] numArray1 = new double[Array.Length];
      Array.CopyTo((Array) numArray1, 0);
      max = new double[MaxNum];
      int[] numArray2 = new int[MaxNum];
      for (int index1 = 0; index1 < MaxNum; ++index1)
      {
        max[index1] = 0.0;
        for (int index2 = 0; index2 < numArray1.Length; ++index2)
        {
          if (max[index1] < numArray1[index2])
          {
            max[index1] = numArray1[index2];
            numArray2[index1] = index2;
          }
        }
        numArray1[numArray2[index1]] = 0.0;
      }
      return numArray2;
    }

    public double MaxValue(double[] array)
    {
      double num1 = 0.0;
      int num2 = 0;
      for (int index = 0; index < array.Length; ++index)
      {
        if (num1 < array[index])
        {
          num1 = array[index];
          num2 = index;
        }
      }
      return num1;
    }

    public double MinValue(double[] array)
    {
      double num1 = 1E+32;
      int num2 = 0;
      for (int index = 0; index < array.Length; ++index)
      {
        if (num1 > array[index])
        {
          num1 = array[index];
          num2 = index;
        }
      }
      return num1;
    }

    public int[] SortIndex(double[] array)
    {
      int[] numArray1 = new int[array.Length];
      double[] numArray2 = new double[array.Length];
      for (int index = 0; index < array.Length; ++index)
        numArray2[index] = array[index];
      for (int index1 = 1; index1 < numArray2.Length; ++index1)
      {
        for (int index2 = 0; index2 < numArray2.Length - index1; ++index2)
        {
          if (numArray2[index2] < numArray2[index2 + 1])
          {
            double num = numArray2[index2];
            numArray2[index2] = numArray2[index2 + 1];
            numArray2[index2 + 1] = num;
          }
        }
      }
      for (int index = 0; index < array.Length; ++index)
        numArray1[index] = Array.IndexOf<double>(array, numArray2[index]);
      return numArray1;
    }

    public void setExtremeValues(float[] arr, ref float smallestValue, ref float greatestValue)
    {
      if (arr == null || arr.Length == 0)
        throw new Exception("用于绘曲线图的数组为空");
      smallestValue = arr[0];
      greatestValue = arr[0];
      for (int index = 1; index < arr.Length; ++index)
      {
        if ((double) smallestValue > (double) arr[index])
          smallestValue = arr[index];
        if ((double) greatestValue < (double) arr[index])
          greatestValue = arr[index];
      }
    }

    public float[] WaveNum_Length(float[] OriginalData)
    {
      float[] numArray = new float[OriginalData.Length];
      for (int index = 0; index < OriginalData.Length; ++index)
        numArray[index] = (float) (1.0 / (double) OriginalData[index] * 10000.0);
      return numArray;
    }

    public double[] SpMean(double[,] X)
    {
      double[] numArray = new double[X.GetLength(1)];
      for (int index1 = 0; index1 < X.GetLength(1); ++index1)
      {
        if (X.GetLength(0) == 1)
        {
          numArray[index1] = X[0, index1];
        }
        else
        {
          double[] array = new double[X.GetLength(0)];
          for (int index2 = 0; index2 < X.GetLength(0); ++index2)
            array[index2] = X[index2, index1];
          numArray[index1] = this.MeanValue(array);
        }
      }
      return numArray;
    }

    public double[] SpStdError(double[,] X)
    {
      double[] numArray = new double[X.GetLength(1)];
      for (int index1 = 0; index1 < X.GetLength(1); ++index1)
      {
        double[] array = new double[X.GetLength(0)];
        for (int index2 = 0; index2 < X.GetLength(0); ++index2)
          array[index2] = X[index2, index1];
        numArray[index1] = this.StdError(array);
      }
      return numArray;
    }
  }
}
