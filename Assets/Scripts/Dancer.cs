using Unity.Mathematics;
using UnityEngine;

public sealed class Dancer : MonoBehaviour
{
    [SerializeField]
    private FloorTiles floor = null;

    [SerializeField]
    private Dancers dancers = null;

    [SerializeField, Range(0.01f, 2)]
    public float maxDistanceFactor = 1;

    [SerializeField]
    private AnimationCurve weightCurve = null;

    [SerializeField, Range(0.01f, 2)]
    public float maxWeightFactor = 1;

    private float MaxDistance => maxDistanceFactor * (0.9f + 0.1f * math.length(transform.localScale));

    private float MaxWeight => maxWeightFactor * (0.9f + 0.1f * math.length(transform.localScale));

    private void Start()
    {
        floor = FindObjectOfType<FloorTiles>();
        dancers = FindObjectOfType<Dancers>();
        dancers.Add(this);
    }

    private void OnDestroy()
    {
        if (dancers != null)
        {
            dancers.Remove(this);
        }
    }

    private void Update()
    {
        var weigthMap = Map.Linear((0, MaxDistance), (0, MaxWeight));
        var feetsPosition = new float3(transform.position - transform.lossyScale / 2).xz;

        foreach (var tile in floor.Tiles)
        {
            float distance = math.distance(feetsPosition, new float3(tile.transform.position).xz);

            float depth = weightCurve.Evaluate(weigthMap[distance]);
            floor.SetDepth(tile, depth);
        }

        float2 distances = feetsPosition - new float3(floor.transform.position).xy;
        floor.AddDancerPosition(distances);
    }
}
