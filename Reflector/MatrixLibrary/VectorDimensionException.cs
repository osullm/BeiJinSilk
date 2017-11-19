namespace MatrixLibrary
{
    using System;

    internal class VectorDimensionException : ApplicationException
    {
        public VectorDimensionException() : base("Dimension of matrix must be [3 , 1] to do this operation !")
        {
        }
    }
}

