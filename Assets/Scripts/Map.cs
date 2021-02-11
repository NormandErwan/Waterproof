using System;
using UnityEngine;

[Serializable]
public sealed class Map
{
    [SerializeField]
    private Interval domain = Interval.Default;

    [SerializeField]
    private Interval image = Interval.Default;

    public Map(Interval domain, Interval image, Func<float, float> function, bool isClamped = false)
    {
        Domain = domain;
        Function = function;
        Image = image;
        IsClamped = isClamped;
    }

    public Interval Domain { get => domain; set => domain = value; }

    public Func<float, float> Function { get; }

    public Interval Image { get => image; set => image = value; }

    public bool IsClamped { get; }

    public float this[float value]
    {
        get
        {
            float result = Function(Image.Min + (value - Domain.Min) * Image.Distance / Domain.Distance);
            return IsClamped ? Image.Clamp(result) : result;
        }
    }

    public static Map Linear(Interval domain, Interval image, bool isClamped = false)
    {
        return new Map(domain, image, x => x, isClamped);
    }
}