using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float downGravity = 4;
    [SerializeField] private float upGravity = 3;
    
    [SerializeField] private float springCompression;
    private float maxCompression;
    [SerializeField] private float compressionSpeed = 1;
    [SerializeField] private float decompressionSpeed = 20;
    [SerializeField] private float springForce = 10;
    [SerializeField] private float springThreshold = 0.1f;
    
    [SerializeField] private float defaultSpringLength = 0.16f;
    [SerializeField] private float minSpringLength = 0.02f;
    [SerializeField] public float dwarfOffset = 0;

    [SerializeField] private GameObject pogoBase;
    [SerializeField] private GameObject dwarfBase;
    [SerializeField] private Collider2D pogoTrigger;
    [SerializeField] private Rigidbody2D dwarfRigidbody;
    [SerializeField] private SpriteRenderer dwarfSprite; 
    
    [SerializeField] private float maxRotationAngle = 20;
    [SerializeField] private float rotationSpeed = 60;
    [SerializeField] private float rotationFlipAxisThreshold = 1f;

    [SerializeField] public bool isPogoEnabled = true;
    [SerializeField] private float disabledPogoJumpForce = 80;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile respawnTile;
    [SerializeField] private Tile deathTile;
    [SerializeField] private Tile superJumpTile;
    [SerializeField] private Tile finishTile1;
    [SerializeField] private Tile finishTile2;
    [SerializeField] private Tile finishTile3;

    [SerializeField] private bool ignoreRespawnPoint;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI gameOverTimer;

    private float _timer;
    
    private Collider2D dwarfCollider;
    private AudioSource[] audioSources;
    private Random random = new Random();

    private bool superJumpCharged;
    private bool jumped;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        dwarfCollider = GetComponent<Collider2D>();
        _timer = 0;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            transform.position = respawnPoint.transform.position;
        
        if (!isPogoEnabled)
        {
            springCompression = 1;
        }
        else
        {
            springCompression = Input.GetKey(KeyCode.Mouse0)
                ? Mathf.Clamp(springCompression + compressionSpeed * Time.deltaTime, 0, 1)
                : Mathf.Clamp(springCompression - decompressionSpeed * Time.deltaTime, 0, 1);
            if (springCompression > maxCompression)
            {
                maxCompression = springCompression;
            }
        }
        
        var targetRotation = -Input.GetAxis("Horizontal") * maxRotationAngle * springCompression;
        pogoBase.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(pogoBase.transform.rotation.eulerAngles.z, targetRotation, Time.deltaTime * rotationSpeed));
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            jumped = true;
        }
    }

    private void MoveDwarfBase()
    {
        dwarfBase.transform.localPosition = new Vector3(0, Mathf.Lerp(defaultSpringLength, minSpringLength, springCompression) + dwarfOffset, 0);
        // dwarfCollider.offset = new Vector2(0,
        //     Mathf.Lerp(defaultColliderOffset, defaultColliderOffset - (defaultSpringLength - minSpringLength) / 2, springCompression));
        // dwarfCollider.size = new Vector2(0.25f,
        //     Mathf.Lerp(defaultColliderHeight, defaultColliderHeight - (defaultSpringLength - minSpringLength), springCompression));
    }


    private bool CanJump()
    {
        // Debug.Log("Can Jump: " + pogoTrigger.IsTouchingLayers(LayerMask.GetMask("Physics")) + " Spring Compression: " + springCompression);
        return pogoTrigger.IsTouchingLayers(LayerMask.GetMask("Physics"))
               && springCompression > springThreshold;
    }
    
    private void HandlePhysics()
    {
        dwarfRigidbody.gravityScale = dwarfRigidbody.velocity.y < 0 ? downGravity : upGravity;
        if (!jumped) return;
        
        jumped = false;
        var compression = maxCompression;
        maxCompression = 0;
        if (!CanJump()) return;
        
        var forceMagnitude = isPogoEnabled ? springForce * compression * (superJumpCharged ? 2 : 1) : disabledPogoJumpForce;
        Vector2 forceDirection = (dwarfBase.transform.position - pogoBase.transform.position).normalized; 
        superJumpCharged = false;
        dwarfRigidbody.AddForce(forceDirection * forceMagnitude);
        
        var index = random.Next(0, audioSources.Length);
        audioSources[index].Play();
    }

    private void HandleSprite()
    {
        var angle = -Input.GetAxis("Horizontal");
        if (0 < angle && angle < rotationFlipAxisThreshold)
            dwarfSprite.flipX = true;
        if (-rotationFlipAxisThreshold < angle && angle < 0)
            dwarfSprite.flipX = false;
    }
    
    public static string FormatTime(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);
        int milliseconds = (int)((timeInSeconds - Mathf.Floor(timeInSeconds)) * 100);

        return string.Format("{0:00}:{1:00}:{2:00}.{3:00}", hours, minutes, seconds, milliseconds);
    }
    
    
    private void Update()
    {
        HandleInput();
        MoveDwarfBase();
        
        _timer += Time.deltaTime;
        timer.text = FormatTime(_timer);
    }

    private void FixedUpdate()
    {
        HandlePhysics();
        HandleSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        maxCompression = 0;
        
        foreach (var contact in collision.contacts)
        {
            var touchingTile = tilemap.GetTile(tilemap.WorldToCell(contact.point));
            if (touchingTile == null) return;

            if (touchingTile == respawnTile)
            {
                respawnPoint.transform.position = contact.point + 2 * Vector2.up;
            }

            if (touchingTile == deathTile)
            {
                transform.position = respawnPoint.transform.position;
            }

            if (touchingTile == superJumpTile)
            {
                superJumpCharged = true;
            }

            if (touchingTile == finishTile1 || touchingTile == finishTile2 || touchingTile == finishTile3)
            {
                gameOverCanvas.SetActive(true);
                gameOverTimer.text = "And it only took: " + FormatTime(_timer);
                timer.gameObject.SetActive(false);
                enabled = false;
            }
        }
    }
}
