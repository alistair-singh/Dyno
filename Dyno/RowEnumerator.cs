using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dyno
{
  public sealed class RowEnumerator : IEnumerator<IRow>
  {
    private readonly SqlDataReader _reader;
    private readonly bool _dispose;

    public RowEnumerator(SqlDataReader reader, bool dispose = false)
    {
      _reader = reader;
      _dispose = dispose;
    }

    public IRow Current
    {
      get { return _reader.Dyno(); }
    }

    public void Dispose()
    {
      if (_dispose)
        _reader.Dispose();
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