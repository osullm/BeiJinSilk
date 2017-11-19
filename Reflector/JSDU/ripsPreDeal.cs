namespace JSDU
{
    using System;
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
            if (!(num3 == 0.0))
            {
                b = num2 / num3;
            }
            a = num4 - (b * num);
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

        public double[,] diff1(double[,] ripsMatrix, int g)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = g - 1;
                while (num2 < (this.m - g))
                {
                    this.rip[i, num2] = (ripsMatrix[i, num2 + g] - ripsMatrix[i, num2]) / ((double) g);
                    num2++;
                }
                num2 = 0;
                while (num2 < (g - 1))
                {
                    this.rip[i, num2] = 0.0;
                    num2++;
                }
                for (num2 = this.m - g; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = 0.0;
                }
            }
            return this.rip;
        }

        public double[,] diff2(double[,] ripsMatrix, int g)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = g;
                while (num2 < (this.m - g))
                {
                    this.rip[i, num2] = ((ripsMatrix[i, num2 + g] - (2.0 * ripsMatrix[i, num2])) + ripsMatrix[i, num2 - g]) / ((double) (g * g));
                    num2++;
                }
                num2 = 0;
                while (num2 < g)
                {
                    this.rip[i, num2] = 0.0;
                    num2++;
                }
                for (num2 = this.m - g; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = 0.0;
                }
            }
            return this.rip;
        }

        public void maDistance(int n, int f, double[,] t, out double[,] rm, out double[] md)
        {
            int num;
            int num2;
            md = new double[n];
            rm = new double[f, f];
            double[,] ripsMatrix = new double[n, f];
            double[,] x = new double[f, n];
            double[,] numArray3 = new double[f, f];
            double[,] numArray4 = new double[1, f];
            double[,] c = new double[f, 1];
            for (num = 0; num < n; num++)
            {
                num2 = 0;
                while (num2 < f)
                {
                    ripsMatrix[num, num2] = t[num, num2];
                    num2++;
                }
            }
            x = this.conMatrix(n, f, ripsMatrix);
            numArray3 = this.multiMatrix(f, n, n, f, x, ripsMatrix);
            if (!this.reverseMatrix(f, f, numArray3, out rm))
            {
                for (num = 0; num < n; num++)
                {
                    for (num2 = 0; num2 < f; num2++)
                    {
                        numArray4[0, num2] = ripsMatrix[num, num2];
                    }
                    c = this.conMatrix(1, f, numArray4);
                    numArray4 = this.multiMatrix(1, f, f, f, numArray4, rm);
                    md[num] = this.mulRowCol(f, f, numArray4, c);
                }
            }
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
            int num2;
            int num4;
            meanx = this.avgRipsMatrix(ripsMatrix);
            double num = this.avg(this.m, meanx);
            double[] numArray = new double[this.m];
            for (num2 = 0; num2 < this.m; num2++)
            {
                numArray[num2] = meanx[num2] - num;
            }
            double num3 = 0.0;
            for (num2 = 0; num2 < this.m; num2++)
            {
                num3 += numArray[num2] * numArray[num2];
            }
            if (num3 == 0.0)
            {
                for (num2 = 0; num2 < this.n; num2++)
                {
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        this.rip[num2, num4] = ripsMatrix[num2, num4];
                        num4++;
                    }
                }
            }
            else
            {
                double[] numArray2 = new double[this.n];
                double[,] numArray3 = new double[this.n, this.m];
                double[] x = new double[this.m];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        x[num4] = ripsMatrix[num2, num4];
                        num4++;
                    }
                    numArray2[num2] = this.avg(this.m, x);
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        numArray3[num2, num4] = ripsMatrix[num2, num4] - numArray2[num2];
                        num4++;
                    }
                }
                double[] numArray5 = new double[this.n];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        numArray5[num2] += numArray[num4] * numArray3[num2, num4];
                        num4++;
                    }
                }
                double[] numArray6 = new double[this.n];
                bool[] flagArray = new bool[this.n];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    numArray6[num2] = numArray5[num2] / num3;
                    if (Math.Abs(numArray6[num2]) < 1E-58)
                    {
                        flagArray[num2] = true;
                    }
                }
                double[] numArray7 = new double[this.n];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    numArray7[num2] = numArray2[num2] - (numArray6[num2] * num);
                }
                for (num2 = 0; num2 < this.n; num2++)
                {
                    if (flagArray[num2])
                    {
                        num4 = 0;
                        while (num4 < this.m)
                        {
                            this.rip[num2, num4] = ripsMatrix[num2, num4];
                            num4++;
                        }
                    }
                    else
                    {
                        for (num4 = 0; num4 < this.m; num4++)
                        {
                            this.rip[num2, num4] = (ripsMatrix[num2, num4] - numArray7[num2]) / numArray6[num2];
                        }
                    }
                }
            }
            return this.rip;
        }

        public double[,] mscPredict(double[,] ripsMatrix, double[] meanx)
        {
            int num2;
            int num4;
            double num = this.avg(this.m, meanx);
            double[] numArray = new double[this.m];
            for (num2 = 0; num2 < this.m; num2++)
            {
                numArray[num2] = meanx[num2] - num;
            }
            double num3 = 0.0;
            for (num2 = 0; num2 < this.m; num2++)
            {
                num3 += numArray[num2] * numArray[num2];
            }
            if (num3 == 0.0)
            {
                for (num2 = 0; num2 < this.n; num2++)
                {
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        this.rip[num2, num4] = ripsMatrix[num2, num4];
                        num4++;
                    }
                }
            }
            else
            {
                double[] numArray2 = new double[this.n];
                double[,] numArray3 = new double[this.n, this.m];
                double[] x = new double[this.m];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        x[num4] = ripsMatrix[num2, num4];
                        num4++;
                    }
                    numArray2[num2] = this.avg(this.m, x);
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        numArray3[num2, num4] = ripsMatrix[num2, num4] - numArray2[num2];
                        num4++;
                    }
                }
                double[] numArray5 = new double[this.n];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    num4 = 0;
                    while (num4 < this.m)
                    {
                        numArray5[num2] += numArray[num4] * numArray3[num2, num4];
                        num4++;
                    }
                }
                double[] numArray6 = new double[this.n];
                bool[] flagArray = new bool[this.n];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    numArray6[num2] = numArray5[num2] / num3;
                    if (Math.Abs(numArray6[num2]) < 1E-58)
                    {
                        flagArray[num2] = true;
                    }
                }
                double[] numArray7 = new double[this.n];
                for (num2 = 0; num2 < this.n; num2++)
                {
                    numArray7[num2] = numArray2[num2] - (numArray6[num2] * num);
                }
                for (num2 = 0; num2 < this.n; num2++)
                {
                    if (flagArray[num2])
                    {
                        num4 = 0;
                        while (num4 < this.m)
                        {
                            this.rip[num2, num4] = ripsMatrix[num2, num4];
                            num4++;
                        }
                    }
                    else
                    {
                        for (num4 = 0; num4 < this.m; num4++)
                        {
                            this.rip[num2, num4] = (ripsMatrix[num2, num4] - numArray7[num2]) / numArray6[num2];
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

        public double nearestDistance(int n, int f, double[,] t)
        {
            int num2;
            int num3;
            double num = 0.0;
            double[,] numArray = new double[n, f];
            double[,] numArray2 = new double[1, f];
            double[,] numArray3 = new double[n, n];
            double[] numArray4 = new double[n];
            for (num2 = 0; num2 < n; num2++)
            {
                num3 = 0;
                while (num3 < f)
                {
                    numArray[num2, num3] = t[num2, num3];
                    num3++;
                }
            }
            for (num2 = 0; num2 < n; num2++)
            {
                numArray3[num2, num2] = 1E+130;
                num3 = num2 + 1;
                while (num3 < n)
                {
                    for (int i = 0; i < f; i++)
                    {
                         numArray3[num2, num3] += (numArray[num2, i] - numArray[num3, i]) * (numArray[num2, i] - numArray[num3, i]);
                    }
                    numArray3[num2, num3] = Math.Sqrt(numArray3[num2, num3]);
                    numArray3[num3, num2] = numArray3[num2, num3];
                    num3++;
                }
            }
            for (num2 = 0; num2 < n; num2++)
            {
                numArray4[num2] = numArray3[num2, 0];
                for (num3 = 1; num3 < n; num3++)
                {
                    if ((num2 != num3) && (numArray3[num2, num3] < numArray4[num2]))
                    {
                        numArray4[num2] = numArray3[num2, num3];
                    }
                }
            }
            num = numArray4[0];
            for (num2 = 1; num2 < n; num2++)
            {
                if (numArray4[num2] > num)
                {
                    num = numArray4[num2];
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

        public void pca(int n, int m, double[,] regressX, int maxrank, out double[,] w, out double[,] t, out double[,] lamd)
        {
            double[,] y = new double[n, m];
            int num = 0;
            while (num < n)
            {
                for (int i = 0; i < m; i++)
                {
                    y[num, i] = regressX[num, i];
                }
                num++;
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
                num = 0;
                while (num < n)
                {
                    numArray9[num, 0] = 1.0;
                    num++;
                }
                num8 = 0.0;
                for (num5 = 0; num5 < 100; num5++)
                {
                    r = this.conMatrix(n, 1, numArray9);
                    num3 = this.mulRowCol(n, n, r, numArray9);
                    numArray4 = this.multiMatrix(1, n, n, m, r, y);
                    if (num3 != 0.0)
                    {
                        num = 0;
                        while (num < m)
                        {
                            w[num4, num] = numArray4[0, num] / num3;
                            num++;
                        }
                    }
                    d = 0.0;
                    num = 0;
                    while (num < m)
                    {
                        d += w[num4, num] * w[num4, num];
                        num++;
                    }
                    d = Math.Sqrt(d);
                    if (d != 0.0)
                    {
                        num = 0;
                        while (num < m)
                        {
                            w[num4, num] /= d;
                            ripsMatrix[0, num] = w[num4, num];
                            num++;
                        }
                    }
                    c = this.conMatrix(1, m, ripsMatrix);
                    num3 = this.mulRowCol(m, m, ripsMatrix, c);
                    numArray8 = this.multiMatrix(n, m, m, 1, y, c);
                    if (num3 != 0.0)
                    {
                        num = 0;
                        while (num < n)
                        {
                            numArray6[num, 0] = numArray8[num, 0] / num3;
                            num++;
                        }
                    }
                    d = 0.0;
                    num = 0;
                    while (num < n)
                    {
                        d += (numArray6[num, 0] - numArray9[num, 0]) * (numArray6[num, 0] - numArray9[num, 0]);
                        num++;
                    }
                    num7 = Math.Sqrt(d);
                    num = 0;
                    while (num < n)
                    {
                        numArray9[num, 0] = numArray6[num, 0];
                        num++;
                    }
                    if (!(num7 == num8))
                    {
                        num8 = num7;
                    }
                    else
                    {
                        break;
                    }
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
                    num = 0;
                    while (num < m)
                    {
                        w[num4, num] = numArray4[0, num] / num3;
                        ripsMatrix[0, num] = w[num4, num];
                        num++;
                    }
                }
                lamd[0, num4] = num3;
                for (num = 0; num < n; num++)
                {
                    t[num, num4] = numArray6[num, 0];
                }
                numArray5 = this.multiMatrix(n, 1, 1, m, numArray6, ripsMatrix);
                y = this.subMatrix(n, m, n, m, y, numArray5);
            }
        }

        public void pcaProgress(int n, int m, double[,] regressX, int maxrank, out double[,] w, out double[,] t, out double[,] lamd, ProgressBar pg, int op)
        {
            this.value = op;
            this.pgv = pg;
            this.pgv.Invoke(new EventHandler(this.updata), new object[] { this, EventArgs.Empty });
            double[,] y = new double[n, m];
            int num = 0;
            while (num < n)
            {
                for (int i = 0; i < m; i++)
                {
                    y[num, i] = regressX[num, i];
                }
                num++;
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
                num = 0;
                while (num < n)
                {
                    numArray9[num, 0] = 1.0;
                    num++;
                }
                num8 = 0.0;
                for (num5 = 0; num5 < 100; num5++)
                {
                    r = this.conMatrix(n, 1, numArray9);
                    num3 = this.mulRowCol(n, n, r, numArray9);
                    numArray4 = this.multiMatrix(1, n, n, m, r, y);
                    if (num3 != 0.0)
                    {
                        num = 0;
                        while (num < m)
                        {
                            w[num4, num] = numArray4[0, num] / num3;
                            num++;
                        }
                    }
                    d = 0.0;
                    num = 0;
                    while (num < m)
                    {
                        d += w[num4, num] * w[num4, num];
                        num++;
                    }
                    d = Math.Sqrt(d);
                    if (d != 0.0)
                    {
                        num = 0;
                        while (num < m)
                        {
                            w[num4, num] /= d;
                            ripsMatrix[0, num] = w[num4, num];
                            num++;
                        }
                    }
                    c = this.conMatrix(1, m, ripsMatrix);
                    num3 = this.mulRowCol(m, m, ripsMatrix, c);
                    numArray8 = this.multiMatrix(n, m, m, 1, y, c);
                    if (num3 != 0.0)
                    {
                        num = 0;
                        while (num < n)
                        {
                            numArray6[num, 0] = numArray8[num, 0] / num3;
                            num++;
                        }
                    }
                    d = 0.0;
                    num = 0;
                    while (num < n)
                    {
                        d += (numArray6[num, 0] - numArray9[num, 0]) * (numArray6[num, 0] - numArray9[num, 0]);
                        num++;
                    }
                    num7 = Math.Sqrt(d);
                    num = 0;
                    while (num < n)
                    {
                        numArray9[num, 0] = numArray6[num, 0];
                        num++;
                    }
                    if (!(num7 == num8))
                    {
                        num8 = num7;
                    }
                    else
                    {
                        break;
                    }
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
                    num = 0;
                    while (num < m)
                    {
                        w[num4, num] = numArray4[0, num] / num3;
                        ripsMatrix[0, num] = w[num4, num];
                        num++;
                    }
                }
                lamd[0, num4] = num3;
                for (num = 0; num < n; num++)
                {
                    t[num, num4] = numArray6[num, 0];
                }
                numArray5 = this.multiMatrix(n, 1, 1, m, numArray6, ripsMatrix);
                y = this.subMatrix(n, m, n, m, y, numArray5);
            }
        }

        public double pcr(int n, int m, double[,] y, double[,] x, double[,] w, double[,] t, int f, out double[,] temtf, out double remainX, out double[,] xe, out double[,] b)
        {
            int num3;
            double num = 0.0;
            double[,] regressX = new double[n, f];
            int num2 = 0;
            while (num2 < f)
            {
                num3 = 0;
                while (num3 < n)
                {
                    regressX[num3, num2] = t[num3, num2];
                    num3++;
                }
                num2++;
            }
            this.mlr(n, f, regressX, y, out b);
            double[,] ripsMatrix = new double[f, m];
            for (num3 = 0; num3 < f; num3++)
            {
                for (num2 = 0; num2 < m; num2++)
                {
                    ripsMatrix[num3, num2] = w[num3, num2];
                }
            }
            double[,] numArray3 = this.conMatrix(f, m, ripsMatrix);
            temtf = this.multiMatrix(1, m, m, f, x, numArray3);
            num = this.mulRowCol(f, f, temtf, b);
            xe = new double[1, m];
            xe = this.subMatrix(1, m, 1, m, x, this.multiMatrix(1, f, f, m, temtf, ripsMatrix));
            remainX = 0.0;
            for (num3 = 0; num3 < m; num3++)
            {
                remainX += xe[0, num3] * xe[0, num3];
            }
            remainX = Math.Sqrt(remainX);
            return num;
        }

        public double pcrPrey(int n, int m, int f, int maxF, double[,] x, double[,] w, double[,] bp, out double[] remainx, out double[,] t)
        {
            int num2;
            double num = 0.0;
            double[,] ripsMatrix = new double[f, m];
            double[,] y = new double[m, maxF];
            double[,] c = new double[f, 1];
            for (num2 = 0; num2 < f; num2++)
            {
                for (int i = 0; i < m; i++)
                {
                    ripsMatrix[num2, i] = w[num2, i];
                }
                c[num2, 0] = bp[num2, 0];
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
            for (num2 = 0; num2 < m; num2++)
            {
                remainx[num2] = x[0, num2] - numArray6[0, num2];
            }
            return num;
        }

        public void pls(int n, int m, int f, double[,] X, double[,] y, out double[,] p, out double[,] q, out double[,] w, out double[,] t, out double[,] u, out double[,] ex, out double[,] ey, out double[,] b)
        {
            int num2;
            double[,] numArray = new double[n, m];
            int num = 0;
            while (num < n)
            {
                for (num2 = 0; num2 < m; num2++)
                {
                    numArray[num, num2] = X[num, num2];
                }
                num++;
            }
            double[,] c = new double[n, 1];
            num2 = 0;
            while (num2 < n)
            {
                c[num2, 0] = y[num2, 0];
                num2++;
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
            double num3 = 0.0;
            double[,] numArray10 = new double[1, m];
            double num4 = 0.0;
            double[,] numArray11 = new double[n, 1];
            double[,] numArray12 = new double[1, m];
            double[,] numArray13 = new double[1, m];
            double d = 0.0;
            double num6 = 0.0;
            double num8 = 0.0;
            for (int i = 0; i < f; i++)
            {
                num2 = 0;
                while (num2 < n)
                {
                    ripsMatrix[num2, 0] = c[num2, 0];
                    numArray5[num2, 0] = 1.0;
                    num2++;
                }
                int num7 = 0;
                num8 = 0.0;
                while (num7 < 100)
                {
                    r = this.conMatrix(n, 1, ripsMatrix);
                    num3 = this.mulRowCol(n, n, r, ripsMatrix);
                    numArray10 = this.multiMatrix(1, n, n, m, r, numArray);
                    if (num3 != 0.0)
                    {
                        num = 0;
                        while (num < m)
                        {
                            w[i, num] = numArray10[0, num] / num3;
                            num++;
                        }
                    }
                    d = 0.0;
                    num = 0;
                    while (num < m)
                    {
                        d += w[i, num] * w[i, num];
                        num++;
                    }
                    num6 = Math.Sqrt(d);
                    if (num6 != 0.0)
                    {
                        num = 0;
                        while (num < m)
                        {
                            w[i, num] /= num6;
                            numArray12[0, num] = w[i, num];
                            num++;
                        }
                    }
                    numArray9 = this.conMatrix(1, m, numArray12);
                    num3 = this.mulRowCol(m, m, numArray12, numArray9);
                    ripsMatrix = this.multiMatrix(n, m, m, 1, numArray, numArray9);
                    if (num3 != 0.0)
                    {
                        num = 0;
                        while (num < n)
                        {
                            t[num, i] = ripsMatrix[num, 0] / num3;
                            numArray6[num, 0] = t[num, i];
                            num++;
                        }
                    }
                    numArray8 = this.conMatrix(n, 1, numArray6);
                    num3 = this.mulRowCol(n, n, numArray8, numArray6);
                    num4 = this.mulRowCol(n, n, numArray8, c);
                    if (!(num3 == 0.0))
                    {
                        q[i, 0] = num4 / num3;
                    }
                    if (q[i, 0] != 0.0)
                    {
                        num = 0;
                        while (num < n)
                        {
                            u[num, i] = c[num, 0] / q[i, 0];
                            ripsMatrix[num, 0] = u[num, i];
                            num++;
                        }
                    }
                    d = 0.0;
                    num = 0;
                    while (num < n)
                    {
                        d += (numArray6[num, 0] - numArray5[num, 0]) * (numArray6[num, 0] - numArray5[num, 0]);
                        num++;
                    }
                    num6 = Math.Sqrt(d);
                    num = 0;
                    while (num < n)
                    {
                        numArray5[num, 0] = numArray6[num, 0];
                        num++;
                    }
                    if (!(num6 == num8))
                    {
                        num8 = num6;
                    }
                    else
                    {
                        break;
                    }
                    if (num6 < 1E-15)
                    {
                        break;
                    }
                    num7++;
                }
                numArray8 = this.conMatrix(n, 1, numArray6);
                num3 = this.mulRowCol(n, n, numArray8, numArray6);
                numArray10 = this.multiMatrix(1, n, n, m, numArray8, numArray);
                if (num3 != 0.0)
                {
                    num = 0;
                    while (num < m)
                    {
                        p[i, num] = numArray10[0, num] / num3;
                        num++;
                    }
                }
                d = 0.0;
                num = 0;
                while (num < m)
                {
                    d += p[i, num] * p[i, num];
                    num++;
                }
                num6 = Math.Sqrt(d);
                if (num6 != 0.0)
                {
                    num = 0;
                    while (num < m)
                    {
                        p[i, num] /= num6;
                        numArray13[0, num] = p[i, num];
                        num++;
                    }
                }
                num = 0;
                while (num < m)
                {
                    w[i, num] *= num6;
                    numArray12[0, num] = w[i, num];
                    num++;
                }
                num = 0;
                while (num < n)
                {
                    t[num, i] *= num6;
                    numArray6[num, 0] = t[num, i];
                    num++;
                }
                r = this.conMatrix(n, 1, ripsMatrix);
                numArray8 = this.conMatrix(n, 1, numArray6);
                num3 = this.mulRowCol(n, n, numArray8, numArray6);
                num4 = this.mulRowCol(n, n, r, numArray6);
                if (!(num3 == 0.0))
                {
                    b[i, 0] = num4 / num3;
                }
                numArray3 = this.multiMatrix(n, 1, 1, m, numArray6, numArray13);
                num = 0;
                while (num < n)
                {
                    for (num2 = 0; num2 < m; num2++)
                    {
                        ex[num, num2] = numArray[num, num2] - numArray3[num, num2];
                    }
                    num++;
                }
                for (num = 0; num < n; num++)
                {
                    ey[num, 0] = c[num, 0] - ((numArray6[num, 0] * b[i, 0]) * q[i, 0]);
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
            int index = 0;
            while (index < m)
            {
                remainX[index] = x[0, index];
                index++;
            }
            for (int i = 0; i < f; i++)
            {
                th[i] = 0.0;
                index = 0;
                while (index < m)
                {
                    th[i] += remainX[index] * w[i, index];
                    index++;
                }
                for (index = 0; index < m; index++)
                {
                    remainX[index] -= th[i] * p[i, index];
                }
                num += (b[i, 0] * th[i]) * q[i, 0];
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
            int num2;
            minMatrix = new double[this.n];
            maxMatrix = new double[this.n];
            bool[] flagArray = new bool[this.n];
            double[] x = new double[this.m];
            for (int i = 0; i < this.n; i++)
            {
                num2 = 0;
                while (num2 < this.m)
                {
                    x[num2] = ripsMatrix[i, num2];
                    num2++;
                }
            }
            for (num2 = 0; num2 < this.n; num2++)
            {
                this.min_max(this.m, x, out minMatrix[num2], out maxMatrix[num2]);
                if (minMatrix[num2] == maxMatrix[num2])
                {
                    flagArray[num2] = true;
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
                int index = 0;
                while (index < this.m)
                {
                    x[index] = ripsMatrix[i, index];
                    index++;
                }
                this.min_max(this.m, x, out minMatrix[i], out maxMatrix[i]);
                if (minMatrix[i] == maxMatrix[i])
                {
                    index = 0;
                    while (index < this.m)
                    {
                        this.rip[i, index] = ripsMatrix[i, index];
                        index++;
                    }
                }
                else
                {
                    for (index = 0; index < this.m; index++)
                    {
                        this.rip[i, index] = ((ripsMatrix[i, index] - minMatrix[i]) / (maxMatrix[i] - minMatrix[i])) + 1E-09;
                    }
                }
            }
            return this.rip;
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
            int index = 0;
            while (index < n)
            {
                numArray7[index] = y[index] - num;
                num2 += (y[index] - num) * (y[index] - num);
                index++;
            }
            for (int i = 0; i < m; i++)
            {
                index = 0;
                while (index < n)
                {
                    x[index] = ripsMatrix[index, i];
                    index++;
                }
                numArray3[i] = this.avg(n, x);
                index = 0;
                while (index < n)
                {
                    numArray4[index, i] = ripsMatrix[index, i] - numArray3[i];
                    index++;
                }
                for (index = 0; index < n; index++)
                {
                    numArray5[i] += numArray4[index, i] * numArray7[index];
                    numArray6[i] += numArray4[index, i] * numArray4[index, i];
                }
                if (!(Math.Sqrt(numArray6[i] * num2) == 0.0))
                {
                    numArray[i] = numArray5[i] / Math.Sqrt(numArray6[i] * num2);
                }
            }
            return numArray;
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

        public double[,] reverseMatrix1(int n, int m, double[,] x, out bool flag)
        {
            int num4;
            int num5;
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
            int num3 = 0;
            while (num3 < n)
            {
                num4 = 0;
                while (num4 < m)
                {
                    numArray3[num3, num4] = x[num3, num4];
                    num4++;
                }
                num4 = m;
                while (num4 < (2 * m))
                {
                    if (num3 == (num4 - m))
                    {
                        numArray3[num3, num4] = 1.0;
                    }
                    num4++;
                }
                num3++;
            }
            for (num5 = 0; num5 < (n - 1); num5++)
            {
                num = 0;
                if (numArray3[num5, num5] == 0.0)
                {
                    num3 = num5 + 1;
                    while (num3 < n)
                    {
                        if (numArray3[num3, num5] != 0.0)
                        {
                            num4 = 0;
                            while (num4 < (2 * m))
                            {
                                numArray2[num4] = numArray3[num3, num4];
                                numArray3[num3, num4] = numArray3[num5, num4];
                                numArray3[num5, num4] = numArray2[num4];
                                num4++;
                            }
                            break;
                        }
                        num++;
                        num3++;
                    }
                    if (num == ((n - 1) - num5))
                    {
                        flag = true;
                        break;
                    }
                }
                num2 = numArray3[num5, num5];
                if (num2 != 1.0)
                {
                    num4 = 0;
                    while (num4 < (2 * m))
                    {
                        numArray3[num5, num4] /= num2;
                        num4++;
                    }
                }
                num3 = num5 + 1;
                while (num3 < n)
                {
                    if (numArray3[num3, num5] != 0.0)
                    {
                        num2 = numArray3[num3, num5];
                        for (num4 = num5; num4 < (2 * m); num4++)
                        {
                            numArray3[num3, num4] -= num2 * numArray3[num5, num4];
                        }
                    }
                    num3++;
                }
            }
            num2 = numArray3[n - 1, m - 1];
            if (num2 != 0.0)
            {
                num4 = m - 1;
                while (num4 < (2 * m))
                {
                    numArray3[n - 1, num4] /= num2;
                    num4++;
                }
            }
            else
            {
                flag = true;
            }
            for (num5 = n - 1; num5 > 0; num5--)
            {
                num3 = num5 - 1;
                while (num3 >= 0)
                {
                    if (numArray3[num3, num5] != 0.0)
                    {
                        num2 = numArray3[num3, num5];
                        num4 = num5;
                        while (num4 < (2 * m))
                        {
                            numArray3[num3, num4] -= num2 * numArray3[num5, num4];
                            num4++;
                        }
                    }
                    num3--;
                }
            }
            for (num3 = 0; num3 < n; num3++)
            {
                for (num4 = m; num4 < (2 * m); num4++)
                {
                    numArray[num3, num4 - m] = numArray3[num3, num4];
                }
            }
            return numArray;
        }

        public void ripsRemain(int n, int m, double[,] regressX, int f, double[,] w, double[,] t, out double[] rmssr)
        {
            int num;
            int num2;
            rmssr = new double[n];
            double[,] x = new double[n, f];
            double[,] numArray2 = new double[1, f];
            double[,] y = new double[f, m];
            double[,] numArray4 = new double[n, m];
            double[,] ripsMatrix = new double[1, m];
            double[,] c = new double[m, 1];
            for (num = 0; num < n; num++)
            {
                num2 = 0;
                while (num2 < f)
                {
                    x[num, num2] = t[num, num2];
                    num2++;
                }
            }
            for (num = 0; num < f; num++)
            {
                num2 = 0;
                while (num2 < m)
                {
                    y[num, num2] = w[num, num2];
                    num2++;
                }
            }
            numArray4 = this.multiMatrix(n, f, f, m, x, y);
            numArray4 = this.subMatrix(n, m, n, m, regressX, numArray4);
            for (num = 0; num < n; num++)
            {
                for (num2 = 0; num2 < m; num2++)
                {
                    ripsMatrix[0, num2] = numArray4[num, num2];
                }
                c = this.conMatrix(1, m, ripsMatrix);
                rmssr[num] = Math.Sqrt(this.mulRowCol(m, m, ripsMatrix, c));
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
            int num;
            int num2;
            rmssr = new double[n];
            XP = new double[n, m];
            double[,] x = new double[n, f];
            double[,] numArray2 = new double[1, f];
            double[,] y = new double[f, m];
            double[,] ripsMatrix = new double[1, m];
            double[,] c = new double[m, 1];
            for (num = 0; num < n; num++)
            {
                num2 = 0;
                while (num2 < f)
                {
                    x[num, num2] = t[num, num2];
                    num2++;
                }
            }
            for (num = 0; num < f; num++)
            {
                num2 = 0;
                while (num2 < m)
                {
                    y[num, num2] = w[num, num2];
                    num2++;
                }
            }
            XP = this.multiMatrix(n, f, f, m, x, y);
            XP = this.subMatrix(n, m, n, m, regressX, XP);
            for (num = 0; num < n; num++)
            {
                for (num2 = 0; num2 < m; num2++)
                {
                    ripsMatrix[0, num2] = XP[num, num2];
                }
                c = this.conMatrix(1, m, ripsMatrix);
                rmssr[num] = Math.Sqrt(this.mulRowCol(m, m, ripsMatrix, c));
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

        public double[,] smooth11(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = 6;
                while (num2 < (this.m - 7))
                {
                    this.rip[i, num2] = (((((((((((-36.0 * ripsMatrix[i, num2 - 5]) + (9.0 * ripsMatrix[i, num2 - 4])) + (44.0 * ripsMatrix[i, num2 - 3])) + (69.0 * ripsMatrix[i, num2 - 2])) + (84.0 * ripsMatrix[i, num2 - 1])) + (89.0 * ripsMatrix[i, num2])) + (84.0 * ripsMatrix[i, num2 + 1])) + (69.0 * ripsMatrix[i, num2 + 2])) + (44.0 * ripsMatrix[i, num2 + 3])) + (9.0 * ripsMatrix[i, num2 + 4])) - (36.0 * ripsMatrix[i, num2 + 5])) / 429.0;
                    num2++;
                }
                num2 = 0;
                while (num2 < 6)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                    num2++;
                }
                for (num2 = this.m - 7; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                }
            }
            return this.rip;
        }

        public double[,] smooth13(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = 6;
                while (num2 < (this.m - 7))
                {
                    this.rip[i, num2] = (((((((((((((-11.0 * ripsMatrix[i, num2 - 6]) + (0.0 * ripsMatrix[i, num2 - 5])) + (9.0 * ripsMatrix[i, num2 - 4])) + (16.0 * ripsMatrix[i, num2 - 3])) + (21.0 * ripsMatrix[i, num2 - 2])) + (24.0 * ripsMatrix[i, num2 - 1])) + (25.0 * ripsMatrix[i, num2])) + (24.0 * ripsMatrix[i, num2 + 1])) + (21.0 * ripsMatrix[i, num2 + 2])) + (16.0 * ripsMatrix[i, num2 + 3])) + (9.0 * ripsMatrix[i, num2 + 4])) + (0.0 * ripsMatrix[i, num2 + 5])) - (11.0 * ripsMatrix[i, num2 + 6])) / 143.0;
                    num2++;
                }
                num2 = 0;
                while (num2 < 6)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                    num2++;
                }
                for (num2 = this.m - 7; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                }
            }
            return this.rip;
        }

        public double[,] smooth5(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = 3;
                while (num2 < (this.m - 4))
                {
                    this.rip[i, num2] = (((((-3.0 * ripsMatrix[i, num2 - 2]) + (12.0 * ripsMatrix[i, num2 - 1])) + (17.0 * ripsMatrix[i, num2])) + (12.0 * ripsMatrix[i, num2 + 1])) - (3.0 * ripsMatrix[i, num2 + 2])) / 35.0;
                    num2++;
                }
                num2 = 0;
                while (num2 < 3)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                    num2++;
                }
                for (num2 = this.m - 4; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                }
            }
            return this.rip;
        }

        public double[,] smooth7(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = 4;
                while (num2 < (this.m - 5))
                {
                    this.rip[i, num2] = (((((((-2.0 * ripsMatrix[i, num2 - 3]) + (3.0 * ripsMatrix[i, num2 - 2])) + (6.0 * ripsMatrix[i, num2 - 1])) + (7.0 * ripsMatrix[i, num2])) + (6.0 * ripsMatrix[i, num2 + 1])) + (3.0 * ripsMatrix[i, num2 + 2])) - (2.0 * ripsMatrix[i, num2 + 3])) / 21.0;
                    num2++;
                }
                num2 = 0;
                while (num2 < 4)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                    num2++;
                }
                for (num2 = this.m - 5; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                }
            }
            return this.rip;
        }

        public double[,] smooth9(double[,] ripsMatrix)
        {
            for (int i = 0; i < this.n; i++)
            {
                int num2 = 5;
                while (num2 < (this.m - 6))
                {
                    this.rip[i, num2] = (((((((((-21.0 * ripsMatrix[i, num2 - 4]) + (14.0 * ripsMatrix[i, num2 - 3])) + (39.0 * ripsMatrix[i, num2 - 2])) + (54.0 * ripsMatrix[i, num2 - 1])) + (59.0 * ripsMatrix[i, num2])) + (54.0 * ripsMatrix[i, num2 + 1])) + (39.0 * ripsMatrix[i, num2 + 2])) + (14.0 * ripsMatrix[i, num2 + 3])) - (21.0 * ripsMatrix[i, num2 + 4])) / 231.0;
                    num2++;
                }
                num2 = 0;
                while (num2 < 5)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                    num2++;
                }
                for (num2 = this.m - 6; num2 < this.m; num2++)
                {
                    this.rip[i, num2] = ripsMatrix[i, num2];
                }
            }
            return this.rip;
        }

        public double[,] snv(double[,] ripsMatrix)
        {
            int num;
            int num2;
            double[] numArray = new double[this.n];
            double[,] numArray2 = new double[this.n, this.m];
            double[] x = new double[this.m];
            for (num = 0; num < this.n; num++)
            {
                num2 = 0;
                while (num2 < this.m)
                {
                    x[num2] = ripsMatrix[num, num2];
                    num2++;
                }
                numArray[num] = this.avg(this.m, x);
                num2 = 0;
                while (num2 < this.m)
                {
                    numArray2[num, num2] = ripsMatrix[num, num2] - numArray[num];
                    num2++;
                }
            }
            double[] numArray4 = new double[this.n];
            double[] numArray5 = new double[this.n];
            for (num = 0; num < this.n; num++)
            {
                num2 = 0;
                while (num2 < this.m)
                {
                    numArray4[num] += numArray2[num, num2] * numArray2[num, num2];
                    num2++;
                }
                numArray5[num] = Math.Sqrt(numArray4[num] / ((double) (this.m - 1)));
            }
            for (num = 0; num < this.n; num++)
            {
                for (num2 = 0; num2 < this.m; num2++)
                {
                    if (Math.Abs(numArray5[num]) < 1E-58)
                    {
                        this.rip[num, num2] = ripsMatrix[num, num2];
                    }
                    else
                    {
                        this.rip[num, num2] = numArray2[num, num2] / numArray5[num];
                    }
                }
            }
            return this.rip;
        }

        public double[,] snv_detrend(double[,] ripsMatrix)
        {
            int num;
            double[,] numArray = this.snv(ripsMatrix);
            double[,] y = new double[this.m, 1];
            double[,] regressX = new double[this.m, 3];
            double[] numArray5 = new double[this.m];
            for (num = 0; num < this.m; num++)
            {
                regressX[num, 0] = 1.0;
                regressX[num, 1] = num + 1;
                regressX[num, 2] = (num + 1) * (num + 1);
            }
            for (num = 0; num < this.n; num++)
            {
                double[,] numArray3;
                int index = 0;
                while (index < this.m)
                {
                    y[index, 0] = numArray[num, index];
                    index++;
                }
                this.mlr(this.m, 3, regressX, y, out numArray3);
                index = 0;
                while (index < this.m)
                {
                    numArray5[index] = (numArray3[0, 0] + (numArray3[1, 0] * regressX[index, 1])) + ((numArray3[2, 0] * regressX[index, 1]) * regressX[index, 1]);
                    index++;
                }
                for (index = 0; index < this.m; index++)
                {
                    this.rip[num, index] = numArray[num, index] - numArray5[index];
                }
            }
            return this.rip;
        }

        public double[] ssRipsMatrix(double[,] ripsMatrix)
        {
            double[] numArray = new double[this.m];
            double[] numArray2 = this.avgRipsMatrix(ripsMatrix);
            double[] numArray3 = new double[this.n];
            double[] numArray4 = new double[this.m];
            for (int i = 0; i < this.m; i++)
            {
                int index = 0;
                while (index < this.n)
                {
                    numArray3[index] = (ripsMatrix[index, i] - numArray2[i]) * (ripsMatrix[index, i] - numArray2[i]);
                    index++;
                }
                for (index = 0; index < this.n; index++)
                {
                    numArray4[i] += numArray3[index];
                }
                numArray[i] = Math.Sqrt(numArray4[i] / ((double) (this.n - 1)));
            }
            return numArray;
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

        public double xma(int n, int f, double[,] t, double[,] trevise)
        {
            int num2;
            double[,] numArray5;
            double[,] ripsMatrix = new double[n, f];
            double[,] x = new double[f, n];
            double[,] numArray3 = new double[f, f];
            for (num2 = 0; num2 < n; num2++)
            {
                for (int i = 0; i < f; i++)
                {
                    ripsMatrix[num2, i] = trevise[num2, i];
                }
            }
            double[,] numArray4 = new double[1, f];
            for (num2 = 0; num2 < f; num2++)
            {
                numArray4[0, num2] = t[0, num2];
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
            int num2;
            int num3;
            int num = 0;
            double[] numArray = new double[n];
            double[,] numArray2 = new double[n, f];
            for (num2 = 0; num2 < n; num2++)
            {
                for (num3 = 0; num3 < f; num3++)
                {
                    numArray2[num2, num3] = tf[num2, num3];
                }
            }
            double[,] numArray3 = new double[1, f];
            num3 = 0;
            while (num3 < f)
            {
                numArray3[0, num3] = t[0, num3];
                num3++;
            }
            for (num2 = 0; num2 < n; num2++)
            {
                numArray[num2] = 0.0;
                for (num3 = 0; num3 < f; num3++)
                {
                    numArray[num2] += (numArray3[0, num3] - numArray2[num2, num3]) * (numArray3[0, num3] - numArray2[num2, num3]);
                }
                numArray[num2] = Math.Sqrt(numArray[num2]);
            }
            num = 0;
            for (num2 = 0; num2 < n; num2++)
            {
                if (numArray[num2] < nnd)
                {
                    num++;
                }
            }
            return num;
        }

        public double xplsPrey(int m, int f, int maxF, double[,] x, double[,] w, double[,] p, double[,] q, double[,] b, out double[] th, out double[] remainX)
        {
            int num3;
            double num = 0.0;
            remainX = new double[m];
            th = new double[maxF];
            double[] numArray = new double[f];
            int index = 0;
            while (index < m)
            {
                remainX[index] = x[0, index];
                index++;
            }
            for (num3 = 0; num3 < maxF; num3++)
            {
                th[num3] = 0.0;
                index = 0;
                while (index < m)
                {
                    th[num3] += remainX[index] * w[num3, index];
                    index++;
                }
                for (index = 0; index < m; index++)
                {
                    remainX[index] -= th[num3] * p[num3, index];
                }
            }
            index = 0;
            while (index < m)
            {
                remainX[index] = x[0, index];
                index++;
            }
            for (num3 = 0; num3 < f; num3++)
            {
                numArray[num3] = 0.0;
                index = 0;
                while (index < m)
                {
                    numArray[num3] += remainX[index] * w[num3, index];
                    index++;
                }
                for (index = 0; index < m; index++)
                {
                    remainX[index] -= numArray[num3] * p[num3, index];
                }
                num += (b[num3, 0] * numArray[num3]) * q[num3, 0];
            }
            return num;
        }

        public double xremainDistanceOfPcr(int f, int m, double[,] x, double[,] t, double[,] w)
        {
            int num2;
            double d = 0.0;
            double[,] ripsMatrix = new double[f, m];
            for (num2 = 0; num2 < f; num2++)
            {
                for (int i = 0; i < m; i++)
                {
                    ripsMatrix[num2, i] = w[num2, i];
                }
            }
            double[,] y = new double[m, f];
            y = this.conMatrix(f, m, ripsMatrix);
            double[,] numArray3 = new double[1, f];
            double[] numArray4 = new double[m];
            for (num2 = 0; num2 < m; num2++)
            {
                numArray4[num2] = x[0, num2];
            }
            double[,] numArray5 = this.multiMatrix(1, m, m, f, x, y);
            double[,] numArray6 = this.multiMatrix(1, f, f, m, numArray5, ripsMatrix);
            for (num2 = 0; num2 < m; num2++)
            {
                numArray4[num2] -= numArray6[0, num2];
            }
            d = 0.0;
            for (num2 = 0; num2 < m; num2++)
            {
                d += numArray4[num2] * numArray4[num2];
            }
            return Math.Sqrt(d);
        }

        public double xremainDistanceOfPls(int f, int m, double[,] x, double[,] w, double[,] p)
        {
            double[] numArray = new double[m];
            double[] numArray2 = new double[f];
            int index = 0;
            while (index < m)
            {
                numArray[index] = x[0, index];
                index++;
            }
            for (int i = 0; i < f; i++)
            {
                numArray2[i] = 0.0;
                index = 0;
                while (index < m)
                {
                    numArray2[i] += numArray[index] * w[i, index];
                    index++;
                }
                for (index = 0; index < m; index++)
                {
                    numArray[index] -= numArray2[i] * p[i, index];
                }
            }
            double d = 0.0;
            for (int j = 0; j < m; j++)
            {
                d += numArray[j] * numArray[j];
            }
            return Math.Sqrt(d);
        }
    }
}

