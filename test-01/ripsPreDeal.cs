namespace test_01
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class ripsPreDeal
    {
        private int m;
        private int n;
        private ProgressBar pgv;
        private double[,] rip;
        private int value;

        public ripsPreDeal()
        {
        }

        public ripsPreDeal(int n, int m)
        {
            this.n = n;
            this.m = m;
            this.rip = new double[n, m];
        }



        public double avg(int n, double[] x)
        {
            double num = 0.0;
            for (int i = 0; i < n; i++)
            {
                num += x[i];
            }
            return (num / ((double) n));
        }

        public double[] avgRipsMatrix(double[,] ripsMatrix)
        {
            double[] numArray = new double[this.m];
            double[] x = new double[this.n];
            for (int i = 0; i < this.m; i++)
            {
                for (int j = 0; j < this.n; j++)
                {
                    x[j] = ripsMatrix[j, i];
                }
                numArray[i] = this.avg(this.n, x);
            }
            return numArray;
        }



        public double[,] conMatrix(int n, int m, double[,] ripsMatrix)
        {
            double[,] numArray = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    numArray[i, j] = ripsMatrix[j, i];
                }
            }
            return numArray;
        }



      
        /// <summary>
        /// K-S 分选计算集和验证集
        /// </summary>
        /// <param name="NC"></param>
        /// <param name="X"></param>
        /// <param name="NoC"></param>
        /// <param name="NoV"></param>
        public void KennardStone(int NC, double[,] X, out int[] NoC, out int[] NoV)
        {
            double num6;
            int length = X.GetLength(0);
            int num2 = X.GetLength(1);
            NoC = new int[NC];
            NoV = new int[length - NC];
            ArrayList list = new ArrayList();
            double[] numArray = this.avgRipsMatrix(X);
            double[] numArray2 = new double[num2];
            double[] array = new double[length];
            for (int i = 0; i < length; i++)
            {
                for (int n = 0; n < num2; n++)
                {
                    numArray2[n] = (X[i, n] - numArray[n]) * (X[i, n] - numArray[n]);
                }
                for (int num5 = 0; num5 < num2; num5++)
                {
                    array[i] += numArray2[num5];
                }
            }
            int num7 = this.MinValue(array, out num6);
            NoC[0] = num7;
            list.Add(num7);
            for (int j = 0; j < length; j++)
            {
                for (int num9 = 0; num9 < num2; num9++)
                {
                    numArray2[num9] = (X[j, num9] - X[num7, num9]) * (X[j, num9] - X[num7, num9]);
                }
                for (int num10 = 0; num10 < num2; num10++)
                {
                    array[j] += numArray2[num10];
                }
            }
            num7 = this.MaxValue(array, out num6);
            NoC[1] = num7;
            list.Add(num7);
            double[] numArray5 = new double[length];
            for (int k = 2; k < NC; k++)
            {
                for (int num12 = 0; num12 < length; num12++)
                {
                    double[] numArray4 = new double[k];
                    if (list.IndexOf(num12) != -1)
                    {
                        numArray5[num12] = -1E+32;
                    }
                    else
                    {
                        for (int num13 = 0; num13 < k; num13++)
                        {
                            for (int num14 = 0; num14 < num2; num14++)
                            {
                                numArray2[num14] = (X[num12, num14] - X[NoC[num13], num14]) * (X[num12, num14] - X[NoC[num13], num14]);
                            }
                            for (int num15 = 0; num15 < num2; num15++)
                            {
                                numArray4[num13] += numArray2[num15];
                            }
                        }
                        num7 = this.MinValue(numArray4, out num6);
                        numArray5[num12] = num6;
                    }
                }
                num7 = this.MaxValue(numArray5, out num6);
                NoC[k] = num7;
                list.Add(num7);
                numArray5 = new double[length];
            }
            int num16 = 0;
            for (int m = 0; m < length; m++)
            {
                if ((list.IndexOf(m) == -1) && (num16 < ((length - NC) + 1)))
                {
                    NoV[num16++] = m;
                }
            }
        }

        /// <summary>
        /// 马氏距离计算第二步
        /// </summary>
        /// <param name="n">样品数量</param>
        /// <param name="f">PCA特征数量</param>
        /// <param name="t"></param>
        /// <param name="rm"></param>
        /// <param name="md">返回的马氏距离</param>
        public void maDistance(int n, int f, double[,] t, out double[,] rm, out double[] md)
        {
            md = new double[n];
            rm = new double[f, f];
            double[,] ripsMatrix = new double[n, f];
            double[,] x = new double[f, n];
            double[,] numArray3 = new double[f, f];
            double[,] numArray4 = new double[1, f];
            double[,] c = new double[f, 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < f; j++)
                {
                    ripsMatrix[i, j] = t[i, j];
                }
            }
            x = this.conMatrix(n, f, ripsMatrix);
            numArray3 = this.multiMatrix(f, n, n, f, x, ripsMatrix);
            if (!this.reverseMatrix(f, f, numArray3, out rm))
            {
                for (int k = 0; k < n; k++)
                {
                    for (int m = 0; m < f; m++)
                    {
                        numArray4[0, m] = ripsMatrix[k, m];
                    }
                    c = this.conMatrix(1, f, numArray4);
                    numArray4 = this.multiMatrix(1, f, f, f, numArray4, rm);
                    md[k] = this.mulRowCol(f, f, numArray4, c);
                }
            }
        }

        /// <summary>
        /// 计算马氏距离，out 马氏距离，
        /// </summary>
        /// <param name="data">穿进去计算马氏距离的数据double[,]</param>
        /// <param name="threshold">马氏距离的阈值</param>
        /// <param name="md">out 返回的马氏距离</param>
        /// <returns>返回大于阈值的序号</returns>
        public int[] maDistanceAbnormalIndex(double[,] data, double threshold, out double[] md)
        {
            md = new double[data.GetLength(0)];
            ArrayList list = new ArrayList();
            try
            {
                double[,] numArray;
                double[,] numArray2;
                double[,] numArray3;
                double[,] numArray4;
                //计算PCA
                this.pca(data.GetLength(0), data.GetLength(1), data, 10, out numArray, out numArray2, out numArray3);
                this.maDistance(data.GetLength(0), 10, numArray2, out numArray4, out md);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            list.Clear();
            for (int i = 0; i < md.Length; i++)
            {
                if (md[i] > threshold)
                {
                    list.Add(i);
                }
            }
            int[] numArray5 = new int[list.Count];
            for (int j = 0; j < list.Count; j++)
            {
                numArray5[j] = (int) list[j];
            }
            return numArray5;
        }

        public int MaxValue(double[] array, out double max)
        {
            max = 0.0;
            int num = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (max < array[i])
                {
                    max = array[i];
                    num = i;
                }
            }
            return num;
        }

        public double MeanValue(double[] array)
        {
            double num = 0.0;
            for (int i = 0; i < array.Length; i++)
            {
                num += array[i];
            }
            return (num / ((double) array.Length));
        }

        public void min_max(int n, double[] x, out double min, out double max)
        {
            min = x[0];
            max = x[0];
            for (int i = 1; i < n; i++)
            {
                if (x[i] < min)
                {
                    min = x[i];
                }
                else if (x[i] > max)
                {
                    max = x[i];
                }
            }
        }

        public int MinValue(double[] array, out double min)
        {
            min = 1E+32;
            int num = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (min > array[i])
                {
                    min = array[i];
                    num = i;
                }
            }
            return num;
        }

        public void mlr(int n, int m, double[,] regressX, double[,] y, out double[,] b)
        {
            double[,] numArray3;
            double[,] x = this.conMatrix(n, m, regressX);
            double[,] numArray2 = this.multiMatrix(m, n, n, m, x, regressX);
            b = new double[m, 1];
            if (!this.reverseMatrix(m, m, numArray2, out numArray3))
            {
                double[,] numArray4 = this.multiMatrix(m, m, m, n, numArray3, x);
                b = this.multiMatrix(m, n, n, 1, numArray4, y);
            }
        }

        public double mulRowCol(int col, int row, double[,] r, double[,] c)
        {
            double num = 0.0;
            if (row == col)
            {
                for (int i = 0; i < row; i++)
                {
                    num += r[0, i] * c[i, 0];
                }
            }
            return num;
        }

        public double[,] multiMatrix(int row1, int col1, int row2, int col2, double[,] x, double[,] y)
        {
            double[,] numArray = new double[row1, col2];
            if (col1 == row2)
            {
                for (int i = 0; i < row1; i++)
                {
                    for (int j = 0; j < col2; j++)
                    {
                        numArray[i, j] = 0.0;
                        for (int k = 0; k < row2; k++)
                        {
                            numArray[i, j] += x[i, k] * y[k, j];
                        }
                    }
                }
            }
            return numArray;
        }

        /// <summary>
        /// 计算PCA
        /// </summary>
        /// <param name="n">regressX的getlength（0），样品数量</param>
        /// <param name="m">regressX的getlength（1），特征数量</param>
        /// <param name="regressX">计算的data</param>
        /// <param name="maxrank">maxrank=10,10个主成分</param>
        /// <param name="w"></param>
        /// <param name="t"></param>
        /// <param name="lamd"></param>
        public void pca(int n, int m, double[,] regressX, int maxrank, out double[,] w, out double[,] t, out double[,] lamd)
        {
            double[,] y = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    y[i, j] = regressX[i, j];
                }
            }
            double[,] r = new double[1, n];
            double num3 = 0.0;
            int num4 = 0;
            double[,] ripsMatrix = new double[1, m];
            double[,] numArray4 = new double[1, m];
            double[,] numArray5 = new double[n, m];
            double[,] numArray6 = new double[n, 1];
            int num5 = 0;
            double d = 0.0;
            double[,] c = new double[m, 1];
            double[,] numArray8 = new double[n, 1];
            double[,] numArray9 = new double[n, 1];

            w = new double[maxrank, m];
            t = new double[n, maxrank];
            lamd = new double[1, maxrank];

            double num7 = 0.0;
            double num8 = 0.0;
            for (num4 = 0; num4 < maxrank; num4++)
            {
                for (int k = 0; k < n; k++)
                {
                    numArray9[k, 0] = 1.0;
                }
                num8 = 0.0;
                for (num5 = 0; num5 < 100; num5++)
                {
                    r = this.conMatrix(n, 1, numArray9);
                    num3 = this.mulRowCol(n, n, r, numArray9);
                    numArray4 = this.multiMatrix(1, n, n, m, r, y);
                    if (num3 != 0.0)
                    {
                        for (int num10 = 0; num10 < m; num10++)
                        {
                            w[num4, num10] = numArray4[0, num10] / num3;
                        }
                    }
                    d = 0.0;
                    for (int num11 = 0; num11 < m; num11++)
                    {
                        d += w[num4, num11] * w[num4, num11];
                    }
                    d = Math.Sqrt(d);
                    if (d != 0.0)
                    {
                        for (int num12 = 0; num12 < m; num12++)
                        {
                            w[num4, num12] /= d;
                            ripsMatrix[0, num12] = w[num4, num12];
                        }
                    }
                    c = this.conMatrix(1, m, ripsMatrix);
                    num3 = this.mulRowCol(m, m, ripsMatrix, c);
                    numArray8 = this.multiMatrix(n, m, m, 1, y, c);
                    if (num3 != 0.0)
                    {
                        for (int num13 = 0; num13 < n; num13++)
                        {
                            numArray6[num13, 0] = numArray8[num13, 0] / num3;
                        }
                    }
                    d = 0.0;
                    for (int num14 = 0; num14 < n; num14++)
                    {
                        d += (numArray6[num14, 0] - numArray9[num14, 0]) * (numArray6[num14, 0] - numArray9[num14, 0]);
                    }
                    num7 = Math.Sqrt(d);
                    for (int num15 = 0; num15 < n; num15++)
                    {
                        numArray9[num15, 0] = numArray6[num15, 0];
                    }
                    if (num7 == num8)
                    {
                        break;
                    }
                    num8 = num7;
                    if (num7 < 1E-15)
                    {
                        break;
                    }
                }
                r = this.conMatrix(n, 1, numArray6);
                num3 = this.mulRowCol(n, n, r, numArray6);
                numArray4 = this.multiMatrix(1, n, n, m, r, y);
                if (num3 != 0.0)
                {
                    for (int num16 = 0; num16 < m; num16++)
                    {
                        w[num4, num16] = numArray4[0, num16] / num3;
                        ripsMatrix[0, num16] = w[num4, num16];
                    }
                }
                lamd[0, num4] = num3;
                for (int num17 = 0; num17 < n; num17++)
                {
                    t[num17, num4] = numArray6[num17, 0];
                }
                numArray5 = this.multiMatrix(n, 1, 1, m, numArray6, ripsMatrix);
                y = this.subMatrix(n, m, n, m, y, numArray5);
            }
        }
        /// <summary>
        /// 随机法--分选计算集和验证集
        /// </summary>
        /// <param name="NC"></param>
        /// <param name="X"></param>
        /// <param name="NoC"></param>
        /// <param name="NoV"></param>
        public void RandomSet(int NC, double[,] X, out int[] NoC, out int[] NoV)
        {
            int length = X.GetLength(0);
            X.GetLength(1);
            NoC = new int[NC];
            NoV = new int[length - NC];
            List<int> list = this.GenerateRandom(length - 1, NC);
            for (int i = 0; i < list.Count; i++)
            {
                NoC[i] = list[i];
            }
            int num3 = 0;
            for (int j = 0; j < length; j++)
            {
                if ((list.IndexOf(j) == -1) && (num3 < ((length - NC) + 1)))
                {
                    NoV[num3++] = j;
                }
            }
        }
        /// <summary>
        /// 随机发生成数组
        /// </summary>
        /// <param name="iMax"></param>
        /// <param name="iNum"></param>
        /// <returns></returns>
        private List<int> GenerateRandom(int iMax, int iNum)
        {
            long ticks = DateTime.Now.Ticks;
            List<int> list = new List<int>();
            for (int i = 0; i < iNum; i++)
            {
                int item = new Random(((int)ticks) * i).Next(iMax);
                if (list.IndexOf(item) > -1)
                {
                    i--;
                }
                else
                {
                    list.Add(item);
                }
                ticks += new Random((int)ticks).Next(0x3d2);
            }
            return list;
        }

        public bool reverseMatrix(int n, int m, double[,] x, out double[,] rip)
        {
            int num3;
            int num4;
            int num5;
            rip = x;
            if (n != m)
            {
                return true;
            }
            int[] numArray = new int[n];
            int[] numArray2 = new int[n];
            double[] numArray3 = new double[n];
            for (num5 = 0; num5 < n; num5++)
            {
                double num = 0.0;
                num3 = num5;
                while (num3 < n)
                {
                    num4 = num5;
                    while (num4 < n)
                    {
                        double num2 = Math.Abs(rip[num3, num4]);
                        if (num2 > num)
                        {
                            num = num2;
                            numArray[num5] = num3;
                            numArray2[num5] = num4;
                        }
                        num4++;
                    }
                    num3++;
                }
                if ((num + 1.0) == 1.0)
                {
                    return true;
                }
                if (numArray[num5] != num5)
                {
                    num4 = 0;
                    while (num4 < n)
                    {
                        numArray3[num4] = rip[num5, num4];
                        rip[num5, num4] = rip[numArray[num5], num4];
                        rip[numArray[num5], num4] = numArray3[num4];
                        num4++;
                    }
                }
                if (numArray2[num5] != num5)
                {
                    num3 = 0;
                    while (num3 < n)
                    {
                        numArray3[num3] = rip[num3, num5];
                        rip[num3, num5] = rip[num3, numArray2[num5]];
                        rip[num3, numArray2[num5]] = numArray3[num3];
                        num3++;
                    }
                }
                rip[num5, num5] = 1.0 / rip[num5, num5];
                num4 = 0;
                while (num4 < n)
                {
                    if (num4 != num5)
                    {
                        rip[num5, num4] *= rip[num5, num5];
                    }
                    num4++;
                }
                num3 = 0;
                while (num3 < n)
                {
                    if (num3 != num5)
                    {
                        num4 = 0;
                        while (num4 < n)
                        {
                            if (num4 != num5)
                            {
                                rip[num3, num4] -= rip[num3, num5] * rip[num5, num4];
                            }
                            num4++;
                        }
                    }
                    num3++;
                }
                num3 = 0;
                while (num3 < n)
                {
                    if (num3 != num5)
                    {
                        rip[num3, num5] = -(rip[num3, num5] * rip[num5, num5]);
                    }
                    num3++;
                }
            }
            for (num5 = n - 1; num5 >= 0; num5--)
            {
                if (numArray2[num5] != num5)
                {
                    for (num4 = 0; num4 < n; num4++)
                    {
                        numArray3[num4] = rip[num5, num4];
                        rip[num5, num4] = rip[numArray2[num5], num4];
                        rip[numArray2[num5], num4] = numArray3[num4];
                    }
                }
                if (numArray[num5] != num5)
                {
                    for (num3 = 0; num3 < n; num3++)
                    {
                        numArray3[num3] = rip[num3, num5];
                        rip[num3, num5] = rip[num3, numArray[num5]];
                        rip[num3, numArray[num5]] = numArray3[num3];
                    }
                }
            }
            return false;
        }

        public double[,] snv(double[,] ripsMatrix)
        {
            double[] numArray = new double[this.n];
            double[,] numArray2 = new double[this.n, this.m];
            double[] x = new double[this.m];
            for (int i = 0; i < this.n; i++)
            {
                for (int m = 0; m < this.m; m++)
                {
                    x[m] = ripsMatrix[i, m];
                }
                numArray[i] = this.avg(this.m, x);
                for (int n = 0; n < this.m; n++)
                {
                    numArray2[i, n] = ripsMatrix[i, n] - numArray[i];
                }
            }
            double[] numArray4 = new double[this.n];
            double[] numArray5 = new double[this.n];
            for (int j = 0; j < this.n; j++)
            {
                for (int num5 = 0; num5 < this.m; num5++)
                {
                    numArray4[j] += numArray2[j, num5] * numArray2[j, num5];
                }
                numArray5[j] = Math.Sqrt(numArray4[j] / ((double) (this.m - 1)));
            }
            for (int k = 0; k < this.n; k++)
            {
                for (int num7 = 0; num7 < this.m; num7++)
                {
                    if (Math.Abs(numArray5[k]) < 1E-58)
                    {
                        this.rip[k, num7] = ripsMatrix[k, num7];
                    }
                    else
                    {
                        this.rip[k, num7] = numArray2[k, num7] / numArray5[k];
                    }
                }
            }
            return this.rip;
        }



        public int[] SortIndex(double[] array)
        {
            int[] numArray = new int[array.Length];
            double[] numArray2 = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                numArray2[i] = array[i];
            }
            double num2 = 0.0;
            for (int j = 1; j < numArray2.Length; j++)
            {
                for (int m = 0; m < (numArray2.Length - j); m++)
                {
                    if (numArray2[m] < numArray2[m + 1])
                    {
                        num2 = numArray2[m];
                        numArray2[m] = numArray2[m + 1];
                        numArray2[m + 1] = num2;
                    }
                }
            }
            for (int k = 0; k < array.Length; k++)
            {
                numArray[k] = Array.IndexOf<double>(array, numArray2[k]);
            }
            return numArray;
        }


        public double[] ssRipsMatrix(double[,] ripsMatrix)
        {
            double[] numArray = new double[this.m];
            double[] numArray2 = this.avgRipsMatrix(ripsMatrix);
            double[] numArray3 = new double[this.n];
            double[] numArray4 = new double[this.m];
            for (int i = 0; i < this.m; i++)
            {
                for (int j = 0; j < this.n; j++)
                {
                    numArray3[j] = (ripsMatrix[j, i] - numArray2[i]) * (ripsMatrix[j, i] - numArray2[i]);
                }
                for (int k = 0; k < this.n; k++)
                {
                    numArray4[i] += numArray3[k];
                }
                numArray[i] = Math.Sqrt(numArray4[i] / ((double) (this.n - 1)));
            }
            return numArray;
        }

        public double StdError(double[] array)
        {
            double num = 0.0;
            double d = 0.0;
            num = this.MeanValue(array);
            for (int i = 0; i < array.Length; i++)
            {
                d += (array[i] - num) * (array[i] - num);
            }
            if (array.Length == 1)
            {
                return Math.Sqrt(d);
            }
            return Math.Sqrt(d / ((double) (array.Length - 1)));
        }

        public double[,] subMatrix(int row1, int col1, int row2, int col2, double[,] x, double[,] y)
        {
            double[,] numArray = new double[row1, col1];
            if ((row1 == row2) || (col1 == col2))
            {
                for (int i = 0; i < row1; i++)
                {
                    for (int j = 0; j < col1; j++)
                    {
                        numArray[i, j] = x[i, j] - y[i, j];
                    }
                }
            }
            return numArray;
        }

        public void updata(object sender, EventArgs e)
        {
            this.pgv.Value = this.value;
        }

        #region 没用到
        public void coeRegress(int n, double[] x, double[] y, out double a, out double b)
        {
            a = 0.0;
            b = 1.0;
            double num = this.avg(n, x);
            double[] numArray = new double[n];
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = this.avg(n, y);
            double[] numArray2 = new double[n];
            for (int i = 0; i < n; i++)
            {
                numArray[i] = x[i] - num;
                numArray2[i] = y[i] - num4;
                num2 += numArray[i] * numArray2[i];
                num3 += numArray[i] * numArray[i];
            }
            if (num3 != 0.0)
            {
                b = num2 / num3;
            }
            a = num4 - (b * num);
        }
        public double[,] addMatrix(int row1, int col1, int row2, int col2, double[,] x, double[,] y)
        {
            double[,] numArray = new double[row1, col1];
            if ((row1 == row2) || (col1 == col2))
            {
                for (int i = 0; i < row1; i++)
                {
                    for (int j = 0; j < col1; j++)
                    {
                        numArray[i, j] = x[i, j] + y[i, j];
                    }
                }
            }
            return numArray;
        }
        public double[,] diff1(double[,] ripsMatrix, int g)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = g - 1; j < (this.m - g); j++)
                {
                    this.rip[i, j] = (ripsMatrix[i, j + g] - ripsMatrix[i, j]) / ((double)g);
                }
                for (int k = 0; k < (g - 1); k++)
                {
                    this.rip[i, k] = 0.0;
                }
                for (int m = this.m - g; m < this.m; m++)
                {
                    this.rip[i, m] = 0.0;
                }
            }
            return this.rip;
        }

        public double[,] diff2(double[,] ripsMatrix, int g)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = g; j < (this.m - g); j++)
                {
                    this.rip[i, j] = ((ripsMatrix[i, j + g] - (2.0 * ripsMatrix[i, j])) + ripsMatrix[i, j - g]) / ((double)(g * g));
                }
                for (int k = 0; k < g; k++)
                {
                    this.rip[i, k] = 0.0;
                }
                for (int m = this.m - g; m < this.m; m++)
                {
                    this.rip[i, m] = 0.0;
                }
            }
            return this.rip;
        }
        public double mlrPrey(double[,] x, double[,] b)
        {
            double num = 0.0;
            for (int i = 0; i < x.GetLength(1); i++)
            {
                num += x[0, i] * b[i, 0];
            }
            return num;
        }

        public double[,] msc(double[,] ripsMatrix, out double[] meanx)
        {
            meanx = this.avgRipsMatrix(ripsMatrix);
            double num = this.avg(this.m, meanx);
            double[] numArray = new double[this.m];
            for (int i = 0; i < this.m; i++)
            {
                numArray[i] = meanx[i] - num;
            }
            double num3 = 0.0;
            for (int j = 0; j < this.m; j++)
            {
                num3 += numArray[j] * numArray[j];
            }
            if (num3 == 0.0)
            {
                for (int k = 0; k < this.n; k++)
                {
                    for (int m = 0; m < this.m; m++)
                    {
                        this.rip[k, m] = ripsMatrix[k, m];
                    }
                }
            }
            else
            {
                double[] numArray2 = new double[this.n];
                double[,] numArray3 = new double[this.n, this.m];
                double[] x = new double[this.m];
                for (int n = 0; n < this.n; n++)
                {
                    for (int num8 = 0; num8 < this.m; num8++)
                    {
                        x[num8] = ripsMatrix[n, num8];
                    }
                    numArray2[n] = this.avg(this.m, x);
                    for (int num9 = 0; num9 < this.m; num9++)
                    {
                        numArray3[n, num9] = ripsMatrix[n, num9] - numArray2[n];
                    }
                }
                double[] numArray5 = new double[this.n];
                for (int num10 = 0; num10 < this.n; num10++)
                {
                    for (int num11 = 0; num11 < this.m; num11++)
                    {
                        numArray5[num10] += numArray[num11] * numArray3[num10, num11];
                    }
                }
                double[] numArray6 = new double[this.n];
                bool[] flagArray = new bool[this.n];
                for (int num12 = 0; num12 < this.n; num12++)
                {
                    numArray6[num12] = numArray5[num12] / num3;
                    if (Math.Abs(numArray6[num12]) < 1E-58)
                    {
                        flagArray[num12] = true;
                    }
                }
                double[] numArray7 = new double[this.n];
                for (int num13 = 0; num13 < this.n; num13++)
                {
                    numArray7[num13] = numArray2[num13] - (numArray6[num13] * num);
                }
                for (int num14 = 0; num14 < this.n; num14++)
                {
                    if (flagArray[num14])
                    {
                        for (int num15 = 0; num15 < this.m; num15++)
                        {
                            this.rip[num14, num15] = ripsMatrix[num14, num15];
                        }
                    }
                    else
                    {
                        for (int num16 = 0; num16 < this.m; num16++)
                        {
                            this.rip[num14, num16] = (ripsMatrix[num14, num16] - numArray7[num14]) / numArray6[num14];
                        }
                    }
                }
            }
            return this.rip;
        }

        public double[,] mscPredict(double[,] ripsMatrix, double[] meanx)
        {
            double num = this.avg(this.m, meanx);
            double[] numArray = new double[this.m];
            for (int i = 0; i < this.m; i++)
            {
                numArray[i] = meanx[i] - num;
            }
            double num3 = 0.0;
            for (int j = 0; j < this.m; j++)
            {
                num3 += numArray[j] * numArray[j];
            }
            if (num3 == 0.0)
            {
                for (int k = 0; k < this.n; k++)
                {
                    for (int m = 0; m < this.m; m++)
                    {
                        this.rip[k, m] = ripsMatrix[k, m];
                    }
                }
            }
            else
            {
                double[] numArray2 = new double[this.n];
                double[,] numArray3 = new double[this.n, this.m];
                double[] x = new double[this.m];
                for (int n = 0; n < this.n; n++)
                {
                    for (int num8 = 0; num8 < this.m; num8++)
                    {
                        x[num8] = ripsMatrix[n, num8];
                    }
                    numArray2[n] = this.avg(this.m, x);
                    for (int num9 = 0; num9 < this.m; num9++)
                    {
                        numArray3[n, num9] = ripsMatrix[n, num9] - numArray2[n];
                    }
                }
                double[] numArray5 = new double[this.n];
                for (int num10 = 0; num10 < this.n; num10++)
                {
                    for (int num11 = 0; num11 < this.m; num11++)
                    {
                        numArray5[num10] += numArray[num11] * numArray3[num10, num11];
                    }
                }
                double[] numArray6 = new double[this.n];
                bool[] flagArray = new bool[this.n];
                for (int num12 = 0; num12 < this.n; num12++)
                {
                    numArray6[num12] = numArray5[num12] / num3;
                    if (Math.Abs(numArray6[num12]) < 1E-58)
                    {
                        flagArray[num12] = true;
                    }
                }
                double[] numArray7 = new double[this.n];
                for (int num13 = 0; num13 < this.n; num13++)
                {
                    numArray7[num13] = numArray2[num13] - (numArray6[num13] * num);
                }
                for (int num14 = 0; num14 < this.n; num14++)
                {
                    if (flagArray[num14])
                    {
                        for (int num15 = 0; num15 < this.m; num15++)
                        {
                            this.rip[num14, num15] = ripsMatrix[num14, num15];
                        }
                    }
                    else
                    {
                        for (int num16 = 0; num16 < this.m; num16++)
                        {
                            this.rip[num14, num16] = (ripsMatrix[num14, num16] - numArray7[num14]) / numArray6[num14];
                        }
                    }
                }
            }
            return this.rip;
        }

        public double[,] mulitipleMatrix(int n, int m, double[,] x, double num)
        {
            double[,] numArray = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    numArray[i, j] = num * x[i, j];
                }
            }
            return numArray;
        }

        public double nearestDistance(int n, int f, double[,] t)
        {
            double num = 0.0;
            double[,] numArray = new double[n, f];
            double[,] numArray2 = new double[n, n];
            double[] numArray3 = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int num3 = 0; num3 < f; num3++)
                {
                    numArray[i, num3] = t[i, num3];
                }
            }
            for (int j = 0; j < n; j++)
            {
                numArray2[j, j] = 1E+130;
                for (int num5 = j + 1; num5 < n; num5++)
                {
                    for (int num6 = 0; num6 < f; num6++)
                    {
                        numArray2[j, num5] += (numArray[j, num6] - numArray[num5, num6]) * (numArray[j, num6] - numArray[num5, num6]);
                    }
                    numArray2[j, num5] = Math.Sqrt(numArray2[j, num5]);
                    numArray2[num5, j] = numArray2[j, num5];
                }
            }
            for (int k = 0; k < n; k++)
            {
                numArray3[k] = numArray2[k, 0];
                for (int num8 = 1; num8 < n; num8++)
                {
                    if ((k != num8) && (numArray2[k, num8] < numArray3[k]))
                    {
                        numArray3[k] = numArray2[k, num8];
                    }
                }
            }
            num = numArray3[0];
            for (int m = 1; m < n; m++)
            {
                if (numArray3[m] > num)
                {
                    num = numArray3[m];
                }
            }
            return num;
        }

        public double[,] norm(double[,] ripsMatrix)
        {
            double[] numArray = this.avgRipsMatrix(ripsMatrix);
            double[] numArray2 = this.ssRipsMatrix(ripsMatrix);
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    if (numArray2[j] == 0.0)
                    {
                        this.rip[i, j] = ripsMatrix[i, j];
                    }
                    else
                    {
                        this.rip[i, j] = (ripsMatrix[i, j] - numArray[j]) / numArray2[j];
                    }
                }
            }
            return this.rip;
        }

        public double[,] normPredict(double[,] ripsMatrix, double[] meanx, double[] ss)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    if (ss[j] == 0.0)
                    {
                        this.rip[i, j] = ripsMatrix[i, j];
                    }
                    else
                    {
                        this.rip[i, j] = (ripsMatrix[i, j] - meanx[j]) / ss[j];
                    }
                }
            }
            return this.rip;
        }

        public void pcaProgress(int n, int m, double[,] regressX, int maxrank, out double[,] w, out double[,] t, out double[,] lamd, ProgressBar pg, int op)
        {
            this.value = op;
            this.pgv = pg;
            this.pgv.Invoke(new EventHandler(this.updata), new object[] { this, EventArgs.Empty });
            double[,] y = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    y[i, j] = regressX[i, j];
                }
            }
            double[,] r = new double[1, n];
            double num3 = 0.0;
            int num4 = 0;
            double[,] ripsMatrix = new double[1, m];
            double[,] numArray4 = new double[1, m];
            double[,] numArray5 = new double[n, m];
            double[,] numArray6 = new double[n, 1];
            int num5 = 0;
            double d = 0.0;
            double[,] c = new double[m, 1];
            double[,] numArray8 = new double[n, 1];
            double[,] numArray9 = new double[n, 1];
            w = new double[maxrank, m];
            t = new double[n, maxrank];
            lamd = new double[1, maxrank];
            double num7 = 0.0;
            double num8 = 0.0;
            for (num4 = 0; num4 < maxrank; num4++)
            {
                this.value = op + num4;
                this.pgv.Invoke(new EventHandler(this.updata), new object[] { this, EventArgs.Empty });
                for (int k = 0; k < n; k++)
                {
                    numArray9[k, 0] = 1.0;
                }
                num8 = 0.0;
                for (num5 = 0; num5 < 100; num5++)
                {
                    r = this.conMatrix(n, 1, numArray9);
                    num3 = this.mulRowCol(n, n, r, numArray9);
                    numArray4 = this.multiMatrix(1, n, n, m, r, y);
                    if (num3 != 0.0)
                    {
                        for (int num10 = 0; num10 < m; num10++)
                        {
                            w[num4, num10] = numArray4[0, num10] / num3;
                        }
                    }
                    d = 0.0;
                    for (int num11 = 0; num11 < m; num11++)
                    {
                        d += w[num4, num11] * w[num4, num11];
                    }
                    d = Math.Sqrt(d);
                    if (d != 0.0)
                    {
                        for (int num12 = 0; num12 < m; num12++)
                        {
                            w[num4, num12] /= d;
                            ripsMatrix[0, num12] = w[num4, num12];
                        }
                    }
                    c = this.conMatrix(1, m, ripsMatrix);
                    num3 = this.mulRowCol(m, m, ripsMatrix, c);
                    numArray8 = this.multiMatrix(n, m, m, 1, y, c);
                    if (num3 != 0.0)
                    {
                        for (int num13 = 0; num13 < n; num13++)
                        {
                            numArray6[num13, 0] = numArray8[num13, 0] / num3;
                        }
                    }
                    d = 0.0;
                    for (int num14 = 0; num14 < n; num14++)
                    {
                        d += (numArray6[num14, 0] - numArray9[num14, 0]) * (numArray6[num14, 0] - numArray9[num14, 0]);
                    }
                    num7 = Math.Sqrt(d);
                    for (int num15 = 0; num15 < n; num15++)
                    {
                        numArray9[num15, 0] = numArray6[num15, 0];
                    }
                    if (num7 == num8)
                    {
                        break;
                    }
                    num8 = num7;
                    if (num7 < 1E-15)
                    {
                        break;
                    }
                }
                r = this.conMatrix(n, 1, numArray6);
                num3 = this.mulRowCol(n, n, r, numArray6);
                numArray4 = this.multiMatrix(1, n, n, m, r, y);
                if (num3 != 0.0)
                {
                    for (int num16 = 0; num16 < m; num16++)
                    {
                        w[num4, num16] = numArray4[0, num16] / num3;
                        ripsMatrix[0, num16] = w[num4, num16];
                    }
                }
                lamd[0, num4] = num3;
                for (int num17 = 0; num17 < n; num17++)
                {
                    t[num17, num4] = numArray6[num17, 0];
                }
                numArray5 = this.multiMatrix(n, 1, 1, m, numArray6, ripsMatrix);
                y = this.subMatrix(n, m, n, m, y, numArray5);
            }
        }

        public double pcr(int n, int m, double[,] y, double[,] x, double[,] w, double[,] t, int f, out double[,] temtf, out double remainX, out double[,] xe, out double[,] b)
        {
            double num = 0.0;
            double[,] regressX = new double[n, f];
            for (int i = 0; i < f; i++)
            {
                for (int num3 = 0; num3 < n; num3++)
                {
                    regressX[num3, i] = t[num3, i];
                }
            }
            this.mlr(n, f, regressX, y, out b);
            double[,] ripsMatrix = new double[f, m];
            for (int j = 0; j < f; j++)
            {
                for (int num5 = 0; num5 < m; num5++)
                {
                    ripsMatrix[j, num5] = w[j, num5];
                }
            }
            double[,] numArray3 = this.conMatrix(f, m, ripsMatrix);
            temtf = this.multiMatrix(1, m, m, f, x, numArray3);
            num = this.mulRowCol(f, f, temtf, b);
            xe = new double[1, m];
            xe = this.subMatrix(1, m, 1, m, x, this.multiMatrix(1, f, f, m, temtf, ripsMatrix));
            remainX = 0.0;
            for (int k = 0; k < m; k++)
            {
                remainX += xe[0, k] * xe[0, k];
            }
            remainX = Math.Sqrt(remainX);
            return num;
        }

        public double pcrPrey(int n, int m, int f, int maxF, double[,] x, double[,] w, double[,] bp, out double[] remainx, out double[,] t)
        {
            double num = 0.0;
            double[,] ripsMatrix = new double[f, m];
            double[,] y = new double[m, maxF];
            double[,] c = new double[f, 1];
            for (int i = 0; i < f; i++)
            {
                for (int k = 0; k < m; k++)
                {
                    ripsMatrix[i, k] = w[i, k];
                }
                c[i, 0] = bp[i, 0];
            }
            double[,] numArray4 = this.conMatrix(f, m, ripsMatrix);
            y = this.conMatrix(maxF, m, w);
            t = new double[1, maxF];
            t = this.multiMatrix(1, m, m, maxF, x, y);
            double[,] r = new double[1, f];
            r = this.multiMatrix(1, m, m, f, x, numArray4);
            num = this.mulRowCol(f, f, r, c);
            remainx = new double[m];
            double[,] numArray6 = this.multiMatrix(1, f, f, m, r, ripsMatrix);
            for (int j = 0; j < m; j++)
            {
                remainx[j] = x[0, j] - numArray6[0, j];
            }
            return num;
        }

        public void pls(int n, int m, int f, double[,] X, double[,] y, out double[,] p, out double[,] q, out double[,] w, out double[,] t, out double[,] u, out double[,] ex, out double[,] ey, out double[,] b)
        {
            double[,] numArray = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int num2 = 0; num2 < m; num2++)
                {
                    numArray[i, num2] = X[i, num2];
                }
            }
            double[,] c = new double[n, 1];
            for (int j = 0; j < n; j++)
            {
                c[j, 0] = y[j, 0];
            }
            p = new double[f, m];
            w = new double[f, m];
            q = new double[f, 1];
            t = new double[n, f];
            u = new double[n, f];
            b = new double[f, 1];
            ex = new double[n, m];
            double[,] numArray3 = new double[n, m];
            ey = new double[n, 1];
            double[,] ripsMatrix = new double[n, 1];
            double[,] numArray5 = new double[n, 1];
            double[,] numArray6 = new double[n, 1];
            double[,] r = new double[1, n];
            double[,] numArray8 = new double[1, n];
            double[,] numArray9 = new double[m, 1];
            double num4 = 0.0;
            double[,] numArray10 = new double[1, m];
            double num5 = 0.0;
            double[,] numArray11 = new double[1, m];
            double[,] numArray12 = new double[1, m];
            double d = 0.0;
            double num7 = 0.0;
            double num9 = 0.0;
            for (int k = 0; k < f; k++)
            {
                for (int num11 = 0; num11 < n; num11++)
                {
                    ripsMatrix[num11, 0] = c[num11, 0];
                    numArray5[num11, 0] = 1.0;
                }
                int num8 = 0;
                num9 = 0.0;
                while (num8 < 100)
                {
                    r = this.conMatrix(n, 1, ripsMatrix);
                    num4 = this.mulRowCol(n, n, r, ripsMatrix);
                    numArray10 = this.multiMatrix(1, n, n, m, r, numArray);
                    if (num4 != 0.0)
                    {
                        for (int num12 = 0; num12 < m; num12++)
                        {
                            w[k, num12] = numArray10[0, num12] / num4;
                        }
                    }
                    d = 0.0;
                    for (int num13 = 0; num13 < m; num13++)
                    {
                        d += w[k, num13] * w[k, num13];
                    }
                    num7 = Math.Sqrt(d);
                    if (num7 != 0.0)
                    {
                        for (int num14 = 0; num14 < m; num14++)
                        {
                            w[k, num14] /= num7;
                            numArray11[0, num14] = w[k, num14];
                        }
                    }
                    numArray9 = this.conMatrix(1, m, numArray11);
                    num4 = this.mulRowCol(m, m, numArray11, numArray9);
                    ripsMatrix = this.multiMatrix(n, m, m, 1, numArray, numArray9);
                    if (num4 != 0.0)
                    {
                        for (int num15 = 0; num15 < n; num15++)
                        {
                            t[num15, k] = ripsMatrix[num15, 0] / num4;
                            numArray6[num15, 0] = t[num15, k];
                        }
                    }
                    numArray8 = this.conMatrix(n, 1, numArray6);
                    num4 = this.mulRowCol(n, n, numArray8, numArray6);
                    num5 = this.mulRowCol(n, n, numArray8, c);
                    if (num4 != 0.0)
                    {
                        q[k, 0] = num5 / num4;
                    }
                    if (q[k, 0] != 0.0)
                    {
                        for (int num16 = 0; num16 < n; num16++)
                        {
                            u[num16, k] = c[num16, 0] / q[k, 0];
                            ripsMatrix[num16, 0] = u[num16, k];
                        }
                    }
                    d = 0.0;
                    for (int num17 = 0; num17 < n; num17++)
                    {
                        d += (numArray6[num17, 0] - numArray5[num17, 0]) * (numArray6[num17, 0] - numArray5[num17, 0]);
                    }
                    num7 = Math.Sqrt(d);
                    for (int num18 = 0; num18 < n; num18++)
                    {
                        numArray5[num18, 0] = numArray6[num18, 0];
                    }
                    if (num7 == num9)
                    {
                        break;
                    }
                    num9 = num7;
                    if (num7 < 1E-15)
                    {
                        break;
                    }
                    num8++;
                }
                numArray8 = this.conMatrix(n, 1, numArray6);
                num4 = this.mulRowCol(n, n, numArray8, numArray6);
                numArray10 = this.multiMatrix(1, n, n, m, numArray8, numArray);
                if (num4 != 0.0)
                {
                    for (int num19 = 0; num19 < m; num19++)
                    {
                        p[k, num19] = numArray10[0, num19] / num4;
                    }
                }
                d = 0.0;
                for (int num20 = 0; num20 < m; num20++)
                {
                    d += p[k, num20] * p[k, num20];
                }
                num7 = Math.Sqrt(d);
                if (num7 != 0.0)
                {
                    for (int num21 = 0; num21 < m; num21++)
                    {
                        p[k, num21] /= num7;
                        numArray12[0, num21] = p[k, num21];
                    }
                }
                for (int num22 = 0; num22 < m; num22++)
                {
                    w[k, num22] *= num7;
                    numArray11[0, num22] = w[k, num22];
                }
                for (int num23 = 0; num23 < n; num23++)
                {
                    t[num23, k] *= num7;
                    numArray6[num23, 0] = t[num23, k];
                }
                r = this.conMatrix(n, 1, ripsMatrix);
                numArray8 = this.conMatrix(n, 1, numArray6);
                num4 = this.mulRowCol(n, n, numArray8, numArray6);
                num5 = this.mulRowCol(n, n, r, numArray6);
                if (num4 != 0.0)
                {
                    b[k, 0] = num5 / num4;
                }
                numArray3 = this.multiMatrix(n, 1, 1, m, numArray6, numArray12);
                for (int num24 = 0; num24 < n; num24++)
                {
                    for (int num25 = 0; num25 < m; num25++)
                    {
                        ex[num24, num25] = numArray[num24, num25] - numArray3[num24, num25];
                    }
                }
                for (int num26 = 0; num26 < n; num26++)
                {
                    ey[num26, 0] = c[num26, 0] - ((numArray6[num26, 0] * b[k, 0]) * q[k, 0]);
                }
                numArray = ex;
                c = ey;
            }
        }

        public double plsPrey(int m, int f, double[,] x, double[,] w, double[,] p, double[,] q, double[,] b, out double[] th, out double[] remainX)
        {
            double num = 0.0;
            remainX = new double[m];
            th = new double[f];
            for (int i = 0; i < m; i++)
            {
                remainX[i] = x[0, i];
            }
            for (int j = 0; j < f; j++)
            {
                th[j] = 0.0;
                for (int k = 0; k < m; k++)
                {
                    th[j] += remainX[k] * w[j, k];
                }
                for (int n = 0; n < m; n++)
                {
                    remainX[n] -= th[j] * p[j, n];
                }
                num += (b[j, 0] * th[j]) * q[j, 0];
            }
            return num;
        }

        public double[,] QualPcaScore(int n, int m, int f, double[,] x, double[,] w)
        {
            double[,] ripsMatrix = new double[f, m];
            double[,] y = new double[m, f];
            double[,] numArray3 = new double[1, f];
            for (int i = 0; i < f; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    ripsMatrix[i, j] = w[i, j];
                }
            }
            y = this.conMatrix(f, m, ripsMatrix);
            return this.multiMatrix(1, m, m, f, x, y);
        }


        public bool[] rangeMinMax(double[,] ripsMatrix, out double[] minMatrix, out double[] maxMatrix)
        {
            minMatrix = new double[this.n];
            maxMatrix = new double[this.n];
            bool[] flagArray = new bool[this.n];
            double[] x = new double[this.m];
            for (int i = 0; i < this.n; i++)
            {
                for (int k = 0; k < this.m; k++)
                {
                    x[k] = ripsMatrix[i, k];
                }
            }
            for (int j = 0; j < this.n; j++)
            {
                this.min_max(this.m, x, out minMatrix[j], out maxMatrix[j]);
                if (minMatrix[j] == maxMatrix[j])
                {
                    flagArray[j] = true;
                }
            }
            return flagArray;
        }

        public double[,] rangescale(double[,] ripsMatrix, out double[] minMatrix, out double[] maxMatrix)
        {
            minMatrix = new double[this.n];
            maxMatrix = new double[this.n];
            double[] x = new double[this.m];
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    x[j] = ripsMatrix[i, j];
                }
                this.min_max(this.m, x, out minMatrix[i], out maxMatrix[i]);
                if (minMatrix[i] == maxMatrix[i])
                {
                    for (int k = 0; k < this.m; k++)
                    {
                        this.rip[i, k] = ripsMatrix[i, k];
                    }
                }
                else
                {
                    for (int m = 0; m < this.m; m++)
                    {
                        this.rip[i, m] = ((ripsMatrix[i, m] - minMatrix[i]) / (maxMatrix[i] - minMatrix[i])) + 1E-09;
                    }
                }
            }
            return this.rip;
        }

        public void RankKS(int NC, double[,] X, double[] Y, int Rankn, out int[] NoC, out int[] NoV)
        {
            double[,] numArray3;
            double[] numArray4;
            int length = X.GetLength(0);
            int num2 = X.GetLength(1);
            NoC = new int[NC];
            NoV = new int[length - NC];
            ArrayList list = new ArrayList();
            int nC = Convert.ToInt32(Math.Floor(Convert.ToDouble((int)(NC / Rankn))));
            int[] numArray = this.SortIndex(Y);
            double[] numArray2 = new double[length];
            for (int i = 0; i < length; i++)
            {
                numArray2[i] = Y[numArray[i]];
            }
            double num5 = numArray2[0] - numArray2[length - 1];
            double num6 = num5 / ((double)Rankn);
            int num7 = 0;
            ArrayList list2 = new ArrayList();
            int n = this.n;
            for (int j = 0; j < Rankn; j++)
            {
                double num9;
                double num8 = numArray2[0] - (num6 * j);
                if (j == (Rankn - 1))
                {
                    num9 = (numArray2[0] - (num6 * (j + 1))) - 1.0;
                }
                else
                {
                    num9 = numArray2[0] - (num6 * (j + 1));
                }
                num7 = 0;
                list2.Clear();
                list2 = null;
                list2 = new ArrayList();
                for (int num12 = 0; num12 < length; num12++)
                {
                    if ((num9 <= Y[num12]) && (num8 > Y[num12]))
                    {
                        if (num12 == 0)
                        {
                            list2.Add(num12);
                        }
                        num7++;
                        list2.Add(num12);
                    }
                }
                numArray3 = new double[num7, num2];
                numArray4 = new double[num7];
                for (int num13 = 0; num13 < num7; num13++)
                {
                    numArray4[num13] = Y[Convert.ToInt32(list2[num13].ToString())];
                    for (int num14 = 0; num14 < num2; num14++)
                    {
                        numArray3[num13, num14] = X[Convert.ToInt32(list2[num13].ToString()), num14];
                    }
                }
                int num15 = 0;
                if (num7 > nC)
                {
                    int[] numArray5;
                    int[] numArray6;
                    num15++;
                    this.n = num7;
                    this.KennardStone(nC, numArray3, out numArray5, out numArray6);
                    for (int num16 = 0; num16 < nC; num16++)
                    {
                        list.Add(Convert.ToInt32(list2[numArray5[num16]].ToString()));
                    }
                }
                else
                {
                    for (int num17 = 0; num17 < num7; num17++)
                    {
                        list.Add(Convert.ToInt32(list2[num17].ToString()));
                    }
                }
            }
            if (list.Count < NC)
            {
                int[] numArray8;
                int[] numArray9;
                int[] numArray7 = new int[length - list.Count];
                int num18 = 0;
                for (int num19 = 0; num19 < length; num19++)
                {
                    if ((list.IndexOf(num19) == -1) && (num18 < (length - list.Count)))
                    {
                        numArray7[num18++] = num19;
                    }
                }
                num7 = length - list.Count;
                numArray3 = new double[num7, num2];
                numArray4 = new double[num7];
                for (int num20 = 0; num20 < num7; num20++)
                {
                    numArray4[num20] = Y[numArray7[num20]];
                    for (int num21 = 0; num21 < num2; num21++)
                    {
                        numArray3[num20, num21] = X[numArray7[num20], num21];
                    }
                }
                this.n = num7;
                this.KennardStone(NC - list.Count, numArray3, out numArray8, out numArray9);
                for (int num22 = 0; num22 < numArray8.Length; num22++)
                {
                    list.Add(numArray7[numArray8[num22]]);
                }
            }
            this.n = n;
            int num23 = 0;
            for (int k = 0; k < length; k++)
            {
                if ((list.IndexOf(k) == -1) && (num23 < ((length - NC) + 1)))
                {
                    NoV[num23++] = k;
                }
            }
            NoC = new int[list.Count];
            for (int m = 0; m < list.Count; m++)
            {
                NoC[m] = Convert.ToInt32(list[m].ToString());
            }
        }

        public int rankMatrix(int n, int m, double[,] x)
        {
            return 0;
        }

        public double[] relation(int n, int m, double[,] ripsMatrix, double[] y)
        {
            double[] numArray = new double[m];
            double[] x = new double[n];
            double[] numArray3 = new double[m];
            double[,] numArray4 = new double[n, m];
            double[] numArray5 = new double[m];
            double[] numArray6 = new double[m];
            double num = this.avg(n, y);
            double[] numArray7 = new double[n];
            double num2 = 0.0;
            for (int i = 0; i < n; i++)
            {
                numArray7[i] = y[i] - num;
                num2 += (y[i] - num) * (y[i] - num);
            }
            for (int j = 0; j < m; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    x[k] = ripsMatrix[k, j];
                }
                numArray3[j] = this.avg(n, x);
                for (int num6 = 0; num6 < n; num6++)
                {
                    numArray4[num6, j] = ripsMatrix[num6, j] - numArray3[j];
                }
                for (int num7 = 0; num7 < n; num7++)
                {
                    numArray5[j] += numArray4[num7, j] * numArray7[num7];
                    numArray6[j] += numArray4[num7, j] * numArray4[num7, j];
                }
                if (Math.Sqrt(numArray6[j] * num2) != 0.0)
                {
                    numArray[j] = numArray5[j] / Math.Sqrt(numArray6[j] * num2);
                }
            }
            return numArray;
        }


        public double[,] reverseMatrix1(int n, int m, double[,] x, out bool flag)
        {
            double[,] numArray = new double[n, m];
            flag = false;
            int num = 0;
            double num2 = 0.0;
            if (n != m)
            {
                flag = true;
                return numArray;
            }
            double[] numArray2 = new double[2 * m];
            double[,] numArray3 = new double[n, 2 * m];
            for (int i = 0; i < n; i++)
            {
                for (int num4 = 0; num4 < m; num4++)
                {
                    numArray3[i, num4] = x[i, num4];
                }
                for (int num5 = m; num5 < (2 * m); num5++)
                {
                    if (i == (num5 - m))
                    {
                        numArray3[i, num5] = 1.0;
                    }
                }
            }
            for (int j = 0; j < (n - 1); j++)
            {
                num = 0;
                if (numArray3[j, j] == 0.0)
                {
                    for (int num7 = j + 1; num7 < n; num7++)
                    {
                        if (numArray3[num7, j] != 0.0)
                        {
                            for (int num8 = 0; num8 < (2 * m); num8++)
                            {
                                numArray2[num8] = numArray3[num7, num8];
                                numArray3[num7, num8] = numArray3[j, num8];
                                numArray3[j, num8] = numArray2[num8];
                            }
                            break;
                        }
                        num++;
                    }
                    if (num == ((n - 1) - j))
                    {
                        flag = true;
                        break;
                    }
                }
                num2 = numArray3[j, j];
                if (num2 != 1.0)
                {
                    for (int num9 = 0; num9 < (2 * m); num9++)
                    {
                        numArray3[j, num9] /= num2;
                    }
                }
                for (int num10 = j + 1; num10 < n; num10++)
                {
                    if (numArray3[num10, j] != 0.0)
                    {
                        num2 = numArray3[num10, j];
                        for (int num11 = j; num11 < (2 * m); num11++)
                        {
                            numArray3[num10, num11] -= num2 * numArray3[j, num11];
                        }
                    }
                }
            }
            num2 = numArray3[n - 1, m - 1];
            if (num2 != 0.0)
            {
                for (int num12 = m - 1; num12 < (2 * m); num12++)
                {
                    numArray3[n - 1, num12] /= num2;
                }
            }
            else
            {
                flag = true;
            }
            for (int k = n - 1; k > 0; k--)
            {
                for (int num14 = k - 1; num14 >= 0; num14--)
                {
                    if (numArray3[num14, k] != 0.0)
                    {
                        num2 = numArray3[num14, k];
                        for (int num15 = k; num15 < (2 * m); num15++)
                        {
                            numArray3[num14, num15] -= num2 * numArray3[k, num15];
                        }
                    }
                }
            }
            for (int num16 = 0; num16 < n; num16++)
            {
                for (int num17 = m; num17 < (2 * m); num17++)
                {
                    numArray[num16, num17 - m] = numArray3[num16, num17];
                }
            }
            return numArray;
        }

        public void ripsRemain(int n, int m, double[,] regressX, int f, double[,] w, double[,] t, out double[] rmssr)
        {
            rmssr = new double[n];
            double[,] x = new double[n, f];
            double[,] y = new double[f, m];
            double[,] numArray3 = new double[n, m];
            double[,] ripsMatrix = new double[1, m];
            double[,] c = new double[m, 1];
            for (int i = 0; i < n; i++)
            {
                for (int num2 = 0; num2 < f; num2++)
                {
                    x[i, num2] = t[i, num2];
                }
            }
            for (int j = 0; j < f; j++)
            {
                for (int num4 = 0; num4 < m; num4++)
                {
                    y[j, num4] = w[j, num4];
                }
            }
            numArray3 = this.multiMatrix(n, f, f, m, x, y);
            numArray3 = this.subMatrix(n, m, n, m, regressX, numArray3);
            for (int k = 0; k < n; k++)
            {
                for (int num6 = 0; num6 < m; num6++)
                {
                    ripsMatrix[0, num6] = numArray3[k, num6];
                }
                c = this.conMatrix(1, m, ripsMatrix);
                rmssr[k] = Math.Sqrt(this.mulRowCol(m, m, ripsMatrix, c));
            }
        }

        public void ripsRemain1(int n, int m, double[,] XP, int f, out double[] rmssr)
        {
            double[,] ripsMatrix = new double[1, m];
            double[,] c = new double[1, m];
            rmssr = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    ripsMatrix[0, j] = XP[i, j];
                }
                c = this.conMatrix(1, m, ripsMatrix);
                rmssr[i] = Math.Sqrt(this.mulRowCol(m, m, ripsMatrix, c));
            }
        }

        public void ripsRemainPca(int n, int m, double[,] regressX, int f, double[,] w, double[,] t, out double[,] XP, out double[] rmssr)
        {
            rmssr = new double[n];
            XP = new double[n, m];
            double[,] x = new double[n, f];
            double[,] y = new double[f, m];
            double[,] ripsMatrix = new double[1, m];
            double[,] c = new double[m, 1];
            for (int i = 0; i < n; i++)
            {
                for (int num2 = 0; num2 < f; num2++)
                {
                    x[i, num2] = t[i, num2];
                }
            }
            for (int j = 0; j < f; j++)
            {
                for (int num4 = 0; num4 < m; num4++)
                {
                    y[j, num4] = w[j, num4];
                }
            }
            XP = this.multiMatrix(n, f, f, m, x, y);
            XP = this.subMatrix(n, m, n, m, regressX, XP);
            for (int k = 0; k < n; k++)
            {
                for (int num6 = 0; num6 < m; num6++)
                {
                    ripsMatrix[0, num6] = XP[k, num6];
                }
                c = this.conMatrix(1, m, ripsMatrix);
                rmssr[k] = Math.Sqrt(this.mulRowCol(m, m, ripsMatrix, c));
            }
        }

        public void ripsRemainPLs(int n, int m, double[,] remainEx, out double[] sr)
        {
            sr = new double[n];
            for (int i = 0; i < n; i++)
            {
                sr[i] = 0.0;
                for (int j = 0; j < m; j++)
                {
                    sr[i] += remainEx[i, j] * remainEx[i, j];
                }
                sr[i] = Math.Sqrt(sr[i]);
            }
        }

        public void setExtremeValues(float[] arr, ref float smallestValue, ref float greatestValue)
        {
            if ((arr == null) || (arr.Length == 0))
            {
                throw new Exception("用于绘曲线图的数组为空");
            }
            smallestValue = arr[0];
            greatestValue = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (smallestValue > arr[i])
                {
                    smallestValue = arr[i];
                }
                if (greatestValue < arr[i])
                {
                    greatestValue = arr[i];
                }
            }
        }

        public double[,] smooth11(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 6; j < (this.m - 7); j++)
                {
                    this.rip[i, j] = (((((((((((-36.0 * ripsMatrix[i, j - 5]) + (9.0 * ripsMatrix[i, j - 4])) + (44.0 * ripsMatrix[i, j - 3])) + (69.0 * ripsMatrix[i, j - 2])) + (84.0 * ripsMatrix[i, j - 1])) + (89.0 * ripsMatrix[i, j])) + (84.0 * ripsMatrix[i, j + 1])) + (69.0 * ripsMatrix[i, j + 2])) + (44.0 * ripsMatrix[i, j + 3])) + (9.0 * ripsMatrix[i, j + 4])) - (36.0 * ripsMatrix[i, j + 5])) / 429.0;
                }
                for (int k = 0; k < 6; k++)
                {
                    this.rip[i, k] = ripsMatrix[i, k];
                }
                for (int m = this.m - 7; m < this.m; m++)
                {
                    this.rip[i, m] = ripsMatrix[i, m];
                }
            }
            return this.rip;
        }

        public double[,] smooth13(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 6; j < (this.m - 7); j++)
                {
                    this.rip[i, j] = (((((((((((((-11.0 * ripsMatrix[i, j - 6]) + (0.0 * ripsMatrix[i, j - 5])) + (9.0 * ripsMatrix[i, j - 4])) + (16.0 * ripsMatrix[i, j - 3])) + (21.0 * ripsMatrix[i, j - 2])) + (24.0 * ripsMatrix[i, j - 1])) + (25.0 * ripsMatrix[i, j])) + (24.0 * ripsMatrix[i, j + 1])) + (21.0 * ripsMatrix[i, j + 2])) + (16.0 * ripsMatrix[i, j + 3])) + (9.0 * ripsMatrix[i, j + 4])) + (0.0 * ripsMatrix[i, j + 5])) - (11.0 * ripsMatrix[i, j + 6])) / 143.0;
                }
                for (int k = 0; k < 6; k++)
                {
                    this.rip[i, k] = ripsMatrix[i, k];
                }
                for (int m = this.m - 7; m < this.m; m++)
                {
                    this.rip[i, m] = ripsMatrix[i, m];
                }
            }
            return this.rip;
        }

        public double[,] smooth5(double[,] ripsMatrix)
        {
            this.rip = new double[this.n, this.m];
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 3; j < (this.m - 4); j++)
                {
                    this.rip[i, j] = (((((-3.0 * ripsMatrix[i, j - 2]) + (12.0 * ripsMatrix[i, j - 1])) + (17.0 * ripsMatrix[i, j])) + (12.0 * ripsMatrix[i, j + 1])) - (3.0 * ripsMatrix[i, j + 2])) / 35.0;
                }
                for (int k = 0; k < 3; k++)
                {
                    this.rip[i, k] = ripsMatrix[i, k];
                }
                for (int m = this.m - 4; m < this.m; m++)
                {
                    this.rip[i, m] = ripsMatrix[i, m];
                }
            }
            return this.rip;
        }

        public double[,] smooth7(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 4; j < (this.m - 5); j++)
                {
                    this.rip[i, j] = (((((((-2.0 * ripsMatrix[i, j - 3]) + (3.0 * ripsMatrix[i, j - 2])) + (6.0 * ripsMatrix[i, j - 1])) + (7.0 * ripsMatrix[i, j])) + (6.0 * ripsMatrix[i, j + 1])) + (3.0 * ripsMatrix[i, j + 2])) - (2.0 * ripsMatrix[i, j + 3])) / 21.0;
                }
                for (int k = 0; k < 4; k++)
                {
                    this.rip[i, k] = ripsMatrix[i, k];
                }
                for (int m = this.m - 5; m < this.m; m++)
                {
                    this.rip[i, m] = ripsMatrix[i, m];
                }
            }
            return this.rip;
        }

        public double[,] smooth9(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 5; j < (this.m - 6); j++)
                {
                    this.rip[i, j] = (((((((((-21.0 * ripsMatrix[i, j - 4]) + (14.0 * ripsMatrix[i, j - 3])) + (39.0 * ripsMatrix[i, j - 2])) + (54.0 * ripsMatrix[i, j - 1])) + (59.0 * ripsMatrix[i, j])) + (54.0 * ripsMatrix[i, j + 1])) + (39.0 * ripsMatrix[i, j + 2])) + (14.0 * ripsMatrix[i, j + 3])) - (21.0 * ripsMatrix[i, j + 4])) / 231.0;
                }
                for (int k = 0; k < 5; k++)
                {
                    this.rip[i, k] = ripsMatrix[i, k];
                }
                for (int m = this.m - 6; m < this.m; m++)
                {
                    this.rip[i, m] = ripsMatrix[i, m];
                }
            }
            return this.rip;
        }

        public double[,] snv_detrend(double[,] ripsMatrix)
        {
            double[,] numArray = this.snv(ripsMatrix);
            double[,] y = new double[this.m, 1];
            double[,] regressX = new double[this.m, 3];
            double[] numArray5 = new double[this.m];
            for (int i = 0; i < this.m; i++)
            {
                regressX[i, 0] = 1.0;
                regressX[i, 1] = i + 1;
                regressX[i, 2] = (i + 1) * (i + 1);
            }
            for (int j = 0; j < this.n; j++)
            {
                double[,] numArray3;
                for (int k = 0; k < this.m; k++)
                {
                    y[k, 0] = numArray[j, k];
                }
                this.mlr(this.m, 3, regressX, y, out numArray3);
                for (int m = 0; m < this.m; m++)
                {
                    numArray5[m] = (numArray3[0, 0] + (numArray3[1, 0] * regressX[m, 1])) + ((numArray3[2, 0] * regressX[m, 1]) * regressX[m, 1]);
                }
                for (int n = 0; n < this.m; n++)
                {
                    this.rip[j, n] = numArray[j, n] - numArray5[n];
                }
            }
            return this.rip;
        }

        public double[] SpMean(double[,] X)
        {
            double[] numArray = new double[X.GetLength(1)];
            for (int i = 0; i < X.GetLength(1); i++)
            {
                double[] array = new double[X.GetLength(0)];
                for (int j = 0; j < X.GetLength(0); j++)
                {
                    array[j] = X[j, i];
                }
                numArray[i] = this.MeanValue(array);
            }
            return numArray;
        }

        public double[] SpStdError(double[,] X)
        {
            double[] numArray = new double[X.GetLength(1)];
            for (int i = 0; i < X.GetLength(1); i++)
            {
                double[] array = new double[X.GetLength(0)];
                for (int j = 0; j < X.GetLength(0); j++)
                {
                    array[j] = X[j, i];
                }
                numArray[i] = this.StdError(array);
            }
            return numArray;
        }

        public void SPXY(int NC, double[,] X, double[,] Y, out int[] NoC, out int[] NoV)
        {
            double num14;
            int length = X.GetLength(0);
            int num2 = X.GetLength(1);
            int num3 = Y.GetLength(1);
            NoC = new int[NC];
            NoV = new int[length - NC];
            ArrayList list = new ArrayList();
            double[,] numArray = new double[length, length];
            double[,] numArray2 = new double[length, length];
            double[] numArray3 = new double[num2];
            double[] numArray4 = new double[num2];
            double[] numArray5 = new double[num3];
            double[] numArray6 = new double[num3];
            double num4 = -1E+32;
            double num5 = -1E+32;
            for (int i = 0; i < length; i++)
            {
                for (int n = 0; n < num2; n++)
                {
                    numArray3[n] = X[i, n];
                }
                for (int num8 = 0; num8 < num3; num8++)
                {
                    numArray5[num8] = Y[i, num8];
                }
                for (int num9 = i + 1; num9 < length; num9++)
                {
                    for (int num10 = 0; num10 < num2; num10++)
                    {
                        numArray4[num10] = X[num9, num10];
                    }
                    for (int num11 = 0; num11 < num3; num11++)
                    {
                        numArray6[num11] = Y[num9, num11];
                    }
                    for (int num12 = 0; num12 < num2; num12++)
                    {
                        numArray[i, num9] += Math.Abs((double)(numArray3[num12] - numArray4[num12])) * Math.Abs((double)(numArray3[num12] - numArray4[num12]));
                    }
                    for (int num13 = 0; num13 < num3; num13++)
                    {
                        numArray2[i, num9] += Math.Abs((double)(numArray5[num13] - numArray6[num13])) * Math.Abs((double)(numArray5[num13] - numArray6[num13]));
                    }
                    numArray[i, num9] = Math.Sqrt(numArray[i, num9]);
                    if (numArray[i, num9] > num4)
                    {
                        num4 = numArray[i, num9];
                    }
                    numArray2[i, num9] = Math.Sqrt(numArray2[i, num9]);
                    if (numArray2[i, num9] > num5)
                    {
                        num5 = numArray2[i, num9];
                    }
                }
            }
            double[,] numArray7 = new double[length, length];
            double[] array = new double[length];
            double[] numArray9 = new double[length];
            int[] numArray10 = new int[length];
            for (int j = 0; j < length; j++)
            {
                for (int num16 = 0; num16 < length; num16++)
                {
                    numArray7[num16, j] = (numArray[num16, j] / num4) + (numArray2[num16, j] / num5);
                    array[num16] = numArray7[num16, j];
                }
                numArray10[j] = this.MaxValue(array, out numArray9[j]);
            }
            int index = this.MaxValue(numArray9, out num14);
            NoC[0] = numArray10[index];
            list.Add(NoC[0]);
            NoC[1] = index;
            list.Add(NoC[1]);
            double[] numArray11 = new double[length];
            for (int k = 2; k < NC; k++)
            {
                for (int num19 = 0; num19 < length; num19++)
                {
                    if (list.IndexOf(num19) != -1)
                    {
                        numArray11[num19] = -1E+32;
                    }
                    else
                    {
                        double[] numArray12 = new double[k];
                        for (int num20 = 0; num20 < k; num20++)
                        {
                            if (num19 < NoC[num20])
                            {
                                numArray12[num20] = numArray7[num19, NoC[num20]];
                            }
                            else
                            {
                                numArray12[num20] = numArray7[NoC[num20], num19];
                            }
                        }
                        this.MinValue(numArray12, out numArray11[num19]);
                    }
                }
                int num21 = this.MaxValue(numArray11, out num14);
                NoC[k] = num21;
                list.Add(num21);
            }
            int num22 = 0;
            for (int m = 0; m < length; m++)
            {
                if ((list.IndexOf(m) == -1) && (num22 < ((length - NC) + 1)))
                {
                    NoV[num22++] = m;
                }
            }
        }

        public float[] WaveNum_Length(float[] OriginalData)
        {
            float[] numArray = new float[OriginalData.Length];
            for (int i = 0; i < OriginalData.Length; i++)
            {
                numArray[i] = ((1f / OriginalData[i]) * 10000f) * 1000f;
            }
            return numArray;
        }

        public double xma(int n, int f, double[,] t, double[,] trevise)
        {
            double[,] numArray5;
            double[,] ripsMatrix = new double[n, f];
            double[,] x = new double[f, n];
            double[,] numArray3 = new double[f, f];
            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < f; k++)
                {
                    ripsMatrix[i, k] = trevise[i, k];
                }
            }
            double[,] numArray4 = new double[1, f];
            for (int j = 0; j < f; j++)
            {
                numArray4[0, j] = t[0, j];
            }
            x = this.conMatrix(n, f, ripsMatrix);
            numArray3 = this.multiMatrix(f, n, n, f, x, ripsMatrix);
            this.reverseMatrix(f, f, numArray3, out numArray5);
            double[,] r = this.multiMatrix(1, f, f, f, numArray4, numArray5);
            double[,] c = this.conMatrix(1, f, numArray4);
            return this.mulRowCol(f, f, r, c);
        }

        public double xmaDistance(int f, double[,] t, double[,] reverseM)
        {
            double[,] y = new double[f, f];
            for (int i = 0; i < f; i++)
            {
                for (int j = 0; j < f; j++)
                {
                    y[i, j] = reverseM[i, j];
                }
            }
            double[,] r = this.multiMatrix(1, f, f, f, t, y);
            double[,] c = this.conMatrix(1, f, t);
            return this.mulRowCol(f, f, r, c);
        }

        public double[,] xmean(double[,] ripsMatrix)
        {
            double[] numArray = this.avgRipsMatrix(ripsMatrix);
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    this.rip[i, j] = ripsMatrix[i, j] - numArray[j];
                }
            }
            return this.rip;
        }

        public double[,] xmeanPredict(double[,] ripsMatrix, double[] meanx)
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    this.rip[i, j] = ripsMatrix[i, j] - meanx[j];
                }
            }
            return this.rip;
        }

        public int xnearDistance(int n, int f, double[,] t, double[,] tf, double nnd)
        {
            int num = 0;
            double[] numArray = new double[n];
            double[,] numArray2 = new double[n, f];
            for (int i = 0; i < n; i++)
            {
                for (int num3 = 0; num3 < f; num3++)
                {
                    numArray2[i, num3] = tf[i, num3];
                }
            }
            double[,] numArray3 = new double[1, f];
            for (int j = 0; j < f; j++)
            {
                numArray3[0, j] = t[0, j];
            }
            for (int k = 0; k < n; k++)
            {
                numArray[k] = 0.0;
                for (int num6 = 0; num6 < f; num6++)
                {
                    numArray[k] += (numArray3[0, num6] - numArray2[k, num6]) * (numArray3[0, num6] - numArray2[k, num6]);
                }
                numArray[k] = Math.Sqrt(numArray[k]);
            }
            num = 0;
            for (int m = 0; m < n; m++)
            {
                if (numArray[m] < nnd)
                {
                    num++;
                }
            }
            return num;
        }

        public double xplsPrey(int m, int f, int maxF, double[,] x, double[,] w, double[,] p, double[,] q, double[,] b, out double[] th, out double[] remainX)
        {
            double num = 0.0;
            remainX = new double[m];
            th = new double[maxF];
            double[] numArray = new double[f];
            for (int i = 0; i < m; i++)
            {
                remainX[i] = x[0, i];
            }
            for (int j = 0; j < maxF; j++)
            {
                th[j] = 0.0;
                for (int num4 = 0; num4 < m; num4++)
                {
                    th[j] += remainX[num4] * w[j, num4];
                }
                for (int num5 = 0; num5 < m; num5++)
                {
                    remainX[num5] -= th[j] * p[j, num5];
                }
            }
            for (int k = 0; k < m; k++)
            {
                remainX[k] = x[0, k];
            }
            for (int n = 0; n < f; n++)
            {
                numArray[n] = 0.0;
                for (int num8 = 0; num8 < m; num8++)
                {
                    numArray[n] += remainX[num8] * w[n, num8];
                }
                for (int num9 = 0; num9 < m; num9++)
                {
                    remainX[num9] -= numArray[n] * p[n, num9];
                }
                num += (b[n, 0] * numArray[n]) * q[n, 0];
            }
            return num;
        }

        public double xremainDistanceOfPcr(int f, int m, double[,] x, double[,] t, double[,] w)
        {
            double d = 0.0;
            double[,] ripsMatrix = new double[f, m];
            for (int i = 0; i < f; i++)
            {
                for (int num3 = 0; num3 < m; num3++)
                {
                    ripsMatrix[i, num3] = w[i, num3];
                }
            }
            double[,] y = new double[m, f];
            y = this.conMatrix(f, m, ripsMatrix);
            double[] numArray3 = new double[m];
            for (int j = 0; j < m; j++)
            {
                numArray3[j] = x[0, j];
            }
            double[,] numArray4 = this.multiMatrix(1, m, m, f, x, y);
            double[,] numArray5 = this.multiMatrix(1, f, f, m, numArray4, ripsMatrix);
            for (int k = 0; k < m; k++)
            {
                numArray3[k] -= numArray5[0, k];
            }
            d = 0.0;
            for (int n = 0; n < m; n++)
            {
                d += numArray3[n] * numArray3[n];
            }
            return Math.Sqrt(d);
        }

        public double xremainDistanceOfPls(int f, int m, double[,] x, double[,] w, double[,] p)
        {
            double[] numArray = new double[m];
            double[] numArray2 = new double[f];
            for (int i = 0; i < m; i++)
            {
                numArray[i] = x[0, i];
            }
            for (int j = 0; j < f; j++)
            {
                numArray2[j] = 0.0;
                for (int n = 0; n < m; n++)
                {
                    numArray2[j] += numArray[n] * w[j, n];
                }
                for (int num4 = 0; num4 < m; num4++)
                {
                    numArray[num4] -= numArray2[j] * p[j, num4];
                }
            }
            double d = 0.0;
            for (int k = 0; k < m; k++)
            {
                d += numArray[k] * numArray[k];
            }
            return Math.Sqrt(d);
        }
#endregion
    }
}

