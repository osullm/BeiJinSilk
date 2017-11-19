namespace MatrixLibrary
{
    using System;

    internal class MatrixNullException : ApplicationException
    {
        public MatrixNullException() : base("To do this operation, matrix can not be null")
        {
        }
    }
}

