namespace MatrixLibrary
{
    using System;

    internal class MatrixSingularException : ApplicationException
    {
        public MatrixSingularException() : base("Matrix is singular this operation cannot continue !")
        {
        }
    }
}

