
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BlueJay.Events
{
    /// <summary>
    /// An unsafe class that provides a set of methods to access the underlying 
    /// data representations of collections.
    /// </summary>
    public static class CollectionsMarshal
    {
        public static Span<T> AsSpan<T>(List<T>? list)
        {
            Span<T> span = default;
            if (list is not null)
            {
                int size = list.Count;
                T[] items = list.ToArray();
                Debug.Assert(items is not null, "Implementation depends on List<T> always having an array.");

                if ((uint)size > (uint)items.Length)
                {
                    throw new InvalidOperationException("ConcurrentOperationsNotSupported");
                }

                Debug.Assert(typeof(T[]) == items.GetType(), 
                    "Implementation depends on List<T> always using a T[] and not U[] where U : T.");
                span = new Span<T>(items, 0, size);
            }

            return span;
        }      
    }
}