using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class BlobUtils
{
    public static BlobAssetReference<BlobArray<T>> CreateBlobArrayRefFromList<T>(List<T> list) where T : unmanaged
    {
        var builder = new BlobBuilder(Allocator.Temp);

        // Construct the root as the array itself
        ref var root = ref builder.ConstructRoot<BlobArray<T>>();

        // Allocate space for the elements
        var arrayBuilder = builder.Allocate(ref root, list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            arrayBuilder[i] = list[i];
        }

        var blobRef = builder.CreateBlobAssetReference<BlobArray<T>>(Allocator.Persistent);
        builder.Dispose();
        return blobRef;
    }

    public static BlobAssetReference<BlobArray<TDest>> CreateBlobArrayRefFromList<TSource, TDest>(
        List<TSource> list,
        Func<TSource, int, TDest> converter)
        where TDest : unmanaged
    {
        var builder = new BlobBuilder(Allocator.Temp);
        ref var root = ref builder.ConstructRoot<BlobArray<TDest>>();
        var arrayBuilder = builder.Allocate(ref root, list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            arrayBuilder[i] = converter(list[i], i);
        }

        var blobRef = builder.CreateBlobAssetReference<BlobArray<TDest>>(Allocator.Persistent);
        builder.Dispose();
        return blobRef;
    }
}
