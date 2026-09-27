namespace Sorting
{
    public class MergeSorter : ISorter
    {
        // Overloaded method covers the most common usecase
        public List<T> Sort<T, TKey>(List<T> list, Func<T, TKey> keySelector)
        {
            return Sort(list, keySelector, true);
        }

        public List<T> Sort<T, TKey>(List<T> list, Func<T, TKey> keySelector, bool sortAscending)
        {
            // Return if the list cannot be split in two (base case)
            if (list.Count <= 1)
                return list;

            int midIndex = list.Count / 2;

            // Split into two lists
            var leftList = list.Take(midIndex).ToList();
            var rightList = list.Skip(midIndex).ToList();

            // Sort each half of list recursively
            leftList = Sort(leftList, keySelector, sortAscending);
            rightList = Sort(rightList, keySelector, sortAscending);

            // Create custom Comparer for reverse sorting if needed (swaps left and right items during comparison)
            IComparer<TKey> comparer = sortAscending
                ? Comparer<TKey>.Default : Comparer<TKey>.Create((left, right) => Comparer<TKey>.Default.Compare(right, left));

            List<T> mergedList = [];
            // Merge each sorted half of the list
            mergedList = MergeAscending(leftList, rightList, keySelector, comparer);

            return mergedList;
        }

        private static List<T> MergeAscending<T, TKey>(List<T> leftList, List<T> rightList, Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            List<T> sortedList = [];

            int iLeft = 0;
            int iRight = 0;

            while (sortedList.Count < leftList.Count + rightList.Count)
            {
                // Grab key to sort by
                TKey leftKey = keySelector(leftList[iLeft]);
                TKey rightKey = keySelector(rightList[iRight]);

                // If the left key is lower than the right
                // *** Note: can sort in opposite order with custom Comparer passed in
                comparer ??= Comparer<TKey>.Default;

                if (comparer.Compare(leftKey, rightKey) <= 0)
                {
                    // Add left item to the sorted list
                    sortedList.Add(leftList[iLeft]);
                    iLeft++;
                }
                else
                {
                    // Otherwise add the right item
                    sortedList.Add(rightList[iRight]);
                    iRight++;
                }

                // If left or right index reaches the end of the list,
                // add the rest of the remaining items from the other half to the sorted list.
                //
                // **Note: AddRange is more efficient than foreach loop because the final List size is calculated up front
                // and resizing is done once before adding elements.
                if (iLeft == leftList.Count)
                {
                    sortedList.AddRange(rightList.Skip(iRight));
                }
                else if (iRight == rightList.Count)
                {
                    sortedList.AddRange(leftList.Skip(iLeft));
                }
                // Loop exits when sortedList is the same length as both source lists combined
            }

            return sortedList;
        }
    }
}
