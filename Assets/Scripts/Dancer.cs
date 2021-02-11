using Unity.Mathematics;
using UnityEngine;

public sealed class Dancer : MonoBehaviour
{
    [SerializeField]
    private FloorTiles floor = null;

    [SerializeField]
    private float maxDistanceFactor = 1;

    [SerializeField]
    private AnimationCurve weightCurve = null;

    [SerializeField]
    private float maxWeightFactor = 1;

    private float MaxDistance => maxDistanceFactor * math.length(transform.localScale);

    private float MaxWeight => maxWeightFactor * math.length(transform.localScale);

    private void Start()
    {
        floor = FindObjectOfType<FloorTiles>();
    }

    private void Update()
    {
        foreach (var tile in floor.Tiles)
        {
            float3 feetsPosition = tile.transform.position - tile.transform.lossyScale / 2;
            float distance = math.distance(new float3(transform.position).xz, feetsPosition.xz);
            if (distance <= MaxDistance)
            {
                floor.SetVisible(tile);
            }

            float depth = weightCurve.Evaluate(distance / MaxDistance) * MaxWeight;
            floor.SetDepth(tile, depth);
        }
    }
}
