using System.Collections;
using System.Collections.Generic;

namespace Dyno
{
  public sealed class Result : IResult
  {
    private readonly StoredProcedureExecutor _executor;

    internal Result(StoredProcedureExecutor executor)
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