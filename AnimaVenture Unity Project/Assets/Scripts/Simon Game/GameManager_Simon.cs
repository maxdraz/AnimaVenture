﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class GameManager_Simon : MonoBehaviour {

    public static GameManager_Simon SharedInstance;

    [Header("Tweakable Variables")]
    
    [SerializeField] float gameStartDelay = 2f;
    [SerializeField] float delayBetweenTelegraphs=1.5f;
    [SerializeField] float lastTelegraphDelay = 1f;
    [SerializeField] float replayTelegraphDelay = 1f;
    [SerializeField] float enableButtonsDelay = 2f;
    [SerializeField] float telegraphLightUpTime = 1.5f;
    [SerializeField] float shootingStarSpeed = 2f;
    [SerializeField] float speedBoostMultiplier;
    [SerializeField] float speedBoostTime;
    float timerTime;
    float timeToSubtract;
    

    [Space(15)]
    [Header("Sequence, Telegraphs, Buttons")]
    //create a list to store colour sequence
    [SerializeField]
    List<int> colourSequence;
    int positionInSequence= 0;
    //An array to store telegraphs
    [SerializeField]
    Telegraph_Simon[] telegraphs;
    [SerializeField]
    Button_Simon[] buttons;
    [SerializeField]
    List<Transform> telegraphSpawnpoints;
    [SerializeField]
    List<Transform> telegraphEndTransforms;

    GameObject startButton;
    GameObject restartButton;

    [Space(15)]
    [Header("Score, Timer, Dolmen")]
    [SerializeField] Score_Simon score;
    [SerializeField] Timer_Simon timer;
    [SerializeField] GameObject endGameMenu;
    [SerializeField] Dolmen_Simon dolmen;
    public bool dolmenFullyRisenBool;
    public bool dolmenAlmostRisenBool;
    public ContinueSimon cs;

    [Space(15)]

    [Header("ParticleSystems")]

    GameObject[] particleSystems;
    [SerializeField]
    List<IntensifyParticleSystem> particleSystemsToIntensify;

    [SerializeField]IntensifyParticleSystem dolmenPS1;
    [SerializeField]IntensifyParticleSystem dolmenPS2;

    [Space(15)]
    [Header("Animations")]
    //Animations
    [SerializeField]
    List<Animator> animators;
    Animator hexAnim;
    [SerializeField] Animator animator;
    public bool continueBool;
    AudioManager AM;
    [Header("Camera Shake Variables")]
    [Space(15)]
    public float pressedMagnitude;
    public float pressedRoughness;
    public float pressedFadeInTime;
    public float pressedFadeOutTime;
    [Space(15)]
    public float sequenceCorrectMagnitude;
    public float sequenceCorrectRoughness;
    public float sequenceCorrectFadeInTime;
    public float sequenceCorrectFadeOutTime;

    public bool sequenceInProgress;

    public Gradient partyGradient;

    private void Awake()
    {
        SharedInstance = this;
        //initialize all references
        startButton = GameObject.Find("StartButton");
        restartButton = GameObject.Find("RestartButton");
        AM = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

        particleSystems = GameObject.FindGameObjectsWithTag("ParticleSystem");

        for(int i = 0; i < particleSystems.Length; i++)
        {
            if(particleSystems[i].GetComponent<IntensifyParticleSystem>() != null)
            {
               particleSystemsToIntensify.Add(particleSystems[i].GetComponent<IntensifyParticleSystem>());
                
            }

            particleSystems[i].SetActive(false);
        }
        
        //score = GameObject.Find("Score").GetComponent<Score_Simon>();

        //colourSequence.Count = colourSequenceLength;
    }

    void Start () {
        
        //Disable end game menu object
        endGameMenu.SetActive(false);
        //Set the timer 
        timer.time = timerTime;
                
        //Assign each button its index in the array
        SetButtonIndex();
    }
	
	// Update is called once per frame
	void Update () {

        if (timer.time <= 0)
        {
           // GameEnd();
        }
	}

    public void StartGame()
    {        
        //make start button disappear       
        //startButton.SetActive(false);
        //call start timer function
        timer.startTimerBool = true;
        //start moving dolmen
        //dolmen.moveDolmenBool = true;
        //make restart button appear        
       // restartButton.SetActive(true);
        // invoke pick random colour method
        Invoke("PickRandomColour", gameStartDelay);
        

    }

     public IEnumerator PlayGame()
    {

        yield return null;
        //if dolmen is finished moving
        if (cs.dolmenCompleteBool)
        {
            positionInSequence = 0;
            DisableButtons();

            yield break;
        }
        //make start button disappear       
        //startButton.SetActive(false);
        //make restart button appear        
        //restartButton.SetActive(true);
        //reset the current input position in sequence
        if (colourSequence.Count < 2)
        {
            yield return new WaitForSeconds(2f);
        }
        positionInSequence = 0;
        yield return new WaitForSeconds(delayBetweenTelegraphs);
        //Play back sequence
        foreach (int colourIndex in colourSequence)
        {
            sequenceInProgress = true;  

            //if dolmen is finished moving
            if (cs.dolmenCompleteBool)
            {
                positionInSequence = 0;
                DisableButtons();               
                
                yield break;
            }
            //whatever colour is stored in the list, display the corresponding telegraph
            //StartCoroutine(telegraphs[colourIndex].DisplayTelegraph(telegraphLightUpTime));

            //get shooting star from pool and store in a reference
            GameObject shootingStar = ObjectPooler.SharedInstance.GetPooledObject("ShootingStar");
            shootingStar.GetComponent<ShootingStar>().endTrans = telegraphEndTransforms[0];
            shootingStar.GetComponent<ShootingStar>().speed = shootingStarSpeed;
            //set start trans to a random spawn point
            shootingStar.GetComponent<ShootingStar>().setStartPos(telegraphSpawnpoints[Random.Range(0, telegraphSpawnpoints.Count)]);
            //set trail colour
            shootingStar.GetComponent<ShootingStar>().colourIndex = colourIndex;
            //set object active
            shootingStar.SetActive(true);
            
            //set telegraph colour index
            shootingStar.GetComponent<ShootingStar>().telegraphIndex = colourIndex;
            // make the star move
            StartCoroutine(shootingStar.GetComponent<ShootingStar>().MoveObject(shootingStarSpeed));
            

            yield return new WaitForSeconds(delayBetweenTelegraphs);
        }
        //pick a new random colour and add it to the list
        // wait for seconds here to make new telegraph stand out more
        yield return new WaitForSeconds(lastTelegraphDelay);
        PickRandomColour();

    }

    IEnumerator PlayBackSequence()
    {

       
        yield return new WaitForSeconds(delayBetweenTelegraphs);

        //if dolmen is finished moving
        if (cs.dolmenCompleteBool)
        {
            positionInSequence = 0;
            DisableButtons();

            yield break;
        }

        positionInSequence = 0;
        //for every number stored in the list
        foreach(int colourIndex in colourSequence)
        {
            sequenceInProgress = true;
            if (cs.dolmenCompleteBool)
            {
                positionInSequence = 0;
                DisableButtons();

                yield break;
            }
            //get shooting star from pool and store in a reference
            GameObject shootingStar = ObjectPooler.SharedInstance.GetPooledObject("ShootingStar");
            shootingStar.GetComponent<ShootingStar>().endTrans = telegraphEndTransforms[0];
            shootingStar.GetComponent<ShootingStar>().speed = shootingStarSpeed;
            //set start trans to a random spawn point
            shootingStar.GetComponent<ShootingStar>().setStartPos(telegraphSpawnpoints[Random.Range(0, telegraphSpawnpoints.Count)]);
            //set trail colour
            shootingStar.GetComponent<ShootingStar>().colourIndex = colourIndex;
            //set object active
            shootingStar.SetActive(true);
            
            //set telegraph colour index
            shootingStar.GetComponent<ShootingStar>().telegraphIndex = colourIndex;
            // make the star move
            StartCoroutine(shootingStar.GetComponent<ShootingStar>().MoveObject(shootingStarSpeed));

            yield return new WaitForSeconds(replayTelegraphDelay);

        }

        //enable buttons again
        EnableButtons();
    }

    void DisableButtons()
    {
        //animation
       // animator.SetBool("incorrectBool", false);
        animator.SetBool("enabledBool", false);
        
        //disable colliders on buttons to prevent player input
        for (int cnt = 0; cnt < buttons.Length; cnt++)
        {
            buttons[cnt].GetComponent<CircleCollider2D>().enabled = false;
           

        }

        
    }

    void EnableButtons()
    {
        sequenceInProgress = false;
        animator.SetBool("incorrectBool", false);
        if(colourSequence.Count < 2)
        {
            if (!EventHandler.SharedInstance.testingMode)
            EventHandler.SharedInstance.tip2Trigger.Invoke();
            //StartCoroutine(GamePauser.sharedInstance.PauseForTime(EventHandler.SharedInstance.tip1Anim.length));
        }
        // play animation        
        animator.SetBool("enabledBool", true);
      
        for (int cnt = 0; cnt < buttons.Length; cnt++)
        {
            buttons[cnt].GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void SetButtonIndex()
    {
        for(int cnt = 0; cnt < buttons.Length; cnt++)
        {
            buttons[cnt].ButtonIndex = cnt;
        }
    }

    void PickRandomColour()
    {
        sequenceInProgress = true;
        
        int randomColourIndex = Random.Range(0, telegraphs.Length);
        //display telegraph
        //get shooting star from pool and store in a reference
        GameObject shootingStar = ObjectPooler.SharedInstance.GetPooledObject("ShootingStar");
        shootingStar.GetComponent<ShootingStar>().endTrans = telegraphEndTransforms[0];
        shootingStar.GetComponent<ShootingStar>().speed = shootingStarSpeed;
        //set start trans to a random spawn point
        shootingStar.GetComponent<ShootingStar>().setStartPos(telegraphSpawnpoints[Random.Range(0, telegraphSpawnpoints.Count)]);
        //set trail color
        shootingStar.GetComponent<ShootingStar>().colourIndex = randomColourIndex;
        //set object active
        shootingStar.SetActive(true);
        

        //set telegraph colour index
        shootingStar.GetComponent<ShootingStar>().telegraphIndex = randomColourIndex;
        // make the star move
        StartCoroutine(shootingStar.GetComponent<ShootingStar>().MoveObject(shootingStarSpeed));

        colourSequence.Add(randomColourIndex);

        //Enable Buttons here
        //I envoke for visual purposes only, so that the collider enables only after telegraph animation is finished
        Invoke("EnableButtons", enableButtonsDelay);
        
    }

    public void RestartGame()
    {
        //delete previous colour sequence
        colourSequence.Clear();
        //stop timer 
        timer.startTimerBool = false;
        //call start game function
        //restartButton.SetActive(false);
        startButton.SetActive(true);
        //disable end game menu 
        endGameMenu.SetActive(false);
        //Reset timer
        timer.time = timerTime;
    }

    public void ButtonPressed(int buttonIndex)
    {
       

        if (colourSequence[positionInSequence] == buttonIndex)
        {
           
            //add 1 to the current position in the sequence            
            positionInSequence++;
            //play sound
            AM.PlayClip(buttonIndex, "Simon Sfx");
            //slight camera shake
           // CameraShaker.Instance.ShakeOnce(pressedMagnitude, pressedRoughness, pressedFadeInTime, pressedFadeOutTime);

            //if the current position has reached the end of the recorded sequence
            if (positionInSequence == colourSequence.Count)
            {
                if(colourSequence.Count == 1)
                {
                    //play screen shake
                    CameraShaker.Instance.ShakeOnce(sequenceCorrectMagnitude, sequenceCorrectRoughness, sequenceCorrectFadeInTime, sequenceCorrectFadeOutTime);
                    //start dolmen
                    StartCoroutine(EventHandler.SharedInstance.StartDolmen());
                 
                } else if (colourSequence.Count == 3)
                {
                    EventHandler.SharedInstance.startRainPS.Invoke();
                    AudioManager.SharedInstance.PlayClip(6, "Simon Sfx", true);
                }
                else if (colourSequence.Count == 4)
                {
                    EventHandler.SharedInstance.startBottomPS.Invoke();
                }
              
                // Sequence is correct!

                //Shake the camera
                // CameraShaker.Instance.ShakeOnce(sequenceCorrectMagnitude, sequenceCorrectRoughness, sequenceCorrectFadeInTime, sequenceCorrectFadeOutTime);
                //Add one to score
                StartCoroutine(score.Add(1));
                //Subtract time from timer
                // timer.SubtractTime(timeToSubtract);

                //Intensify Particle systems
                for(int i = 0; i < particleSystemsToIntensify.Count; i ++)
                {
                    
                    particleSystemsToIntensify[0].gameObject.SetActive(true);
                    

                    StartCoroutine(particleSystemsToIntensify[i].Intensify());
                }

                //play sound
                AM.PlayClip(4, "Simon Sfx");

                //play All Correct Animations
                foreach(Animator anim in animators){
                    anim.SetBool("correctBool", true);
                }

               // hexAnim.SetBool("correctBool", true);

                //add speed boost
                StartCoroutine(dolmen.SpeedUpDolmen(speedBoostMultiplier, speedBoostTime));
                //replay sequence and add a new colour to the end
                DisableButtons();
                StartCoroutine(PlayGame());
            }
        }
        else if(positionInSequence != colourSequence.Count)
        {
            //INCORRECT INPUT HAS BEEN MADE

            //PLAY BUTTON INCORRECT ANIMS
            animator.SetBool("incorrectBool", true);
            animator.SetBool("enabledBool", false);

            foreach(Animator anim in animators)
            {
                anim.SetBool("incorrectBool", true);
            }

          //  hexAnim.SetBool("incorrectBool", true);
            
            

            
            //restart game
            //RestartGame();
            //set score to be current score
            score.Set(score.currentScore);
            //Play back sequence
            StartCoroutine(PlayBackSequence());
            DisableButtons();
        }       
        
    }

    void GameEnd()
    {
        // if timer reaches 0:
        // disable buttons
        DisableButtons();

        //disable telegraphs
        DisableTelegraphs();
        // - enable menu object
        endGameMenu.SetActive(true);
        
    }

    void DisableTelegraphs()
    {
        for(int cnt = 0; cnt < telegraphs.Length; cnt ++)
        {
            telegraphs[cnt].ResetTelegraph();
        }
    }

    public void LoadNewScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

        IEnumerator PartyMode()
    {
        while (true)
        {
            for (int i = 0; i < particleSystemsToIntensify.Count; i++)
            {

                var main = particleSystemsToIntensify[i].GetComponent<ParticleSystem>().main;
                var startColour = main.startColor;
                startColour.gradient = partyGradient;

                if (i == particleSystemsToIntensify.Count - 1)
                {
                    foreach (Animator anim in animators)
                    {
                        if (anim.gameObject.tag == "Background")
                        {
                            anim.SetBool("partyModeBool", true);
                            yield break;
                        }
                    }
                }
                yield return null;
            }
        }
    }

   // public void ContinueGame()
    //{
      //  endGameMenu.SetActive(false);
        //timer.time = timerTime;
        //timer.gameObject.SetActive(false);
        //StartCoroutine(PlayBackSequence());
    //}
}
