using System.Reflection;

namespace MoodMate.Components.Data.Comparer;

sealed internal class GenericComparer<T> : IComparer<T>
{
    private readonly string SortColumn;
    public GenericComparer(string sortColumn)
    {
        SortColumn = sortColumn;
    }
    public int Compare(T x, T y)
    {
        PropertyInfo propertyInfo = typeof(T).GetProperty(SortColumn);
        IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
        IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);
        return obj2.CompareTo(obj1);
    }
}
