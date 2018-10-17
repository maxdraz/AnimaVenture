﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dolmen_Simon : MonoBehaviour {

    public float time;
    [SerializeField] float distance;
    [SerializeField] float distanceToTarget;
    [SerializeField] float speed;    
    
    public bool moveDolmenBool;
    [SerializeField] GameObject meditationButton;
    

    [SerializeField] Transform targetTransform;

    private void Start()
    {
        
        
        distance = CalculateDistance(targetTransform.position.y, transform.position.y);
        speed = distance / time;
    }

    private void FixedUpdate()
    {
        distanceToTarget = CalculateDistance(targetTransform.position.y, transform.position.y);
        
        if ( distanceToTarget >= 0 && moveDolmenBool)
        { 
            MoveDolmen();
        }
        
        if(distanceToTarget <= 0)
        {
            meditationButton.SetActive(true);
        }
       
        
    }

    //calculates the distance between two scalars
    float CalculateDistance(float targetY, float myY)
    {
        float d;
        d = targetY - myY;

        return d;
    }

    public void MoveDolmen()
    {
        // add speed to y value of the transform
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    public IEnumerator SpeedUpDolmen(float spdMultiplier, float t)
    {
        float oldSpeed = speed;
        speed *= spdMultiplier;
        yield return new WaitForSeconds(t);
        speed = oldSpeed;
    }
}