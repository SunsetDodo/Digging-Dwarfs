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
    }
}
