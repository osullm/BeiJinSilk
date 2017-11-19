namespace MatrixLibrary
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class Matrix
    {
        private double[,] in_Mat;

        public Matrix(double[,] Mat)
        {
            this.in_Mat = (double[,]) Mat.Clone();
        }

        public Matrix(int noRows, int noCols)
        {
            this.in_Mat = new double[noRows, noCols];
        }

        public static double[,] Add(double[,] Mat1, double[,] Mat2)
        {
            int num3;
            int num4;
            int num5;
            int num6;
            try
            {
                Find_R_C(Mat1, out num3, out num4);
                Find_R_C(Mat2, out num5, out num6);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if ((num3 != num5) || (num4 != num6))
            {
                throw new MatrixDimensionException();
            }
            double[,] numArray = new double[num3 + 1, num4 + 1];
            for (int i = 0; i <= num3; i++)
            {
                for (int j = 0; j <= num4; j++)
                {
                    numArray[i, j] = Mat1[i, j] + Mat2[i, j];
                }
            }
            return numArray;
        }

        public static Matrix Add(Matrix Mat1, Matrix Mat2)
        {
            return new Matrix(Add(Mat1.in_Mat, Mat2.in_Mat));
        }

        public static double[] CrossProduct(double[] V1, double[] V2)
        {
            int num4;
            int num5;
            double[] numArray = new double[2];
            try
            {
                Find_R_C(V1, out num4);
                Find_R_C(V2, out num5);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num4 != 2)
            {
                throw new VectorDimensionException();
            }
            if (num5 != 2)
            {
                throw new VectorDimensionException();
            }
            double num = (V1[1] * V2[2]) - (V1[2] * V2[1]);
            double num2 = (V1[2] * V2[0]) - (V1[0] * V2[2]);
            double num3 = (V1[0] * V2[1]) - (V1[1] * V2[0]);
            numArray[0] = num;
            numArray[1] = num2;
            numArray[2] = num3;
            return numArray;
        }

        public static double[,] CrossProduct(double[,] V1, double[,] V2)
        {
            int num4;
            int num5;
            int num6;
            int num7;
            double[,] numArray = new double[3, 1];
            try
            {
                Find_R_C(V1, out num4, out num5);
                Find_R_C(V2, out num6, out num7);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if ((num4 != 2) || (num5 != 0))
            {
                throw new VectorDimensionException();
            }
            if ((num6 != 2) || (num7 != 0))
            {
                throw new VectorDimensionException();
            }
            double num = (V1[1, 0] * V2[2, 0]) - (V1[2, 0] * V2[1, 0]);
            double num2 = (V1[2, 0] * V2[0, 0]) - (V1[0, 0] * V2[2, 0]);
            double num3 = (V1[0, 0] * V2[1, 0]) - (V1[1, 0] * V2[0, 0]);
            numArray[0, 0] = num;
            numArray[1, 0] = num2;
            numArray[2, 0] = num3;
            return numArray;
        }

        public static Matrix CrossProduct(Matrix V1, Matrix V2)
        {
            return new Matrix(CrossProduct(V1.in_Mat, V2.in_Mat));
        }

        public static double Det(double[,] Mat)
        {
            double[,] numArray;
            int num9;
            int num10;
            try
            {
                numArray = (double[,]) Mat.Clone();
                Find_R_C(Mat, out num9, out num10);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num9 != num10)
            {
                throw new MatrixNotSquare();
            }
            int num = num9;
            double num8 = 1.0;
            for (int i = 0; i <= num; i++)
            {
                int num4;
                int num5;
                if (numArray[i, i] == 0.0)
                {
                    num5 = i;
                    while ((num5 < num) && (numArray[i, num5] == 0.0))
                    {
                        num5++;
                    }
                    if (numArray[i, num5] == 0.0)
                    {
                        return 0.0;
                    }
                    num4 = i;
                    while (num4 <= num)
                    {
                        double num6 = numArray[num4, num5];
                        numArray[num4, num5] = numArray[num4, i];
                        numArray[num4, i] = num6;
                        num4++;
                    }
                    num8 = -num8;
                }
                double num7 = numArray[i, i];
                num8 *= num7;
                if (i < num)
                {
                    int num3 = i + 1;
                    for (num4 = num3; num4 <= num; num4++)
                    {
                        for (num5 = num3; num5 <= num; num5++)
                        {
                            numArray[num4, num5] -= numArray[num4, i] * (numArray[i, num5] / num7);
                        }
                    }
                }
            }
            return num8;
        }

        public static double Det(Matrix Mat)
        {
            return Det(Mat.in_Mat);
        }

        public static double DotProduct(double[] V1, double[] V2)
        {
            int num;
            int num2;
            try
            {
                Find_R_C(V1, out num);
                Find_R_C(V2, out num2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num != 2)
            {
                throw new VectorDimensionException();
            }
            if (num2 != 2)
            {
                throw new VectorDimensionException();
            }
            return (((V1[0] * V2[0]) + (V1[1] * V2[1])) + (V1[2] * V2[2]));
        }

        public static double DotProduct(double[,] V1, double[,] V2)
        {
            int num;
            int num2;
            int num3;
            int num4;
            try
            {
                Find_R_C(V1, out num, out num2);
                Find_R_C(V2, out num3, out num4);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if ((num != 2) || (num2 != 0))
            {
                throw new VectorDimensionException();
            }
            if ((num3 != 2) || (num4 != 0))
            {
                throw new VectorDimensionException();
            }
            return (((V1[0, 0] * V2[0, 0]) + (V1[1, 0] * V2[1, 0])) + (V1[2, 0] * V2[2, 0]));
        }

        public static double DotProduct(Matrix V1, Matrix V2)
        {
            return DotProduct(V1.in_Mat, V2.in_Mat);
        }

        public static void Eigen(double[,] Mat, out double[,] d, out double[,] v)
        {
            double[,] numArray;
            int num;
            int num2;
            int num4;
            int num5;
            try
            {
                Find_R_C(Mat, out num, out num2);
                numArray = (double[,]) Mat.Clone();
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num != num2)
            {
                throw new MatrixNotSquare();
            }
            int num7 = num;
            d = new double[num7 + 1, 1];
            v = new double[num7 + 1, num7 + 1];
            double[] numArray2 = new double[num7 + 1];
            double[] numArray3 = new double[num7 + 1];
            for (num5 = 0; num5 <= num7; num5++)
            {
                num4 = 0;
                while (num4 <= num7)
                {
                    v[num5, num4] = 0.0;
                    num4++;
                }
                v[num5, num5] = 1.0;
            }
            num5 = 0;
            while (num5 <= num7)
            {
                numArray2[num5] = d[num5, 0] = numArray[num5, num5];
                numArray3[num5] = 0.0;
                num5++;
            }
            int num8 = 0;
            for (int i = 0; i <= 50; i++)
            {
                double num9;
                double num13 = 0.0;
                num5 = 0;
                while (num5 <= (num7 - 1))
                {
                    num4 = num5 + 1;
                    while (num4 <= num7)
                    {
                        num13 += Math.Abs(numArray[num5, num4]);
                        num4++;
                    }
                    num5++;
                }
                if (num13 == 0.0)
                {
                    return;
                }
                if (i < 4)
                {
                    num9 = (0.2 * num13) / ((double) (num7 * num7));
                }
                else
                {
                    num9 = 0.0;
                }
                num5 = 0;
                while (num5 <= (num7 - 1))
                {
                    for (num4 = num5 + 1; num4 <= num7; num4++)
                    {
                        double g = 100.0 * Math.Abs(numArray[num5, num4]);
                        if (((i > 4) && ((Math.Abs(d[num5, 0]) + g) == Math.Abs(d[num5, 0]))) && ((Math.Abs(d[num4, 0]) + g) == Math.Abs(d[num4, 0])))
                        {
                            numArray[num5, num4] = 0.0;
                        }
                        else if (Math.Abs(numArray[num5, num4]) > num9)
                        {
                            double num12;
                            double num15 = d[num4, 0] - d[num5, 0];
                            if ((Math.Abs(num15) + g) == Math.Abs(num15))
                            {
                                num12 = numArray[num5, num4] / num15;
                            }
                            else
                            {
                                double num10 = (0.5 * num15) / numArray[num5, num4];
                                num12 = 1.0 / (Math.Abs(num10) + Math.Sqrt(1.0 + (num10 * num10)));
                                if (num10 < 0.0)
                                {
                                    num12 = -num12;
                                }
                            }
                            double num17 = 1.0 / Math.Sqrt(1.0 + (num12 * num12));
                            double s = num12 * num17;
                            double tau = s / (1.0 + num17);
                            num15 = num12 * numArray[num5, num4];
                            numArray3[num5] -= num15;
                            numArray3[num4] += num15;
                            d[num5, 0] -= num15;
                            d[num4, 0] += num15;
                            numArray[num5, num4] = 0.0;
                            int num3 = 0;
                            while (num3 <= (num5 - 1))
                            {
                                ROT(g, num15, s, tau, numArray, num3, num5, num3, num4);
                                num3++;
                            }
                            num3 = num5 + 1;
                            while (num3 <= (num4 - 1))
                            {
                                ROT(g, num15, s, tau, numArray, num5, num3, num3, num4);
                                num3++;
                            }
                            num3 = num4 + 1;
                            while (num3 <= num7)
                            {
                                ROT(g, num15, s, tau, numArray, num5, num3, num4, num3);
                                num3++;
                            }
                            for (num3 = 0; num3 <= num7; num3++)
                            {
                                ROT(g, num15, s, tau, v, num3, num5, num3, num4);
                            }
                            num8++;
                        }
                    }
                    num5++;
                }
                for (num5 = 0; num5 <= num7; num5++)
                {
                    numArray2[num5] += numArray3[num5];
                    d[num5, 0] = numArray2[num5];
                    numArray3[num5] = 0.0;
                }
            }
            Console.WriteLine("Too many iterations in routine jacobi");
        }

        public static void Eigen(Matrix Mat, out Matrix d, out Matrix v)
        {
            double[,] numArray;
            double[,] numArray2;
            Eigen(Mat.in_Mat, out numArray, out numArray2);
            d = new Matrix(numArray);
            v = new Matrix(numArray2);
        }

        public override bool Equals(object obj)
        {
            try
            {
                return (this == ((Matrix) obj));
            }
            catch
            {
                return false;
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

        public static double[,] Identity(int n)
        {
            double[,] numArray = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                numArray[i, i] = 1.0;
            }
            return numArray;
        }

        public static double[,] Inverse(double[,] Mat)
        {
            double[,] numArray2;
            int num3;
            int num4;
            try
            {
                Find_R_C(Mat, out num3, out num4);
                numArray2 = (double[,]) Mat.Clone();
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num3 != num4)
            {
                throw new MatrixNotSquare();
            }
            if (Det(Mat) == 0.0)
            {
                throw new MatrixDeterminentZero();
            }
            int num5 = num3;
            int num6 = num4;
            double[,] numArray = new double[num5 + 1, num5 + 1];
            for (int i = 0; i <= num5; i++)
            {
                for (int k = 0; k <= num5; k++)
                {
                    numArray[k, i] = 0.0;
                }
                numArray[i, i] = 1.0;
            }
            for (int j = 0; j <= num5; j++)
            {
                int num10;
                int num11;
                if (Math.Abs(numArray2[j, j]) < 1E-10)
                {
                    num10 = j + 1;
                    while (num10 <= num5)
                    {
                        if ((num10 != j) && (Math.Abs(numArray2[j, num10]) > 1E-10))
                        {
                            num11 = 0;
                            while (num11 <= num5)
                            {
                                numArray2[num11, j] += numArray2[num11, num10];
                                numArray[num11, j] += numArray[num11, num10];
                                num11++;
                            }
                            break;
                        }
                        num10++;
                    }
                }
                double num = 1.0 / numArray2[j, j];
                num10 = 0;
                while (num10 <= num5)
                {
                    numArray2[num10, j] = num * numArray2[num10, j];
                    numArray[num10, j] = num * numArray[num10, j];
                    num10++;
                }
                for (num10 = 0; num10 <= num5; num10++)
                {
                    if (num10 != j)
                    {
                        double num2 = numArray2[j, num10];
                        for (num11 = 0; num11 <= num5; num11++)
                        {
                            numArray2[num11, num10] -= num2 * numArray2[num11, j];
                            numArray[num11, num10] -= num2 * numArray[num11, j];
                        }
                    }
                }
            }
            return numArray;
        }

        public static Matrix Inverse(Matrix Mat)
        {
            return new Matrix(Inverse(Mat.in_Mat));
        }

        public static bool IsEqual(double[,] Mat1, double[,] Mat2)
        {
            int num2;
            int num3;
            int num4;
            int num5;
            double num = 1E-14;
            try
            {
                Find_R_C(Mat1, out num2, out num3);
                Find_R_C(Mat2, out num4, out num5);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if ((num2 != num4) || (num3 != num5))
            {
                throw new MatrixDimensionException();
            }
            for (int i = 0; i <= num2; i++)
            {
                for (int j = 0; j <= num3; j++)
                {
                    if (Math.Abs((double) (Mat1[i, j] - Mat2[i, j])) > num)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsEqual(Matrix Mat1, Matrix Mat2)
        {
            return IsEqual(Mat1.in_Mat, Mat2.in_Mat);
        }

        public static void LU(double[,] Mat, out double[,] L, out double[,] U, out double[,] P)
        {
            double[,] numArray;
            int num2;
            int num4;
            int num5;
            double num8;
            try
            {
                numArray = (double[,]) Mat.Clone();
                Find_R_C(Mat, out num4, out num5);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num4 != num5)
            {
                throw new MatrixNotSquare();
            }
            int index = 0;
            int num7 = num4;
            double num11 = 1E-20;
            int[] numArray2 = new int[num7 + 1];
            double[] numArray3 = new double[num7 * 10];
            double num12 = 1.0;
            int num = 0;
            while (num <= num7)
            {
                num8 = 0.0;
                for (num2 = 0; num2 <= num7; num2++)
                {
                    if (Math.Abs(numArray[num, num2]) > num8)
                    {
                        num8 = Math.Abs(numArray[num, num2]);
                    }
                }
                if (num8 == 0.0)
                {
                    throw new MatrixSingularException();
                }
                numArray3[num] = 1.0 / num8;
                num++;
            }
            num2 = 0;
            while (num2 <= num7)
            {
                int num3;
                double num9;
                double num10;
                if (num2 > 0)
                {
                    num = 0;
                    while (num <= (num2 - 1))
                    {
                        num9 = numArray[num, num2];
                        if (num > 0)
                        {
                            num3 = 0;
                            while (num3 <= (num - 1))
                            {
                                num9 -= numArray[num, num3] * numArray[num3, num2];
                                num3++;
                            }
                            numArray[num, num2] = num9;
                        }
                        num++;
                    }
                }
                num8 = 0.0;
                num = num2;
                while (num <= num7)
                {
                    num9 = numArray[num, num2];
                    if (num2 > 0)
                    {
                        num3 = 0;
                        while (num3 <= (num2 - 1))
                        {
                            num9 -= numArray[num, num3] * numArray[num3, num2];
                            num3++;
                        }
                        numArray[num, num2] = num9;
                    }
                    num10 = numArray3[num] * Math.Abs(num9);
                    if (num10 >= num8)
                    {
                        index = num;
                        num8 = num10;
                    }
                    num++;
                }
                if (num2 != index)
                {
                    for (num3 = 0; num3 <= num7; num3++)
                    {
                        num10 = numArray[index, num3];
                        numArray[index, num3] = numArray[num2, num3];
                        numArray[num2, num3] = num10;
                    }
                    num12 = -num12;
                    numArray3[index] = numArray3[num2];
                }
                numArray2[num2] = index;
                if (num2 != num7)
                {
                    if (numArray[num2, num2] == 0.0)
                    {
                        numArray[num2, num2] = num11;
                    }
                    num10 = 1.0 / numArray[num2, num2];
                    num = num2 + 1;
                    while (num <= num7)
                    {
                        numArray[num, num2] *= num10;
                        num++;
                    }
                }
                num2++;
            }
            if (numArray[num7, num7] == 0.0)
            {
                numArray[num7, num7] = num11;
            }
            int num13 = 0;
            double[,] numArray4 = new double[num7 + 1, num7 + 1];
            double[,] numArray5 = new double[num7 + 1, num7 + 1];
            for (num = 0; num <= num7; num++)
            {
                for (num2 = 0; num2 <= num13; num2++)
                {
                    if (num != 0)
                    {
                        numArray4[num, num2] = numArray[num, num2];
                    }
                    if (num == num2)
                    {
                        numArray4[num, num2] = 1.0;
                    }
                    numArray5[num7 - num, num7 - num2] = numArray[num7 - num, num7 - num2];
                }
                num13++;
            }
            L = numArray4;
            U = numArray5;
            P = Identity(num7 + 1);
            for (num = 0; num <= num7; num++)
            {
                SwapRows(P, num, numArray2[num]);
            }
        }

        public static void LU(Matrix Mat, out Matrix L, out Matrix U, out Matrix P)
        {
            double[,] numArray;
            double[,] numArray2;
            double[,] numArray3;
            LU(Mat.in_Mat, out numArray, out numArray2, out numArray3);
            L = new Matrix(numArray);
            U = new Matrix(numArray2);
            P = new Matrix(numArray3);
        }

        public static double[,] Multiply(double[,] Mat1, double[,] Mat2)
        {
            int num;
            int num2;
            int num3;
            int num4;
            double num5 = 0.0;
            try
            {
                Find_R_C(Mat1, out num, out num2);
                Find_R_C(Mat2, out num3, out num4);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num2 != num3)
            {
                throw new MatrixDimensionException();
            }
            double[,] numArray = new double[num + 1, num4 + 1];
            for (int i = 0; i <= num; i++)
            {
                for (int j = 0; j <= num4; j++)
                {
                    for (int k = 0; k <= num2; k++)
                    {
                        num5 += Mat1[i, k] * Mat2[k, j];
                    }
                    numArray[i, j] = num5;
                    num5 = 0.0;
                }
            }
            return numArray;
        }

        public static Matrix Multiply(Matrix Mat1, Matrix Mat2)
        {
            if ((((Mat1.NoRows == 3) && (Mat2.NoRows == 3)) && (Mat1.NoCols == 1)) && (Mat1.NoCols == 1))
            {
                return new Matrix(CrossProduct(Mat1.in_Mat, Mat2.in_Mat));
            }
            return new Matrix(Multiply(Mat1.in_Mat, Mat2.in_Mat));
        }

        public static double[,] OneD_2_TwoD(double[] Mat)
        {
            int num;
            try
            {
                Find_R_C(Mat, out num);
            }
            catch
            {
                throw new MatrixNullException();
            }
            double[,] numArray = new double[num + 1, 1];
            for (int i = 0; i <= num; i++)
            {
                numArray[i, 0] = Mat[i];
            }
            return numArray;
        } 

        public static Matrix operator +(Matrix Mat1, Matrix Mat2)
        {
            return new Matrix(Add(Mat1.in_Mat, Mat2.in_Mat));
        }

        public static Matrix operator /(Matrix Mat, double Value)
        {
            return new Matrix(ScalarDivide(Value, Mat.in_Mat));
        }

        public static bool operator ==(Matrix Mat1, Matrix Mat2)
        {
            return IsEqual(Mat1.in_Mat, Mat2.in_Mat);
        }

        public static bool operator !=(Matrix Mat1, Matrix Mat2)
        {
            return !IsEqual(Mat1.in_Mat, Mat2.in_Mat);
        }

        public static Matrix operator *(Matrix Mat1, Matrix Mat2)
        {
            if ((((Mat1.NoRows == 3) && (Mat2.NoRows == 3)) && (Mat1.NoCols == 1)) && (Mat1.NoCols == 1))
            {
                return new Matrix(CrossProduct(Mat1.in_Mat, Mat2.in_Mat));
            }
            return new Matrix(Multiply(Mat1.in_Mat, Mat2.in_Mat));
        }

        public static Matrix operator *(Matrix Mat, double Value)
        {
            return new Matrix(ScalarMultiply(Value, Mat.in_Mat));
        }

        public static Matrix operator *(double Value, Matrix Mat)
        {
            return new Matrix(ScalarMultiply(Value, Mat.in_Mat));
        }

        public static Matrix operator -(Matrix Mat1, Matrix Mat2)
        {
            return new Matrix(Subtract(Mat1.in_Mat, Mat2.in_Mat));
        }

        public static double[,] PINV(double[,] Mat)
        {
            int num;
            int num2;
            int num3;
            int num4;
            int num5;
            double num8;
            double[,] numArray5;
            double[,] numArray6;
            double[,] numArray7;
            double num9 = 0.0;
            try
            {
                double[,] numArray = (double[,]) Mat.Clone();
                Find_R_C(Mat, out num2, out num3);
            }
            catch
            {
                throw new MatrixNullException();
            }
            SVD(Mat, out numArray6, out numArray5, out numArray7);
            double num6 = 2.2204E-16;
            num2++;
            num3++;
            double[,] numArray3 = new double[num2, num3];
            double[,] mat = new double[num2, num3];
            double[,] numArray2 = new double[num3, num3];
            double num7 = 0.0;
            for (num = 0; num <= numArray6.GetUpperBound(0); num++)
            {
                if (num == 0)
                {
                    num9 = numArray6[0, 0];
                }
                if (num9 < numArray6[num, num])
                {
                    num9 = numArray6[num, num];
                }
            }
            if (num2 > num3)
            {
                num8 = (num2 * num9) * num6;
            }
            else
            {
                num8 = (num3 * num9) * num6;
            }
            num = 0;
            while (num < num3)
            {
                numArray2[num, num] = numArray6[num, num];
                num++;
            }
            for (num = 0; num < num2; num++)
            {
                num4 = 0;
                while (num4 < num3)
                {
                    num5 = 0;
                    while (num5 < num3)
                    {
                        if (numArray2[num5, num4] > num8)
                        {
                            num7 += numArray5[num, num5] * (1.0 / numArray2[num5, num4]);
                        }
                        num5++;
                    }
                    numArray3[num, num4] = num7;
                    num7 = 0.0;
                    num4++;
                }
            }
            for (num = 0; num < num2; num++)
            {
                for (num4 = 0; num4 < num3; num4++)
                {
                    for (num5 = 0; num5 < num3; num5++)
                    {
                        num7 += numArray3[num, num5] * numArray7[num4, num5];
                    }
                    mat[num, num4] = num7;
                    num7 = 0.0;
                }
            }
            return Transpose(mat);
        }

        public static Matrix PINV(Matrix Mat)
        {
            return new Matrix(PINV(Mat.in_Mat));
        }

        public static string PrintMat(double[,] Mat)
        {
            int num;
            int num2;
            try
            {
                Find_R_C(Mat, out num, out num2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            string str4 = "";
            string str2 = "";
            string str3 = "";
            int[] numArray = new int[num2 + 1];
            for (int i = 0; i <= num; i++)
            {
                for (int j = 0; j <= num2; j++)
                {
                    int num3;
                    string str;
                    int length;
                    if (i == 0)
                    {
                        numArray[j] = 0;
                        for (int k = 0; k <= num; k++)
                        {
                            str = Mat[k, j].ToString("0.0000");
                            length = str.Length;
                            if (numArray[j] < length)
                            {
                                numArray[j] = length;
                                str2 = str;
                            }
                        }
                        if (str2.StartsWith("-"))
                        {
                            numArray[j] = numArray[j];
                        }
                    }
                    str = Mat[i, j].ToString("0.0000");
                    if (str.StartsWith("-"))
                    {
                        length = str.Length;
                        if (numArray[j] >= length)
                        {
                            num3 = 1;
                            while (num3 <= (numArray[j] - length))
                            {
                                str3 = str3 + "  ";
                                num3++;
                            }
                            str3 = str3 + " ";
                        }
                    }
                    else
                    {
                        length = str.Length;
                        if (numArray[j] > length)
                        {
                            for (num3 = 1; num3 <= (numArray[j] - length); num3++)
                            {
                                str3 = str3 + "  ";
                            }
                        }
                    }
                    str3 = str3 + "  " + Mat[i, j].ToString("0.0000");
                }
                if (i != num)
                {
                    str4 = str4 + str3 + "\n";
                    str3 = "";
                }
                str4 = str4 + str3;
                str3 = "";
            }
            return str4;
        }

        public static string PrintMat(Matrix Mat)
        {
            return PrintMat(Mat.in_Mat);
        }

        private static double PYTHAG(double a, double b)
        {
            double num = Math.Abs(a);
            double num2 = Math.Abs(b);
            if (num > num2)
            {
                return (num * Math.Sqrt(1.0 + SQR(num2 / num)));
            }
            return ((num2 == 0.0) ? 0.0 : (num2 * Math.Sqrt(1.0 + SQR(num / num2))));
        }

        public static int Rank(double[,] Mat)
        {
            double[,] numArray;
            double[,] numArray2;
            double[,] numArray3;
            int num = 0;
            try
            {
                int num2;
                int num3;
                Find_R_C(Mat, out num2, out num3);
            }
            catch
            {
                throw new MatrixNullException();
            }
            double num4 = 2.2204E-16;
            SVD(Mat, out numArray, out numArray2, out numArray3);
            for (int i = 0; i <= numArray.GetUpperBound(0); i++)
            {
                if (Math.Abs(numArray[i, i]) > num4)
                {
                    num++;
                }
            }
            return num;
        }

        public static int Rank(Matrix Mat)
        {
            return Rank(Mat.in_Mat);
        }

        private static void ROT(double g, double h, double s, double tau, double[,] a, int i, int j, int k, int l)
        {
            g = a[i, j];
            h = a[k, l];
            a[i, j] = g - (s * (h + (g * tau)));
            a[k, l] = h + (s * (g - (h * tau)));
        }

        public static double[,] ScalarDivide(double Value, double[,] Mat)
        {
            int num3;
            int num4;
            try
            {
                Find_R_C(Mat, out num3, out num4);
            }
            catch
            {
                throw new MatrixNullException();
            }
            double[,] numArray = new double[num3 + 1, num4 + 1];
            for (int i = 0; i <= num3; i++)
            {
                for (int j = 0; j <= num4; j++)
                {
                    numArray[i, j] = Mat[i, j] / Value;
                }
            }
            return numArray;
        }

        public static Matrix ScalarDivide(double Value, Matrix Mat)
        {
            return new Matrix(ScalarDivide(Value, Mat.in_Mat));
        }

        public static double[,] ScalarMultiply(double Value, double[,] Mat)
        {
            int num3;
            int num4;
            try
            {
                Find_R_C(Mat, out num3, out num4);
            }
            catch
            {
                throw new MatrixNullException();
            }
            double[,] numArray = new double[num3 + 1, num4 + 1];
            for (int i = 0; i <= num3; i++)
            {
                for (int j = 0; j <= num4; j++)
                {
                    numArray[i, j] = Mat[i, j] * Value;
                }
            }
            return numArray;
        }

        public static Matrix ScalarMultiply(double Value, Matrix Mat)
        {
            return new Matrix(ScalarMultiply(Value, Mat.in_Mat));
        }

        private static double Sign(double a, double b)
        {
            if (b >= 0.0)
            {
                return Math.Abs(a);
            }
            return -Math.Abs(a);
        }

        public static double[,] SolveLinear(double[,] MatA, double[,] MatB)
        {
            double[,] numArray;
            double[,] numArray2;
            double num;
            int num4;
            int num7;
            int num8;
            double num11;
            try
            {
                numArray = (double[,]) MatA.Clone();
                numArray2 = (double[,]) MatB.Clone();
                Find_R_C(numArray, out num7, out num8);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num7 != num8)
            {
                throw new MatrixNotSquare();
            }
            if ((numArray2.GetUpperBound(0) != num7) || (numArray2.GetUpperBound(1) != 0))
            {
                throw new MatrixDimensionException();
            }
            int index = 0;
            int num10 = num7;
            double num14 = 1E-20;
            int[] numArray3 = new int[num10 + 1];
            double[] numArray4 = new double[num10 * 10];
            double num15 = 1.0;
            int num2 = 0;
            while (num2 <= num10)
            {
                num11 = 0.0;
                for (num4 = 0; num4 <= num10; num4++)
                {
                    if (Math.Abs(numArray[num2, num4]) > num11)
                    {
                        num11 = Math.Abs(numArray[num2, num4]);
                    }
                }
                if (num11 == 0.0)
                {
                    throw new MatrixSingularException();
                }
                numArray4[num2] = 1.0 / num11;
                num2++;
            }
            num4 = 0;
            while (num4 <= num10)
            {
                int num5;
                double num12;
                double num13;
                if (num4 > 0)
                {
                    num2 = 0;
                    while (num2 <= (num4 - 1))
                    {
                        num12 = numArray[num2, num4];
                        if (num2 > 0)
                        {
                            num5 = 0;
                            while (num5 <= (num2 - 1))
                            {
                                num12 -= numArray[num2, num5] * numArray[num5, num4];
                                num5++;
                            }
                            numArray[num2, num4] = num12;
                        }
                        num2++;
                    }
                }
                num11 = 0.0;
                num2 = num4;
                while (num2 <= num10)
                {
                    num12 = numArray[num2, num4];
                    if (num4 > 0)
                    {
                        num5 = 0;
                        while (num5 <= (num4 - 1))
                        {
                            num12 -= numArray[num2, num5] * numArray[num5, num4];
                            num5++;
                        }
                        numArray[num2, num4] = num12;
                    }
                    num13 = numArray4[num2] * Math.Abs(num12);
                    if (num13 >= num11)
                    {
                        index = num2;
                        num11 = num13;
                    }
                    num2++;
                }
                if (num4 != index)
                {
                    for (num5 = 0; num5 <= num10; num5++)
                    {
                        num13 = numArray[index, num5];
                        numArray[index, num5] = numArray[num4, num5];
                        numArray[num4, num5] = num13;
                    }
                    num15 = -num15;
                    numArray4[index] = numArray4[num4];
                }
                numArray3[num4] = index;
                if (num4 != num10)
                {
                    if (numArray[num4, num4] == 0.0)
                    {
                        numArray[num4, num4] = num14;
                    }
                    num13 = 1.0 / numArray[num4, num4];
                    num2 = num4 + 1;
                    while (num2 <= num10)
                    {
                        numArray[num2, num4] *= num13;
                        num2++;
                    }
                }
                num4++;
            }
            if (numArray[num10, num10] == 0.0)
            {
                numArray[num10, num10] = num14;
            }
            int num3 = -1;
            for (num2 = 0; num2 <= num10; num2++)
            {
                int num6 = numArray3[num2];
                num = numArray2[num6, 0];
                numArray2[num6, 0] = numArray2[num2, 0];
                if (num3 != -1)
                {
                    num4 = num3;
                    while (num4 <= (num2 - 1))
                    {
                        num -= numArray[num2, num4] * numArray2[num4, 0];
                        num4++;
                    }
                }
                else if (!(num == 0.0))
                {
                    num3 = num2;
                }
                numArray2[num2, 0] = num;
            }
            for (num2 = num10; num2 >= 0; num2--)
            {
                num = numArray2[num2, 0];
                if (num2 < num10)
                {
                    for (num4 = num2 + 1; num4 <= num10; num4++)
                    {
                        num -= numArray[num2, num4] * numArray2[num4, 0];
                    }
                }
                numArray2[num2, 0] = num / numArray[num2, num2];
            }
            return numArray2;
        }

        public static Matrix SolveLinear(Matrix MatA, Matrix MatB)
        {
            return new Matrix(SolveLinear(MatA.in_Mat, MatB.in_Mat));
        }

        private static double SQR(double a)
        {
            return (a * a);
        }

        public static double[,] Subtract(double[,] Mat1, double[,] Mat2)
        {
            int num3;
            int num4;
            int num5;
            int num6;
            try
            {
                Find_R_C(Mat1, out num3, out num4);
                Find_R_C(Mat2, out num5, out num6);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if ((num3 != num5) || (num4 != num6))
            {
                throw new MatrixDimensionException();
            }
            double[,] numArray = new double[num3 + 1, num4 + 1];
            for (int i = 0; i <= num3; i++)
            {
                for (int j = 0; j <= num4; j++)
                {
                    numArray[i, j] = Mat1[i, j] - Mat2[i, j];
                }
            }
            return numArray;
        }

        public static Matrix Subtract(Matrix Mat1, Matrix Mat2)
        {
            return new Matrix(Subtract(Mat1.in_Mat, Mat2.in_Mat));
        }

        public static void SVD(double[,] Mat_, out double[,] S_, out double[,] U_, out double[,] V_)
        {
            int num;
            int num2;
            int num4;
            int num6;
            int num9;
            int num13;
            int num15;
            double num19;
            double num21;
            double num22;
            try
            {
                Find_R_C(Mat_, out num, out num2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            int num3 = num + 1;
            int num5 = num2 + 1;
            if (num3 < num5)
            {
                num3 = num5;
                num4 = num6 = num5;
            }
            else if (num3 > num5)
            {
                num5 = num3;
                num6 = num4 = num3;
            }
            else
            {
                num4 = num3;
                num6 = num5;
            }
            double[,] numArray2 = new double[num3 + 1, num5 + 1];
            for (int i = 1; i <= (num + 1); i++)
            {
                for (int j = 1; j <= (num2 + 1); j++)
                {
                    numArray2[i, j] = Mat_[i - 1, j - 1];
                }
            }
            double[,] numArray3 = new double[num6 + 1, num6 + 1];
            double[] numArray = new double[num6 + 1];
            double[] numArray7 = new double[100];
            int index = 0;
            int num11 = 0;
            double b = 0.0;
            double num23 = 0.0;
            double num17 = 0.0;
            for (num13 = 1; num13 <= num5; num13++)
            {
                index = num13 + 1;
                numArray7[num13] = num23 * b;
                b = num22 = num23 = 0.0;
                if (num13 <= num3)
                {
                    num9 = num13;
                    while (num9 <= num3)
                    {
                        num23 += Math.Abs(numArray2[num9, num13]);
                        num9++;
                    }
                    if (num23 != 0.0)
                    {
                        num9 = num13;
                        while (num9 <= num3)
                        {
                             numArray2[num9, num13] /= num23;
                            num22 += numArray2[num9, num13] * numArray2[num9, num13];
                            num9++;
                        }
                        num19 = numArray2[num13, num13];
                        b = -Sign(Math.Sqrt(num22), num19);
                        num21 = (num19 * b) - num22;
                        numArray2[num13, num13] = num19 - b;
                        if (num13 != num5)
                        {
                            num15 = index;
                            while (num15 <= num5)
                            {
                                num22 = 0.0;
                                num9 = num13;
                                while (num9 <= num3)
                                {
                                    num22 += numArray2[num9, num13] * numArray2[num9, num15];
                                    num9++;
                                }
                                num19 = num22 / num21;
                                num9 = num13;
                                while (num9 <= num3)
                                {
                                     numArray2[num9, num15] += num19 * numArray2[num9, num13];
                                    num9++;
                                }
                                num15++;
                            }
                        }
                        num9 = num13;
                        while (num9 <= num3)
                        {
                             numArray2[num9, num13]*= num23;
                            num9++;
                        }
                    }
                }
                numArray[num13] = num23 * b;
                b = num22 = num23 = 0.0;
                if ((num13 <= num3) && (num13 != num5))
                {
                    num9 = index;
                    while (num9 <= num5)
                    {
                        num23 += Math.Abs(numArray2[num13, num9]);
                        num9++;
                    }
                    if (num23 != 0.0)
                    {
                        num9 = index;
                        while (num9 <= num5)
                        {
                            numArray2[num13, num9] /= num23;
                            num22 += numArray2[num13, num9] * numArray2[num13, num9];
                            num9++;
                        }
                        num19 = numArray2[num13, index];
                        b = -Sign(Math.Sqrt(num22), num19);
                        num21 = (num19 * b) - num22;
                        numArray2[num13, index] = num19 - b;
                        num9 = index;
                        while (num9 <= num5)
                        {
                            numArray7[num9] = numArray2[num13, num9] / num21;
                            num9++;
                        }
                        if (num13 != num3)
                        {
                            num15 = index;
                            while (num15 <= num3)
                            {
                                num22 = 0.0;
                                num9 = index;
                                while (num9 <= num5)
                                {
                                    num22 += numArray2[num15, num9] * numArray2[num13, num9];
                                    num9++;
                                }
                                num9 = index;
                                while (num9 <= num5)
                                {
                                    numArray2[num15, num9]+= num22 * numArray7[num9];
                                    num9++;
                                }
                                num15++;
                            }
                        }
                        num9 = index;
                        while (num9 <= num5)
                        {
                            numArray2[num13, num9] *= num23;
                            num9++;
                        }
                    }
                }
                num17 = Math.Max(num17, Math.Abs(numArray[num13]) + Math.Abs(numArray7[num13]));
            }
            for (num13 = num5; num13 >= 1; num13--)
            {
                if (num13 < num5)
                {
                    if (b != 0.0)
                    {
                        num15 = index;
                        while (num15 <= num5)
                        {
                            numArray3[num15, num13] = (numArray2[num13, num15] / numArray2[num13, index]) / b;
                            num15++;
                        }
                        num15 = index;
                        while (num15 <= num5)
                        {
                            num22 = 0.0;
                            num9 = index;
                            while (num9 <= num5)
                            {
                                num22 += numArray2[num13, num9] * numArray3[num9, num15];
                                num9++;
                            }
                            num9 = index;
                            while (num9 <= num5)
                            {
                                 numArray3[num9, num15] += num22 * numArray3[num9, num13];
                                num9++;
                            }
                            num15++;
                        }
                    }
                    num15 = index;
                    while (num15 <= num5)
                    {
                        numArray3[num13, num15] = numArray3[num15, num13] = 0.0;
                        num15++;
                    }
                }
                numArray3[num13, num13] = 1.0;
                b = numArray7[num13];
                index = num13;
            }
            num13 = num5;
            while (num13 >= 1)
            {
                index = num13 + 1;
                b = numArray[num13];
                if (num13 < num5)
                {
                    num15 = index;
                    while (num15 <= num5)
                    {
                        numArray2[num13, num15] = 0.0;
                        num15++;
                    }
                }
                if (b != 0.0)
                {
                    b = 1.0 / b;
                    if (num13 != num5)
                    {
                        num15 = index;
                        while (num15 <= num5)
                        {
                            num22 = 0.0;
                            num9 = index;
                            while (num9 <= num3)
                            {
                                num22 += numArray2[num9, num13] * numArray2[num9, num15];
                                num9++;
                            }
                            num19 = (num22 / numArray2[num13, num13]) * b;
                            num9 = num13;
                            while (num9 <= num3)
                            {
                                 numArray2[num9, num15] += num19 * numArray2[num9, num13];
                                num9++;
                            }
                            num15++;
                        }
                    }
                    num15 = num13;
                    while (num15 <= num3)
                    {
                         numArray2[num15, num13] *= b;
                        num15++;
                    }
                }
                else
                {
                    num15 = num13;
                    while (num15 <= num3)
                    {
                        numArray2[num15, num13] = 0.0;
                        num15++;
                    }
                }
                ++numArray2[num13, num13];
                
                num13--;
            }
            for (num9 = num5; num9 >= 1; num9--)
            {
                for (int k = 1; k <= 30; k++)
                {
                    double num18;
                    double num25;
                    double num26;
                    int num12 = 1;
                    index = num9;
                    while (index >= 1)
                    {
                        num11 = index - 1;
                        if ((Math.Abs(numArray7[index]) + num17) == num17)
                        {
                            num12 = 0;
                            break;
                        }
                        if ((Math.Abs(numArray[num11]) + num17) == num17)
                        {
                            break;
                        }
                        index--;
                    }
                    if (num12 != 0)
                    {
                        num18 = 0.0;
                        num22 = 1.0;
                        num13 = index;
                        while (num13 <= num9)
                        {
                            num19 = num22 * numArray7[num13];
                            if ((Math.Abs(num19) + num17) != num17)
                            {
                                b = numArray[num13];
                                num21 = PYTHAG(num19, b);
                                numArray[num13] = num21;
                                num21 = 1.0 / num21;
                                num18 = b * num21;
                                num22 = -num19 * num21;
                                num15 = 1;
                                while (num15 <= num3)
                                {
                                    num25 = numArray2[num15, num11];
                                    num26 = numArray2[num15, num13];
                                    numArray2[num15, num11] = (num25 * num18) + (num26 * num22);
                                    numArray2[num15, num13] = (num26 * num18) - (num25 * num22);
                                    num15++;
                                }
                            }
                            num13++;
                        }
                    }
                    num26 = numArray[num9];
                    if (index == num9)
                    {
                        if (num26 < 0.0)
                        {
                            numArray[num9] = -num26;
                            num15 = 1;
                            while (num15 <= num5)
                            {
                                numArray3[num15, num9] = -numArray3[num15, num9];
                                num15++;
                            }
                        }
                        break;
                    }
                    if (k == 30)
                    {
                        Console.WriteLine("No convergence in 30 SVDCMP iterations");
                    }
                    double num24 = numArray[index];
                    num11 = num9 - 1;
                    num25 = numArray[num11];
                    b = numArray7[num11];
                    num21 = numArray7[num9];
                    num19 = (((num25 - num26) * (num25 + num26)) + ((b - num21) * (b + num21))) / ((2.0 * num21) * num25);
                    b = PYTHAG(num19, 1.0);
                    num19 = (((num24 - num26) * (num24 + num26)) + (num21 * ((num25 / (num19 + Sign(b, num19))) - num21))) / num24;
                    num18 = num22 = 1.0;
                    num15 = index;
                    while (num15 <= num11)
                    {
                        num13 = num15 + 1;
                        b = numArray7[num13];
                        num25 = numArray[num13];
                        num21 = num22 * b;
                        b = num18 * b;
                        num26 = PYTHAG(num19, num21);
                        numArray7[num15] = num26;
                        num18 = num19 / num26;
                        num22 = num21 / num26;
                        num19 = (num24 * num18) + (b * num22);
                        b = (b * num18) - (num24 * num22);
                        num21 = num25 * num22;
                        num25 *= num18;
                        int num16 = 1;
                        while (num16 <= num5)
                        {
                            num24 = numArray3[num16, num15];
                            num26 = numArray3[num16, num13];
                            numArray3[num16, num15] = (num24 * num18) + (num26 * num22);
                            numArray3[num16, num13] = (num26 * num18) - (num24 * num22);
                            num16++;
                        }
                        num26 = PYTHAG(num19, num21);
                        numArray[num15] = num26;
                        if (!(num26 == 0.0))
                        {
                            num26 = 1.0 / num26;
                            num18 = num19 * num26;
                            num22 = num21 * num26;
                        }
                        num19 = (num18 * b) + (num22 * num25);
                        num24 = (num18 * num25) - (num22 * b);
                        for (num16 = 1; num16 <= num3; num16++)
                        {
                            num25 = numArray2[num16, num15];
                            num26 = numArray2[num16, num13];
                            numArray2[num16, num15] = (num25 * num18) + (num26 * num22);
                            numArray2[num16, num13] = (num26 * num18) - (num25 * num22);
                        }
                        num15++;
                    }
                    numArray7[index] = 0.0;
                    numArray7[num9] = num19;
                    numArray[num9] = num24;
                }
            }
            double[,] numArray5 = new double[num6, num6];
            double[,] numArray6 = new double[num6, num6];
            double[,] numArray4 = new double[num4, num6];
            for (num13 = 1; num13 <= num6; num13++)
            {
                numArray5[num13 - 1, num13 - 1] = numArray[num13];
            }
            S_ = numArray5;
            for (num13 = 1; num13 <= num6; num13++)
            {
                num15 = 1;
                while (num15 <= num6)
                {
                    numArray6[num13 - 1, num15 - 1] = numArray3[num13, num15];
                    num15++;
                }
            }
            V_ = numArray6;
            for (num13 = 1; num13 <= num4; num13++)
            {
                for (num15 = 1; num15 <= num6; num15++)
                {
                    numArray4[num13 - 1, num15 - 1] = numArray2[num13, num15];
                }
            }
            U_ = numArray4;
        }

        public static void SVD(Matrix Mat, out Matrix S, out Matrix U, out Matrix V)
        {
            double[,] numArray;
            double[,] numArray2;
            double[,] numArray3;
            SVD(Mat.in_Mat, out numArray, out numArray2, out numArray3);
            S = new Matrix(numArray);
            U = new Matrix(numArray2);
            V = new Matrix(numArray3);
        }

        private static void SwapRows(double[,] Mat, int Row, int toRow)
        {
            int upperBound = Mat.GetUpperBound(0);
            double[,] numArray = new double[1, upperBound + 1];
            for (int i = 0; i <= upperBound; i++)
            {
                numArray[0, i] = Mat[Row, i];
                Mat[Row, i] = Mat[toRow, i];
                Mat[toRow, i] = numArray[0, i];
            }
        }

        public override string ToString()
        {
            return PrintMat(this.in_Mat);
        }

        public static double[,] Transpose(double[,] Mat)
        {
            int num3;
            int num4;
            try
            {
                Find_R_C(Mat, out num3, out num4);
            }
            catch
            {
                throw new MatrixNullException();
            }
            double[,] numArray = new double[num4 + 1, num3 + 1];
            for (int i = 0; i <= num3; i++)
            {
                for (int j = 0; j <= num4; j++)
                {
                    numArray[j, i] = Mat[i, j];
                }
            }
            return numArray;
        }

        public static Matrix Transpose(Matrix Mat)
        {
            return new Matrix(Transpose(Mat.in_Mat));
        }

        public static double[] TwoD_2_OneD(double[,] Mat)
        {
            int num;
            int num2;
            try
            {
                Find_R_C(Mat, out num, out num2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num2 != 0)
            {
                throw new MatrixDimensionException();
            }
            double[] numArray = new double[num + 1];
            for (int i = 0; i <= num; i++)
            {
                numArray[i] = Mat[i, 0];
            }
            return numArray;
        }

        public static double VectorMagnitude(double[] V)
        {
            int num;
            try
            {
                Find_R_C(V, out num);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if (num != 2)
            {
                throw new VectorDimensionException();
            }
            return Math.Sqrt(((V[0] * V[0]) + (V[1] * V[1])) + (V[2] * V[2]));
        }

        public static double VectorMagnitude(double[,] V)
        {
            int num;
            int num2;
            try
            {
                Find_R_C(V, out num, out num2);
            }
            catch
            {
                throw new MatrixNullException();
            }
            if ((num != 2) || (num2 != 0))
            {
                throw new VectorDimensionException();
            }
            return Math.Sqrt(((V[0, 0] * V[0, 0]) + (V[1, 0] * V[1, 0])) + (V[2, 0] * V[2, 0]));
        }

        public static double VectorMagnitude(Matrix V)
        {
            return VectorMagnitude(V.in_Mat);
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

        public int NoCols
        {
            get
            {
                return (this.in_Mat.GetUpperBound(1) + 1);
            }
            set
            {
                this.in_Mat = new double[this.in_Mat.GetUpperBound(0), value];
            }
        }

        public int NoRows
        {
            get
            {
                return (this.in_Mat.GetUpperBound(0) + 1);
            }
            set
            {
                this.in_Mat = new double[value, this.in_Mat.GetUpperBound(0)];
            }
        }

        public double[,] toArray
        {
            get
            {
                return this.in_Mat;
            }
        }
    }
}

