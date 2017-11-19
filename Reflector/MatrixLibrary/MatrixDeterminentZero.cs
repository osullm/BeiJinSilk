namespace MatrixLibrary
{
    using System;

    internal class MatrixDeterminentZero : ApplicationException
    {
        public MatrixDeterminentZero() : base("Determinent of matrix equals zero, inverse can't be found !")
        {
        }
    }
}

