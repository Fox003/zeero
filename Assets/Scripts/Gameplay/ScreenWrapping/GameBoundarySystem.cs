using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct GameBoundarySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<ScreenBoundaries>(out var bounds)) return;

        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>())
        {
            float3 pos = transform.ValueRO.Position;

            if (pos.x > bounds.RightBoundary) pos.x = bounds.LeftBoundary;
            else if (pos.x < bounds.LeftBoundary) pos.x = bounds.RightBoundary;

            if (pos.y > bounds.TopBoundary) pos.y = bounds.BottomBoundary;
            else if (pos.y < bounds.BottomBoundary) pos.y = bounds.TopBoundary;

            transform.ValueRW.Position = pos;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
