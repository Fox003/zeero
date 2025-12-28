using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

partial struct UpgradePickerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<UpgradeLibraryBlob>(out var library)) return;
        int totalUpgrades = library.AllUpgrades.Value.Length;

        if (totalUpgrades < 3) return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var random = new Unity.Mathematics.Random((uint)SystemAPI.Time.ElapsedTime + 1);

        foreach (var (upgrades, entity) in SystemAPI.Query<RefRW<CurrentUpgrades>>()
                     .WithAll<RequestUpgradeRollTag>()
                     .WithEntityAccess())
        {
            // --- UNIQUE SELECTION LOGIC ---
            int a = random.NextInt(0, totalUpgrades);

            int b;
            do { b = random.NextInt(0, totalUpgrades); } while (b == a);

            int c;
            do { c = random.NextInt(0, totalUpgrades); } while (c == a || c == b);

            upgrades.ValueRW.UpgradeID1 = a;
            upgrades.ValueRW.UpgradeID2 = b;
            upgrades.ValueRW.UpgradeID3 = c;

            ecb.SetComponentEnabled<RequestUpgradeRollTag>(entity, false);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
