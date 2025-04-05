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
    
    [SerializeField] private float defaultSpringLength = 0.16f;
    [SerializeField] private float minSpringLength = 0.02f;


    [SerializeField] private GameObject pogoBase;
    [SerializeField] private GameObject dwarfBase;
    [SerializeField] private Collider2D pogoTrigger;
    [SerializeField] private Rigidbody2D dwarfRigidbody;
    [SerializeField] private SpriteRenderer dwarfSprite; 
        
    [SerializeField] private float defaultColliderOffset = 0.25f;
    [SerializeField] private float defaultColliderHeight = 0.5f;
    
    
    [SerializeField] private float maxRotationAngle = 20;
    [SerializeField] private float rotationSpeed = 60;
    

    private void HandleInput()
    {
        springCompression = Input.GetKey(KeyCode.Space) ? Mathf.Clamp(springCompression + compressionSpeed * Time.deltaTime, 0, 1) : Mathf.Clamp(springCompression - decompressionSpeed * Time.deltaTime, 0, 1);
        var targetRotation = -Input.GetAxis("Horizontal") * maxRotationAngle;
        pogoBase.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(pogoBase.transform.rotation.eulerAngles.z, targetRotation, Time.deltaTime * rotationSpeed));
    }

    private void MoveDwarfBase()
    {
        dwarfBase.transform.localPosition = new Vector3(0, Mathf.Lerp(defaultSpringLength, minSpringLength, springCompression), 0);
        // dwarfCollider.offset = new Vector2(0,
        //     Mathf.Lerp(defaultColliderOffset, defaultColliderOffset - (defaultSpringLength - minSpringLength) / 2, springCompression));
        // dwarfCollider.size = new Vector2(0.25f,
        //     Mathf.Lerp(defaultColliderHeight, defaultColliderHeight - (defaultSpringLength - minSpringLength), springCompression));
    }


    private bool CanJump()
    {
        return pogoTrigger.IsTouchingLayers(LayerMask.GetMask("Physics"))
               && springCompression > springThreshold;
    }
    
    private void HandlePhysics()
    {
        if (!(CanJump() && Input.GetKeyUp(KeyCode.Space)))
            return;

        var forceMagnitude = springForce * springCompression;
        Vector2 forceDirection = (dwarfBase.transform.position - pogoBase.transform.position).normalized; 
        
        // Debug.Log("Force Direction: " + forceMagnitude * forceDirection);
        dwarfRigidbody.AddForce(forceDirection * forceMagnitude);
    }

    private void HandleSprite()
    {
        dwarfSprite.flipX = pogoBase.transform.rotation.eulerAngles.z - 180 < 0;
    }
    
    private void Update()
    {
        HandleInput();
        MoveDwarfBase();
        HandlePhysics();
        HandleSprite();
    }
}
