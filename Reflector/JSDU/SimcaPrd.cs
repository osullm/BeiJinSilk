namespace JSDU
{
    using System;
    using System.Runtime.InteropServices;

    internal class SimcaPrd
    {
        private ripsPreDeal PreDeal = new ripsPreDeal();

        public int MinValue(double[] array)
        {
            double num = 1E+32;
            int num2 = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (num > array[i])
                {
                    num = array[i];
                    num2 = i;
                }
            }
            return num2;
        }

        public void PCAPro(double[,] NewData, double[,] loads, double[,] ssq, out double[,] scores, out double[] res, out double[] tsqvals)
        {
            int num8;
            int length = NewData.GetLength(0);
            int num2 = NewData.GetLength(1);
            int num3 = loads.GetLength(0);
            int num4 = loads.GetLength(1);
            scores = this.PreDeal.multiMatrix(length, num2, num3, num4, NewData, loads);
            double[,] x = this.PreDeal.conMatrix(length, num2, NewData);
            double[,] y = this.PreDeal.conMatrix(length, num4, scores);
            double[,] numArray3 = this.PreDeal.multiMatrix(num3, num4, num4, length, loads, y);
            double[,] numArray4 = this.PreDeal.subMatrix(num2, length, num3, length, x, numArray3);
            int num5 = numArray4.GetLength(0);
            int num6 = numArray4.GetLength(1);
            res = new double[num6];
            int index = 0;
            while (index < num6)
            {
                res[index] = 0.0;
                num8 = 0;
                while (num8 < num5)
                {
                    res[index] += numArray4[num8, index] * numArray4[num8, index];
                    num8++;
                }
                index++;
            }
            double[,] numArray5 = new double[length, num4];
            for (num8 = 0; num8 < length; num8++)
            {
                index = 0;
                while (index < num4)
                {
                    numArray5[num8, index] = scores[num8, index] * scores[num8, index];
                    index++;
                }
            }
            double[] numArray6 = new double[num4];
            for (num8 = 0; num8 < num4; num8++)
            {
                numArray6[num8] = 1.0 / ssq[num8, 1];
            }
            tsqvals = new double[length];
            for (num8 = 0; num8 < length; num8++)
            {
                tsqvals[num8] = 0.0;
                for (index = 0; index < num4; index++)
                {
                    tsqvals[num8] += numArray5[num8, index] * numArray6[index];
                }
            }
        }

        public int[] SimcaPrdt(double[,] newx, double[,] mod)
        {
            int num6;
            int index = (int) mod[0, 9];
            int length = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            length = newx.GetLength(0);
            num3 = newx.GetLength(1);
            num4 = mod.GetLength(0);
            num5 = mod.GetLength(1);
            int[] numArray = new int[index + 1];
            for (num6 = 0; num6 < index; num6++)
            {
                numArray[num6] = (int) mod[1, num6];
            }
            numArray[index] = num4 + 1;
            double[,] numArray2 = new double[length, index];
            double[,] numArray3 = new double[length, index];
            double[,] numArray4 = new double[length, index];
            double[] numArray5 = new double[index];
            int[] numArray6 = new int[length];
            numArray6[0] = -1;
            for (num6 = 0; num6 < index; num6++)
            {
                int num12;
                int num13;
                double[,] numArray7 = new double[numArray[num6 + 1] - numArray[num6], num5];
                for (int i = 0; i < (numArray[num6 + 1] - numArray[num6]); i++)
                {
                    for (int j = 0; j < num5; j++)
                    {
                        numArray7[i, j] = mod[(numArray[num6] + i) - 1, j];
                    }
                }
                num4 = numArray7.GetLength(0);
                num5 = numArray7.GetLength(1);
                double num9 = numArray7[0, 14];
                double num10 = numArray7[0, 15];
                double num11 = numArray7[0, 6];
                double[,] ssq = new double[(int) numArray7[0, 13], 4];
                numArray5[num6] = numArray7[0, 9];
                double[,] loads = null;
                double[,] newData = null;
                if (1.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num4 - 2];
                    num12 = 2;
                    while (num12 < num4)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            loads[num13, num12 - 2] = numArray7[num12, num13];
                            num13++;
                        }
                        num12++;
                    }
                    newData = (double[,]) newx.Clone();
                    num12 = 0;
                    while (num12 < ((int) numArray7[0, 13]))
                    {
                        ssq[num12, 1] = numArray7[1, num12];
                        num12++;
                    }
                }
                else if (2.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num4 - 3];
                    num12 = 3;
                    while (num12 < num4)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            loads[num13, num12 - 3] = numArray7[num12, num13];
                            num13++;
                        }
                        num12++;
                    }
                    newData = (double[,]) newx.Clone();
                    num12 = 0;
                    while (num12 < ((int) numArray7[0, 13]))
                    {
                        ssq[num12, 1] = numArray7[2, num12];
                        num12++;
                    }
                    num12 = 0;
                    while (num12 < length)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            newData[num12, num13] = newx[num12, num13] - numArray7[1, num13];
                            num13++;
                        }
                        num12++;
                    }
                }
                else if (3.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num4 - 4];
                    num12 = 4;
                    while (num12 < num4)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            loads[num13, num12 - 4] = numArray7[num12, num13];
                            num13++;
                        }
                        num12++;
                    }
                    newData = (double[,]) newx.Clone();
                    num12 = 0;
                    while (num12 < ((int) numArray7[0, 13]))
                    {
                        ssq[num12, 1] = numArray7[3, num12];
                        num12++;
                    }
                    for (num12 = 0; num12 < length; num12++)
                    {
                        for (num13 = 0; num13 < ((int) numArray7[0, 7]); num13++)
                        {
                            newData[num12, num13] = (newx[num12, num13] - numArray7[1, num13]) / numArray7[2, num13];
                        }
                    }
                }
                double[,] scores = null;
                double[] res = null;
                double[] tsqvals = null;
                this.PCAPro(newData, loads, ssq, out scores, out res, out tsqvals);
                int num14 = 0;
                while (num14 < length)
                {
                    numArray2[num14, num6] = tsqvals[num14] / num10;
                    numArray3[num14, num6] = res[num14] / num9;
                    numArray4[num14, num6] = Math.Sqrt((numArray2[num14, num6] * numArray2[num14, num6]) + (numArray3[num14, num6] * numArray3[num14, num6]));
                    num14++;
                }
                for (num14 = 0; num14 < length; num14++)
                {
                    double[] array = new double[index];
                    for (int k = 0; k < index; k++)
                    {
                        array[k] = numArray4[num14, k];
                    }
                    numArray6[num14] = this.MinValue(array);
                }
            }
            return numArray6;
        }

        public int[] SimcaPrdt(double[,] newx, double[,] mod, out string[] parameterOut)
        {
            int num6;
            parameterOut = new string[newx.GetLength(0)];
            int index = (int) mod[0, 9];
            int length = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            length = newx.GetLength(0);
            num3 = newx.GetLength(1);
            num4 = mod.GetLength(0);
            num5 = mod.GetLength(1);
            int[] numArray = new int[index + 1];
            for (num6 = 0; num6 < index; num6++)
            {
                numArray[num6] = (int) mod[1, num6];
            }
            numArray[index] = num4 + 1;
            double[,] numArray2 = new double[length, index];
            double[,] numArray3 = new double[length, index];
            double[,] numArray4 = new double[length, index];
            double[] numArray5 = new double[index];
            int[] numArray6 = new int[length];
            numArray6[0] = -1;
            for (num6 = 0; num6 < index; num6++)
            {
                int num12;
                int num13;
                double[,] numArray7 = new double[numArray[num6 + 1] - numArray[num6], num5];
                for (int i = 0; i < (numArray[num6 + 1] - numArray[num6]); i++)
                {
                    for (int j = 0; j < num5; j++)
                    {
                        numArray7[i, j] = mod[(numArray[num6] + i) - 1, j];
                    }
                }
                num4 = numArray7.GetLength(0);
                num5 = numArray7.GetLength(1);
                double num9 = numArray7[0, 14];
                double num10 = numArray7[0, 15];
                double num11 = numArray7[0, 6];
                double[,] ssq = new double[(int) numArray7[0, 13], 4];
                numArray5[num6] = numArray7[0, 9];
                double[,] loads = null;
                double[,] newData = null;
                if (1.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num4 - 2];
                    num12 = 2;
                    while (num12 < num4)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            loads[num13, num12 - 2] = numArray7[num12, num13];
                            num13++;
                        }
                        num12++;
                    }
                    newData = (double[,]) newx.Clone();
                    num12 = 0;
                    while (num12 < ((int) numArray7[0, 13]))
                    {
                        ssq[num12, 1] = numArray7[1, num12];
                        num12++;
                    }
                }
                else if (2.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num4 - 3];
                    num12 = 3;
                    while (num12 < num4)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            loads[num13, num12 - 3] = numArray7[num12, num13];
                            num13++;
                        }
                        num12++;
                    }
                    newData = (double[,]) newx.Clone();
                    num12 = 0;
                    while (num12 < ((int) numArray7[0, 13]))
                    {
                        ssq[num12, 1] = numArray7[2, num12];
                        num12++;
                    }
                    num12 = 0;
                    while (num12 < length)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            newData[num12, num13] = newx[num12, num13] - numArray7[1, num13];
                            num13++;
                        }
                        num12++;
                    }
                }
                else if (3.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num4 - 4];
                    num12 = 4;
                    while (num12 < num4)
                    {
                        num13 = 0;
                        while (num13 < ((int) numArray7[0, 7]))
                        {
                            loads[num13, num12 - 4] = numArray7[num12, num13];
                            num13++;
                        }
                        num12++;
                    }
                    newData = (double[,]) newx.Clone();
                    num12 = 0;
                    while (num12 < ((int) numArray7[0, 13]))
                    {
                        ssq[num12, 1] = numArray7[3, num12];
                        num12++;
                    }
                    for (num12 = 0; num12 < length; num12++)
                    {
                        for (num13 = 0; num13 < ((int) numArray7[0, 7]); num13++)
                        {
                            newData[num12, num13] = (newx[num12, num13] - numArray7[1, num13]) / numArray7[2, num13];
                        }
                    }
                }
                double[,] scores = null;
                double[] res = null;
                double[] tsqvals = null;
                this.PCAPro(newData, loads, ssq, out scores, out res, out tsqvals);
                int num14 = 0;
                while (num14 < length)
                {
                    numArray2[num14, num6] = tsqvals[num14] / num10;
                    numArray3[num14, num6] = res[num14] / num9;
                    numArray4[num14, num6] = Math.Sqrt((numArray2[num14, num6] * numArray2[num14, num6]) + (numArray3[num14, num6] * numArray3[num14, num6]));
                    num14++;
                }
                for (num14 = 0; num14 < length; num14++)
                {
                    string[] strArray;
                    IntPtr ptr;
                    double[] array = new double[index];
                    parameterOut[num14] = "rt2rqsum";
                    for (int k = 0; k < index; k++)
                    {
                        array[k] = numArray4[num14, k];
                        (strArray = parameterOut)[(int) (ptr = (IntPtr) num14)] = strArray[(int) ptr] + array[k].ToString() + ",";
                    }
                    numArray6[num14] = this.MinValue(array);
                    (strArray = parameterOut)[(int) (ptr = (IntPtr) num14)] = strArray[(int) ptr] + "    结果：" + numArray6[num14].ToString();
                }
            }
            return numArray6;
        }
    }
}

