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
    [SerializeField] private float rotationCompressionThreshold = 0.5f;
    [SerializeField] private float rotationFlipAxisThreshold = 1f;

    [SerializeField] public bool isPogoEnabled = true;
    [SerializeField] public float disabledPogoJumpForce = 80;
    

    private void HandleInput()
    {
        if (!CanJump() && Input.GetKeyUp(KeyCode.Mouse1))
            isPogoEnabled = !isPogoEnabled;
        
        if (!isPogoEnabled)
        {
            springCompression = 1;
        }
        else
        {
            springCompression = Input.GetKey(KeyCode.Mouse0)
                ? Mathf.Clamp(springCompression + compressionSpeed * Time.deltaTime, 0, 1)
                : Mathf.Clamp(springCompression - decompressionSpeed * Time.deltaTime, 0, 1);
        }
        
        var targetRotation = -Input.GetAxis("Horizontal") * maxRotationAngle * springCompression;
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
        if (!(CanJump() && Input.GetKeyUp(KeyCode.Mouse0)))
            return;

        var forceMagnitude = isPogoEnabled ? springForce * springCompression : disabledPogoJumpForce;
        Vector2 forceDirection = (dwarfBase.transform.position - pogoBase.transform.position).normalized; 
        
        // Debug.Log("Force Direction: " + forceMagnitude * forceDirection);
        dwarfRigidbody.AddForce(forceDirection * forceMagnitude);
    }

    private void HandleSprite()
    {
        var angle = -Input.GetAxis("Horizontal");
        if (0 < angle && angle < rotationFlipAxisThreshold)
            dwarfSprite.flipX = true;
        if (-rotationFlipAxisThreshold < angle && angle < 0)
            dwarfSprite.flipX = false;
    }
    
    private void Update()
    {
        HandleInput();
        MoveDwarfBase();
        HandlePhysics();
        HandleSprite();
    }
}
