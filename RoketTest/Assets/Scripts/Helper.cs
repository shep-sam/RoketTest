using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Helper
{
    public static List<int> GetUnrepitedList(int totalCount, int finalCount)
    {
        var possible = Enumerable.Range(0, totalCount).ToList();
        var listNumbers = new List<int>();

        for (int i = 0; i < finalCount; i++)
        {
            int index = Random.Range(0, possible.Count);
            listNumbers.Add(possible[index]);
            possible.RemoveAt(index);
        }

        return listNumbers;
    }
}
