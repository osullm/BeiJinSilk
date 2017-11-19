namespace JSDU
{
    using System;

    internal class DataHandling
    {
        public double MaxValue(double[] array)
        {
            double num = 0.0;
            for (int i = 0; i < array.Length; i++)
            {
                if (num < array[i])
                {
                    num = array[i];
                }
            }
            return num;
        }

        public int MaxValueIndex(double[] array)
        {
            double num = 0.0;
            int num2 = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (num < array[i])
                {
                    num = array[i];
                    num2 = i;
                }
            }
            return num2;
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

        public double MinValue(double[] array)
        {
            double num = 1E+32;
            for (int i = 0; i < array.Length; i++)
            {
                if (num > array[i])
                {
                    num = array[i];
                }
            }
            return num;
        }

        public int MinValueIndex(double[] array)
        {
            double num = 1E+32;
            int num2 = 0;
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

        public double sum(double[] array)
        {
            double num = 0.0;
            for (int i = 0; i < array.Length; i++)
            {
                num += array[i];
            }
            return num;
        }

        public float[] WaveNum_Length(float[] OriginalData)
        {
            float[] numArray = new float[OriginalData.Length];
            for (int i = 0; i < OriginalData.Length; i++)
            {
                numArray[i] = (1f / OriginalData[i]) * 10000f;
            }
            return numArray;
        }
    }
}

