using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dyno
{
  public sealed class SetEnumerator : IEnumerator<Set>
  {
    private readonly SqlDataReader _reader;
    private bool _firstSet;
    private readonly Set _current;

    public SetEnumerator(SqlDataReader reader)
    {
      _firstSet = true;
      _reader = reader;
      _current = new Set(_reader);
    }

    public Set Current
    {
      get { return _current; }
    }

    public void Dispose()
    {
      _reader.Dispose();
    }

    object IEnumerator.Current
    {
      get { return this.Current; }
    }

    public bool MoveNext()
    {
      if (_firstSet)
      {
        _firstSet = false;
        return true;
      }

      return _reader.NextResult();
    }

    public void Reset()
    {
      throw new NotImplementedException();
    }
  }
}