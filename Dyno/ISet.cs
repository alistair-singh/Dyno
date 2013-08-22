using System.Collections.Generic;

namespace Dyno
{
  public interface ISet : IEnumerable<IRow>
  {
    IEnumerable<string> Columns { get; }
  }
}