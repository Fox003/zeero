using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ScreenWrappingAuthoring : MonoBehaviour
{
    public Transform TopBorder;
    public Transform BottomBorder;
    public Transform LeftBorder;
    public Transform RightBorder;
}

class ScreenWrappingAuthoringBaker : Baker<ScreenWrappingAuthoring>
{
    public override void Bake(ScreenWrappingAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new ScreenBoundaries()
        {
            LeftBoundary = authoring.LeftBorder.position.x,
            RightBoundary = authoring.RightBorder.position.x,
            TopBoundary = authoring.TopBorder.position.y,
            BottomBoundary = authoring.BottomBorder.position.y,
        });
    }
}

public struct ScreenBoundaries : IComponentData
{
    public float LeftBoundary;
    public float RightBoundary;
    public float TopBoundary;
    public float BottomBoundary;
}