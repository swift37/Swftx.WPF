﻿namespace System.Collections.ObjectModel;

public static class ObservableCollectionExtensions
{
    public static void Add<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
            collection.Add(item);
    }

    public static void ClearAdd<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        collection.Clear();
        collection.Add(items);
    }
}