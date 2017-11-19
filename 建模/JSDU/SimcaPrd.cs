namespace JSDU
{
    using Algorithm;
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
            for (int i = 0; i < num6; i++)
            {
                res[i] = 0.0;
                for (int n = 0; n < num5; n++)
                {
                    res[i] += numArray4[n, i] * numArray4[n, i];
                }
            }
            double[,] numArray5 = new double[length, num4];
            for (int j = 0; j < length; j++)
            {
                for (int num10 = 0; num10 < num4; num10++)
                {
                    numArray5[j, num10] = scores[j, num10] * scores[j, num10];
                }
            }
            double[] numArray6 = new double[num4];
            for (int k = 0; k < num4; k++)
            {
                numArray6[k] = 1.0 / ssq[k, 1];
            }
            tsqvals = new double[length];
            for (int m = 0; m < length; m++)
            {
                tsqvals[m] = 0.0;
                for (int num13 = 0; num13 < num4; num13++)
                {
                    tsqvals[m] += numArray5[m, num13] * numArray6[num13];
                }
            }
        }

        public int[] SimcaPrdt(double[,] newx, double[,] mod)
        {
            int index = (int) mod[0, 9];
            int length = 0;
            int num3 = 0;
            int num4 = 0;
            length = newx.GetLength(0);
            newx.GetLength(1);
            num3 = mod.GetLength(0);
            num4 = mod.GetLength(1);
            int[] numArray = new int[index + 1];
            for (int i = 0; i < index; i++)
            {
                numArray[i] = (int) mod[1, i];
            }
            numArray[index] = num3 + 1;
            double[,] numArray2 = new double[length, index];
            double[,] numArray3 = new double[length, index];
            double[,] numArray4 = new double[length, index];
            double[] numArray5 = new double[index];
            int[] numArray6 = new int[length];
            numArray6[0] = -1;
            for (int j = 0; j < index; j++)
            {
                double[,] numArray7 = new double[numArray[j + 1] - numArray[j], num4];
                for (int k = 0; k < (numArray[j + 1] - numArray[j]); k++)
                {
                    for (int num8 = 0; num8 < num4; num8++)
                    {
                        numArray7[k, num8] = mod[(numArray[j] + k) - 1, num8];
                    }
                }
                num3 = numArray7.GetLength(0);
                num4 = numArray7.GetLength(1);
                double num9 = numArray7[0, 14];
                double num10 = numArray7[0, 15];
                double num1 = numArray7[0, 6];
                double[,] ssq = new double[(int) numArray7[0, 13], 4];
                numArray5[j] = numArray7[0, 9];
                double[,] loads = null;
                double[,] newData = null;
                if (1.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num3 - 2];
                    for (int num11 = 2; num11 < num3; num11++)
                    {
                        for (int num12 = 0; num12 < ((int) numArray7[0, 7]); num12++)
                        {
                            loads[num12, num11 - 2] = numArray7[num11, num12];
                        }
                    }
                    newData = (double[,]) newx.Clone();
                    for (int num13 = 0; num13 < ((int) numArray7[0, 13]); num13++)
                    {
                        ssq[num13, 1] = numArray7[1, num13];
                    }
                }
                else if (2.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num3 - 3];
                    for (int num14 = 3; num14 < num3; num14++)
                    {
                        for (int num15 = 0; num15 < ((int) numArray7[0, 7]); num15++)
                        {
                            loads[num15, num14 - 3] = numArray7[num14, num15];
                        }
                    }
                    newData = (double[,]) newx.Clone();
                    for (int num16 = 0; num16 < ((int) numArray7[0, 13]); num16++)
                    {
                        ssq[num16, 1] = numArray7[2, num16];
                    }
                    for (int num17 = 0; num17 < length; num17++)
                    {
                        for (int num18 = 0; num18 < ((int) numArray7[0, 7]); num18++)
                        {
                            newData[num17, num18] = newx[num17, num18] - numArray7[1, num18];
                        }
                    }
                }
                else if (3.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num3 - 4];
                    for (int num19 = 4; num19 < num3; num19++)
                    {
                        for (int num20 = 0; num20 < ((int) numArray7[0, 7]); num20++)
                        {
                            loads[num20, num19 - 4] = numArray7[num19, num20];
                        }
                    }
                    newData = (double[,]) newx.Clone();
                    for (int num21 = 0; num21 < ((int) numArray7[0, 13]); num21++)
                    {
                        ssq[num21, 1] = numArray7[3, num21];
                    }
                    for (int num22 = 0; num22 < length; num22++)
                    {
                        for (int num23 = 0; num23 < ((int) numArray7[0, 7]); num23++)
                        {
                            newData[num22, num23] = (newx[num22, num23] - numArray7[1, num23]) / numArray7[2, num23];
                        }
                    }
                }
                double[,] scores = null;
                double[] res = null;
                double[] tsqvals = null;
                this.PCAPro(newData, loads, ssq, out scores, out res, out tsqvals);
                for (int m = 0; m < length; m++)
                {
                    numArray2[m, j] = tsqvals[m] / num10;
                    numArray3[m, j] = res[m] / num9;
                    numArray4[m, j] = Math.Sqrt((numArray2[m, j] * numArray2[m, j]) + (numArray3[m, j] * numArray3[m, j]));
                }
                for (int n = 0; n < length; n++)
                {
                    double[] array = new double[index];
                    for (int num26 = 0; num26 < index; num26++)
                    {
                        array[num26] = numArray4[n, num26];
                    }
                    numArray6[n] = this.MinValue(array);
                }
            }
            return numArray6;
        }

        public int[] SimcaPrdt(double[,] newx, double[,] mod, out string[] parameterOut)
        {
            parameterOut = new string[newx.GetLength(0)];
            int index = (int) mod[0, 9];
            int length = 0;
            int num3 = 0;
            int num4 = 0;
            length = newx.GetLength(0);
            newx.GetLength(1);
            num3 = mod.GetLength(0);
            num4 = mod.GetLength(1);
            int[] numArray = new int[index + 1];
            for (int i = 0; i < index; i++)
            {
                numArray[i] = (int) mod[1, i];
            }
            numArray[index] = num3 + 1;
            double[,] numArray2 = new double[length, index];
            double[,] numArray3 = new double[length, index];
            double[,] numArray4 = new double[length, index];
            double[] numArray5 = new double[index];
            int[] numArray6 = new int[length];
            for (int j = 0; j < length; j++)
            {
                numArray6[j] = -1;
            }
            for (int k = 0; k < index; k++)
            {
                double[,] numArray7 = new double[numArray[k + 1] - numArray[k], num4];
                for (int m = 0; m < (numArray[k + 1] - numArray[k]); m++)
                {
                    for (int num9 = 0; num9 < num4; num9++)
                    {
                        numArray7[m, num9] = mod[(numArray[k] + m) - 1, num9];
                    }
                }
                num3 = numArray7.GetLength(0);
                num4 = numArray7.GetLength(1);
                double num10 = numArray7[0, 14];
                double num11 = numArray7[0, 15];
                double num1 = numArray7[0, 6];
                double[,] ssq = new double[(int) numArray7[0, 13], 4];
                numArray5[k] = numArray7[0, 9];
                double[,] loads = null;
                double[,] newData = null;
                if (1.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num3 - 2];
                    for (int num12 = 2; num12 < num3; num12++)
                    {
                        for (int num13 = 0; num13 < ((int) numArray7[0, 7]); num13++)
                        {
                            loads[num13, num12 - 2] = numArray7[num12, num13];
                        }
                    }
                    newData = (double[,]) newx.Clone();
                    for (int num14 = 0; num14 < ((int) numArray7[0, 13]); num14++)
                    {
                        ssq[num14, 1] = numArray7[1, num14];
                    }
                }
                else if (2.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num3 - 3];
                    for (int num15 = 3; num15 < num3; num15++)
                    {
                        for (int num16 = 0; num16 < ((int) numArray7[0, 7]); num16++)
                        {
                            loads[num16, num15 - 3] = numArray7[num15, num16];
                        }
                    }
                    newData = (double[,]) newx.Clone();
                    for (int num17 = 0; num17 < ((int) numArray7[0, 13]); num17++)
                    {
                        ssq[num17, 1] = numArray7[2, num17];
                    }
                    for (int num18 = 0; num18 < length; num18++)
                    {
                        for (int num19 = 0; num19 < ((int) numArray7[0, 7]); num19++)
                        {
                            newData[num18, num19] = newx[num18, num19] - numArray7[1, num19];
                        }
                    }
                }
                else if (3.0 == numArray7[0, 11])
                {
                    loads = new double[(int) numArray7[0, 7], num3 - 4];
                    for (int num20 = 4; num20 < num3; num20++)
                    {
                        for (int num21 = 0; num21 < ((int) numArray7[0, 7]); num21++)
                        {
                            loads[num21, num20 - 4] = numArray7[num20, num21];
                        }
                    }
                    newData = (double[,]) newx.Clone();
                    for (int num22 = 0; num22 < ((int) numArray7[0, 13]); num22++)
                    {
                        ssq[num22, 1] = numArray7[3, num22];
                    }
                    for (int num23 = 0; num23 < length; num23++)
                    {
                        for (int num24 = 0; num24 < ((int) numArray7[0, 7]); num24++)
                        {
                            newData[num23, num24] = (newx[num23, num24] - numArray7[1, num24]) / numArray7[2, num24];
                        }
                    }
                }
                double[,] scores = null;
                double[] res = null;
                double[] tsqvals = null;
                this.PCAPro(newData, loads, ssq, out scores, out res, out tsqvals);
                for (int n = 0; n < length; n++)
                {
                    numArray2[n, k] = tsqvals[n] / num11;
                    numArray3[n, k] = res[n] / num10;
                    numArray4[n, k] = Math.Sqrt((numArray2[n, k] * numArray2[n, k]) + (numArray3[n, k] * numArray3[n, k]));
                }
                for (int num26 = 0; num26 < length; num26++)
                {
                    string[] strArray2;
                    IntPtr ptr2;
                    double[] array = new double[index];
                    parameterOut[num26] = "rt2rqsum";
                    for (int num27 = 0; num27 < index; num27++)
                    {
                        string[] strArray;
                        IntPtr ptr;
                        array[num27] = numArray4[num26, num27];
                        (strArray = parameterOut)[(int) (ptr = (IntPtr) num26)] = strArray[(int) ptr] + array[num27].ToString() + ",";
                    }
                    numArray6[num26] = this.MinValue(array);
                    (strArray2 = parameterOut)[(int) (ptr2 = (IntPtr) num26)] = strArray2[(int) ptr2] + "    结果：" + numArray6[num26].ToString();
                }
            }
            return numArray6;
        }

        public double[] SimcaPrdtOneModel(double[,] newx, double[,] mod)
        {
            int length = 0;
            int num2 = 0;
            length = newx.GetLength(0);
            newx.GetLength(1);
            num2 = mod.GetLength(0);
            mod.GetLength(1);
            double[] numArray = new double[length];
            double[] numArray2 = new double[length];
            double[] numArray3 = new double[length];
            int[] numArray4 = new int[length];
            numArray4[0] = -1;
            double[,] numArray5 = mod;
            num2 = numArray5.GetLength(0);
            numArray5.GetLength(1);
            double num3 = numArray5[0, 14];
            double num4 = numArray5[0, 15];
            double num1 = numArray5[0, 6];
            double[,] ssq = new double[(int) numArray5[0, 13], 4];
            double[,] loads = null;
            double[,] newData = null;
            if (1.0 == numArray5[0, 11])
            {
                loads = new double[(int) numArray5[0, 7], num2 - 2];
                for (int j = 2; j < num2; j++)
                {
                    for (int m = 0; m < ((int) numArray5[0, 7]); m++)
                    {
                        loads[m, j - 2] = numArray5[j, m];
                    }
                }
                newData = (double[,]) newx.Clone();
                for (int k = 0; k < ((int) numArray5[0, 13]); k++)
                {
                    ssq[k, 1] = numArray5[1, k];
                }
            }
            else if (2.0 == numArray5[0, 11])
            {
                loads = new double[(int) numArray5[0, 7], num2 - 3];
                for (int n = 3; n < num2; n++)
                {
                    for (int num9 = 0; num9 < ((int) numArray5[0, 7]); num9++)
                    {
                        loads[num9, n - 3] = numArray5[n, num9];
                    }
                }
                newData = (double[,]) newx.Clone();
                for (int num10 = 0; num10 < ((int) numArray5[0, 13]); num10++)
                {
                    ssq[num10, 1] = numArray5[2, num10];
                }
                for (int num11 = 0; num11 < length; num11++)
                {
                    for (int num12 = 0; num12 < ((int) numArray5[0, 7]); num12++)
                    {
                        newData[num11, num12] = newx[num11, num12] - numArray5[1, num12];
                    }
                }
            }
            else if (3.0 == numArray5[0, 11])
            {
                loads = new double[(int) numArray5[0, 7], num2 - 4];
                for (int num13 = 4; num13 < num2; num13++)
                {
                    for (int num14 = 0; num14 < ((int) numArray5[0, 7]); num14++)
                    {
                        loads[num14, num13 - 4] = numArray5[num13, num14];
                    }
                }
                newData = (double[,]) newx.Clone();
                for (int num15 = 0; num15 < ((int) numArray5[0, 13]); num15++)
                {
                    ssq[num15, 1] = numArray5[3, num15];
                }
                for (int num16 = 0; num16 < length; num16++)
                {
                    for (int num17 = 0; num17 < ((int) numArray5[0, 7]); num17++)
                    {
                        newData[num16, num17] = (newx[num16, num17] - numArray5[1, num17]) / numArray5[2, num17];
                    }
                }
            }
            double[,] scores = null;
            double[] res = null;
            double[] tsqvals = null;
            this.PCAPro(newData, loads, ssq, out scores, out res, out tsqvals);
            for (int i = 0; i < length; i++)
            {
                numArray[i] = tsqvals[i] / num4;
                numArray2[i] = res[i] / num3;
                numArray3[i] = Math.Sqrt((numArray[i] * numArray[i]) + (numArray2[i] * numArray2[i]));
            }
            return numArray3;
        }
    }
}

