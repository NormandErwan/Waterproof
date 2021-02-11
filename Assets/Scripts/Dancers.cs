using System.Collections.Generic;
using UnityEngine;

public class Dancers : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve distanceFactorCurve = null;

    [SerializeField]
    private AnimationCurve weigthFactorCurve = null;

    [SerializeField]
    private List<Dancer> list = new List<Dancer>();

    public void Add(Dancer dancer)
    {
        list.Add(dancer);
    }

    public void Remove(Dancer dancer)
    {
        list.Remove(dancer);
    }

    public void SetDistanceFactor(float factor)
    {
        foreach (var dancer in list)
        {
            dancer.maxDistanceFactor = distanceFactorCurve.Evaluate(factor);
        }
    }

    public void SetWeightFactor(float factor)
    {
        foreach (var dancer in list)
        {
            dancer.maxWeightFactor = weigthFactorCurve.Evaluate(factor);
        }
    }
}
