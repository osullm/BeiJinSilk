namespace MatrixLibrary
{
    using System;

    internal class MatrixNotSquare : ApplicationException
    {
        public MatrixNotSquare() : base("To do this operation, matrix must be a square matrix !")
        {
        }
    }
}

