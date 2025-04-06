using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LightSettings
{
    public float minLevel;
    public float maxLevel;
    public float intensity;
    public float color; 
}



public class LightController : MonoBehaviour
{
    [SerializeField] private Light globalLight;
    [SerializeField] private Transform player;
    
    [SerializeField] private LightSettings[] lightSettings;

    private LightSettings GetCurrentLightSettings()
    {
        
        return lightSettings[0];
    }
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
