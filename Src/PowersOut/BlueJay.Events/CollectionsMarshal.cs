// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BlueJay.Events//System.Runtime.InteropServices
{
    /// <summary>
    /// An unsafe class that provides a set of methods to access the underlying data representations of collections.
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
                    ThrowException("ConcurrentOperationsNotSupported");
                }

                Debug.Assert(typeof(T[]) == items.GetType(), "Implementation depends on List<T> always using a T[] and not U[] where U : T.");
                span = new Span<T>(items, 0, size);
            }

            return span;
        }

        /*
        public static ref TValue GetValueRefOrNullRef<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return ref Unsafe.AsRef(value); // RnD / TODO
            }
            return ref Unsafe.NullRef<TValue>();
        }

        // RnD / TODO
        public static ref TValue? GetValueRefOrAddDefault<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, out bool exists) where TKey : notnull
            => ref Dictionary<TKey, TValue>.CollectionsMarshalHelper.GetValueRefOrAddDefault(dictionary, key, out exists);

        public static void SetCount<T>(List<T> list, int count)
        {
            if (count < 0)
            {
                ThrowException("Need NonNeg Number: " + nameof(count));
            }

            // Increment the version of the list
            // This is a workaround since we cannot access the internal _version field
            // We will use reflection to access the private field '_version'
            var versionField = typeof(List<T>).GetField("_version", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (versionField != null)
            {
                int version = (int)versionField.GetValue(list);
                versionField.SetValue(list, version + 1);
            }

            if (count > list.Capacity)
            {
                list.Capacity = count;
            }
            else if (count < list.Count && RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                Array.Clear(list.ToArray(), count, list.Count - count);
            }

            var sizeField = typeof(List<T>).GetField("_size", System.Reflection.BindingFlags.NonPublic 
                | System.Reflection.BindingFlags.Instance);

            if (sizeField != null)
            {
                sizeField.SetValue(list, count);
            }
        }
        */

        private static void ThrowException(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}