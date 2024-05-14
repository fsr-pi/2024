using System;

namespace Logging;

public class A
{
  public A() { Console.WriteLine("CTOR od A"); }
}

public class B
{
  public B(A a) { Console.WriteLine("CTOR od B"); }
}
