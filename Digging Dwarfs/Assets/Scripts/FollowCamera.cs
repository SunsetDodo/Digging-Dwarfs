using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float maxHeight;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.position.x, Mathf.Min(camera.position.y, maxHeight), transform.position.z);
    }
}
