﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Simon : MonoBehaviour {

    [SerializeField] float lightUpTime;

    GameManager_Simon gm;
    Animator anim; 
    SpriteRenderer sRenderer;

    //assign the same index as in the array on gm
    public int ButtonIndex { get; set; }

    private void Awake()
    {
        //References to objects / components
        anim = transform.root.GetComponent<Animator>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager_Simon>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        //animate

        
        //display button colour
        DisplayButton();

        //tell the GM which button was pressed
        gm.ButtonPressed(ButtonIndex);
    }

    private void OnMouseUp()
    {
        //animate
        
        //reset button colour
        StartCoroutine(ResetButton(lightUpTime));
    }

    void DisplayButton()
    {
        sRenderer.color = new Color(
            sRenderer.color.r,
            sRenderer.color.g,
            sRenderer.color.b,
            1.0f);
    }

    IEnumerator ResetButton(float t)
    {
        yield return new WaitForSeconds(t);
        sRenderer.color = new Color(
            sRenderer.color.r,
            sRenderer.color.g,
            sRenderer.color.b,
            0.5f);
    }
}
