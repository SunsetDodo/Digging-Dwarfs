using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPogoHead : MonoBehaviour
{
    
    [SerializeField] private float velocityMagnitudeThreshold = 0.1f;
    
    private Rigidbody2D rigidbody;
    private Collider2D collider;

    private float velocityCache;
    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        Debug.Log("Started");
    }


    void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude > velocityMagnitudeThreshold)
            velocityCache = rigidbody.velocity.magnitude;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(velocityCache);
    }
}
