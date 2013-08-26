using System;
using System.Collections;
using System.Collections.Generic;

namespace Dyno
{
  public sealed class Result : IResult
  {
    private readonly ExecutorBase _executor;

    internal Result(ExecutorBase executor)
    {
      _executor = executor;
    }

    public IEnumerator<Set> GetEnumerator()
    {
      return new SetEnumerator(_executor.Reader());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}