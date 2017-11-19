// Decompiled with JetBrains decompiler
// Type: MatrixLibrary.Matrix
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;

namespace MatrixLibrary
{
  public class Matrix
  {
    private double[,] in_Mat;

    public Matrix(int noRows, int noCols)
    {
      this.in_Mat = new double[noRows, noCols];
    }

    public Matrix(double[,] Mat)
    {
      this.in_Mat = (double[,]) Mat.Clone();
    }

    public double this[int Row, int Col]
    {
      get
      {
        return this.in_Mat[Row, Col];
      }
      set
      {
        this.in_Mat[Row, Col] = value;
      }
    }

    public int NoRows
    {
      get
      {
        return this.in_Mat.GetUpperBound(0) + 1;
      }
      set
      {
        this.in_Mat = new double[value, this.in_Mat.GetUpperBound(0)];
      }
    }

    public int NoCols
    {
      get
      {
        return this.in_Mat.GetUpperBound(1) + 1;
      }
      set
      {
        this.in_Mat = new double[this.in_Mat.GetUpperBound(0), value];
      }
    }

    public double[,] toArray
    {
      get
      {
        return this.in_Mat;
      }
    }

    private static void Find_R_C(double[] Mat, out int Row)
    {
      Row = Mat.GetUpperBound(0);
    }

    private static void Find_R_C(double[,] Mat, out int Row, out int Col)
    {
      Row = Mat.GetUpperBound(0);
      Col = Mat.GetUpperBound(1);
    }

    public static double[,] OneD_2_TwoD(double[] Mat)
    {
      int Row;
      try
      {
        Matrix.Find_R_C(Mat, out Row);
      }
      catch
      {
        throw new MatrixNullException();
      }
      double[,] numArray = new double[Row + 1, 1];
      for (int index = 0; index <= Row; ++index)
        numArray[index, 0] = Mat[index];
      return numArray;
    }

    public static double[] TwoD_2_OneD(double[,] Mat)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Col != 0)
        throw new MatrixDimensionException();
      double[] numArray = new double[Row + 1];
      for (int index = 0; index <= Row; ++index)
        numArray[index] = Mat[index, 0];
      return numArray;
    }

    public static double[,] Identity(int n)
    {
      double[,] numArray = new double[n, n];
      for (int index = 0; index < n; ++index)
        numArray[index, index] = 1.0;
      return numArray;
    }

    public static double[,] Add(double[,] Mat1, double[,] Mat2)
    {
      int Row1;
      int Col1;
      int Row2;
      int Col2;
      try
      {
        Matrix.Find_R_C(Mat1, out Row1, out Col1);
        Matrix.Find_R_C(Mat2, out Row2, out Col2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != Row2 || Col1 != Col2)
        throw new MatrixDimensionException();
      double[,] numArray = new double[Row1 + 1, Col1 + 1];
      for (int index1 = 0; index1 <= Row1; ++index1)
      {
        for (int index2 = 0; index2 <= Col1; ++index2)
          numArray[index1, index2] = Mat1[index1, index2] + Mat2[index1, index2];
      }
      return numArray;
    }

    public static Matrix Add(Matrix Mat1, Matrix Mat2)
    {
      return new Matrix(Matrix.Add(Mat1.in_Mat, Mat2.in_Mat));
    }

    public static Matrix operator +(Matrix Mat1, Matrix Mat2)
    {
      return new Matrix(Matrix.Add(Mat1.in_Mat, Mat2.in_Mat));
    }

    public static double[,] Subtract(double[,] Mat1, double[,] Mat2)
    {
      int Row1;
      int Col1;
      int Row2;
      int Col2;
      try
      {
        Matrix.Find_R_C(Mat1, out Row1, out Col1);
        Matrix.Find_R_C(Mat2, out Row2, out Col2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != Row2 || Col1 != Col2)
        throw new MatrixDimensionException();
      double[,] numArray = new double[Row1 + 1, Col1 + 1];
      for (int index1 = 0; index1 <= Row1; ++index1)
      {
        for (int index2 = 0; index2 <= Col1; ++index2)
          numArray[index1, index2] = Mat1[index1, index2] - Mat2[index1, index2];
      }
      return numArray;
    }

    public static Matrix Subtract(Matrix Mat1, Matrix Mat2)
    {
      return new Matrix(Matrix.Subtract(Mat1.in_Mat, Mat2.in_Mat));
    }

    public static Matrix operator -(Matrix Mat1, Matrix Mat2)
    {
      return new Matrix(Matrix.Subtract(Mat1.in_Mat, Mat2.in_Mat));
    }

    public static double[,] Multiply(double[,] Mat1, double[,] Mat2)
    {
      double num = 0.0;
      int Row1;
      int Col1;
      int Row2;
      int Col2;
      try
      {
        Matrix.Find_R_C(Mat1, out Row1, out Col1);
        Matrix.Find_R_C(Mat2, out Row2, out Col2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Col1 != Row2)
        throw new MatrixDimensionException();
      double[,] numArray = new double[Row1 + 1, Col2 + 1];
      for (int index1 = 0; index1 <= Row1; ++index1)
      {
        for (int index2 = 0; index2 <= Col2; ++index2)
        {
          for (int index3 = 0; index3 <= Col1; ++index3)
            num += Mat1[index1, index3] * Mat2[index3, index2];
          numArray[index1, index2] = num;
          num = 0.0;
        }
      }
      return numArray;
    }

    public static Matrix Multiply(Matrix Mat1, Matrix Mat2)
    {
      if (Mat1.NoRows == 3 && Mat2.NoRows == 3 && Mat1.NoCols == 1 && Mat1.NoCols == 1)
        return new Matrix(Matrix.CrossProduct(Mat1.in_Mat, Mat2.in_Mat));
      return new Matrix(Matrix.Multiply(Mat1.in_Mat, Mat2.in_Mat));
    }

    public static Matrix operator *(Matrix Mat1, Matrix Mat2)
    {
      if (Mat1.NoRows == 3 && Mat2.NoRows == 3 && Mat1.NoCols == 1 && Mat1.NoCols == 1)
        return new Matrix(Matrix.CrossProduct(Mat1.in_Mat, Mat2.in_Mat));
      return new Matrix(Matrix.Multiply(Mat1.in_Mat, Mat2.in_Mat));
    }

    public static double Det(double[,] Mat)
    {
      double[,] numArray;
      int Row;
      int Col;
      try
      {
        numArray = (double[,]) Mat.Clone();
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row != Col)
        throw new MatrixNotSquare();
      int num1 = Row;
      double num2 = 1.0;
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        if (numArray[index1, index1] == 0.0)
        {
          int index2 = index1;
          while (index2 < num1 && numArray[index1, index2] == 0.0)
            ++index2;
          if (numArray[index1, index2] == 0.0)
            return 0.0;
          for (int index3 = index1; index3 <= num1; ++index3)
          {
            double num3 = numArray[index3, index2];
            numArray[index3, index2] = numArray[index3, index1];
            numArray[index3, index1] = num3;
          }
          num2 = -num2;
        }
        double num4 = numArray[index1, index1];
        num2 *= num4;
        if (index1 < num1)
        {
          int num3 = index1 + 1;
          for (int index2 = num3; index2 <= num1; ++index2)
          {
            for (int index3 = num3; index3 <= num1; ++index3)
              numArray[index2, index3] = numArray[index2, index3] - numArray[index2, index1] * (numArray[index1, index3] / num4);
          }
        }
      }
      return num2;
    }

    public static double Det(Matrix Mat)
    {
      return Matrix.Det(Mat.in_Mat);
    }

    public static double[,] Inverse(double[,] Mat)
    {
      int Row;
      int Col;
      double[,] numArray1;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
        numArray1 = (double[,]) Mat.Clone();
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row != Col)
        throw new MatrixNotSquare();
      if (Matrix.Det(Mat) == 0.0)
        throw new MatrixDeterminentZero();
      int num1 = Row;
      double[,] numArray2 = new double[num1 + 1, num1 + 1];
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        for (int index2 = 0; index2 <= num1; ++index2)
          numArray2[index2, index1] = 0.0;
        numArray2[index1, index1] = 1.0;
      }
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        if (Math.Abs(numArray1[index1, index1]) < 1E-10)
        {
          for (int index2 = index1 + 1; index2 <= num1; ++index2)
          {
            if (index2 != index1 && Math.Abs(numArray1[index1, index2]) > 1E-10)
            {
              for (int index3 = 0; index3 <= num1; ++index3)
              {
                numArray1[index3, index1] = numArray1[index3, index1] + numArray1[index3, index2];
                numArray2[index3, index1] = numArray2[index3, index1] + numArray2[index3, index2];
              }
              break;
            }
          }
        }
        double num2 = 1.0 / numArray1[index1, index1];
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          numArray1[index2, index1] = num2 * numArray1[index2, index1];
          numArray2[index2, index1] = num2 * numArray2[index2, index1];
        }
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          if (index2 != index1)
          {
            double num3 = numArray1[index1, index2];
            for (int index3 = 0; index3 <= num1; ++index3)
            {
              numArray1[index3, index2] = numArray1[index3, index2] - num3 * numArray1[index3, index1];
              numArray2[index3, index2] = numArray2[index3, index2] - num3 * numArray2[index3, index1];
            }
          }
        }
      }
      return numArray2;
    }

    public static Matrix Inverse(Matrix Mat)
    {
      return new Matrix(Matrix.Inverse(Mat.in_Mat));
    }

    public static double[,] Transpose(double[,] Mat)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      double[,] numArray = new double[Col + 1, Row + 1];
      for (int index1 = 0; index1 <= Row; ++index1)
      {
        for (int index2 = 0; index2 <= Col; ++index2)
          numArray[index2, index1] = Mat[index1, index2];
      }
      return numArray;
    }

    public static Matrix Transpose(Matrix Mat)
    {
      return new Matrix(Matrix.Transpose(Mat.in_Mat));
    }

    public static void SVD(double[,] Mat_, out double[,] S_, out double[,] U_, out double[,] V_)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(Mat_, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      int num1 = Row + 1;
      int num2 = Col + 1;
      int length1;
      int length2;
      if (num1 < num2)
      {
        num1 = num2;
        length2 = length1 = num2;
      }
      else if (num1 > num2)
      {
        num2 = num1;
        length1 = length2 = num1;
      }
      else
      {
        length2 = num1;
        length1 = num2;
      }
      double[,] numArray1 = new double[num1 + 1, num2 + 1];
      for (int index1 = 1; index1 <= Row + 1; ++index1)
      {
        for (int index2 = 1; index2 <= Col + 1; ++index2)
          numArray1[index1, index2] = Mat_[index1 - 1, index2 - 1];
      }
      double[,] numArray2 = new double[length1 + 1, length1 + 1];
      double[] numArray3 = new double[length1 + 1];
      double[] numArray4 = new double[100];
      int index3 = 0;
      int index4 = 0;
      double num3 = 0.0;
      double num4 = 0.0;
      double val1 = 0.0;
      for (int index1 = 1; index1 <= num2; ++index1)
      {
        index3 = index1 + 1;
        numArray4[index1] = num4 * num3;
        double num5;
        double num6 = num5 = 0.0;
        double d1 = num5;
        double num7 = num5;
        if (index1 <= num1)
        {
          for (int index2 = index1; index2 <= num1; ++index2)
            num6 += Math.Abs(numArray1[index2, index1]);
          if (num6 != 0.0)
          {
            for (int index2 = index1; index2 <= num1; ++index2)
            {
              numArray1[index2, index1] /= num6;
              d1 += numArray1[index2, index1] * numArray1[index2, index1];
            }
            double b = numArray1[index1, index1];
            num7 = -Matrix.Sign(Math.Sqrt(d1), b);
            double num8 = b * num7 - d1;
            numArray1[index1, index1] = b - num7;
            if (index1 != num2)
            {
              for (int index2 = index3; index2 <= num2; ++index2)
              {
                double num9 = 0.0;
                for (int index5 = index1; index5 <= num1; ++index5)
                  num9 += numArray1[index5, index1] * numArray1[index5, index2];
                double num10 = num9 / num8;
                for (int index5 = index1; index5 <= num1; ++index5)
                  numArray1[index5, index2] += num10 * numArray1[index5, index1];
              }
            }
            for (int index2 = index1; index2 <= num1; ++index2)
              numArray1[index2, index1] *= num6;
          }
        }
        numArray3[index1] = num6 * num7;
        double num11;
        num4 = num11 = 0.0;
        double d2 = num11;
        num3 = num11;
        if (index1 <= num1 && index1 != num2)
        {
          for (int index2 = index3; index2 <= num2; ++index2)
            num4 += Math.Abs(numArray1[index1, index2]);
          if (num4 != 0.0)
          {
            for (int index2 = index3; index2 <= num2; ++index2)
            {
              numArray1[index1, index2] /= num4;
              d2 += numArray1[index1, index2] * numArray1[index1, index2];
            }
            double b = numArray1[index1, index3];
            num3 = -Matrix.Sign(Math.Sqrt(d2), b);
            double num8 = b * num3 - d2;
            numArray1[index1, index3] = b - num3;
            for (int index2 = index3; index2 <= num2; ++index2)
              numArray4[index2] = numArray1[index1, index2] / num8;
            if (index1 != num1)
            {
              for (int index2 = index3; index2 <= num1; ++index2)
              {
                double num9 = 0.0;
                for (int index5 = index3; index5 <= num2; ++index5)
                  num9 += numArray1[index2, index5] * numArray1[index1, index5];
                for (int index5 = index3; index5 <= num2; ++index5)
                  numArray1[index2, index5] += num9 * numArray4[index5];
              }
            }
            for (int index2 = index3; index2 <= num2; ++index2)
              numArray1[index1, index2] *= num4;
          }
        }
        val1 = Math.Max(val1, Math.Abs(numArray3[index1]) + Math.Abs(numArray4[index1]));
      }
      for (int index1 = num2; index1 >= 1; --index1)
      {
        if (index1 < num2)
        {
          if (num3 != 0.0)
          {
            for (int index2 = index3; index2 <= num2; ++index2)
              numArray2[index2, index1] = numArray1[index1, index2] / numArray1[index1, index3] / num3;
            for (int index2 = index3; index2 <= num2; ++index2)
            {
              double num5 = 0.0;
              for (int index5 = index3; index5 <= num2; ++index5)
                num5 += numArray1[index1, index5] * numArray2[index5, index2];
              for (int index5 = index3; index5 <= num2; ++index5)
                numArray2[index5, index2] += num5 * numArray2[index5, index1];
            }
          }
          for (int index2 = index3; index2 <= num2; ++index2)
            numArray2[index1, index2] = numArray2[index2, index1] = 0.0;
        }
        numArray2[index1, index1] = 1.0;
        num3 = numArray4[index1];
        index3 = index1;
      }
      for (int index1 = num2; index1 >= 1; --index1)
      {
        int num5 = index1 + 1;
        double num6 = numArray3[index1];
        if (index1 < num2)
        {
          for (int index2 = num5; index2 <= num2; ++index2)
            numArray1[index1, index2] = 0.0;
        }
        if (num6 != 0.0)
        {
          double num7 = 1.0 / num6;
          if (index1 != num2)
          {
            for (int index2 = num5; index2 <= num2; ++index2)
            {
              double num8 = 0.0;
              for (int index5 = num5; index5 <= num1; ++index5)
                num8 += numArray1[index5, index1] * numArray1[index5, index2];
              double num9 = num8 / numArray1[index1, index1] * num7;
              for (int index5 = index1; index5 <= num1; ++index5)
                numArray1[index5, index2] += num9 * numArray1[index5, index1];
            }
          }
          for (int index2 = index1; index2 <= num1; ++index2)
            numArray1[index2, index1] *= num7;
        }
        else
        {
          for (int index2 = index1; index2 <= num1; ++index2)
            numArray1[index2, index1] = 0.0;
        }
        ++numArray1[index1, index1];
      }
      for (int index1 = num2; index1 >= 1; --index1)
      {
        for (int index2 = 1; index2 <= 30; ++index2)
        {
          int num5 = 1;
          int index5;
          for (index5 = index1; index5 >= 1; --index5)
          {
            index4 = index5 - 1;
            if (Math.Abs(numArray4[index5]) + val1 == val1)
            {
              num5 = 0;
              break;
            }
            if (Math.Abs(numArray3[index4]) + val1 == val1)
              break;
          }
          if (num5 != 0)
          {
            double num6 = 1.0;
            for (int index6 = index5; index6 <= index1; ++index6)
            {
              double a = num6 * numArray4[index6];
              if (Math.Abs(a) + val1 != val1)
              {
                double b = numArray3[index6];
                double num7 = Matrix.PYTHAG(a, b);
                numArray3[index6] = num7;
                double num8 = 1.0 / num7;
                double num9 = b * num8;
                num6 = -a * num8;
                for (int index7 = 1; index7 <= num1; ++index7)
                {
                  double num10 = numArray1[index7, index4];
                  double num11 = numArray1[index7, index6];
                  numArray1[index7, index4] = num10 * num9 + num11 * num6;
                  numArray1[index7, index6] = num11 * num9 - num10 * num6;
                }
              }
            }
          }
          double num12 = numArray3[index1];
          if (index5 == index1)
          {
            if (num12 < 0.0)
            {
              numArray3[index1] = -num12;
              for (int index6 = 1; index6 <= num2; ++index6)
                numArray2[index6, index1] = -numArray2[index6, index1];
              break;
            }
            break;
          }
          if (index2 == 30)
            Console.WriteLine("No convergence in 30 SVDCMP iterations");
          double num13 = numArray3[index5];
          index4 = index1 - 1;
          double num14 = numArray3[index4];
          double num15 = numArray4[index4];
          double num16 = numArray4[index1];
          double num17 = ((num14 - num12) * (num14 + num12) + (num15 - num16) * (num15 + num16)) / (2.0 * num16 * num14);
          double a1 = Matrix.PYTHAG(num17, 1.0);
          double a2 = ((num13 - num12) * (num13 + num12) + num16 * (num14 / (num17 + Matrix.Sign(a1, num17)) - num16)) / num13;
          double num18;
          double num19 = num18 = 1.0;
          for (int index6 = index5; index6 <= index4; ++index6)
          {
            int index7 = index6 + 1;
            double num6 = numArray4[index7];
            double num7 = numArray3[index7];
            double b1 = num18 * num6;
            double num8 = num19 * num6;
            double num9 = Matrix.PYTHAG(a2, b1);
            numArray4[index6] = num9;
            num19 = a2 / num9;
            num18 = b1 / num9;
            double a3 = num13 * num19 + num8 * num18;
            double num10 = num8 * num19 - num13 * num18;
            double b2 = num7 * num18;
            double num11 = num7 * num19;
            for (int index8 = 1; index8 <= num2; ++index8)
            {
              double num20 = numArray2[index8, index6];
              double num21 = numArray2[index8, index7];
              numArray2[index8, index6] = num20 * num19 + num21 * num18;
              numArray2[index8, index7] = num21 * num19 - num20 * num18;
            }
            double num22 = Matrix.PYTHAG(a3, b2);
            numArray3[index6] = num22;
            if (num22 != 0.0)
            {
              double num20 = 1.0 / num22;
              num19 = a3 * num20;
              num18 = b2 * num20;
            }
            a2 = num19 * num10 + num18 * num11;
            num13 = num19 * num11 - num18 * num10;
            for (int index8 = 1; index8 <= num1; ++index8)
            {
              double num20 = numArray1[index8, index6];
              double num21 = numArray1[index8, index7];
              numArray1[index8, index6] = num20 * num19 + num21 * num18;
              numArray1[index8, index7] = num21 * num19 - num20 * num18;
            }
          }
          numArray4[index5] = 0.0;
          numArray4[index1] = a2;
          numArray3[index1] = num13;
        }
      }
      double[,] numArray5 = new double[length1, length1];
      double[,] numArray6 = new double[length1, length1];
      double[,] numArray7 = new double[length2, length1];
      for (int index1 = 1; index1 <= length1; ++index1)
        numArray5[index1 - 1, index1 - 1] = numArray3[index1];
      S_ = numArray5;
      for (int index1 = 1; index1 <= length1; ++index1)
      {
        for (int index2 = 1; index2 <= length1; ++index2)
          numArray6[index1 - 1, index2 - 1] = numArray2[index1, index2];
      }
      V_ = numArray6;
      for (int index1 = 1; index1 <= length2; ++index1)
      {
        for (int index2 = 1; index2 <= length1; ++index2)
          numArray7[index1 - 1, index2 - 1] = numArray1[index1, index2];
      }
      U_ = numArray7;
    }

    private static double SQR(double a)
    {
      return a * a;
    }

    private static double Sign(double a, double b)
    {
      if (b >= 0.0)
        return Math.Abs(a);
      return -Math.Abs(a);
    }

    private static double PYTHAG(double a, double b)
    {
      double num1 = Math.Abs(a);
      double num2 = Math.Abs(b);
      if (num1 > num2)
        return num1 * Math.Sqrt(1.0 + Matrix.SQR(num2 / num1));
      return num2 == 0.0 ? 0.0 : num2 * Math.Sqrt(1.0 + Matrix.SQR(num1 / num2));
    }

    public static void SVD(Matrix Mat, out Matrix S, out Matrix U, out Matrix V)
    {
      double[,] S_;
      double[,] U_;
      double[,] V_;
      Matrix.SVD(Mat.in_Mat, out S_, out U_, out V_);
      S = new Matrix(S_);
      U = new Matrix(U_);
      V = new Matrix(V_);
    }

    public static void LU(double[,] Mat, out double[,] L, out double[,] U, out double[,] P)
    {
      double[,] numArray1;
      int Row1;
      int Col;
      try
      {
        numArray1 = (double[,]) Mat.Clone();
        Matrix.Find_R_C(Mat, out Row1, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != Col)
        throw new MatrixNotSquare();
      int index1 = 0;
      int index2 = Row1;
      double num1 = 1E-20;
      int[] numArray2 = new int[index2 + 1];
      double[] numArray3 = new double[index2 * 10];
      double num2 = 1.0;
      for (int index3 = 0; index3 <= index2; ++index3)
      {
        double num3 = 0.0;
        for (int index4 = 0; index4 <= index2; ++index4)
        {
          if (Math.Abs(numArray1[index3, index4]) > num3)
            num3 = Math.Abs(numArray1[index3, index4]);
        }
        if (num3 == 0.0)
          throw new MatrixSingularException();
        numArray3[index3] = 1.0 / num3;
      }
      for (int index3 = 0; index3 <= index2; ++index3)
      {
        if (index3 > 0)
        {
          for (int index4 = 0; index4 <= index3 - 1; ++index4)
          {
            double num3 = numArray1[index4, index3];
            if (index4 > 0)
            {
              for (int index5 = 0; index5 <= index4 - 1; ++index5)
                num3 -= numArray1[index4, index5] * numArray1[index5, index3];
              numArray1[index4, index3] = num3;
            }
          }
        }
        double num4 = 0.0;
        for (int index4 = index3; index4 <= index2; ++index4)
        {
          double num3 = numArray1[index4, index3];
          if (index3 > 0)
          {
            for (int index5 = 0; index5 <= index3 - 1; ++index5)
              num3 -= numArray1[index4, index5] * numArray1[index5, index3];
            numArray1[index4, index3] = num3;
          }
          double num5 = numArray3[index4] * Math.Abs(num3);
          if (num5 >= num4)
          {
            index1 = index4;
            num4 = num5;
          }
        }
        if (index3 != index1)
        {
          for (int index4 = 0; index4 <= index2; ++index4)
          {
            double num3 = numArray1[index1, index4];
            numArray1[index1, index4] = numArray1[index3, index4];
            numArray1[index3, index4] = num3;
          }
          num2 = -num2;
          numArray3[index1] = numArray3[index3];
        }
        numArray2[index3] = index1;
        if (index3 != index2)
        {
          if (numArray1[index3, index3] == 0.0)
            numArray1[index3, index3] = num1;
          double num3 = 1.0 / numArray1[index3, index3];
          for (int index4 = index3 + 1; index4 <= index2; ++index4)
            numArray1[index4, index3] = numArray1[index4, index3] * num3;
        }
      }
      if (numArray1[index2, index2] == 0.0)
        numArray1[index2, index2] = num1;
      int num6 = 0;
      double[,] numArray4 = new double[index2 + 1, index2 + 1];
      double[,] numArray5 = new double[index2 + 1, index2 + 1];
      for (int index3 = 0; index3 <= index2; ++index3)
      {
        for (int index4 = 0; index4 <= num6; ++index4)
        {
          if (index3 != 0)
            numArray4[index3, index4] = numArray1[index3, index4];
          if (index3 == index4)
            numArray4[index3, index4] = 1.0;
          numArray5[index2 - index3, index2 - index4] = numArray1[index2 - index3, index2 - index4];
        }
        ++num6;
      }
      L = numArray4;
      U = numArray5;
      P = Matrix.Identity(index2 + 1);
      for (int Row2 = 0; Row2 <= index2; ++Row2)
        Matrix.SwapRows(P, Row2, numArray2[Row2]);
    }

    private static void SwapRows(double[,] Mat, int Row, int toRow)
    {
      int upperBound = Mat.GetUpperBound(0);
      double[,] numArray = new double[1, upperBound + 1];
      for (int index = 0; index <= upperBound; ++index)
      {
        numArray[0, index] = Mat[Row, index];
        Mat[Row, index] = Mat[toRow, index];
        Mat[toRow, index] = numArray[0, index];
      }
    }

    public static void LU(Matrix Mat, out Matrix L, out Matrix U, out Matrix P)
    {
      double[,] L1;
      double[,] U1;
      double[,] P1;
      Matrix.LU(Mat.in_Mat, out L1, out U1, out P1);
      L = new Matrix(L1);
      U = new Matrix(U1);
      P = new Matrix(P1);
    }

    public static double[,] SolveLinear(double[,] MatA, double[,] MatB)
    {
      double[,] Mat;
      double[,] numArray1;
      int Row;
      int Col;
      try
      {
        Mat = (double[,]) MatA.Clone();
        numArray1 = (double[,]) MatB.Clone();
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row != Col)
        throw new MatrixNotSquare();
      if (numArray1.GetUpperBound(0) != Row || numArray1.GetUpperBound(1) != 0)
        throw new MatrixDimensionException();
      int index1 = 0;
      int index2 = Row;
      double num1 = 1E-20;
      int[] numArray2 = new int[index2 + 1];
      double[] numArray3 = new double[index2 * 10];
      double num2 = 1.0;
      for (int index3 = 0; index3 <= index2; ++index3)
      {
        double num3 = 0.0;
        for (int index4 = 0; index4 <= index2; ++index4)
        {
          if (Math.Abs(Mat[index3, index4]) > num3)
            num3 = Math.Abs(Mat[index3, index4]);
        }
        if (num3 == 0.0)
          throw new MatrixSingularException();
        numArray3[index3] = 1.0 / num3;
      }
      for (int index3 = 0; index3 <= index2; ++index3)
      {
        if (index3 > 0)
        {
          for (int index4 = 0; index4 <= index3 - 1; ++index4)
          {
            double num3 = Mat[index4, index3];
            if (index4 > 0)
            {
              for (int index5 = 0; index5 <= index4 - 1; ++index5)
                num3 -= Mat[index4, index5] * Mat[index5, index3];
              Mat[index4, index3] = num3;
            }
          }
        }
        double num4 = 0.0;
        for (int index4 = index3; index4 <= index2; ++index4)
        {
          double num3 = Mat[index4, index3];
          if (index3 > 0)
          {
            for (int index5 = 0; index5 <= index3 - 1; ++index5)
              num3 -= Mat[index4, index5] * Mat[index5, index3];
            Mat[index4, index3] = num3;
          }
          double num5 = numArray3[index4] * Math.Abs(num3);
          if (num5 >= num4)
          {
            index1 = index4;
            num4 = num5;
          }
        }
        if (index3 != index1)
        {
          for (int index4 = 0; index4 <= index2; ++index4)
          {
            double num3 = Mat[index1, index4];
            Mat[index1, index4] = Mat[index3, index4];
            Mat[index3, index4] = num3;
          }
          num2 = -num2;
          numArray3[index1] = numArray3[index3];
        }
        numArray2[index3] = index1;
        if (index3 != index2)
        {
          if (Mat[index3, index3] == 0.0)
            Mat[index3, index3] = num1;
          double num3 = 1.0 / Mat[index3, index3];
          for (int index4 = index3 + 1; index4 <= index2; ++index4)
            Mat[index4, index3] = Mat[index4, index3] * num3;
        }
      }
      if (Mat[index2, index2] == 0.0)
        Mat[index2, index2] = num1;
      int num6 = -1;
      for (int index3 = 0; index3 <= index2; ++index3)
      {
        int index4 = numArray2[index3];
        double num3 = numArray1[index4, 0];
        numArray1[index4, 0] = numArray1[index3, 0];
        if (num6 != -1)
        {
          for (int index5 = num6; index5 <= index3 - 1; ++index5)
            num3 -= Mat[index3, index5] * numArray1[index5, 0];
        }
        else if (num3 != 0.0)
          num6 = index3;
        numArray1[index3, 0] = num3;
      }
      for (int index3 = index2; index3 >= 0; --index3)
      {
        double num3 = numArray1[index3, 0];
        if (index3 < index2)
        {
          for (int index4 = index3 + 1; index4 <= index2; ++index4)
            num3 -= Mat[index3, index4] * numArray1[index4, 0];
        }
        numArray1[index3, 0] = num3 / Mat[index3, index3];
      }
      return numArray1;
    }

    public static Matrix SolveLinear(Matrix MatA, Matrix MatB)
    {
      return new Matrix(Matrix.SolveLinear(MatA.in_Mat, MatB.in_Mat));
    }

    public static int Rank(double[,] Mat)
    {
      int num1 = 0;
      try
      {
        int Row;
        int Col;
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      double num2 = 2.2204E-16;
      double[,] S_;
      double[,] U_;
      double[,] V_;
      Matrix.SVD(Mat, out S_, out U_, out V_);
      for (int index = 0; index <= S_.GetUpperBound(0); ++index)
      {
        if (Math.Abs(S_[index, index]) > num2)
          ++num1;
      }
      return num1;
    }

    public static int Rank(Matrix Mat)
    {
      return Matrix.Rank(Mat.in_Mat);
    }

    public static double[,] PINV(double[,] Mat)
    {
      double num1 = 0.0;
      int Row;
      int Col;
      try
      {
        double[,] numArray = (double[,]) Mat.Clone();
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      double[,] S_;
      double[,] U_;
      double[,] V_;
      Matrix.SVD(Mat, out S_, out U_, out V_);
      double num2 = 2.2204E-16;
      int length1 = Row + 1;
      int length2 = Col + 1;
      double[,] numArray1 = new double[length1, length2];
      double[,] Mat1 = new double[length1, length2];
      double[,] numArray2 = new double[length2, length2];
      double num3 = 0.0;
      for (int index = 0; index <= S_.GetUpperBound(0); ++index)
      {
        if (index == 0)
          num1 = S_[0, 0];
        if (num1 < S_[index, index])
          num1 = S_[index, index];
      }
      double num4 = length1 <= length2 ? (double) length2 * num1 * num2 : (double) length1 * num1 * num2;
      for (int index = 0; index < length2; ++index)
        numArray2[index, index] = S_[index, index];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
        {
          for (int index3 = 0; index3 < length2; ++index3)
          {
            if (numArray2[index3, index2] > num4)
              num3 += U_[index1, index3] * (1.0 / numArray2[index3, index2]);
          }
          numArray1[index1, index2] = num3;
          num3 = 0.0;
        }
      }
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
        {
          for (int index3 = 0; index3 < length2; ++index3)
            num3 += numArray1[index1, index3] * V_[index2, index3];
          Mat1[index1, index2] = num3;
          num3 = 0.0;
        }
      }
      return Matrix.Transpose(Mat1);
    }

    public static Matrix PINV(Matrix Mat)
    {
      return new Matrix(Matrix.PINV(Mat.in_Mat));
    }

    public static void Eigen(double[,] Mat, out double[,] d, out double[,] v)
    {
      int Row;
      int Col;
      double[,] a;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
        a = (double[,]) Mat.Clone();
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row != Col)
        throw new MatrixNotSquare();
      int num1 = Row;
      d = new double[num1 + 1, 1];
      v = new double[num1 + 1, num1 + 1];
      double[] numArray1 = new double[num1 + 1];
      double[] numArray2 = new double[num1 + 1];
      for (int index1 = 0; index1 <= num1; ++index1)
      {
        for (int index2 = 0; index2 <= num1; ++index2)
          v[index1, index2] = 0.0;
        v[index1, index1] = 1.0;
      }
      for (int index = 0; index <= num1; ++index)
      {
        numArray1[index] = d[index, 0] = a[index, index];
        numArray2[index] = 0.0;
      }
      int num2 = 0;
      for (int index1 = 0; index1 <= 50; ++index1)
      {
        double num3 = 0.0;
        for (int index2 = 0; index2 <= num1 - 1; ++index2)
        {
          for (int index3 = index2 + 1; index3 <= num1; ++index3)
            num3 += Math.Abs(a[index2, index3]);
        }
        if (num3 == 0.0)
          return;
        double num4 = index1 >= 4 ? 0.0 : 0.2 * num3 / (double) (num1 * num1);
        for (int index2 = 0; index2 <= num1 - 1; ++index2)
        {
          for (int index3 = index2 + 1; index3 <= num1; ++index3)
          {
            double g = 100.0 * Math.Abs(a[index2, index3]);
            if (index1 > 4 && Math.Abs(d[index2, 0]) + g == Math.Abs(d[index2, 0]) && Math.Abs(d[index3, 0]) + g == Math.Abs(d[index3, 0]))
              a[index2, index3] = 0.0;
            else if (Math.Abs(a[index2, index3]) > num4)
            {
              double num5 = d[index3, 0] - d[index2, 0];
              double num6;
              if (Math.Abs(num5) + g == Math.Abs(num5))
              {
                num6 = a[index2, index3] / num5;
              }
              else
              {
                double num7 = 0.5 * num5 / a[index2, index3];
                num6 = 1.0 / (Math.Abs(num7) + Math.Sqrt(1.0 + num7 * num7));
                if (num7 < 0.0)
                  num6 = -num6;
              }
              double num8 = 1.0 / Math.Sqrt(1.0 + num6 * num6);
              double s = num6 * num8;
              double tau = s / (1.0 + num8);
              double h = num6 * a[index2, index3];
              numArray2[index2] -= h;
              numArray2[index3] += h;
              d[index2, 0] -= h;
              d[index3, 0] += h;
              a[index2, index3] = 0.0;
              for (int index4 = 0; index4 <= index2 - 1; ++index4)
                Matrix.ROT(g, h, s, tau, a, index4, index2, index4, index3);
              for (int index4 = index2 + 1; index4 <= index3 - 1; ++index4)
                Matrix.ROT(g, h, s, tau, a, index2, index4, index4, index3);
              for (int index4 = index3 + 1; index4 <= num1; ++index4)
                Matrix.ROT(g, h, s, tau, a, index2, index4, index3, index4);
              for (int index4 = 0; index4 <= num1; ++index4)
                Matrix.ROT(g, h, s, tau, v, index4, index2, index4, index3);
              ++num2;
            }
          }
        }
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          numArray1[index2] += numArray2[index2];
          d[index2, 0] = numArray1[index2];
          numArray2[index2] = 0.0;
        }
      }
      Console.WriteLine("Too many iterations in routine jacobi");
    }

    private static void ROT(double g, double h, double s, double tau, double[,] a, int i, int j, int k, int l)
    {
      g = a[i, j];
      h = a[k, l];
      a[i, j] = g - s * (h + g * tau);
      a[k, l] = h + s * (g - h * tau);
    }

    public static void Eigen(Matrix Mat, out Matrix d, out Matrix v)
    {
      double[,] d1;
      double[,] v1;
      Matrix.Eigen(Mat.in_Mat, out d1, out v1);
      d = new Matrix(d1);
      v = new Matrix(v1);
    }

    public static double[,] ScalarMultiply(double Value, double[,] Mat)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      double[,] numArray = new double[Row + 1, Col + 1];
      for (int index1 = 0; index1 <= Row; ++index1)
      {
        for (int index2 = 0; index2 <= Col; ++index2)
          numArray[index1, index2] = Mat[index1, index2] * Value;
      }
      return numArray;
    }

    public static Matrix ScalarMultiply(double Value, Matrix Mat)
    {
      return new Matrix(Matrix.ScalarMultiply(Value, Mat.in_Mat));
    }

    public static Matrix operator *(Matrix Mat, double Value)
    {
      return new Matrix(Matrix.ScalarMultiply(Value, Mat.in_Mat));
    }

    public static Matrix operator *(double Value, Matrix Mat)
    {
      return new Matrix(Matrix.ScalarMultiply(Value, Mat.in_Mat));
    }

    public static double[,] ScalarDivide(double Value, double[,] Mat)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      double[,] numArray = new double[Row + 1, Col + 1];
      for (int index1 = 0; index1 <= Row; ++index1)
      {
        for (int index2 = 0; index2 <= Col; ++index2)
          numArray[index1, index2] = Mat[index1, index2] / Value;
      }
      return numArray;
    }

    public static Matrix ScalarDivide(double Value, Matrix Mat)
    {
      return new Matrix(Matrix.ScalarDivide(Value, Mat.in_Mat));
    }

    public static Matrix operator /(Matrix Mat, double Value)
    {
      return new Matrix(Matrix.ScalarDivide(Value, Mat.in_Mat));
    }

    public static double[] CrossProduct(double[] V1, double[] V2)
    {
      double[] numArray = new double[2];
      int Row1;
      int Row2;
      try
      {
        Matrix.Find_R_C(V1, out Row1);
        Matrix.Find_R_C(V2, out Row2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != 2)
        throw new VectorDimensionException();
      if (Row2 != 2)
        throw new VectorDimensionException();
      double num1 = V1[1] * V2[2] - V1[2] * V2[1];
      double num2 = V1[2] * V2[0] - V1[0] * V2[2];
      double num3 = V1[0] * V2[1] - V1[1] * V2[0];
      numArray[0] = num1;
      numArray[1] = num2;
      numArray[2] = num3;
      return numArray;
    }

    public static double[,] CrossProduct(double[,] V1, double[,] V2)
    {
      double[,] numArray = new double[3, 1];
      int Row1;
      int Col1;
      int Row2;
      int Col2;
      try
      {
        Matrix.Find_R_C(V1, out Row1, out Col1);
        Matrix.Find_R_C(V2, out Row2, out Col2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != 2 || Col1 != 0)
        throw new VectorDimensionException();
      if (Row2 != 2 || Col2 != 0)
        throw new VectorDimensionException();
      double num1 = V1[1, 0] * V2[2, 0] - V1[2, 0] * V2[1, 0];
      double num2 = V1[2, 0] * V2[0, 0] - V1[0, 0] * V2[2, 0];
      double num3 = V1[0, 0] * V2[1, 0] - V1[1, 0] * V2[0, 0];
      numArray[0, 0] = num1;
      numArray[1, 0] = num2;
      numArray[2, 0] = num3;
      return numArray;
    }

    public static Matrix CrossProduct(Matrix V1, Matrix V2)
    {
      return new Matrix(Matrix.CrossProduct(V1.in_Mat, V2.in_Mat));
    }

    public static double DotProduct(double[] V1, double[] V2)
    {
      int Row1;
      int Row2;
      try
      {
        Matrix.Find_R_C(V1, out Row1);
        Matrix.Find_R_C(V2, out Row2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != 2)
        throw new VectorDimensionException();
      if (Row2 != 2)
        throw new VectorDimensionException();
      return V1[0] * V2[0] + V1[1] * V2[1] + V1[2] * V2[2];
    }

    public static double DotProduct(double[,] V1, double[,] V2)
    {
      int Row1;
      int Col1;
      int Row2;
      int Col2;
      try
      {
        Matrix.Find_R_C(V1, out Row1, out Col1);
        Matrix.Find_R_C(V2, out Row2, out Col2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != 2 || Col1 != 0)
        throw new VectorDimensionException();
      if (Row2 != 2 || Col2 != 0)
        throw new VectorDimensionException();
      return V1[0, 0] * V2[0, 0] + V1[1, 0] * V2[1, 0] + V1[2, 0] * V2[2, 0];
    }

    public static double DotProduct(Matrix V1, Matrix V2)
    {
      return Matrix.DotProduct(V1.in_Mat, V2.in_Mat);
    }

    public static double VectorMagnitude(double[] V)
    {
      int Row;
      try
      {
        Matrix.Find_R_C(V, out Row);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row != 2)
        throw new VectorDimensionException();
      return Math.Sqrt(V[0] * V[0] + V[1] * V[1] + V[2] * V[2]);
    }

    public static double VectorMagnitude(double[,] V)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(V, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row != 2 || Col != 0)
        throw new VectorDimensionException();
      return Math.Sqrt(V[0, 0] * V[0, 0] + V[1, 0] * V[1, 0] + V[2, 0] * V[2, 0]);
    }

    public static double VectorMagnitude(Matrix V)
    {
      return Matrix.VectorMagnitude(V.in_Mat);
    }

    public static bool IsEqual(double[,] Mat1, double[,] Mat2)
    {
      double num = 1E-14;
      int Row1;
      int Col1;
      int Row2;
      int Col2;
      try
      {
        Matrix.Find_R_C(Mat1, out Row1, out Col1);
        Matrix.Find_R_C(Mat2, out Row2, out Col2);
      }
      catch
      {
        throw new MatrixNullException();
      }
      if (Row1 != Row2 || Col1 != Col2)
        throw new MatrixDimensionException();
      for (int index1 = 0; index1 <= Row1; ++index1)
      {
        for (int index2 = 0; index2 <= Col1; ++index2)
        {
          if (Math.Abs(Mat1[index1, index2] - Mat2[index1, index2]) > num)
            return false;
        }
      }
      return true;
    }

    public static bool IsEqual(Matrix Mat1, Matrix Mat2)
    {
      return Matrix.IsEqual(Mat1.in_Mat, Mat2.in_Mat);
    }

    public static bool operator ==(Matrix Mat1, Matrix Mat2)
    {
      return Matrix.IsEqual(Mat1.in_Mat, Mat2.in_Mat);
    }

    public static bool operator !=(Matrix Mat1, Matrix Mat2)
    {
      return !Matrix.IsEqual(Mat1.in_Mat, Mat2.in_Mat);
    }

    public override bool Equals(object obj)
    {
      try
      {
        return this == (Matrix) obj;
      }
      catch
      {
        return false;
      }
    }

    public static string PrintMat(double[,] Mat)
    {
      int Row;
      int Col;
      try
      {
        Matrix.Find_R_C(Mat, out Row, out Col);
      }
      catch
      {
        throw new MatrixNullException();
      }
      string str1 = "";
      string str2 = "";
      string str3 = "";
      int[] numArray = new int[Col + 1];
      for (int index1 = 0; index1 <= Row; ++index1)
      {
        for (int index2 = 0; index2 <= Col; ++index2)
        {
          if (index1 == 0)
          {
            numArray[index2] = 0;
            for (int index3 = 0; index3 <= Row; ++index3)
            {
              string str4 = Mat[index3, index2].ToString("0.0000");
              int length = str4.Length;
              if (numArray[index2] < length)
              {
                numArray[index2] = length;
                str2 = str4;
              }
            }
            if (str2.StartsWith("-"))
              numArray[index2] = numArray[index2];
          }
          string str5 = Mat[index1, index2].ToString("0.0000");
          if (str5.StartsWith("-"))
          {
            int length = str5.Length;
            if (numArray[index2] >= length)
            {
              for (int index3 = 1; index3 <= numArray[index2] - length; ++index3)
                str3 += "  ";
              str3 += " ";
            }
          }
          else
          {
            int length = str5.Length;
            if (numArray[index2] > length)
            {
              for (int index3 = 1; index3 <= numArray[index2] - length; ++index3)
                str3 += "  ";
            }
          }
          str3 = str3 + "  " + Mat[index1, index2].ToString("0.0000");
        }
        if (index1 != Row)
        {
          str1 = str1 + str3 + "\n";
          str3 = "";
        }
        str1 += str3;
        str3 = "";
      }
      return str1;
    }

    public static string PrintMat(Matrix Mat)
    {
      return Matrix.PrintMat(Mat.in_Mat);
    }

    public override string ToString()
    {
      return Matrix.PrintMat(this.in_Mat);
    }
  }
}
