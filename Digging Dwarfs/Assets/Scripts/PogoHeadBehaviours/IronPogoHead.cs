using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IronPogoHead : MonoBehaviour
{
    
    [SerializeField] private float velocityMagnitudeThreshold = 0.1f;
    [SerializeField] private float breakTileVelocityThreshold = 3.2f;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform pogoBase;

    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private Inventory inventory;
    
    private Rigidbody2D _rigidbody;
    private float _velocityCache;

    private bool _canDestroy;

    private void OnEnable()
    {
        _canDestroy = true;
    }
    
    private void OnDisable()
    {
        _canDestroy = false;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > velocityMagnitudeThreshold)
            _velocityCache = _rigidbody.velocity.magnitude;
    }


    private Vector3Int? GetClosestNonNullTile(Vector3 worldPosition)
    {
        var centerCell = tilemap.WorldToCell(worldPosition);
        Vector3Int? closestCell = null;
        var closestDistance = Mathf.Infinity;

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                var offset = new Vector3Int(x, y, 0);
                var currentCell = centerCell + offset;

                if (tilemap.GetTile(currentCell) == null) continue;
                var tileCenter = tilemap.GetCellCenterWorld(currentCell);
                var distance = Vector3.Distance(worldPosition, tileCenter);

                if (!(distance < closestDistance)) continue;
                closestDistance = distance;
                closestCell = currentCell;
            }
        }
        return closestCell;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_canDestroy) return;
        if (!(_velocityCache > breakTileVelocityThreshold)) return;
        Debug.Log("Breaking tile" + _velocityCache);
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                var closestTile = GetClosestNonNullTile(collision.contacts[0].point);
                if (!closestTile.HasValue) return;
                var oreIndex = worldGenerator.OreIndex(closestTile.Value);
                Debug.Log(oreIndex);
                tilemap.SetTile(closestTile.Value, null);
                
                if (oreIndex == -1) continue;
                inventory.AddStone(1, oreIndex);
            }
        }
        _rigidbody.velocity = Vector2.zero;
        _velocityCache = 0;
    }
}
