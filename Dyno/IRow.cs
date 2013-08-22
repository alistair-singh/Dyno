namespace Dyno
{
  public interface IRow
  {
    T Get<T>(string columnName);
  }
}