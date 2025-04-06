using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RockBehaviour : MonoBehaviour
{
    private Camera _camera;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform cursor;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private SpriteRenderer spriteRenderer;


    public float pickaxeStrength = 10;
    public int pickaxeReach = 3;
    public int pickaxeRadius = 1;

    [SerializeField]
    private float _pressTime = 0;
    private Vector3 _lastFrameTile = Vector3.zero;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        cursor.gameObject.SetActive(true);
        characterMovement.enabled = false;
        spriteRenderer.flipX = false;
        spriteRenderer.transform.localPosition = new Vector3(0, 0.01f, 0);
        spriteRenderer.transform.parent.localPosition = new Vector3(0, 0.14f, 0);
    }

    private void OnDisable()
    {
        cursor.gameObject.SetActive(false);
        characterMovement.enabled = true;
        spriteRenderer.transform.localPosition = new Vector3(0, 0.16f, 0);
    }
    
    private void Update()
    {
        if (!_camera) return;
        var worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = tilemap.WorldToCell(worldPosition);
        var gridPosition = tilemap.GetCellCenterWorld(cellPosition);

        cursor.gameObject.SetActive(tilemap.GetTile(cellPosition));
        
        cursor.position = gridPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0) || (_lastFrameTile != gridPosition && tilemap.GetTile(cellPosition)))
            _pressTime = 10 / pickaxeStrength;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _pressTime -= Time.deltaTime;
            if (_pressTime <= 0)
            {
                tilemap.SetTile(cellPosition, null);
            }
        }
        _lastFrameTile = gridPosition;
    }
}
