using System.Collections.Generic;
using Augmenta;
using UnityEngine;
using Unity.Mathematics;

public sealed class FloorTiles : MonoBehaviour
{
    private const float DefaultDepth = 0;

    [SerializeField]
    private AugmentaManager augmentaManager = null;

    [SerializeField]
    private float3 tileSize = new float3(1);

    [SerializeField]
    private float2 tileMargin = 0.1f * new float2(1);

    [SerializeField]
    private Transform tilePrefab = null;

    private HashSet<Transform> visibles = new HashSet<Transform>();
    private Dictionary<Transform, float> depths = new Dictionary<Transform, float>();
    private Pool<Transform> tilesPool;

    public IEnumerable<Transform> Tiles => tilesPool.Actives;

    private void Start()
    {
        tilesPool = new Pool<Transform>(tilePrefab);
        augmentaManager.sceneUpdated += OnValidate;
    }

    private void OnValidate()
    {
        if (tilesPool != null)
        {
            ClearTiles();
            InitTiles();
            augmentaManager.sceneUpdated -= OnValidate;
        }
    }

    private void LateUpdate()
    {
        UpdateTiles();
    }

    public void SetVisible(Transform tile)
    {
        visibles.Add(tile);
    }

    public void SetDepth(Transform tile, float depth)
    {
        if (!depths.TryGetValue(tile, out float value))
        {
            value = DefaultDepth;
        }
        depths[tile] = math.max(value, depth);
    }

    private void ClearTiles()
    {
        visibles.Clear();
        UpdateTiles();
        tilesPool.ReturnAll();
    }

    private void InitTiles()
    {
        var sceneSize = new float3(augmentaManager.augmentaScene.width, augmentaManager.augmentaScene.height, 0);
        var totalTileSize = tileSize + new float3(tileMargin, 0);
        var tileCount = math.ceil(sceneSize / totalTileSize);
        var origin = tileCount * totalTileSize / 2;

        for (int x = 0; x <= tileCount.x; x++)
        {
            for (int y = 0; y <= tileCount.y; y++)
            {
                var tile = tilesPool.Get();
                tile.transform.parent = transform;
                tile.transform.localPosition = new float3(x, y, 0) * totalTileSize - origin;
                tile.transform.localEulerAngles = float3.zero;
                tile.transform.localScale = tileSize;
            }
        }
    }

    private void UpdateTiles()
    {
        foreach (var tile in Tiles)
        {
            tile.gameObject.SetActive(visibles.Contains(tile));

            if (!depths.TryGetValue(tile, out float depth))
            {
                depth = DefaultDepth;
            }
            tile.SetLocalPositionZ(depth);
        }

        visibles.Clear();
        depths.Clear();
    }
}
