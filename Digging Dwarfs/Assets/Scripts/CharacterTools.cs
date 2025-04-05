using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PogoHead{
    public float offset;
    public float colliderOffset;
    public Sprite sprite;
    public int customBehaviour;
}

[Serializable]
public enum PogoHeads {
    Jump,
    BaseShovel,
    IronShovel,
    Jetpack,
    Bomb,
    Drill,
    Capsule,
    PortalGun
}

public class CharacterTools : MonoBehaviour
{
    public List<PogoHead> pogoHeads;
       
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;
    
    
    [SerializeField] private PogoHeads pogoHead;

    private PogoHeads _lastFramePogoHead;

    private void ChangePogoHead(PogoHeads newPogoHead)
    {
        spriteRenderer.sprite = pogoHeads[(int)newPogoHead].sprite;
        spriteRenderer.transform.localPosition = new Vector3(0, pogoHeads[(int)newPogoHead].offset, 0);
        circleCollider.offset = new Vector2(0, pogoHeads[(int)newPogoHead].colliderOffset);
        _lastFramePogoHead = newPogoHead;
    }
    
    private void Start()
    {
        _lastFramePogoHead = PogoHeads.Jump;
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (pogoHead != _lastFramePogoHead)
        {
            ChangePogoHead(pogoHead);
        }
    }
}
