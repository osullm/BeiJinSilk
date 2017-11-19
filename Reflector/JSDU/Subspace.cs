namespace JSDU
{
    using MatrixLibrary;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class Subspace
    {
        public bool GetEigenValueAndEigenVector(double[,] data, int num, ref double[] eigenvalue, ref double[,] eigenvector)
        {
            try
            {
                int num16;
                int num19;
                double[,] numArray = data;
                double[,] numArray2 = new double[num, num];
                for (int i = 0; i < num; i++)
                {
                    for (int num3 = 0; num3 < num; num3++)
                    {
                        if (i == num3)
                        {
                            numArray2[i, num3] = 1.0;
                        }
                        else
                        {
                            numArray2[i, num3] = 0.0;
                        }
                    }
                }
                double[] numArray3 = new double[num];
                for (int j = 0; j < num; j++)
                {
                    numArray3[j] = 0.0;
                }
                double num5 = 1E-06;
                int num6 = 10;
                int num7 = num;
                for (int k = 0; k < num6; k++)
                {
                    double num12 = 0.0;
                    for (int num14 = 0; num14 < (num7 - 1); num14++)
                    {
                        for (int num15 = num14 + 1; num15 < num7; num15++)
                        {
                            if (Math.Abs(numArray[num14, num15]) > num12)
                            {
                                num12 = Math.Abs(numArray[num14, num15]);
                            }
                            if (Math.Abs(numArray[num14, num15]) > num5)
                            {
                                double num9;
                                double num8 = (0.5 * (numArray[num15, num15] - numArray[num14, num14])) / numArray[num14, num15];
                                if (num8 >= 0.0)
                                {
                                    num9 = -num8 + Math.Sqrt(1.0 + (num8 * num8));
                                }
                                else
                                {
                                    num9 = -num8 - Math.Sqrt(1.0 + (num8 * num8));
                                }
                                double num10 = 1.0 / Math.Sqrt(1.0 + (num9 * num9));
                                double num11 = num9 * num10;
                                num16 = 0;
                                while (num16 < num7)
                                {
                                    double num17 = numArray[num14, num16];
                                    double num18 = numArray[num15, num16];
                                    numArray[num14, num16] = (num10 * num17) - (num11 * num18);
                                    numArray[num15, num16] = (num11 * num17) + (num10 * num18);
                                    num16++;
                                }
                                num19 = 0;
                                while (num19 < num7)
                                {
                                    double num20 = numArray[num19, num14];
                                    double num21 = numArray[num19, num15];
                                    numArray[num19, num14] = (num10 * num20) - (num11 * num21);
                                    numArray[num19, num15] = (num11 * num20) + (num10 * num21);
                                    num19++;
                                }
                                for (int num22 = 0; num22 < num7; num22++)
                                {
                                    double num23 = numArray2[num22, num14];
                                    double num24 = numArray2[num22, num15];
                                    numArray2[num22, num14] = (num10 * num23) - (num11 * num24);
                                    numArray2[num22, num15] = (num11 * num23) + (num10 * num24);
                                }
                            }
                        }
                    }
                    if (num12 < num5)
                    {
                        break;
                    }
                }
                for (int m = 0; m < num7; m++)
                {
                    numArray3[m] = numArray[m, m];
                }
                double[] numArray4 = new double[num7];
                for (num16 = 1; num16 < num7; num16++)
                {
                    num19 = num16;
                    double num26 = numArray3[num16];
                    int index = 0;
                    while (index < num7)
                    {
                        numArray4[index] = numArray2[index, num16];
                        index++;
                    }
                    while ((num19 > 0) && (numArray3[num19 - 1] < num26))
                    {
                        numArray3[num19] = numArray3[num19 - 1];
                        for (index = 0; index < num7; index++)
                        {
                            numArray2[index, num19] = numArray2[index, num19 - 1];
                        }
                        num19--;
                    }
                    numArray3[num19] = num26;
                    for (int num28 = 0; num28 < num7; num28++)
                    {
                        numArray2[num28, num19] = numArray4[num28];
                    }
                }
                for (int n = 0; n < num; n++)
                {
                    for (int num30 = 0; num30 < num; num30++)
                    {
                        eigenvector[n, num30] = numArray2[n, num30];
                    }
                }
                for (int num31 = 0; num31 < num; num31++)
                {
                    eigenvalue[num31] = numArray3[num31];
                }
                return true;
            }
            catch
            {
                MessageBox.Show("计算特征值特征向量过程出现错误");
                return false;
            }
        }

        public void GetSubspaceAngle(double[,] A, double[,] B, ref double theta1)
        {
            int num12;
            int sum = 0;
            int num2 = 0;
            double[,] numArray = this.orth(A, out sum);
            double[,] numArray2 = this.orth(B, out num2);
            int length = numArray.GetLength(0);
            int num4 = numArray2.GetLength(0);
            int num5 = numArray.GetLength(1);
            int num6 = numArray2.GetLength(1);
            if (num5 < num6)
            {
                double[,] numArray3 = (double[,]) numArray.Clone();
                numArray = numArray2;
                numArray2 = numArray3;
            }
            int num7 = numArray.GetLength(1);
            int noCols = numArray2.GetLength(1);
            int noRows = numArray.GetLength(0);
            int num10 = numArray2.GetLength(0);
            Matrix mat = new Matrix(noRows, 1);
            Matrix matrix2 = new Matrix(num10, noCols);
            int num11 = 0;
            while (num11 < num10)
            {
                for (num12 = 0; num12 < noCols; num12++)
                {
                    matrix2[num11, num12] = numArray2[num11, num12];
                }
                num11++;
            }
            try
            {
                double num14;
                for (num12 = 0; num12 < num7; num12++)
                {
                    for (num11 = 0; num11 < noRows; num11++)
                    {
                        mat[num11, 0] = numArray[num11, num12];
                    }
                    matrix2 -= mat * (Matrix.Transpose(mat) * matrix2);
                }
                Matrix matrix3 = Matrix.Transpose(matrix2) * matrix2;
                double[,] toArray = matrix3.toArray;
                int num = toArray.GetLength(1);
                double[,] eigenvector = new double[num, num];
                double[] eigenvalue = new double[num];
                double[,] data = (double[,]) toArray.Clone();
                if (this.GetEigenValueAndEigenVector(data, num, ref eigenvalue, ref eigenvector))
                {
                    num14 = Math.Sqrt(eigenvalue[0]);
                }
                else
                {
                    MessageBox.Show("求取特征值与特征向量失败，请检查输入的子空间矩阵");
                    num14 = 2.0;
                }
                if (num14 <= 1.0)
                {
                    theta1 = Math.Asin(num14);
                }
                else
                {
                    theta1 = Math.Asin(1.0);
                }
            }
            catch
            {
                MessageBox.Show("输入矩阵有误，计算出现错误，请检查");
            }
        }

        public double[,] orth(double[,] mat, out int sum)
        {
            int num3;
            int num4;
            int length = mat.GetLength(0);
            int noCols = mat.GetLength(1);
            Matrix matrix = new Matrix(length, noCols);
            for (num3 = 0; num3 < length; num3++)
            {
                num4 = 0;
                while (num4 < noCols)
                {
                    matrix[num3, num4] = mat[num3, num4];
                    num4++;
                }
            }
            Matrix matrix2 = Matrix.Transpose(matrix) * matrix;
            double[,] eigenvector = new double[noCols, noCols];
            double[] eigenvalue = new double[noCols];
            double[,] numArray3 = new double[noCols, noCols];
            for (num3 = 0; num3 < noCols; num3++)
            {
                num4 = 0;
                while (num4 < noCols)
                {
                    numArray3[num3, num4] = matrix2[num3, num4];
                    num4++;
                }
            }
            double[,] data = (double[,]) numArray3.Clone();
            if (!this.GetEigenValueAndEigenVector(data, noCols, ref eigenvalue, ref eigenvector))
            {
                MessageBox.Show("特征值求取有误");
                Application.Exit();
            }
            Matrix matrix3 = new Matrix(noCols, noCols);
            for (num3 = 0; num3 < noCols; num3++)
            {
                num4 = 0;
                while (num4 < noCols)
                {
                    matrix3[num3, num4] = eigenvector[num3, num4];
                    num4++;
                }
            }
            Matrix matrix4 = new Matrix(noCols, noCols);
            for (num3 = 0; num3 < noCols; num3++)
            {
                num4 = 0;
                while (num4 < noCols)
                {
                    if ((num3 == num4) && (eigenvalue[num3] > 1E-05))
                    {
                        matrix4[num3, num4] = 1.0 / Math.Sqrt(eigenvalue[num3]);
                    }
                    else
                    {
                        matrix4[num3, num4] = 0.0;
                    }
                    num4++;
                }
            }
            int num5 = 0;
            for (num3 = 0; num3 < noCols; num3++)
            {
                if (eigenvalue[num3] <= 1E-05)
                {
                    break;
                }
                num5++;
            }
            sum = num5;
            Matrix matrix5 = new Matrix(noCols, num5);
            Matrix matrix6 = new Matrix(num5, num5);
            for (num3 = 0; num3 < noCols; num3++)
            {
                num4 = 0;
                while (num4 < num5)
                {
                    matrix5[num3, num4] = eigenvector[num3, num4];
                    num4++;
                }
            }
            for (num3 = 0; num3 < num5; num3++)
            {
                for (num4 = 0; num4 < num5; num4++)
                {
                    matrix6[num3, num4] = matrix4[num3, num4];
                }
            }
            Matrix matrix7 = (matrix * matrix5) * matrix6;
            return matrix7.toArray;
        }
    }
}

