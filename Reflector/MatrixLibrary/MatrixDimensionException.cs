namespace MatrixLibrary
{
    using System;

    internal class MatrixDimensionException : ApplicationException
    {
        public MatrixDimensionException() : base("Dimension of the two matrices not suitable for this operation !")
        {
        }
    }
}

