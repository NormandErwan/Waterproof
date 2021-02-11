using System;
using UnityEngine;

[Serializable]
public sealed class Interval
{
    public static readonly Interval Default = (DefaultMin, DefaultMax);

    public static readonly float DefaultMax = 1;

    public static readonly float DefaultMin = 0;

    [SerializeField]
    private float min = DefaultMin;

    [SerializeField]
    private float max = DefaultMax;

    public Interval(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float Distance => Max - Min;

    public float Max { get => max; set => max = value; }

    public float Min { get => min; set => min = value; }

    public static implicit operator Interval((float min, float max) value)
    {
        return new Interval(value.min, value.max);
    }

    public static implicit operator (float min, float max)(Interval interval)
    {
        return (interval.Min, interval.Max);
    }

    public float Clamp(float value)
    {
        if (Min.CompareTo(value) > 0)
        {
            return Min;
        }
        else if (value.CompareTo(Max) > 0)
        {
            return Max;
        }
        else
        {
            return value;
        }
    }
}