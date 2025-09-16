using System;
using System.Collections.Generic;

namespace ButbleConfig
{
    public static class RecordBinarySearch
    {
        public static int R_BinarySearch<T>(this List<T> records, RecordComparer<T> recordComparer, params object[] keyValues) where T : class
        {
            int minIndex = 0;
            int maxIndex = records.Count - 1;

            while (minIndex <= maxIndex)
            {
                int midIndex = (minIndex + maxIndex) / 2;
                int comparisonResult = recordComparer.Compare(records[midIndex], keyValues);

                if (comparisonResult == 0)
                    return midIndex;

                if (comparisonResult < 1)
                    maxIndex = midIndex - 1;
                else
                    minIndex = midIndex + 1;
            }

            return -1;
        }
    }
}