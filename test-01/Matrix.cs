namespace test_01
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public class Matrix
    {
        private double[,] m_data;

        public Matrix(Matrix m)
        {
            int row = m.Row;
            int col = m.Col;
            this.m_data = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    this.m_data[i, j] = m[i, j];
                }
            }
        }

        public Matrix(int row)
        {
            this.m_data = new double[row, row];
        }

        public Matrix(double[,] sdata)
        {
            this.m_data = new double[sdata.GetLength(0), sdata.GetLength(1)];
            for (int i = 0; i < sdata.GetLength(0); i++)
            {
                for (int j = 0; j < sdata.GetLength(1); j++)
                {
                    this.m_data[i, j] = sdata[i, j];
                }
            }
        }

        public Matrix(int row, int col)
        {
            this.m_data = new double[row, col];
        }

        public Matrix Exchange(int i, int j)
        {
            for (int k = 0; k < this.Col; k++)
            {
                double num = this.m_data[i, k];
                this.m_data[i, k] = this.m_data[j, k];
                this.m_data[j, k] = num;
            }
            return this;
        }

        public Matrix Inverse()
        {
            double num2;
            if (this.Row != this.Col)
            {
                Exception exception = new Exception("求逆的矩阵不是方阵");
                throw exception;
            }
            Matrix matrix = new Matrix(this);
            Matrix matrix2 = new Matrix(this.Row);
            matrix2.SetUnit();
            for (int i = 0; i < this.Row; i++)
            {
                int num = matrix.Pivot(i);
                if (matrix.m_data[num, i] == 0.0)
                {
                    Exception exception2 = new Exception("求逆的矩阵的行列式的值等于0,");
                    throw exception2;
                }
                if (num != i)
                {
                    matrix.Exchange(i, num);
                    matrix2.Exchange(i, num);
                }
                matrix2.Multiple(i, 1.0 / matrix[i, i]);
                matrix.Multiple(i, 1.0 / matrix[i, i]);
                for (int k = i + 1; k < this.Row; k++)
                {
                    num2 = -matrix[k, i] / matrix[i, i];
                    matrix.MultipleAdd(k, i, num2);
                }
            }
            for (int j = this.Row - 1; j > 0; j--)
            {
                for (int m = j - 1; m >= 0; m--)
                {
                    num2 = -matrix[m, j] / matrix[j, j];
                    matrix.MultipleAdd(m, j, num2);
                    matrix2.MultipleAdd(m, j, num2);
                }
            }
            return matrix2;
        }

        public bool IsSquare()
        {
            return (this.Row == this.Col);
        }

        public bool IsSymmetric()
        {
            if (this.Row != this.Col)
            {
                return false;
            }
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = i + 1; j < this.Col; j++)
                {
                    if (this.m_data[i, j] != this.m_data[j, i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private Matrix Multiple(int index, double mul)
        {
            for (int i = 0; i < this.Col; i++)
            {
                this.m_data[index, i]*= mul;
            }
            return this;
        }

        private Matrix MultipleAdd(int index, int src, double mul)
        {
            for (int i = 0; i < this.Col; i++)
            {
                this.m_data[index, i] += this.m_data[src, i] * mul;
            }
            return this;
        }

        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            if (lhs.Row != rhs.Row)
            {
                Exception exception = new Exception("相加的两个矩阵的行数不等");
                throw exception;
            }
            if (lhs.Col != rhs.Col)
            {
                Exception exception2 = new Exception("相加的两个矩阵的列数不等");
                throw exception2;
            }
            int row = lhs.Row;
            int col = lhs.Col;
            Matrix matrix = new Matrix(row, col);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    double num5 = lhs[i, j] + rhs[i, j];
                    matrix[i, j] = num5;
                }
            }
            return matrix;
        }

        public static Matrix operator /(Matrix lhs, Matrix rhs)
        {
            return (lhs * rhs.Inverse());
        }

        public static Matrix operator /(double d, Matrix m)
        {
            return (Matrix) (d * m.Inverse());
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            if (lhs.Col != rhs.Row)
            {
                Exception exception = new Exception("相乘的两个矩阵的行列数不匹配");
                throw exception;
            }
            Matrix matrix = new Matrix(lhs.Row, rhs.Col);
            for (int i = 0; i < lhs.Row; i++)
            {
                for (int j = 0; j < rhs.Col; j++)
                {
                    double num = 0.0;
                    for (int k = 0; k < lhs.Col; k++)
                    {
                        num += lhs[i, k] * rhs[k, j];
                    }
                    matrix[i, j] = num;
                }
            }
            return matrix;
        }

        public static Matrix operator *(double d, Matrix m)
        {
            Matrix matrix = new Matrix(m);
            for (int i = 0; i < matrix.Row; i++)
            {
                for (int j = 0; j < matrix.Col; j++)
                {
                    Matrix matrix2;
                    int num3;
                    int num4;
                    (matrix2 = matrix)[num3 = i, num4 = j] = matrix2[num3, num4] * d;
                }
            }
            return matrix;
        }

        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            if (lhs.Row != rhs.Row)
            {
                Exception exception = new Exception("相减的两个矩阵的行数不等");
                throw exception;
            }
            if (lhs.Col != rhs.Col)
            {
                Exception exception2 = new Exception("相减的两个矩阵的列数不等");
                throw exception2;
            }
            int row = lhs.Row;
            int col = lhs.Col;
            Matrix matrix = new Matrix(row, col);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    double num5 = lhs[i, j] - rhs[i, j];
                    matrix[i, j] = num5;
                }
            }
            return matrix;
        }

        public static Matrix operator -(Matrix m)
        {
            Matrix matrix = new Matrix(m);
            for (int i = 0; i < matrix.Row; i++)
            {
                for (int j = 0; j < matrix.Col; j++)
                {
                    matrix[i, j] = -matrix[i, j];
                }
            }
            return matrix;
        }

        public static Matrix operator +(Matrix m)
        {
            return new Matrix(m);
        }

        private int Pivot(int row)
        {
            int num = row;
            for (int i = row + 1; i < this.Row; i++)
            {
                if (this.m_data[i, row] > this.m_data[num, row])
                {
                    num = i;
                }
            }
            return num;
        }

        public void Setdiag(double[] diagvalue)
        {
            this.m_data = new double[diagvalue.Length, diagvalue.Length];
            for (int i = 0; i < diagvalue.Length; i++)
            {
                this.m_data[i, i] = diagvalue[i];
            }
        }

        public void SetUnit()
        {
            for (int i = 0; i < this.m_data.GetLength(0); i++)
            {
                for (int j = 0; j < this.m_data.GetLength(1); j++)
                {
                    this.m_data[i, j] = (i == j) ? ((double) 1) : ((double) 0);
                }
            }
        }

        public void SetValue(double d)
        {
            for (int i = 0; i < this.m_data.GetLength(0); i++)
            {
                for (int j = 0; j < this.m_data.GetLength(1); j++)
                {
                    this.m_data[i, j] = d;
                }
            }
        }

        public static Matrix SqureDot(Matrix a, Matrix d)
        {
            int row = a.Row;
            int col = a.Col;
            if ((d.Row != row) || (d.Col != col))
            {
                throw new Exception("矩阵点乘出错");
            }
            double[,] sdata = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    sdata[i, j] = a[i, j] * d[i, j];
                }
            }
            return new Matrix(sdata);
        }

        public static Matrix sumMatrix(Matrix a, int d)
        {
            int row = a.Row;
            int col = a.Col;
            double[,] sdata = null;
            if (d == 1)
            {
                sdata = new double[1, a.Col];
                for (int j = 0; j < col; j++)
                {
                    double num4 = 0.0;
                    for (int k = 0; k < row; k++)
                    {
                        num4 += a[k, j];
                    }
                    sdata[0, j] = num4;
                }
                return new Matrix(sdata);
            }
            if (d != 2)
            {
                throw new Exception("d in subMatrix is error");
            }
            sdata = new double[a.Row, 1];
            for (int i = 0; i < row; i++)
            {
                double num7 = 0.0;
                for (int m = 0; m < col; m++)
                {
                    num7 += a[i, m];
                }
                sdata[i, 0] = num7;
            }
            return new Matrix(sdata);
        }

        public double[,] toArray(Matrix a)
        {
            int row = a.Row;
            int col = a.Col;
            double[,] numArray = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    numArray[i, j] = a[i, j];
                }
            }
            return numArray;
        }

        public double ToDouble()
        {
            Trace.Assert((this.Row == 1) && (this.Col == 1));
            return this.m_data[0, 0];
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    str = str + string.Format("{0} ", this.m_data[i, j]);
                }
                str = str + "\r\n";
            }
            return str;
        }

        public Matrix Transpose()
        {
            Matrix matrix = new Matrix(this.Col, this.Row);
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < this.Col; j++)
                {
                    matrix[j, i] = this.m_data[i, j];
                }
            }
            return matrix;
        }

        public int Col
        {
            get
            {
                return this.m_data.GetLength(1);
            }
        }

        public double this[int row, int col]
        {
            get
            {
                return this.m_data[row, col];
            }
            set
            {
                this.m_data[row, col] = value;
            }
        }

        public int Row
        {
            get
            {
                return this.m_data.GetLength(0);
            }
        }
    }
}

