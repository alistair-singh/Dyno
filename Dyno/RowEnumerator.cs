using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dyno
{
  public sealed class RowEnumerator : IEnumerator<IRow>
  {
    private readonly SqlDataReader _reader;

    public RowEnumerator(SqlDataReader reader)
    {
      _reader = reader;
    }

    public IRow Current
    {
      get { return _reader.Dyno(); }
    }

    public void Dispose()
    {
    }

    object IEnumerator.Current
    {
      get { return Current; }
    }

    public bool MoveNext()
    {
      return _reader.Read();
    }

    public void Reset()
    {
      throw new NotImplementedException();
    }
  }
}