// Decompiled with JetBrains decompiler
// Type: JSDU.ripsPreDeal
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;
using System.Windows.Forms;

namespace JSDU
{
  internal class ripsPreDeal
  {
    private int n;
    private int m;
    private double[,] rip;
    private ProgressBar pgv;
    private int value;

    public ripsPreDeal(int n, int m)
    {
      this.n = n;
      this.m = m;
      this.rip = new double[n, m];
    }

    public double[,] diff1(double[,] ripsMatrix, int g)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = g - 1; index2 < this.m - g; ++index2)
          this.rip[index1, index2] = (ripsMatrix[index1, index2 + g] - ripsMatrix[index1, index2]) / (double) g;
        for (int index2 = 0; index2 < g - 1; ++index2)
          this.rip[index1, index2] = 0.0;
        for (int index2 = this.m - g; index2 < this.m; ++index2)
          this.rip[index1, index2] = 0.0;
      }
      return this.rip;
    }

    public double[,] diff2(double[,] ripsMatrix, int g)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = g; index2 < this.m - g; ++index2)
          this.rip[index1, index2] = (ripsMatrix[index1, index2 + g] - 2.0 * ripsMatrix[index1, index2] + ripsMatrix[index1, index2 - g]) / (double) (g * g);
        for (int index2 = 0; index2 < g; ++index2)
          this.rip[index1, index2] = 0.0;
        for (int index2 = this.m - g; index2 < this.m; ++index2)
          this.rip[index1, index2] = 0.0;
      }
      return this.rip;
    }

    public double[,] xmean(double[,] ripsMatrix)
    {
      double[] numArray = this.avgRipsMatrix(ripsMatrix);
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2] - numArray[index2];
      }
      return this.rip;
    }

    public double[,] xmeanPredict(double[,] ripsMatrix, double[] meanx)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2] - meanx[index2];
      }
      return this.rip;
    }

    public double avg(int n, double[] x)
    {
      double num = 0.0;
      for (int index = 0; index < n; ++index)
        num += x[index];
      return num / (double) n;
    }

    public double[] avgRipsMatrix(double[,] ripsMatrix)
    {
      double[] numArray = new double[this.m];
      double[] x = new double[this.n];
      for (int index1 = 0; index1 < this.m; ++index1)
      {
        for (int index2 = 0; index2 < this.n; ++index2)
          x[index2] = ripsMatrix[index2, index1];
        numArray[index1] = this.avg(this.n, x);
      }
      return numArray;
    }

    public double[] ssRipsMatrix(double[,] ripsMatrix)
    {
      double[] numArray1 = new double[this.m];
      double[] numArray2 = this.avgRipsMatrix(ripsMatrix);
      double[] numArray3 = new double[this.n];
      double[] numArray4 = new double[this.m];
      for (int index1 = 0; index1 < this.m; ++index1)
      {
        for (int index2 = 0; index2 < this.n; ++index2)
          numArray3[index2] = (ripsMatrix[index2, index1] - numArray2[index1]) * (ripsMatrix[index2, index1] - numArray2[index1]);
        for (int index2 = 0; index2 < this.n; ++index2)
          numArray4[index1] += numArray3[index2];
        numArray1[index1] = Math.Sqrt(numArray4[index1] / (double) (this.n - 1));
      }
      return numArray1;
    }

    public double[,] norm(double[,] ripsMatrix)
    {
      double[] numArray1 = this.avgRipsMatrix(ripsMatrix);
      double[] numArray2 = this.ssRipsMatrix(ripsMatrix);
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          this.rip[index1, index2] = numArray2[index2] != 0.0 ? (ripsMatrix[index1, index2] - numArray1[index2]) / numArray2[index2] : ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] normPredict(double[,] ripsMatrix, double[] meanx, double[] ss)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          this.rip[index1, index2] = ss[index2] != 0.0 ? (ripsMatrix[index1, index2] - meanx[index2]) / ss[index2] : ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] smooth5(double[,] ripsMatrix)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 3; index2 < this.m - 4; ++index2)
          this.rip[index1, index2] = (-3.0 * ripsMatrix[index1, index2 - 2] + 12.0 * ripsMatrix[index1, index2 - 1] + 17.0 * ripsMatrix[index1, index2] + 12.0 * ripsMatrix[index1, index2 + 1] - 3.0 * ripsMatrix[index1, index2 + 2]) / 35.0;
        for (int index2 = 0; index2 < 3; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
        for (int index2 = this.m - 4; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] smooth7(double[,] ripsMatrix)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 4; index2 < this.m - 5; ++index2)
          this.rip[index1, index2] = (-2.0 * ripsMatrix[index1, index2 - 3] + 3.0 * ripsMatrix[index1, index2 - 2] + 6.0 * ripsMatrix[index1, index2 - 1] + 7.0 * ripsMatrix[index1, index2] + 6.0 * ripsMatrix[index1, index2 + 1] + 3.0 * ripsMatrix[index1, index2 + 2] - 2.0 * ripsMatrix[index1, index2 + 3]) / 21.0;
        for (int index2 = 0; index2 < 4; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
        for (int index2 = this.m - 5; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] smooth9(double[,] ripsMatrix)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 5; index2 < this.m - 6; ++index2)
          this.rip[index1, index2] = (-21.0 * ripsMatrix[index1, index2 - 4] + 14.0 * ripsMatrix[index1, index2 - 3] + 39.0 * ripsMatrix[index1, index2 - 2] + 54.0 * ripsMatrix[index1, index2 - 1] + 59.0 * ripsMatrix[index1, index2] + 54.0 * ripsMatrix[index1, index2 + 1] + 39.0 * ripsMatrix[index1, index2 + 2] + 14.0 * ripsMatrix[index1, index2 + 3] - 21.0 * ripsMatrix[index1, index2 + 4]) / 231.0;
        for (int index2 = 0; index2 < 5; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
        for (int index2 = this.m - 6; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] smooth11(double[,] ripsMatrix)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 6; index2 < this.m - 7; ++index2)
          this.rip[index1, index2] = (-36.0 * ripsMatrix[index1, index2 - 5] + 9.0 * ripsMatrix[index1, index2 - 4] + 44.0 * ripsMatrix[index1, index2 - 3] + 69.0 * ripsMatrix[index1, index2 - 2] + 84.0 * ripsMatrix[index1, index2 - 1] + 89.0 * ripsMatrix[index1, index2] + 84.0 * ripsMatrix[index1, index2 + 1] + 69.0 * ripsMatrix[index1, index2 + 2] + 44.0 * ripsMatrix[index1, index2 + 3] + 9.0 * ripsMatrix[index1, index2 + 4] - 36.0 * ripsMatrix[index1, index2 + 5]) / 429.0;
        for (int index2 = 0; index2 < 6; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
        for (int index2 = this.m - 7; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] smooth13(double[,] ripsMatrix)
    {
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 6; index2 < this.m - 7; ++index2)
          this.rip[index1, index2] = (-11.0 * ripsMatrix[index1, index2 - 6] + 0.0 * ripsMatrix[index1, index2 - 5] + 9.0 * ripsMatrix[index1, index2 - 4] + 16.0 * ripsMatrix[index1, index2 - 3] + 21.0 * ripsMatrix[index1, index2 - 2] + 24.0 * ripsMatrix[index1, index2 - 1] + 25.0 * ripsMatrix[index1, index2] + 24.0 * ripsMatrix[index1, index2 + 1] + 21.0 * ripsMatrix[index1, index2 + 2] + 16.0 * ripsMatrix[index1, index2 + 3] + 9.0 * ripsMatrix[index1, index2 + 4] + 0.0 * ripsMatrix[index1, index2 + 5] - 11.0 * ripsMatrix[index1, index2 + 6]) / 143.0;
        for (int index2 = 0; index2 < 6; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
        for (int index2 = this.m - 7; index2 < this.m; ++index2)
          this.rip[index1, index2] = ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] msc(double[,] ripsMatrix, out double[] meanx)
    {
      meanx = this.avgRipsMatrix(ripsMatrix);
      double num1 = this.avg(this.m, meanx);
      double[] numArray1 = new double[this.m];
      for (int index = 0; index < this.m; ++index)
        numArray1[index] = meanx[index] - num1;
      double num2 = 0.0;
      for (int index = 0; index < this.m; ++index)
        num2 += numArray1[index] * numArray1[index];
      if (num2 == 0.0)
      {
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            this.rip[index1, index2] = ripsMatrix[index1, index2];
        }
      }
      else
      {
        double[] numArray2 = new double[this.n];
        double[,] numArray3 = new double[this.n, this.m];
        double[] x = new double[this.m];
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            x[index2] = ripsMatrix[index1, index2];
          numArray2[index1] = this.avg(this.m, x);
          for (int index2 = 0; index2 < this.m; ++index2)
            numArray3[index1, index2] = ripsMatrix[index1, index2] - numArray2[index1];
        }
        double[] numArray4 = new double[this.n];
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            numArray4[index1] += numArray1[index2] * numArray3[index1, index2];
        }
        double[] numArray5 = new double[this.n];
        bool[] flagArray = new bool[this.n];
        for (int index = 0; index < this.n; ++index)
        {
          numArray5[index] = numArray4[index] / num2;
          if (Math.Abs(numArray5[index]) < 1E-58)
            flagArray[index] = true;
        }
        double[] numArray6 = new double[this.n];
        for (int index = 0; index < this.n; ++index)
          numArray6[index] = numArray2[index] - numArray5[index] * num1;
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          if (flagArray[index1])
          {
            for (int index2 = 0; index2 < this.m; ++index2)
              this.rip[index1, index2] = ripsMatrix[index1, index2];
          }
          else
          {
            for (int index2 = 0; index2 < this.m; ++index2)
              this.rip[index1, index2] = (ripsMatrix[index1, index2] - numArray6[index1]) / numArray5[index1];
          }
        }
      }
      return this.rip;
    }

    public double[,] mscPredict(double[,] ripsMatrix, double[] meanx)
    {
      double num1 = this.avg(this.m, meanx);
      double[] numArray1 = new double[this.m];
      for (int index = 0; index < this.m; ++index)
        numArray1[index] = meanx[index] - num1;
      double num2 = 0.0;
      for (int index = 0; index < this.m; ++index)
        num2 += numArray1[index] * numArray1[index];
      if (num2 == 0.0)
      {
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            this.rip[index1, index2] = ripsMatrix[index1, index2];
        }
      }
      else
      {
        double[] numArray2 = new double[this.n];
        double[,] numArray3 = new double[this.n, this.m];
        double[] x = new double[this.m];
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            x[index2] = ripsMatrix[index1, index2];
          numArray2[index1] = this.avg(this.m, x);
          for (int index2 = 0; index2 < this.m; ++index2)
            numArray3[index1, index2] = ripsMatrix[index1, index2] - numArray2[index1];
        }
        double[] numArray4 = new double[this.n];
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            numArray4[index1] += numArray1[index2] * numArray3[index1, index2];
        }
        double[] numArray5 = new double[this.n];
        bool[] flagArray = new bool[this.n];
        for (int index = 0; index < this.n; ++index)
        {
          numArray5[index] = numArray4[index] / num2;
          if (Math.Abs(numArray5[index]) < 1E-58)
            flagArray[index] = true;
        }
        double[] numArray6 = new double[this.n];
        for (int index = 0; index < this.n; ++index)
          numArray6[index] = numArray2[index] - numArray5[index] * num1;
        for (int index1 = 0; index1 < this.n; ++index1)
        {
          if (flagArray[index1])
          {
            for (int index2 = 0; index2 < this.m; ++index2)
              this.rip[index1, index2] = ripsMatrix[index1, index2];
          }
          else
          {
            for (int index2 = 0; index2 < this.m; ++index2)
              this.rip[index1, index2] = (ripsMatrix[index1, index2] - numArray6[index1]) / numArray5[index1];
          }
        }
      }
      return this.rip;
    }

    public double[,] snv(double[,] ripsMatrix)
    {
      double[] numArray1 = new double[this.n];
      double[,] numArray2 = new double[this.n, this.m];
      double[] x = new double[this.m];
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          x[index2] = ripsMatrix[index1, index2];
        numArray1[index1] = this.avg(this.m, x);
        for (int index2 = 0; index2 < this.m; ++index2)
          numArray2[index1, index2] = ripsMatrix[index1, index2] - numArray1[index1];
      }
      double[] numArray3 = new double[this.n];
      double[] numArray4 = new double[this.n];
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          numArray3[index1] += numArray2[index1, index2] * numArray2[index1, index2];
        numArray4[index1] = Math.Sqrt(numArray3[index1] / (double) (this.m - 1));
      }
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          this.rip[index1, index2] = Math.Abs(numArray4[index1]) >= 1E-58 ? numArray2[index1, index2] / numArray4[index1] : ripsMatrix[index1, index2];
      }
      return this.rip;
    }

    public double[,] snv_detrend(double[,] ripsMatrix)
    {
      double[,] numArray1 = this.snv(ripsMatrix);
      double[,] y = new double[this.m, 1];
      double[,] regressX = new double[this.m, 3];
      double[] numArray2 = new double[this.m];
      for (int index = 0; index < this.m; ++index)
      {
        regressX[index, 0] = 1.0;
        regressX[index, 1] = (double) (index + 1);
        regressX[index, 2] = (double) ((index + 1) * (index + 1));
      }
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          y[index2, 0] = numArray1[index1, index2];
        double[,] b;
        this.mlr(this.m, 3, regressX, y, out b);
        for (int index2 = 0; index2 < this.m; ++index2)
          numArray2[index2] = b[0, 0] + b[1, 0] * regressX[index2, 1] + b[2, 0] * regressX[index2, 1] * regressX[index2, 1];
        for (int index2 = 0; index2 < this.m; ++index2)
          this.rip[index1, index2] = numArray1[index1, index2] - numArray2[index2];
      }
      return this.rip;
    }

    public void min_max(int n, double[] x, out double min, out double max)
    {
      min = x[0];
      max = x[0];
      for (int index = 1; index < n; ++index)
      {
        if (x[index] < min)
          min = x[index];
        else if (x[index] > max)
          max = x[index];
      }
    }

    public bool[] rangeMinMax(double[,] ripsMatrix, out double[] minMatrix, out double[] maxMatrix)
    {
      minMatrix = new double[this.n];
      maxMatrix = new double[this.n];
      bool[] flagArray = new bool[this.n];
      double[] x = new double[this.m];
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          x[index2] = ripsMatrix[index1, index2];
      }
      for (int index = 0; index < this.n; ++index)
      {
        this.min_max(this.m, x, out minMatrix[index], out maxMatrix[index]);
        if (minMatrix[index] == maxMatrix[index])
          flagArray[index] = true;
      }
      return flagArray;
    }

    public double[,] rangescale(double[,] ripsMatrix, out double[] minMatrix, out double[] maxMatrix)
    {
      minMatrix = new double[this.n];
      maxMatrix = new double[this.n];
      double[] x = new double[this.m];
      for (int index1 = 0; index1 < this.n; ++index1)
      {
        for (int index2 = 0; index2 < this.m; ++index2)
          x[index2] = ripsMatrix[index1, index2];
        this.min_max(this.m, x, out minMatrix[index1], out maxMatrix[index1]);
        if (minMatrix[index1] == maxMatrix[index1])
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            this.rip[index1, index2] = ripsMatrix[index1, index2];
        }
        else
        {
          for (int index2 = 0; index2 < this.m; ++index2)
            this.rip[index1, index2] = (ripsMatrix[index1, index2] - minMatrix[index1]) / (maxMatrix[index1] - minMatrix[index1]) + 1E-09;
        }
      }
      return this.rip;
    }

    public ripsPreDeal()
    {
    }

    public void mlr(int n, int m, double[,] regressX, double[,] y, out double[,] b)
    {
      double[,] numArray = this.conMatrix(n, m, regressX);
      double[,] x1 = this.multiMatrix(m, n, n, m, numArray, regressX);
      b = new double[m, 1];
      double[,] rip;
      if (this.reverseMatrix(m, m, x1, out rip))
        return;
      double[,] x2 = this.multiMatrix(m, m, m, n, rip, numArray);
      b = this.multiMatrix(m, n, n, 1, x2, y);
    }

    public double mlrPrey(double[,] x, double[,] b)
    {
      double num = 0.0;
      for (int index = 0; index < x.GetLength(1); ++index)
        num += x[0, index] * b[index, 0];
      return num;
    }

    public void pca(int n, int m, double[,] regressX, int maxrank, out double[,] w, out double[,] t, out double[,] lamd)
    {
      double[,] numArray1 = new double[n, m];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[index1, index2] = regressX[index1, index2];
      }
      double[,] numArray2 = new double[1, n];
      double[,] numArray3 = new double[1, m];
      double[,] numArray4 = new double[1, m];
      double[,] numArray5 = new double[n, m];
      double[,] numArray6 = new double[n, 1];
      double[,] numArray7 = new double[m, 1];
      double[,] numArray8 = new double[n, 1];
      double[,] numArray9 = new double[n, 1];
      w = new double[maxrank, m];
      t = new double[n, maxrank];
      lamd = new double[1, maxrank];
      for (int index1 = 0; index1 < maxrank; ++index1)
      {
        for (int index2 = 0; index2 < n; ++index2)
          numArray9[index2, 0] = 1.0;
        double num1 = 0.0;
        for (int index2 = 0; index2 < 100; ++index2)
        {
          double[,] numArray10 = this.conMatrix(n, 1, numArray9);
          double num2 = this.mulRowCol(n, n, numArray10, numArray9);
          double[,] numArray11 = this.multiMatrix(1, n, n, m, numArray10, numArray1);
          if (num2 != 0.0)
          {
            for (int index3 = 0; index3 < m; ++index3)
              w[index1, index3] = numArray11[0, index3] / num2;
          }
          double d1 = 0.0;
          for (int index3 = 0; index3 < m; ++index3)
            d1 += w[index1, index3] * w[index1, index3];
          double num3 = Math.Sqrt(d1);
          if (num3 != 0.0)
          {
            for (int index3 = 0; index3 < m; ++index3)
            {
              w[index1, index3] = w[index1, index3] / num3;
              numArray3[0, index3] = w[index1, index3];
            }
          }
          double[,] numArray12 = this.conMatrix(1, m, numArray3);
          double num4 = this.mulRowCol(m, m, numArray3, numArray12);
          double[,] numArray13 = this.multiMatrix(n, m, m, 1, numArray1, numArray12);
          if (num4 != 0.0)
          {
            for (int index3 = 0; index3 < n; ++index3)
              numArray6[index3, 0] = numArray13[index3, 0] / num4;
          }
          double d2 = 0.0;
          for (int index3 = 0; index3 < n; ++index3)
            d2 += (numArray6[index3, 0] - numArray9[index3, 0]) * (numArray6[index3, 0] - numArray9[index3, 0]);
          double num5 = Math.Sqrt(d2);
          for (int index3 = 0; index3 < n; ++index3)
            numArray9[index3, 0] = numArray6[index3, 0];
          if (num5 != num1)
          {
            num1 = num5;
            if (num5 < 1E-15)
              break;
          }
          else
            break;
        }
        double[,] numArray14 = this.conMatrix(n, 1, numArray6);
        double num6 = this.mulRowCol(n, n, numArray14, numArray6);
        double[,] numArray15 = this.multiMatrix(1, n, n, m, numArray14, numArray1);
        if (num6 != 0.0)
        {
          for (int index2 = 0; index2 < m; ++index2)
          {
            w[index1, index2] = numArray15[0, index2] / num6;
            numArray3[0, index2] = w[index1, index2];
          }
        }
        lamd[0, index1] = num6;
        for (int index2 = 0; index2 < n; ++index2)
          t[index2, index1] = numArray6[index2, 0];
        double[,] y = this.multiMatrix(n, 1, 1, m, numArray6, numArray3);
        numArray1 = this.subMatrix(n, m, n, m, numArray1, y);
      }
    }

    public void updata(object sender, EventArgs e)
    {
      this.pgv.Value = this.value;
    }

    public void pcaProgress(int n, int m, double[,] regressX, int maxrank, out double[,] w, out double[,] t, out double[,] lamd, ProgressBar pg, int op)
    {
      this.value = op;
      this.pgv = pg;
      this.pgv.Invoke((Delegate) new EventHandler(this.updata), (object) this, (object) EventArgs.Empty);
      double[,] numArray1 = new double[n, m];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[index1, index2] = regressX[index1, index2];
      }
      double[,] numArray2 = new double[1, n];
      double[,] numArray3 = new double[1, m];
      double[,] numArray4 = new double[1, m];
      double[,] numArray5 = new double[n, m];
      double[,] numArray6 = new double[n, 1];
      double[,] numArray7 = new double[m, 1];
      double[,] numArray8 = new double[n, 1];
      double[,] numArray9 = new double[n, 1];
      w = new double[maxrank, m];
      t = new double[n, maxrank];
      lamd = new double[1, maxrank];
      for (int index1 = 0; index1 < maxrank; ++index1)
      {
        this.value = op + index1;
        this.pgv.Invoke((Delegate) new EventHandler(this.updata), (object) this, (object) EventArgs.Empty);
        for (int index2 = 0; index2 < n; ++index2)
          numArray9[index2, 0] = 1.0;
        double num1 = 0.0;
        for (int index2 = 0; index2 < 100; ++index2)
        {
          double[,] numArray10 = this.conMatrix(n, 1, numArray9);
          double num2 = this.mulRowCol(n, n, numArray10, numArray9);
          double[,] numArray11 = this.multiMatrix(1, n, n, m, numArray10, numArray1);
          if (num2 != 0.0)
          {
            for (int index3 = 0; index3 < m; ++index3)
              w[index1, index3] = numArray11[0, index3] / num2;
          }
          double d1 = 0.0;
          for (int index3 = 0; index3 < m; ++index3)
            d1 += w[index1, index3] * w[index1, index3];
          double num3 = Math.Sqrt(d1);
          if (num3 != 0.0)
          {
            for (int index3 = 0; index3 < m; ++index3)
            {
              w[index1, index3] = w[index1, index3] / num3;
              numArray3[0, index3] = w[index1, index3];
            }
          }
          double[,] numArray12 = this.conMatrix(1, m, numArray3);
          double num4 = this.mulRowCol(m, m, numArray3, numArray12);
          double[,] numArray13 = this.multiMatrix(n, m, m, 1, numArray1, numArray12);
          if (num4 != 0.0)
          {
            for (int index3 = 0; index3 < n; ++index3)
              numArray6[index3, 0] = numArray13[index3, 0] / num4;
          }
          double d2 = 0.0;
          for (int index3 = 0; index3 < n; ++index3)
            d2 += (numArray6[index3, 0] - numArray9[index3, 0]) * (numArray6[index3, 0] - numArray9[index3, 0]);
          double num5 = Math.Sqrt(d2);
          for (int index3 = 0; index3 < n; ++index3)
            numArray9[index3, 0] = numArray6[index3, 0];
          if (num5 != num1)
          {
            num1 = num5;
            if (num5 < 1E-15)
              break;
          }
          else
            break;
        }
        double[,] numArray14 = this.conMatrix(n, 1, numArray6);
        double num6 = this.mulRowCol(n, n, numArray14, numArray6);
        double[,] numArray15 = this.multiMatrix(1, n, n, m, numArray14, numArray1);
        if (num6 != 0.0)
        {
          for (int index2 = 0; index2 < m; ++index2)
          {
            w[index1, index2] = numArray15[0, index2] / num6;
            numArray3[0, index2] = w[index1, index2];
          }
        }
        lamd[0, index1] = num6;
        for (int index2 = 0; index2 < n; ++index2)
          t[index2, index1] = numArray6[index2, 0];
        double[,] y = this.multiMatrix(n, 1, 1, m, numArray6, numArray3);
        numArray1 = this.subMatrix(n, m, n, m, numArray1, y);
      }
    }

    public double[,] QualPcaScore(int n, int m, int f, double[,] x, double[,] w)
    {
      double[,] ripsMatrix = new double[f, m];
      double[,] numArray1 = new double[m, f];
      double[,] numArray2 = new double[1, f];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          ripsMatrix[index1, index2] = w[index1, index2];
      }
      double[,] y = this.conMatrix(f, m, ripsMatrix);
      return this.multiMatrix(1, m, m, f, x, y);
    }

    public double pcr(int n, int m, double[,] y, double[,] x, double[,] w, double[,] t, int f, out double[,] temtf, out double remainX, out double[,] xe, out double[,] b)
    {
      double[,] regressX = new double[n, f];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < n; ++index2)
          regressX[index2, index1] = t[index2, index1];
      }
      this.mlr(n, f, regressX, y, out b);
      double[,] numArray = new double[f, m];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray[index1, index2] = w[index1, index2];
      }
      double[,] y1 = this.conMatrix(f, m, numArray);
      temtf = this.multiMatrix(1, m, m, f, x, y1);
      double num = this.mulRowCol(f, f, temtf, b);
      xe = new double[1, m];
      xe = this.subMatrix(1, m, 1, m, x, this.multiMatrix(1, f, f, m, temtf, numArray));
      remainX = 0.0;
      for (int index = 0; index < m; ++index)
        remainX += xe[0, index] * xe[0, index];
      remainX = Math.Sqrt(remainX);
      return num;
    }

    public double pcrPrey(int n, int m, int f, int maxF, double[,] x, double[,] w, double[,] bp, out double[] remainx, out double[,] t)
    {
      double[,] numArray1 = new double[f, m];
      double[,] numArray2 = new double[m, maxF];
      double[,] c = new double[f, 1];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[index1, index2] = w[index1, index2];
        c[index1, 0] = bp[index1, 0];
      }
      double[,] y1 = this.conMatrix(f, m, numArray1);
      double[,] y2 = this.conMatrix(maxF, m, w);
      t = new double[1, maxF];
      t = this.multiMatrix(1, m, m, maxF, x, y2);
      double[,] numArray3 = new double[1, f];
      double[,] numArray4 = this.multiMatrix(1, m, m, f, x, y1);
      double num = this.mulRowCol(f, f, numArray4, c);
      remainx = new double[m];
      double[,] numArray5 = this.multiMatrix(1, f, f, m, numArray4, numArray1);
      for (int index = 0; index < m; ++index)
        remainx[index] = x[0, index] - numArray5[0, index];
      return num;
    }

    public void pls(int n, int m, int f, double[,] X, double[,] y, out double[,] p, out double[,] q, out double[,] w, out double[,] t, out double[,] u, out double[,] ex, out double[,] ey, out double[,] b)
    {
      double[,] numArray1 = new double[n, m];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[index1, index2] = X[index1, index2];
      }
      double[,] c = new double[n, 1];
      for (int index = 0; index < n; ++index)
        c[index, 0] = y[index, 0];
      p = new double[f, m];
      w = new double[f, m];
      q = new double[f, 1];
      t = new double[n, f];
      u = new double[n, f];
      b = new double[f, 1];
      ex = new double[n, m];
      double[,] numArray2 = new double[n, m];
      ey = new double[n, 1];
      double[,] numArray3 = new double[n, 1];
      double[,] numArray4 = new double[n, 1];
      double[,] numArray5 = new double[n, 1];
      double[,] numArray6 = new double[1, n];
      double[,] numArray7 = new double[1, n];
      double[,] numArray8 = new double[m, 1];
      double[,] numArray9 = new double[1, m];
      double[,] numArray10 = new double[n, 1];
      double[,] numArray11 = new double[1, m];
      double[,] y1 = new double[1, m];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < n; ++index2)
        {
          numArray3[index2, 0] = c[index2, 0];
          numArray4[index2, 0] = 1.0;
        }
        int num1 = 0;
        double num2 = 0.0;
        for (; num1 < 100; ++num1)
        {
          double[,] numArray12 = this.conMatrix(n, 1, numArray3);
          double num3 = this.mulRowCol(n, n, numArray12, numArray3);
          double[,] numArray13 = this.multiMatrix(1, n, n, m, numArray12, numArray1);
          if (num3 != 0.0)
          {
            for (int index2 = 0; index2 < m; ++index2)
              w[index1, index2] = numArray13[0, index2] / num3;
          }
          double d1 = 0.0;
          for (int index2 = 0; index2 < m; ++index2)
            d1 += w[index1, index2] * w[index1, index2];
          double num4 = Math.Sqrt(d1);
          if (num4 != 0.0)
          {
            for (int index2 = 0; index2 < m; ++index2)
            {
              w[index1, index2] = w[index1, index2] / num4;
              numArray11[0, index2] = w[index1, index2];
            }
          }
          double[,] numArray14 = this.conMatrix(1, m, numArray11);
          double num5 = this.mulRowCol(m, m, numArray11, numArray14);
          numArray3 = this.multiMatrix(n, m, m, 1, numArray1, numArray14);
          if (num5 != 0.0)
          {
            for (int index2 = 0; index2 < n; ++index2)
            {
              t[index2, index1] = numArray3[index2, 0] / num5;
              numArray5[index2, 0] = t[index2, index1];
            }
          }
          double[,] r = this.conMatrix(n, 1, numArray5);
          double num6 = this.mulRowCol(n, n, r, numArray5);
          double num7 = this.mulRowCol(n, n, r, c);
          if (num6 != 0.0)
            q[index1, 0] = num7 / num6;
          if (q[index1, 0] != 0.0)
          {
            for (int index2 = 0; index2 < n; ++index2)
            {
              u[index2, index1] = c[index2, 0] / q[index1, 0];
              numArray3[index2, 0] = u[index2, index1];
            }
          }
          double d2 = 0.0;
          for (int index2 = 0; index2 < n; ++index2)
            d2 += (numArray5[index2, 0] - numArray4[index2, 0]) * (numArray5[index2, 0] - numArray4[index2, 0]);
          double num8 = Math.Sqrt(d2);
          for (int index2 = 0; index2 < n; ++index2)
            numArray4[index2, 0] = numArray5[index2, 0];
          if (num8 != num2)
          {
            num2 = num8;
            if (num8 < 1E-15)
              break;
          }
          else
            break;
        }
        double[,] numArray15 = this.conMatrix(n, 1, numArray5);
        double num9 = this.mulRowCol(n, n, numArray15, numArray5);
        double[,] numArray16 = this.multiMatrix(1, n, n, m, numArray15, numArray1);
        if (num9 != 0.0)
        {
          for (int index2 = 0; index2 < m; ++index2)
            p[index1, index2] = numArray16[0, index2] / num9;
        }
        double d = 0.0;
        for (int index2 = 0; index2 < m; ++index2)
          d += p[index1, index2] * p[index1, index2];
        double num10 = Math.Sqrt(d);
        if (num10 != 0.0)
        {
          for (int index2 = 0; index2 < m; ++index2)
          {
            p[index1, index2] = p[index1, index2] / num10;
            y1[0, index2] = p[index1, index2];
          }
        }
        for (int index2 = 0; index2 < m; ++index2)
        {
          w[index1, index2] = w[index1, index2] * num10;
          numArray11[0, index2] = w[index1, index2];
        }
        for (int index2 = 0; index2 < n; ++index2)
        {
          t[index2, index1] = t[index2, index1] * num10;
          numArray5[index2, 0] = t[index2, index1];
        }
        double[,] r1 = this.conMatrix(n, 1, numArray3);
        double[,] r2 = this.conMatrix(n, 1, numArray5);
        double num11 = this.mulRowCol(n, n, r2, numArray5);
        double num12 = this.mulRowCol(n, n, r1, numArray5);
        if (num11 != 0.0)
          b[index1, 0] = num12 / num11;
        double[,] numArray17 = this.multiMatrix(n, 1, 1, m, numArray5, y1);
        for (int index2 = 0; index2 < n; ++index2)
        {
          for (int index3 = 0; index3 < m; ++index3)
            ex[index2, index3] = numArray1[index2, index3] - numArray17[index2, index3];
        }
        for (int index2 = 0; index2 < n; ++index2)
          ey[index2, 0] = c[index2, 0] - numArray5[index2, 0] * b[index1, 0] * q[index1, 0];
        numArray1 = ex;
        c = ey;
      }
    }

    public double plsPrey(int m, int f, double[,] x, double[,] w, double[,] p, double[,] q, double[,] b, out double[] th, out double[] remainX)
    {
      double num = 0.0;
      remainX = new double[m];
      th = new double[f];
      for (int index = 0; index < m; ++index)
        remainX[index] = x[0, index];
      for (int index1 = 0; index1 < f; ++index1)
      {
        th[index1] = 0.0;
        for (int index2 = 0; index2 < m; ++index2)
          th[index1] += remainX[index2] * w[index1, index2];
        for (int index2 = 0; index2 < m; ++index2)
          remainX[index2] = remainX[index2] - th[index1] * p[index1, index2];
        num += b[index1, 0] * th[index1] * q[index1, 0];
      }
      return num;
    }

    public double xplsPrey(int m, int f, int maxF, double[,] x, double[,] w, double[,] p, double[,] q, double[,] b, out double[] th, out double[] remainX)
    {
      double num = 0.0;
      remainX = new double[m];
      th = new double[maxF];
      double[] numArray = new double[f];
      for (int index = 0; index < m; ++index)
        remainX[index] = x[0, index];
      for (int index1 = 0; index1 < maxF; ++index1)
      {
        th[index1] = 0.0;
        for (int index2 = 0; index2 < m; ++index2)
          th[index1] += remainX[index2] * w[index1, index2];
        for (int index2 = 0; index2 < m; ++index2)
          remainX[index2] = remainX[index2] - th[index1] * p[index1, index2];
      }
      for (int index = 0; index < m; ++index)
        remainX[index] = x[0, index];
      for (int index1 = 0; index1 < f; ++index1)
      {
        numArray[index1] = 0.0;
        for (int index2 = 0; index2 < m; ++index2)
          numArray[index1] += remainX[index2] * w[index1, index2];
        for (int index2 = 0; index2 < m; ++index2)
          remainX[index2] = remainX[index2] - numArray[index1] * p[index1, index2];
        num += b[index1, 0] * numArray[index1] * q[index1, 0];
      }
      return num;
    }

    public void coeRegress(int n, double[] x, double[] y, out double a, out double b)
    {
      a = 0.0;
      b = 1.0;
      double num1 = this.avg(n, x);
      double[] numArray1 = new double[n];
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = this.avg(n, y);
      double[] numArray2 = new double[n];
      for (int index = 0; index < n; ++index)
      {
        numArray1[index] = x[index] - num1;
        numArray2[index] = y[index] - num4;
        num2 += numArray1[index] * numArray2[index];
        num3 += numArray1[index] * numArray1[index];
      }
      if (num3 != 0.0)
        b = num2 / num3;
      a = num4 - b * num1;
    }

    public double[] relation(int n, int m, double[,] ripsMatrix, double[] y)
    {
      double[] numArray1 = new double[m];
      double[] x = new double[n];
      double[] numArray2 = new double[m];
      double[,] numArray3 = new double[n, m];
      double[] numArray4 = new double[m];
      double[] numArray5 = new double[m];
      double num1 = this.avg(n, y);
      double[] numArray6 = new double[n];
      double num2 = 0.0;
      for (int index = 0; index < n; ++index)
      {
        numArray6[index] = y[index] - num1;
        num2 += (y[index] - num1) * (y[index] - num1);
      }
      for (int index1 = 0; index1 < m; ++index1)
      {
        for (int index2 = 0; index2 < n; ++index2)
          x[index2] = ripsMatrix[index2, index1];
        numArray2[index1] = this.avg(n, x);
        for (int index2 = 0; index2 < n; ++index2)
          numArray3[index2, index1] = ripsMatrix[index2, index1] - numArray2[index1];
        for (int index2 = 0; index2 < n; ++index2)
        {
          numArray4[index1] += numArray3[index2, index1] * numArray6[index2];
          numArray5[index1] += numArray3[index2, index1] * numArray3[index2, index1];
        }
        if (Math.Sqrt(numArray5[index1] * num2) != 0.0)
          numArray1[index1] = numArray4[index1] / Math.Sqrt(numArray5[index1] * num2);
      }
      return numArray1;
    }

    public int rankMatrix(int n, int m, double[,] x)
    {
      return 0;
    }

    public bool reverseMatrix(int n, int m, double[,] x, out double[,] rip)
    {
      rip = x;
      bool flag = false;
      if (n != m)
        return true;
      int[] numArray1 = new int[n];
      int[] numArray2 = new int[n];
      double[] numArray3 = new double[n];
      for (int index1 = 0; index1 < n; ++index1)
      {
        double num1 = 0.0;
        for (int index2 = index1; index2 < n; ++index2)
        {
          for (int index3 = index1; index3 < n; ++index3)
          {
            double num2 = Math.Abs(rip[index2, index3]);
            if (num2 > num1)
            {
              num1 = num2;
              numArray1[index1] = index2;
              numArray2[index1] = index3;
            }
          }
        }
        if (num1 + 1.0 == 1.0)
          return true;
        if (numArray1[index1] != index1)
        {
          for (int index2 = 0; index2 < n; ++index2)
          {
            numArray3[index2] = rip[index1, index2];
            rip[index1, index2] = rip[numArray1[index1], index2];
            rip[numArray1[index1], index2] = numArray3[index2];
          }
        }
        if (numArray2[index1] != index1)
        {
          for (int index2 = 0; index2 < n; ++index2)
          {
            numArray3[index2] = rip[index2, index1];
            rip[index2, index1] = rip[index2, numArray2[index1]];
            rip[index2, numArray2[index1]] = numArray3[index2];
          }
        }
        rip[index1, index1] = 1.0 / rip[index1, index1];
        for (int index2 = 0; index2 < n; ++index2)
        {
          if (index2 != index1)
            rip[index1, index2] = rip[index1, index2] * rip[index1, index1];
        }
        for (int index2 = 0; index2 < n; ++index2)
        {
          if (index2 != index1)
          {
            for (int index3 = 0; index3 < n; ++index3)
            {
              if (index3 != index1)
                rip[index2, index3] = rip[index2, index3] - rip[index2, index1] * rip[index1, index3];
            }
          }
        }
        for (int index2 = 0; index2 < n; ++index2)
        {
          if (index2 != index1)
            rip[index2, index1] = -(rip[index2, index1] * rip[index1, index1]);
        }
      }
      for (int index1 = n - 1; index1 >= 0; --index1)
      {
        if (numArray2[index1] != index1)
        {
          for (int index2 = 0; index2 < n; ++index2)
          {
            numArray3[index2] = rip[index1, index2];
            rip[index1, index2] = rip[numArray2[index1], index2];
            rip[numArray2[index1], index2] = numArray3[index2];
          }
        }
        if (numArray1[index1] != index1)
        {
          for (int index2 = 0; index2 < n; ++index2)
          {
            numArray3[index2] = rip[index2, index1];
            rip[index2, index1] = rip[index2, numArray1[index1]];
            rip[index2, numArray1[index1]] = numArray3[index2];
          }
        }
      }
      return flag;
    }

    public double[,] reverseMatrix1(int n, int m, double[,] x, out bool flag)
    {
      double[,] numArray1 = new double[n, m];
      flag = false;
      if (n != m)
      {
        flag = true;
      }
      else
      {
        double[] numArray2 = new double[2 * m];
        double[,] numArray3 = new double[n, 2 * m];
        for (int index1 = 0; index1 < n; ++index1)
        {
          for (int index2 = 0; index2 < m; ++index2)
            numArray3[index1, index2] = x[index1, index2];
          for (int index2 = m; index2 < 2 * m; ++index2)
          {
            if (index1 == index2 - m)
              numArray3[index1, index2] = 1.0;
          }
        }
        for (int index1 = 0; index1 < n - 1; ++index1)
        {
          int num1 = 0;
          if (numArray3[index1, index1] == 0.0)
          {
            for (int index2 = index1 + 1; index2 < n; ++index2)
            {
              if (numArray3[index2, index1] != 0.0)
              {
                for (int index3 = 0; index3 < 2 * m; ++index3)
                {
                  numArray2[index3] = numArray3[index2, index3];
                  numArray3[index2, index3] = numArray3[index1, index3];
                  numArray3[index1, index3] = numArray2[index3];
                }
                break;
              }
              ++num1;
            }
            if (num1 == n - 1 - index1)
            {
              flag = true;
              break;
            }
          }
          double num2 = numArray3[index1, index1];
          if (num2 != 1.0)
          {
            for (int index2 = 0; index2 < 2 * m; ++index2)
              numArray3[index1, index2] = numArray3[index1, index2] / num2;
          }
          for (int index2 = index1 + 1; index2 < n; ++index2)
          {
            if (numArray3[index2, index1] != 0.0)
            {
              double num3 = numArray3[index2, index1];
              for (int index3 = index1; index3 < 2 * m; ++index3)
                numArray3[index2, index3] = numArray3[index2, index3] - num3 * numArray3[index1, index3];
            }
          }
        }
        double num4 = numArray3[n - 1, m - 1];
        if (num4 != 0.0)
        {
          for (int index = m - 1; index < 2 * m; ++index)
            numArray3[n - 1, index] = numArray3[n - 1, index] / num4;
        }
        else
          flag = true;
        for (int index1 = n - 1; index1 > 0; --index1)
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
          {
            if (numArray3[index2, index1] != 0.0)
            {
              double num1 = numArray3[index2, index1];
              for (int index3 = index1; index3 < 2 * m; ++index3)
                numArray3[index2, index3] = numArray3[index2, index3] - num1 * numArray3[index1, index3];
            }
          }
        }
        for (int index1 = 0; index1 < n; ++index1)
        {
          for (int index2 = m; index2 < 2 * m; ++index2)
            numArray1[index1, index2 - m] = numArray3[index1, index2];
        }
      }
      return numArray1;
    }

    public double[,] multiMatrix(int row1, int col1, int row2, int col2, double[,] x, double[,] y)
    {
      double[,] numArray = new double[row1, col2];
      if (col1 == row2)
      {
        for (int index1 = 0; index1 < row1; ++index1)
        {
          for (int index2 = 0; index2 < col2; ++index2)
          {
            numArray[index1, index2] = 0.0;
            for (int index3 = 0; index3 < row2; ++index3)
              numArray[index1, index2] += x[index1, index3] * y[index3, index2];
          }
        }
      }
      return numArray;
    }

    public double[,] mulitipleMatrix(int n, int m, double[,] x, double num)
    {
      double[,] numArray = new double[n, m];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray[index1, index2] = num * x[index1, index2];
      }
      return numArray;
    }

    public double[,] addMatrix(int row1, int col1, int row2, int col2, double[,] x, double[,] y)
    {
      double[,] numArray = new double[row1, col1];
      if (row1 == row2 || col1 == col2)
      {
        for (int index1 = 0; index1 < row1; ++index1)
        {
          for (int index2 = 0; index2 < col1; ++index2)
            numArray[index1, index2] = x[index1, index2] + y[index1, index2];
        }
      }
      return numArray;
    }

    public double[,] subMatrix(int row1, int col1, int row2, int col2, double[,] x, double[,] y)
    {
      double[,] numArray = new double[row1, col1];
      if (row1 == row2 || col1 == col2)
      {
        for (int index1 = 0; index1 < row1; ++index1)
        {
          for (int index2 = 0; index2 < col1; ++index2)
            numArray[index1, index2] = x[index1, index2] - y[index1, index2];
        }
      }
      return numArray;
    }

    public double mulRowCol(int col, int row, double[,] r, double[,] c)
    {
      double num = 0.0;
      if (row == col)
      {
        for (int index = 0; index < row; ++index)
          num += r[0, index] * c[index, 0];
      }
      return num;
    }

    public double[,] conMatrix(int n, int m, double[,] ripsMatrix)
    {
      double[,] numArray = new double[m, n];
      for (int index1 = 0; index1 < m; ++index1)
      {
        for (int index2 = 0; index2 < n; ++index2)
          numArray[index1, index2] = ripsMatrix[index2, index1];
      }
      return numArray;
    }

    public void maDistance(int n, int f, double[,] t, out double[,] rm, out double[] md)
    {
      md = new double[n];
      rm = new double[f, f];
      double[,] numArray1 = new double[n, f];
      double[,] numArray2 = new double[f, n];
      double[,] numArray3 = new double[f, f];
      double[,] numArray4 = new double[1, f];
      double[,] numArray5 = new double[f, 1];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          numArray1[index1, index2] = t[index1, index2];
      }
      double[,] x1 = this.conMatrix(n, f, numArray1);
      double[,] x2 = this.multiMatrix(f, n, n, f, x1, numArray1);
      if (this.reverseMatrix(f, f, x2, out rm))
        return;
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          numArray4[0, index2] = numArray1[index1, index2];
        double[,] c = this.conMatrix(1, f, numArray4);
        numArray4 = this.multiMatrix(1, f, f, f, numArray4, rm);
        md[index1] = this.mulRowCol(f, f, numArray4, c);
      }
    }

    public void ripsRemain(int n, int m, double[,] regressX, int f, double[,] w, double[,] t, out double[] rmssr)
    {
      rmssr = new double[n];
      double[,] x = new double[n, f];
      double[,] numArray1 = new double[1, f];
      double[,] y1 = new double[f, m];
      double[,] numArray2 = new double[n, m];
      double[,] numArray3 = new double[1, m];
      double[,] numArray4 = new double[m, 1];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          x[index1, index2] = t[index1, index2];
      }
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          y1[index1, index2] = w[index1, index2];
      }
      double[,] y2 = this.multiMatrix(n, f, f, m, x, y1);
      double[,] numArray5 = this.subMatrix(n, m, n, m, regressX, y2);
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray3[0, index2] = numArray5[index1, index2];
        double[,] c = this.conMatrix(1, m, numArray3);
        rmssr[index1] = Math.Sqrt(this.mulRowCol(m, m, numArray3, c));
      }
    }

    public void ripsRemainPca(int n, int m, double[,] regressX, int f, double[,] w, double[,] t, out double[,] XP, out double[] rmssr)
    {
      rmssr = new double[n];
      XP = new double[n, m];
      double[,] x = new double[n, f];
      double[,] numArray1 = new double[1, f];
      double[,] y = new double[f, m];
      double[,] numArray2 = new double[1, m];
      double[,] numArray3 = new double[m, 1];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          x[index1, index2] = t[index1, index2];
      }
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          y[index1, index2] = w[index1, index2];
      }
      XP = this.multiMatrix(n, f, f, m, x, y);
      XP = this.subMatrix(n, m, n, m, regressX, XP);
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray2[0, index2] = XP[index1, index2];
        double[,] c = this.conMatrix(1, m, numArray2);
        rmssr[index1] = Math.Sqrt(this.mulRowCol(m, m, numArray2, c));
      }
    }

    public void ripsRemain1(int n, int m, double[,] XP, int f, out double[] rmssr)
    {
      double[,] numArray1 = new double[1, m];
      double[,] numArray2 = new double[1, m];
      rmssr = new double[n];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[0, index2] = XP[index1, index2];
        double[,] c = this.conMatrix(1, m, numArray1);
        rmssr[index1] = Math.Sqrt(this.mulRowCol(m, m, numArray1, c));
      }
    }

    public void ripsRemainPLs(int n, int m, double[,] remainEx, out double[] sr)
    {
      sr = new double[n];
      for (int index1 = 0; index1 < n; ++index1)
      {
        sr[index1] = 0.0;
        for (int index2 = 0; index2 < m; ++index2)
          sr[index1] += remainEx[index1, index2] * remainEx[index1, index2];
        sr[index1] = Math.Sqrt(sr[index1]);
      }
    }

    public double nearestDistance(int n, int f, double[,] t)
    {
      double[,] numArray1 = new double[n, f];
      double[,] numArray2 = new double[1, f];
      double[,] numArray3 = new double[n, n];
      double[] numArray4 = new double[n];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          numArray1[index1, index2] = t[index1, index2];
      }
      for (int index1 = 0; index1 < n; ++index1)
      {
        numArray3[index1, index1] = 1E+130;
        for (int index2 = index1 + 1; index2 < n; ++index2)
        {
          for (int index3 = 0; index3 < f; ++index3)
            numArray3[index1, index2] += (numArray1[index1, index3] - numArray1[index2, index3]) * (numArray1[index1, index3] - numArray1[index2, index3]);
          numArray3[index1, index2] = Math.Sqrt(numArray3[index1, index2]);
          numArray3[index2, index1] = numArray3[index1, index2];
        }
      }
      for (int index1 = 0; index1 < n; ++index1)
      {
        numArray4[index1] = numArray3[index1, 0];
        for (int index2 = 1; index2 < n; ++index2)
        {
          if (index1 != index2 && numArray3[index1, index2] < numArray4[index1])
            numArray4[index1] = numArray3[index1, index2];
        }
      }
      double num = numArray4[0];
      for (int index = 1; index < n; ++index)
      {
        if (numArray4[index] > num)
          num = numArray4[index];
      }
      return num;
    }

    public double xmaDistance(int f, double[,] t, double[,] reverseM)
    {
      double[,] y = new double[f, f];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          y[index1, index2] = reverseM[index1, index2];
      }
      double[,] r = this.multiMatrix(1, f, f, f, t, y);
      double[,] c = this.conMatrix(1, f, t);
      return this.mulRowCol(f, f, r, c);
    }

    public double xma(int n, int f, double[,] t, double[,] trevise)
    {
      double[,] numArray1 = new double[n, f];
      double[,] numArray2 = new double[f, n];
      double[,] numArray3 = new double[f, f];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          numArray1[index1, index2] = trevise[index1, index2];
      }
      double[,] numArray4 = new double[1, f];
      for (int index = 0; index < f; ++index)
        numArray4[0, index] = t[0, index];
      double[,] x1 = this.conMatrix(n, f, numArray1);
      double[,] x2 = this.multiMatrix(f, n, n, f, x1, numArray1);
      double[,] rip;
      this.reverseMatrix(f, f, x2, out rip);
      double[,] r = this.multiMatrix(1, f, f, f, numArray4, rip);
      double[,] c = this.conMatrix(1, f, numArray4);
      return this.mulRowCol(f, f, r, c);
    }

    public double xremainDistanceOfPcr(int f, int m, double[,] x, double[,] t, double[,] w)
    {
      double[,] numArray1 = new double[f, m];
      for (int index1 = 0; index1 < f; ++index1)
      {
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[index1, index2] = w[index1, index2];
      }
      double[,] numArray2 = new double[m, f];
      double[,] y = this.conMatrix(f, m, numArray1);
      double[,] numArray3 = new double[1, f];
      double[] numArray4 = new double[m];
      for (int index = 0; index < m; ++index)
        numArray4[index] = x[0, index];
      double[,] x1 = this.multiMatrix(1, m, m, f, x, y);
      double[,] numArray5 = this.multiMatrix(1, f, f, m, x1, numArray1);
      for (int index = 0; index < m; ++index)
        numArray4[index] = numArray4[index] - numArray5[0, index];
      double d = 0.0;
      for (int index = 0; index < m; ++index)
        d += numArray4[index] * numArray4[index];
      return Math.Sqrt(d);
    }

    public double xremainDistanceOfPls(int f, int m, double[,] x, double[,] w, double[,] p)
    {
      double[] numArray1 = new double[m];
      double[] numArray2 = new double[f];
      for (int index = 0; index < m; ++index)
        numArray1[index] = x[0, index];
      for (int index1 = 0; index1 < f; ++index1)
      {
        numArray2[index1] = 0.0;
        for (int index2 = 0; index2 < m; ++index2)
          numArray2[index1] += numArray1[index2] * w[index1, index2];
        for (int index2 = 0; index2 < m; ++index2)
          numArray1[index2] = numArray1[index2] - numArray2[index1] * p[index1, index2];
      }
      double d = 0.0;
      for (int index = 0; index < m; ++index)
        d += numArray1[index] * numArray1[index];
      return Math.Sqrt(d);
    }

    public int xnearDistance(int n, int f, double[,] t, double[,] tf, double nnd)
    {
      double[] numArray1 = new double[n];
      double[,] numArray2 = new double[n, f];
      for (int index1 = 0; index1 < n; ++index1)
      {
        for (int index2 = 0; index2 < f; ++index2)
          numArray2[index1, index2] = tf[index1, index2];
      }
      double[,] numArray3 = new double[1, f];
      for (int index = 0; index < f; ++index)
        numArray3[0, index] = t[0, index];
      for (int index1 = 0; index1 < n; ++index1)
      {
        numArray1[index1] = 0.0;
        for (int index2 = 0; index2 < f; ++index2)
          numArray1[index1] += (numArray3[0, index2] - numArray2[index1, index2]) * (numArray3[0, index2] - numArray2[index1, index2]);
        numArray1[index1] = Math.Sqrt(numArray1[index1]);
      }
      int num = 0;
      for (int index = 0; index < n; ++index)
      {
        if (numArray1[index] < nnd)
          ++num;
      }
      return num;
    }
  }
}
