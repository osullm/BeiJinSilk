// Decompiled with JetBrains decompiler
// Type: MatrixLibrary.MatrixDimensionException
// Assembly: 蚕蛹快速分拣系统, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F3346BD-13EF-4147-9CD3-93C638874BA1
// Assembly location: E:\04 软件编程\C# 程序\01 不用\新建文件夹\程序\NIRQUEST.exe

using System;

namespace MatrixLibrary
{
  internal class MatrixDimensionException : ApplicationException
  {
    public MatrixDimensionException()
      : base("Dimension of the two matrices not suitable for this operation !")
    {
    }
  }
}
