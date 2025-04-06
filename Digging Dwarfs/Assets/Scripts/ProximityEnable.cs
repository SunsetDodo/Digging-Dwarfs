using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityEnable : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float proximity;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < proximity)
            for(int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
        else
            for(int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
    }
}
