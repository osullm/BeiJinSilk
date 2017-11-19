// Decompiled with JetBrains decompiler
// Type: JSDU.SimcaPrd
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;

namespace JSDU
{
  internal class SimcaPrd
  {
    private ripsPreDeal PreDeal = new ripsPreDeal();

    public int[] SimcaPrdt(double[,] newx, double[,] mod)
    {
      int length1 = (int) mod[0, 9];
      int num1 = 0;
      int length2 = newx.GetLength(0);
      num1 = newx.GetLength(1);
      int length3 = mod.GetLength(0);
      int length4 = mod.GetLength(1);
      int[] numArray1 = new int[length1 + 1];
      for (int index = 0; index < length1; ++index)
        numArray1[index] = (int) mod[1, index];
      numArray1[length1] = length3 + 1;
      double[,] numArray2 = new double[length2, length1];
      double[,] numArray3 = new double[length2, length1];
      double[,] numArray4 = new double[length2, length1];
      double[] numArray5 = new double[length1];
      int[] numArray6 = new int[length2];
      numArray6[0] = -1;
      for (int index1 = 0; index1 < length1; ++index1)
      {
        double[,] numArray7 = new double[numArray1[index1 + 1] - numArray1[index1], length4];
        for (int index2 = 0; index2 < numArray1[index1 + 1] - numArray1[index1]; ++index2)
        {
          for (int index3 = 0; index3 < length4; ++index3)
            numArray7[index2, index3] = mod[numArray1[index1] + index2 - 1, index3];
        }
        int length5 = numArray7.GetLength(0);
        length4 = numArray7.GetLength(1);
        double num2 = numArray7[0, 14];
        double num3 = numArray7[0, 15];
        double num4 = numArray7[0, 6];
        double[,] ssq = new double[(int) numArray7[0, 13], 4];
        numArray5[index1] = numArray7[0, 9];
        double[,] loads = (double[,]) null;
        double[,] NewData = (double[,]) null;
        if (1.0 == numArray7[0, 11])
        {
          loads = new double[(int) numArray7[0, 7], length5 - 2];
          for (int index2 = 2; index2 < length5; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              loads[index3, index2 - 2] = numArray7[index2, index3];
          }
          NewData = (double[,]) newx.Clone();
          for (int index2 = 0; index2 < (int) numArray7[0, 13]; ++index2)
            ssq[index2, 1] = numArray7[1, index2];
        }
        else if (2.0 == numArray7[0, 11])
        {
          loads = new double[(int) numArray7[0, 7], length5 - 3];
          for (int index2 = 3; index2 < length5; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              loads[index3, index2 - 3] = numArray7[index2, index3];
          }
          NewData = (double[,]) newx.Clone();
          for (int index2 = 0; index2 < (int) numArray7[0, 13]; ++index2)
            ssq[index2, 1] = numArray7[2, index2];
          for (int index2 = 0; index2 < length2; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              NewData[index2, index3] = newx[index2, index3] - numArray7[1, index3];
          }
        }
        else if (3.0 == numArray7[0, 11])
        {
          loads = new double[(int) numArray7[0, 7], length5 - 4];
          for (int index2 = 4; index2 < length5; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              loads[index3, index2 - 4] = numArray7[index2, index3];
          }
          NewData = (double[,]) newx.Clone();
          for (int index2 = 0; index2 < (int) numArray7[0, 13]; ++index2)
            ssq[index2, 1] = numArray7[3, index2];
          for (int index2 = 0; index2 < length2; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              NewData[index2, index3] = (newx[index2, index3] - numArray7[1, index3]) / numArray7[2, index3];
          }
        }
        double[,] scores = (double[,]) null;
        double[] res = (double[]) null;
        double[] tsqvals = (double[]) null;
        this.PCAPro(NewData, loads, ssq, out scores, out res, out tsqvals);
        for (int index2 = 0; index2 < length2; ++index2)
        {
          numArray2[index2, index1] = tsqvals[index2] / num3;
          numArray3[index2, index1] = res[index2] / num2;
          numArray4[index2, index1] = Math.Sqrt(numArray2[index2, index1] * numArray2[index2, index1] + numArray3[index2, index1] * numArray3[index2, index1]);
        }
        for (int index2 = 0; index2 < length2; ++index2)
        {
          double[] array = new double[length1];
          for (int index3 = 0; index3 < length1; ++index3)
            array[index3] = numArray4[index2, index3];
          numArray6[index2] = this.MinValue(array);
        }
      }
      return numArray6;
    }

    public int[] SimcaPrdt(double[,] newx, double[,] mod, out string[] parameterOut)
    {
      parameterOut = new string[newx.GetLength(0)];
      int length1 = (int) mod[0, 9];
      int num1 = 0;
      int length2 = newx.GetLength(0);
      num1 = newx.GetLength(1);
      int length3 = mod.GetLength(0);
      int length4 = mod.GetLength(1);
      int[] numArray1 = new int[length1 + 1];
      for (int index = 0; index < length1; ++index)
        numArray1[index] = (int) mod[1, index];
      numArray1[length1] = length3 + 1;
      double[,] numArray2 = new double[length2, length1];
      double[,] numArray3 = new double[length2, length1];
      double[,] numArray4 = new double[length2, length1];
      double[] numArray5 = new double[length1];
      int[] numArray6 = new int[length2];
      numArray6[0] = -1;
      for (int index1 = 0; index1 < length1; ++index1)
      {
        double[,] numArray7 = new double[numArray1[index1 + 1] - numArray1[index1], length4];
        for (int index2 = 0; index2 < numArray1[index1 + 1] - numArray1[index1]; ++index2)
        {
          for (int index3 = 0; index3 < length4; ++index3)
            numArray7[index2, index3] = mod[numArray1[index1] + index2 - 1, index3];
        }
        int length5 = numArray7.GetLength(0);
        length4 = numArray7.GetLength(1);
        double num2 = numArray7[0, 14];
        double num3 = numArray7[0, 15];
        double num4 = numArray7[0, 6];
        double[,] ssq = new double[(int) numArray7[0, 13], 4];
        numArray5[index1] = numArray7[0, 9];
        double[,] loads = (double[,]) null;
        double[,] NewData = (double[,]) null;
        if (1.0 == numArray7[0, 11])
        {
          loads = new double[(int) numArray7[0, 7], length5 - 2];
          for (int index2 = 2; index2 < length5; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              loads[index3, index2 - 2] = numArray7[index2, index3];
          }
          NewData = (double[,]) newx.Clone();
          for (int index2 = 0; index2 < (int) numArray7[0, 13]; ++index2)
            ssq[index2, 1] = numArray7[1, index2];
        }
        else if (2.0 == numArray7[0, 11])
        {
          loads = new double[(int) numArray7[0, 7], length5 - 3];
          for (int index2 = 3; index2 < length5; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              loads[index3, index2 - 3] = numArray7[index2, index3];
          }
          NewData = (double[,]) newx.Clone();
          for (int index2 = 0; index2 < (int) numArray7[0, 13]; ++index2)
            ssq[index2, 1] = numArray7[2, index2];
          for (int index2 = 0; index2 < length2; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              NewData[index2, index3] = newx[index2, index3] - numArray7[1, index3];
          }
        }
        else if (3.0 == numArray7[0, 11])
        {
          loads = new double[(int) numArray7[0, 7], length5 - 4];
          for (int index2 = 4; index2 < length5; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              loads[index3, index2 - 4] = numArray7[index2, index3];
          }
          NewData = (double[,]) newx.Clone();
          for (int index2 = 0; index2 < (int) numArray7[0, 13]; ++index2)
            ssq[index2, 1] = numArray7[3, index2];
          for (int index2 = 0; index2 < length2; ++index2)
          {
            for (int index3 = 0; index3 < (int) numArray7[0, 7]; ++index3)
              NewData[index2, index3] = (newx[index2, index3] - numArray7[1, index3]) / numArray7[2, index3];
          }
        }
        double[,] scores = (double[,]) null;
        double[] res = (double[]) null;
        double[] tsqvals = (double[]) null;
        this.PCAPro(NewData, loads, ssq, out scores, out res, out tsqvals);
        for (int index2 = 0; index2 < length2; ++index2)
        {
          numArray2[index2, index1] = tsqvals[index2] / num3;
          numArray3[index2, index1] = res[index2] / num2;
          numArray4[index2, index1] = Math.Sqrt(numArray2[index2, index1] * numArray2[index2, index1] + numArray3[index2, index1] * numArray3[index2, index1]);
        }
        for (int index2 = 0; index2 < length2; ++index2)
        {
          double[] array = new double[length1];
          parameterOut[index2] = "rt2rqsum";
          for (int index3 = 0; index3 < length1; ++index3)
          {
            array[index3] = numArray4[index2, index3];
            string[] strArray;
            IntPtr index4;
            (strArray = parameterOut)[(int) (index4 = (IntPtr) index2)] = strArray[(int)index4] + array[index3].ToString() + ",";
          }
          numArray6[index2] = this.MinValue(array);
          string[] strArray1;
          IntPtr index5;
          (strArray1 = parameterOut)[(int) (index5 = (IntPtr) index2)] = strArray1[(int)index5] + "    结果：" + numArray6[index2].ToString();
        }
      }
      return numArray6;
    }

    public void PCAPro(double[,] NewData, double[,] loads, double[,] ssq, out double[,] scores, out double[] res, out double[] tsqvals)
    {
      int length1 = NewData.GetLength(0);
      int length2 = NewData.GetLength(1);
      int length3 = loads.GetLength(0);
      int length4 = loads.GetLength(1);
      scores = this.PreDeal.multiMatrix(length1, length2, length3, length4, NewData, loads);
      double[,] x = this.PreDeal.conMatrix(length1, length2, NewData);
      double[,] y1 = this.PreDeal.conMatrix(length1, length4, scores);
      double[,] y2 = this.PreDeal.multiMatrix(length3, length4, length4, length1, loads, y1);
      double[,] numArray1 = this.PreDeal.subMatrix(length2, length1, length3, length1, x, y2);
      int length5 = numArray1.GetLength(0);
      int length6 = numArray1.GetLength(1);
      res = new double[length6];
      for (int index1 = 0; index1 < length6; ++index1)
      {
        res[index1] = 0.0;
        for (int index2 = 0; index2 < length5; ++index2)
          res[index1] += numArray1[index2, index1] * numArray1[index2, index1];
      }
      double[,] numArray2 = new double[length1, length4];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length4; ++index2)
          numArray2[index1, index2] = scores[index1, index2] * scores[index1, index2];
      }
      double[] numArray3 = new double[length4];
      for (int index = 0; index < length4; ++index)
        numArray3[index] = 1.0 / ssq[index, 1];
      tsqvals = new double[length1];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        tsqvals[index1] = 0.0;
        for (int index2 = 0; index2 < length4; ++index2)
          tsqvals[index1] += numArray2[index1, index2] * numArray3[index2];
      }
    }

    public int MinValue(double[] array)
    {
      double num1 = 1E+32;
      int num2 = -1;
      for (int index = 0; index < array.Length; ++index)
      {
        if (num1 > array[index])
        {
          num1 = array[index];
          num2 = index;
        }
      }
      return num2;
    }
  }
}
