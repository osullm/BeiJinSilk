// Decompiled with JetBrains decompiler
// Type: JSDU.Subspace
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using MatrixLibrary;
using System;
using System.Windows.Forms;

namespace JSDU
{
  internal class Subspace
  {
    public void GetSubspaceAngle(double[,] A, double[,] B, ref double theta1)
    {
      int sum1 = 0;
      int sum2 = 0;
      double[,] numArray1 = this.orth(A, out sum1);
      double[,] numArray2 = this.orth(B, out sum2);
      numArray1.GetLength(0);
      numArray2.GetLength(0);
      if (numArray1.GetLength(1) < numArray2.GetLength(1))
      {
        double[,] numArray3 = (double[,]) numArray1.Clone();
        numArray1 = numArray2;
        numArray2 = numArray3;
      }
      int length1 = numArray1.GetLength(1);
      int length2 = numArray2.GetLength(1);
      int length3 = numArray1.GetLength(0);
      int length4 = numArray2.GetLength(0);
      Matrix Mat1 = new Matrix(length3, 1);
      Matrix Mat2 = new Matrix(length4, length2);
      for (int index1 = 0; index1 < length4; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
          Mat2[index1, index2] = numArray2[index1, index2];
      }
      try
      {
        for (int index1 = 0; index1 < length1; ++index1)
        {
          for (int index2 = 0; index2 < length3; ++index2)
            Mat1[index2, 0] = numArray1[index2, index1];
          Mat2 -= Mat1 * (Matrix.Transpose(Mat1) * Mat2);
        }
        double[,] toArray = (Matrix.Transpose(Mat2) * Mat2).toArray;
        int length5 = toArray.GetLength(1);
        double[,] eigenvector = new double[length5, length5];
        double[] eigenvalue = new double[length5];
        double d;
        if (this.GetEigenValueAndEigenVector((double[,]) toArray.Clone(), length5, ref eigenvalue, ref eigenvector))
        {
          d = Math.Sqrt(eigenvalue[0]);
        }
        else
        {
          int num = (int) MessageBox.Show("求取特征值与特征向量失败，请检查输入的子空间矩阵");
          d = 2.0;
        }
        if (d <= 1.0)
          theta1 = Math.Asin(d);
        else
          theta1 = Math.Asin(1.0);
      }
      catch
      {
        int num = (int) MessageBox.Show("输入矩阵有误，计算出现错误，请检查");
      }
    }

    public double[,] orth(double[,] mat, out int sum)
    {
      int length1 = mat.GetLength(0);
      int length2 = mat.GetLength(1);
      Matrix Mat = new Matrix(length1, length2);
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
          Mat[index1, index2] = mat[index1, index2];
      }
      Matrix matrix1 = Matrix.Transpose(Mat) * Mat;
      double[,] eigenvector = new double[length2, length2];
      double[] eigenvalue = new double[length2];
      double[,] numArray = new double[length2, length2];
      for (int index1 = 0; index1 < length2; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
          numArray[index1, index2] = matrix1[index1, index2];
      }
      if (!this.GetEigenValueAndEigenVector((double[,]) numArray.Clone(), length2, ref eigenvalue, ref eigenvector))
      {
        int num = (int) MessageBox.Show("特征值求取有误");
        Application.Exit();
      }
      Matrix matrix2 = new Matrix(length2, length2);
      for (int index1 = 0; index1 < length2; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
          matrix2[index1, index2] = eigenvector[index1, index2];
      }
      Matrix matrix3 = new Matrix(length2, length2);
      for (int index1 = 0; index1 < length2; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
        {
          int num = index1 != index2 ? 1 : (eigenvalue[index1] <= 1E-05 ? 1 : 0);
          matrix3[index1, index2] = num != 0 ? 0.0 : 1.0 / Math.Sqrt(eigenvalue[index1]);
        }
      }
      int num1 = 0;
      for (int index = 0; index < length2 && eigenvalue[index] > 1E-05; ++index)
        ++num1;
      sum = num1;
      Matrix matrix4 = new Matrix(length2, num1);
      Matrix matrix5 = new Matrix(num1, num1);
      for (int index1 = 0; index1 < length2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          matrix4[index1, index2] = eigenvector[index1, index2];
      }
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          matrix5[index1, index2] = matrix3[index1, index2];
      }
      return (Mat * matrix4 * matrix5).toArray;
    }

    public bool GetEigenValueAndEigenVector(double[,] data, int num, ref double[] eigenvalue, ref double[,] eigenvector)
    {
      try
      {
        double[,] numArray1 = data;
        double[,] numArray2 = new double[num, num];
        for (int index1 = 0; index1 < num; ++index1)
        {
          for (int index2 = 0; index2 < num; ++index2)
            numArray2[index1, index2] = index1 != index2 ? 0.0 : 1.0;
        }
        double[] numArray3 = new double[num];
        for (int index = 0; index < num; ++index)
          numArray3[index] = 0.0;
        double num1 = 1E-06;
        int num2 = 10;
        int length = num;
        for (int index1 = 0; index1 < num2; ++index1)
        {
          double num3 = 0.0;
          for (int index2 = 0; index2 < length - 1; ++index2)
          {
            for (int index3 = index2 + 1; index3 < length; ++index3)
            {
              if (Math.Abs(numArray1[index2, index3]) > num3)
                num3 = Math.Abs(numArray1[index2, index3]);
              if (Math.Abs(numArray1[index2, index3]) > num1)
              {
                double num4 = 0.5 * (numArray1[index3, index3] - numArray1[index2, index2]) / numArray1[index2, index3];
                double num5 = num4 < 0.0 ? -num4 - Math.Sqrt(1.0 + num4 * num4) : -num4 + Math.Sqrt(1.0 + num4 * num4);
                double num6 = 1.0 / Math.Sqrt(1.0 + num5 * num5);
                double num7 = num5 * num6;
                for (int index4 = 0; index4 < length; ++index4)
                {
                  double num8 = numArray1[index2, index4];
                  double num9 = numArray1[index3, index4];
                  numArray1[index2, index4] = num6 * num8 - num7 * num9;
                  numArray1[index3, index4] = num7 * num8 + num6 * num9;
                }
                for (int index4 = 0; index4 < length; ++index4)
                {
                  double num8 = numArray1[index4, index2];
                  double num9 = numArray1[index4, index3];
                  numArray1[index4, index2] = num6 * num8 - num7 * num9;
                  numArray1[index4, index3] = num7 * num8 + num6 * num9;
                }
                for (int index4 = 0; index4 < length; ++index4)
                {
                  double num8 = numArray2[index4, index2];
                  double num9 = numArray2[index4, index3];
                  numArray2[index4, index2] = num6 * num8 - num7 * num9;
                  numArray2[index4, index3] = num7 * num8 + num6 * num9;
                }
              }
            }
          }
          if (num3 < num1)
            break;
        }
        for (int index = 0; index < length; ++index)
          numArray3[index] = numArray1[index, index];
        double[] numArray4 = new double[length];
        for (int index1 = 1; index1 < length; ++index1)
        {
          int index2 = index1;
          double num3 = numArray3[index1];
          for (int index3 = 0; index3 < length; ++index3)
            numArray4[index3] = numArray2[index3, index1];
          for (; index2 > 0 && numArray3[index2 - 1] < num3; --index2)
          {
            numArray3[index2] = numArray3[index2 - 1];
            for (int index3 = 0; index3 < length; ++index3)
              numArray2[index3, index2] = numArray2[index3, index2 - 1];
          }
          numArray3[index2] = num3;
          for (int index3 = 0; index3 < length; ++index3)
            numArray2[index3, index2] = numArray4[index3];
        }
        for (int index1 = 0; index1 < num; ++index1)
        {
          for (int index2 = 0; index2 < num; ++index2)
            eigenvector[index1, index2] = numArray2[index1, index2];
        }
        for (int index = 0; index < num; ++index)
          eigenvalue[index] = numArray3[index];
        return true;
      }
      catch
      {
        int num1 = (int) MessageBox.Show("计算特征值特征向量过程出现错误");
        return false;
      }
    }
  }
}
