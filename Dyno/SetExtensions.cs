using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dyno
{
  public static class SetExtensions
  {
    public static IEnumerable<T> Map<T>(this ISet set)
    {
      var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Select(x => new KeyValuePair<string, MethodInfo>(x.Name, x.GetSetMethod()))
        .Where(x => x.Value != null).ToArray();

      foreach (var row in set)
      {
        var newT = Activator.CreateInstance<T>();
        foreach (var prop in properties)
          prop.Value.Invoke(newT, new[] { row.Get<object>(prop.Key) });
        yield return newT;
      }
    }
  }
}
