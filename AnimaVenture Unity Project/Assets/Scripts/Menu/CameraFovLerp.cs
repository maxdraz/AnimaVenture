﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFovLerp : MonoBehaviour {

	public Animator FadeAnimation;
    AudioManager AM;

	public Camera camera;
	public float cameraSize;
	public float targetSize;

	public float kingdomSelectSize;
	public float journeySelectSize;

	public bool isLerping;
	bool isReadyForInput;
	public GameObject centralNodeGlow;

	// Use this for initialization

	void Start () {

		isLerping = false;
		isReadyForInput = true;
		cameraSize = camera.orthographicSize;

	}

    private void Awake()
    {
        AM = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void FixedUpdate () {
		
		if (isLerping == true) {
            
                cameraSize = Mathf.Lerp (cameraSize, targetSize, .02f);
				camera.orthographicSize = cameraSize;
				isReadyForInput = false;
			//Check for arrival
			if (camera.orthographicSize < journeySelectSize + .01f || camera.orthographicSize > kingdomSelectSize - .05f) {
                AM.PlayClip(1, 0.1f, false);
                ReadyForPlayerInput ();
					ToggleTargetSize ();
					ToggleLerpBool ();
					
			}
		}
	}



	void ToggleTargetSize () {
		if (targetSize == kingdomSelectSize) {
			targetSize = journeySelectSize;

		} else {
		targetSize = kingdomSelectSize;
		}
	}

	void ReadyForPlayerInput () {

		isReadyForInput = true;
	}

	public void ToggleLerpBool () {
		if (isReadyForInput) {
		if (isLerping == false) {
			
			isLerping = true;
				//Check to animate from journey to kingdom select
				if (camera.orthographicSize < journeySelectSize + .01f) {

					FadeAnimation.SetBool ("Wait" ,false);
					FadeAnimation.SetBool ("FadeToKingdom" ,true);
				}
				//Check to animate from kingdom to journey select
				if (camera.orthographicSize > kingdomSelectSize - .05f) {

					FadeAnimation.SetBool ("Wait" ,false);
					FadeAnimation.SetBool ("FadeToKingdom" ,false);
				}
				centralNodeGlow.SetActive (false);

		} else {
		
			isLerping = false;
			}
		}
	}
}


