using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PogoHead
{
    public Sprite sprite;
    public float offset;
    public float pogoHeadOffset;
    public float characterOffset;
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
    [SerializeField] private Transform pogoBase;
    [SerializeField] private CharacterMovement characterMovement;
    
    [SerializeField] private PogoHeads pogoHead;

    private PogoHeads _lastFramePogoHead;

    private void ChangePogoHead(PogoHeads newPogoHead)
    {
        spriteRenderer.sprite = pogoHeads[(int)newPogoHead].sprite;
        pogoBase.localPosition = new Vector3(0, pogoHeads[(int)newPogoHead].offset, 0);
        spriteRenderer.transform.localPosition = new Vector3(0, pogoHeads[(int)newPogoHead].pogoHeadOffset, 0);
        characterMovement.dwarfOffset = pogoHeads[(int)newPogoHead].characterOffset;
        _lastFramePogoHead = newPogoHead;
    }
    
    private void Start()
    {
        ChangePogoHead(pogoHead);
    }
    
    // Update is called once per frame
    private void Update()
    {
        ChangePogoHead(pogoHead);
    }
}
