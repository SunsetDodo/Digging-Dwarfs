using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float upGravity = 10f;
    [SerializeField] private float downGravity = 20f;

    [SerializeField] private float springCompression = 0;
    [SerializeField] private float compressionSpeed = 1;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            springCompression = Mathf.Clamp(springCompression + compressionSpeed * Time.deltaTime, 0, 1);
        }
        else
        {
            springCompression = 0;
        }
    }
}
