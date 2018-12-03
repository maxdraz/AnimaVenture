﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomSelectAnimationHandler : MonoBehaviour {
	//kingdom assets
	public ParticleSystem kSelectStarsParticle;
	public GameObject centralNode;
	public GameObject kingdomMenu;
	public GameObject kingdomLines;
    public GameObject menuReturn;
    public GameObject kSettings;

    //journey assets
    public GameObject backButton;
    public GameObject jSettings;
	public GameObject journeyLines;
	public Animation journeyFadeAnim;

	public Animation KingdomSelectScaleAnim;

	public CameraFOVLerpRework cameraScript;
	public AudioManager AM;

    public GameObject bigTrail;
    public GameObject smallTrail;


    public void ToggleCameraLerp() {
	
		cameraScript.ToggleLerpBool ();
	
	}
		

	public void LeaveKingdom () {
		Debug.Log ("Leave kingdom");
		kingdomMenu.SetActive (false);
        menuReturn.SetActive(false);
        kSettings.SetActive(false);
		kingdomLines.SetActive (false);
		journeyFadeAnim.clip = journeyFadeAnim.GetClip ("JourneyFadeIn");	
		journeyFadeAnim.Play ();
	}


	public void ArriveAtKingdom () {
		Debug.Log ("arrive at kingdom");
        menuReturn.SetActive(true);
        kSettings.SetActive(true);
        kingdomLines.SetActive (true);
        centralNode.GetComponent<CircleCollider2D> ().enabled = true;
		AM.PlayClip(1, 0.1f, false);
        bigTrail.SetActive(true);
        smallTrail.SetActive(false);
		kSelectStarsParticle.Play ();

    }

    public void LeaveJourney () {
		Debug.Log ("Leave journey");
		backButton.SetActive (false);
		journeyLines.SetActive (false);
        jSettings.SetActive(false);
		journeyFadeAnim.clip = journeyFadeAnim.GetClip ("JourneyFadeOut");
		journeyFadeAnim.Play ();
	}

	public void ArriveAtJourney () {
		Debug.Log ("arrive at journey");
		backButton.SetActive (true);
		journeyLines.SetActive (true);
        jSettings.SetActive(true);
        AM.PlayClip(1, 0.1f, false);
        bigTrail.SetActive(false);
        smallTrail.SetActive(true);
    }

	public void ScaleUpKingdomSelect () {
		centralNode.GetComponent<Animation> ().clip = centralNode.GetComponent<Animation> ().GetClip ("CentralNodeScaleUp");
		centralNode.GetComponent<Animation> ().Play ();
		KingdomSelectScaleAnim.clip = KingdomSelectScaleAnim.GetClip ("ScaleUp");	
		KingdomSelectScaleAnim.Play ();
		kSelectStarsParticle.Stop ();


	}

	public void ScaleDownKingdomSelect () {
		kingdomMenu.SetActive (true);
		centralNode.GetComponent<Animation> ().clip = centralNode.GetComponent<Animation> ().GetClip ("CentralNodeScaleDown");
		centralNode.GetComponent<Animation> ().Play ();
		centralNode.GetComponent<centralNode> ().ReadyForInput = true;
		KingdomSelectScaleAnim.clip = KingdomSelectScaleAnim.GetClip ("ScaleDown");	
		KingdomSelectScaleAnim.Play ();

	}
}
