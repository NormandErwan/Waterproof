using Unity.Mathematics;
using UnityEngine;

public static class AnimationCurveExtensions
{
    public static float2 Evaluate(this AnimationCurve curve, float2 value)
    {
        return new float2(
            x: curve.Evaluate(value.x),
            y: curve.Evaluate(value.y)
        );
    }
}
