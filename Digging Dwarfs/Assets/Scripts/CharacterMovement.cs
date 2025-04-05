using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float upGravity = 10f;
    [SerializeField] private float downGravity = 20f;

    [SerializeField] private float springCompression;
    [SerializeField] private float compressionSpeed = 1;
    [SerializeField] private float decompressionSpeed = 20;
    [SerializeField] private float springForce = 10;
    [SerializeField] private float springThreshold = 0.1f;
    [SerializeField] private float forceTimeThreshold = 0.1f;
    
    [SerializeField] private float defaultSpringLength = 0.16f;
    [SerializeField] private float minSpringLength = 0.02f;


    [SerializeField] private GameObject pogoBase;
    [SerializeField] private GameObject dwarfBase;
    [SerializeField] private CapsuleCollider2D dwarfCollider;
    [SerializeField] private Collider2D pogoTrigger;
    [SerializeField] private Rigidbody2D dwarfRigidbody;
        
    [SerializeField] private float defaultColliderOffset = 0.25f;
    [SerializeField] private float defaultColliderHeight = 0.5f;
    
    
    [SerializeField] private float maxRotationAngle = 30;
    [SerializeField] private float rotationSpeed = 60;

    private float currentForceTimeout = 0;
    
    void Start()
    {
        
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            springCompression = Mathf.Clamp(springCompression + compressionSpeed * Time.deltaTime, 0, 1);
        }
        else
        {
            springCompression = Mathf.Clamp(springCompression - decompressionSpeed * Time.deltaTime, 0, 1);
            
        }
    }

    private void MoveDwarfBase()
    {
        dwarfBase.transform.localPosition = new Vector3(0, Mathf.Lerp(defaultSpringLength, minSpringLength, springCompression), 0);
        // dwarfCollider.offset = new Vector2(0,
        //     Mathf.Lerp(defaultColliderOffset, defaultColliderOffset - (defaultSpringLength - minSpringLength) / 2, springCompression));
        // dwarfCollider.size = new Vector2(0.25f,
        //     Mathf.Lerp(defaultColliderHeight, defaultColliderHeight - (defaultSpringLength - minSpringLength), springCompression));
    }

    private void HandlePhysics()
    {
        if (true 
            && pogoTrigger.IsTouchingLayers(LayerMask.GetMask("Physics")) 
            && !Input.GetKey(KeyCode.Space) 
            && springCompression > springThreshold
            && currentForceTimeout <= 0
            )
        {
            currentForceTimeout = forceTimeThreshold;
            float forceMagnitude = springForce * springCompression;
            Vector2 forceDirection = dwarfBase.transform.position - pogoBase.transform.position; 
            
            Debug.Log("Force Direction: " + forceMagnitude * forceDirection);
            dwarfRigidbody.AddForce(forceDirection * forceMagnitude);
        }
        
        if (currentForceTimeout > 0)
        {
            currentForceTimeout -= Time.deltaTime;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        HandleInput();
        MoveDwarfBase();
        HandlePhysics();
    }
}
