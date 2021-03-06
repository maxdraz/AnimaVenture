﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAnimation : MonoBehaviour
{

    public Animator animator;
    [SerializeField] bool tappedOnce;
    [SerializeField] bool tappedTwice;
    [SerializeField] float tapCooldown = 2f;
    float originalCooldown;
    public GameObject skipText;
    



    // Use this for initialization
    void Start()
    {
        if (animator == null && gameObject.GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
        originalCooldown = tapCooldown;


    }

    private void Update()
    {
       

        if (Input.GetMouseButtonDown(0) && tappedOnce == false)
        {
            tappedOnce = true;
            if (skipText != null)
            {
                skipText.SetActive(true);
            }
            StartCoroutine(StartCooldown());
        }



        if (tappedOnce && tappedTwice)
        {
            Skip();
        }
    }

    void Skip()
    {
        if (skipText != null)
        {
            skipText.SetActive(false);
        }
        if (animator != null)
        {
            animator.CrossFade("End State", 0f);
        }
    }

    IEnumerator StartCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (Input.GetMouseButtonDown(0))
            {
                tappedTwice = true;
            }
            tapCooldown -= Time.deltaTime;
            if (tapCooldown <= 0)
            {
                tappedOnce = false;
                tappedTwice = false;
                tapCooldown = originalCooldown;
                if (skipText != null)
                {
                    skipText.SetActive(false);
                }
                yield break;

            }
            else if (tappedOnce && tappedTwice)
            {
                yield return null;
                tapCooldown = originalCooldown;
                tappedOnce = false;
                tappedTwice = false;
                skipText.SetActive(false);
                DisableSkipping(); 
                yield break;
            }
            yield return null;
        }
    }

    public void DisableSkipping()
    {
        SkipAnimation[] allScripts = FindObjectsOfType<SkipAnimation>();

        for(int i = 0; i < allScripts.Length; i++)
        {
            allScripts[i].enabled = false;
        }
    }
}