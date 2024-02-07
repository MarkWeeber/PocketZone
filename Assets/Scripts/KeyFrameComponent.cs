using Unity.Mathematics;

[System.Serializable]
public struct KeyFrameComponent
{
    public float Time;
    public float3 Position;
    public float4 Rotation;
    public string Name;
}