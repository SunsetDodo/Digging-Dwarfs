using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform camera;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.position.x, transform.position.y, transform.position.z);
    }
}
