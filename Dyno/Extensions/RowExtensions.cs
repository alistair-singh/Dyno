using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dyno
{
  public static class RowExtensions
  {
    public static T Map<T>(this IRow row)
    {
      var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Select(x => new KeyValuePair<string, MethodInfo>(x.Name, x.GetSetMethod()))
        .Where(x => x.Value != null);

      var newT = Activator.CreateInstance<T>();
      foreach (var prop in properties)
        prop.Value.Invoke(newT, new[] { row.Get<object>(prop.Key) });

      return newT;
    }
  }
}