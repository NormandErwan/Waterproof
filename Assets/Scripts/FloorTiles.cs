using System.Collections.Generic;
using Augmenta;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using System.Collections;

public sealed class FloorTiles : MonoBehaviour
{
    private const float DefaultDepth = 0;

    [SerializeField]
    private AugmentaManager augmentaManager = null;

    [Header("Tiles")]
    [SerializeField]
    private float3 tileSize = new float3(1);

    [SerializeField]
    private AnimationCurve tileSizeFactorCurve = null;

    [SerializeField]
    private float2 tileMargin = 0.1f * new float2(1);

    [SerializeField]
    private Transform tilePrefab = null;

    [Header("Modes")]
    [SerializeField]
    private List<Modes> sequence = new List<Modes>();

    [SerializeField]
    private int mode = 0;

    [SerializeField]
    private AnimationCurve inclinationTilesCurve = null;

    [SerializeField]
    private AnimationCurve inclinationFloorCurve = null;

    [SerializeField]
    private Glitchs glitchs = null;

    private float oldTileSizeFactor = 1;
    private float2 dancerPositions = float2.zero;
    private Modes Mode => sequence[mode];
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetNextMode(127);
            SetNextMode(0);
        }
    }

    private void LateUpdate()
    {
        UpdateTiles();
    }

    private float oldModeValue = 0;

    public void SetNextMode(float value)
    {
        if (value != oldModeValue)
        {
            if (value != 0)
            {
                var previousMode = Mode;

                //mode = mode.Next();
                mode = sequence.GetNextIndex(mode);
                ClearUpdateTiles();
                UpdateTiles();

                if (Mode == Modes.Fixed || previousMode == Modes.Fixed)
                {
                    glitchs.Pixelize(127);
                    glitchs.Pixelize(0);
                }
            }
            oldModeValue = value;
        }
    }

    public void SetSizeFactor(float factor)
    {
        if (oldTileSizeFactor != factor)
        {
            tileSize = tileSizeFactorCurve.Evaluate(factor);
            OnValidate();
        }
    }

    public void SetDepth(Transform tile, float depth)
    {
        if (Mode == Modes.Distance)
        {
            if (!depths.TryGetValue(tile, out float value))
            {
                value = DefaultDepth;
            }
            depths[tile] = math.max(value, depth);
        }
    }

    public void AddDancerPosition(float2 distances)
    {
        if (Mode == Modes.InclinationTiles || Mode == Modes.InclinationFloor)
        {
            dancerPositions += distances;
        }
    }

    private void ClearTiles()
    {
        ClearUpdateTiles();

        foreach (var tile in tilesPool.Actives)
        {
            tile.gameObject.SetActive(false);
        }
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
                var coordinates = new float2(x, y);

                tile.gameObject.SetActive(true);
                tile.gameObject.layer = gameObject.layer;

                tile.transform.parent = transform;
                tile.transform.localPosition = new float3(coordinates, 0) * totalTileSize - origin;
                tile.transform.localEulerAngles = float3.zero;
                tile.transform.localScale = tileSize;
            }
        }
    }

    private void ClearUpdateTiles()
    {
        depths.Clear();
        dancerPositions = float2.zero;
    }

    private void UpdateTiles()
    {
        foreach (var tile in Tiles)
        {
            if (!depths.TryGetValue(tile, out float depth))
            {
                depth = 1;
            }
            tile.transform.localScale = tileSize * depth;

            var tileRot = Mode == Modes.InclinationTiles ? inclinationTilesCurve.Evaluate(dancerPositions) : float2.zero;
            tile.transform.localEulerAngles = new float3(tileRot.y, -tileRot.x, 0);
        }

        var floorRot = Mode == Modes.InclinationFloor ? inclinationFloorCurve.Evaluate(dancerPositions) : float2.zero;
        //print($"{dancerPositions} -> {floorRot}");
        transform.localEulerAngles = new float3(floorRot.y, -floorRot.x, 0);

        ClearUpdateTiles();
    }

    public enum Modes
    {
        Distance,
        Fixed,
        InclinationTiles,
        InclinationFloor
    }
}
