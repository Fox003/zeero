using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public static class FSMUtilities
{
    public static void ValidateTransition(ref DynamicBuffer<DisableStateRequest> removeBuffer, ref DynamicBuffer<EnableStateRequest> addBuffer, ref BlobArray<TransitionPair> pairs, ComponentType currentState, Entity fsmEntity)
    {
        for (int i = 0; i < addBuffer.Length; i++)
        {
            var request = addBuffer[i];
            var requestedTransition = new TransitionPair(currentState, request.StateToEnable);
            bool valid = false;
            
            for (int j = 0; j < pairs.Length; j++)
            {
                var pair = pairs[j];
                if (pair.FromState == requestedTransition.FromState && pair.ToState == requestedTransition.ToState)
                {
                    valid = true;
                    break;
                }
            }

            if (valid)
            {
                Debug.Log($"Valid transition! Transitioning from: {requestedTransition.FromState} to: {requestedTransition.ToState}");
                removeBuffer.Add(new DisableStateRequest
                {
                    Entity = fsmEntity,
                    StateToDisable = currentState
                });
            }
            else
            {
                Debug.Log($"Invalid state change... from: {requestedTransition.FromState.TypeIndex} to: {requestedTransition.ToState.TypeIndex}");
                request.IgnoreRequestFlag = true;
                addBuffer[i] = request;
            }
        }
    }

    /*public static BlobAssetReference<TransitionMapBlob> CreateTransitionMapBlobAssetReference(List<TransitionPair> transitionPairs)
    {
        var builder = new BlobBuilder(Allocator.Temp);
        ref var root = ref builder.ConstructRoot<TransitionMapBlob>();
        var array = builder.Allocate(ref root.Pairs, transitionPairs.Count);

        for (int i = 0; i < transitionPairs.Count; i++)
            array[i] = transitionPairs[i];

        var blobRef = builder.CreateBlobAssetReference<TransitionMapBlob>(Allocator.Persistent);
        builder.Dispose();
        
        return blobRef;
    }*/
    
    public static void SetComponentStateReflectively(
        IBaker baker, 
        Entity entity, 
        ComponentType stateType, 
        bool isEnabled)
    {
        Type componentSystemType = TypeManager.GetType(stateType.TypeIndex);

        if (componentSystemType == null)
        {
            UnityEngine.Debug.LogError($"Cannot resolve System.Type for ComponentType: {stateType}");
            return;
        }
        
        Type bakerType = typeof(IBaker);
        
        MethodInfo genericMethod = bakerType.GetMethod(
            nameof(baker.SetComponentEnabled), 
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new Type[] { typeof(Entity), typeof(bool) },
            null
        );

        if (genericMethod == null)
        {
            UnityEngine.Debug.LogError("Reflection failed: Could not find the generic SetComponentEnabled method.");
            return;
        }
        
        MethodInfo specificMethod = genericMethod.MakeGenericMethod(componentSystemType);
        
        specificMethod.Invoke(baker, new object[] { entity, isEnabled });
    }
}
