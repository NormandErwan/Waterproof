using UnityEngine;

public static class TranformExtensions
{
    public static void SetLocalPositionZ(this Transform transform, float value)
    {
        var position = transform.localPosition;
        position.z = value;
        transform.localPosition = position;
    }
}