
namespace Sorting
{
    public interface ISorter
    {
        List<T> Sort<T, TKey>(List<T> list, Func<T, TKey> sortByKey);
        List<T> Sort<T, TKey>(List<T> list, Func<T, TKey> sortByKey, bool sortAscending);
    }
}
