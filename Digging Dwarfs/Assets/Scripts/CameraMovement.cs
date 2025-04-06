using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;   
    [SerializeField] private Rigidbody2D playerRB;

    [SerializeField] private float zoomSmoothness;
    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;

    [SerializeField] private float movementSmoothness;
    
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var targetFOV = Mathf.Lerp(minFOV, maxFOV, playerRB.velocity.magnitude *  playerRB.velocity.magnitude);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetFOV, zoomSmoothness * Time.deltaTime);
        
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.position.x, player.position.y, -10), movementSmoothness * Time.deltaTime);
    }
}
