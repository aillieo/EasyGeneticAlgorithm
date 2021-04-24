using System;
using System.Collections.Generic;
using System.Text;

namespace AillieoUtils
{
    [Serializable]
    public class Matrix
    {
        public int column { get; private set; }
        public int row { get; private set; }
        internal double[] data { get; private set; }

        public Matrix(int row, int column)
        {
            this.row = row;
            this.column = column;
            this.data = new double[row * column];
        }
        public Matrix(double[,] values)
        {
            row = values.GetUpperBound(0) + 1;
            column = values.GetUpperBound(1) + 1;
            data = new double[row * column];
            for (int i = 0; i < column; ++i)
            {
                for (int j = 0; j < row; ++j)
                {
                    this[j, i] = values[j, i];
                }
            }
        }
        public Matrix(double[] vector)
        {
            row = 1;
            column = vector.GetUpperBound(0) + 1;
            data = new double[1 * column];
            for (int i = 0; i < vector.Length; i++)
            {
                data[0 * column + i] = vector[i];
            }
        }

        public double this[int r, int c]
        {
            get
            {
                return data[r * column + c];
            }
            set
            {
                data[r * column + c] = value;
            }
        }

        public Matrix Transpose()
        {
            throw new NotImplementedException();
        }

        public Vector Flat()
        {
            Vector v = new Vector(column * row);
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < column; ++j)
                {
                    v[i * column + j] = this[i, j];
                }
            }
            return v;
        }

        public void Load(Vector serialized)
        {
            if (serialized.size != this.column * this.row)
            {
                throw new Exception("load fail: size mismatch");
            }

            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < column; ++j)
                {
                    this[i, j] = serialized[i * column + j];
                }
            }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.row != b.row || a.column != b.column)
            {
                throw new Exception("mismatch dimensions ");
            }

            Matrix result = new Matrix(a.row, a.column);
            for (int i = 0; i < a.row; i++)
            {
                for (int j = 0; j < a.column; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }
            return result;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            return a + b * (-1);
        }

        public static Matrix operator *(Matrix matrix, double scalar)
        {
            for (int i = 0; i < matrix.row; i++)
            {
                for (int j = 0; j < matrix.column; j++)
                {
                    matrix[i, j] = matrix[i, j] * scalar;
                }
            }

            return matrix;
        }

        public static Matrix operator *(double scalar, Matrix matrix)
        {
            return matrix * scalar;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.column != b.row)
            {
                throw new Exception("mismatch dimensions");
            }

            Matrix result = new Matrix(a.row, b.column);
            for (int i = 0; i < a.row; i++)
            {
                for (int j = 0; j < b.column; j++)
                {
                    for (int k = 0; k < a.column; k++)
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return result;
        }

        public static void MultiplyNoAlloc(Vector a, Matrix b, Vector r)
        {
            if (a.size != b.row)
            {
                throw new Exception($"mismatch dimensions {a.size} : {b.row}");
            }

            if (r.size != b.column)
            {
                throw new Exception($"mismatch dimensions {r.size} : {b.column}");
            }

            for (int i = 0; i < a.size; i++)
            {
                for (int j = 0; j < b.column; j++)
                {
                    r[j] += a[i] * b[i, j];
                }
            }
        }

        public static void MultiplyNoAllocUnsafe(Vector a, Matrix b, Vector r)
        {
            if (a.size != b.row)
            {
                throw new Exception($"mismatch dimensions {a.size} : {b.row}");
            }

            if (r.size != b.column)
            {
                throw new Exception($"mismatch dimensions {r.size} : {b.column}");
            }

            int column = b.column;
            unsafe
            {
                fixed (double* pa = &a.data[0], pb = &b.data[0], pr = &r.data[0])
                {
                    for (int i = 0; i < a.size; i++)
                    {
                        for (int j = 0; j < b.column; j++)
                        {
                            *(pr + j) += *(pa + i) * *(pb + i * column + j);
                        }
                    }
                }
            }
        }

        public static Vector operator *(Vector a, Matrix b)
        {
            Vector result = new Vector(b.column);
            //MultiplyNoAlloc(a, b, result);
            MultiplyNoAllocUnsafe(a, b, result);
            return result;
        }

        public static Vector operator *(Matrix a, Vector b)
        {
            if (a.column != b.size)
            {
                throw new Exception("mismatch dimensions");
            }

            Vector result = new Vector(a.row);
            for (int i = 0; i < a.row; i++)
            {
                for (int j = 0; j < b.size; j++)
                {
                    result[i] += a[i, j] * b[j];
                }
            }

            return result;
        }


        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            for (int j = 0; j < row; ++j)
            {
                stringBuilder.Append("[");
                for (int k = 0; k < column; ++k)
                {
                    stringBuilder.Append(this[j, k]);
                    stringBuilder.Append(",");
                }
                stringBuilder.Append("]");
                stringBuilder.AppendLine();
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

    }
}
