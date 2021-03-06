﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {
	
	float startYPos;
	public AudioManager audioManager;
	AudioSource audio;
	public AudioClip[] clips;
	public int position;
	public GameObject firstTransform;
	public GameObject secondTransform;
	public GameObject thirdTransform;
	public float lerpSpeed;
	public int toneToPlay;
	public int first;
	public int second;
	public int third;
	public string mixer;


	LineRenderer renderer;

	// Use this for initialization
	void Awake () {
		float startYPos = transform.position.y;
		renderer = GetComponent<LineRenderer> ();
		renderer.endColor = Color.gray;
		audio = GetComponent<AudioSource> ();

		
	}
	
	// Update is called once per frame
	void Update () {
		renderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, transform.position.z));

		if (position == 0) {

			transform.position = Vector3.Lerp (transform.position, firstTransform.transform.position, lerpSpeed * Time.deltaTime);
		}

		if (position == 1) {

			transform.position = Vector3.Lerp (transform.position, secondTransform.transform.position, lerpSpeed * Time.deltaTime);
		}

		if (position == 2) {

			transform.position = Vector3.Lerp (transform.position, thirdTransform.transform.position, lerpSpeed * Time.deltaTime);
		}
    }

	void OnMouseDown () {
		Debug.Log ("HIT");
		PlayAudioAndChangePosition ();
	}


	void PlayAudioAndChangePosition() {
	

		if (position == 0){

			//audio.clip = clips[0];
			//audio.Play ();
			toneToPlay = first;
			audioManager.PlayClip(toneToPlay, mixer);
		} 
			
			if (position == 1) {
				
				//audio.clip = clips[1];
				//audio.Play ();
			    toneToPlay = second;
			    audioManager.PlayClip(toneToPlay, mixer);

			}

				if (position == 2){

					//audio.clip = clips[2];
					//audio.Play ();
			        toneToPlay = third;
			        audioManager.PlayClip(toneToPlay, mixer);
			}

		position++;
		if (position == 3) {

			position = 0;
		}
	}
}
