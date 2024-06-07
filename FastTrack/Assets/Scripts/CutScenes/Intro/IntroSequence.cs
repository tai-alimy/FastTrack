
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] Image introImage = null;
    [SerializeField] List<Camera> regularRaceCamera = null;
    [SerializeField] Animator pulseAnimator = null;
    [SerializeField] AudioSource introMusic = null;
    [SerializeField] Camera introCamera = null;
    [SerializeField] GameObject rotationObject = null;
    [SerializeField] float introCameraMoveSpeed;
    [SerializeField] AudioSource engineRevSound = null;
    [SerializeField] GameObject regRacingHUD = null;
    [SerializeField] GameObject countDownObj = null;
    [SerializeField] GameObject startRaceObj = null;
    [SerializeField] AudioSource startAudio = null;

    private void Start()
    {
        Time.timeScale = 1;
        foreach (Camera rc in regularRaceCamera) 
        {
            rc.gameObject.SetActive(false);
        }

        InvokeRepeating("ChangeIntroImageToRandomColor", 0f, 0.2f);
        Invoke("HideIntroImage", 1.5f);
        InvokeRepeating("MoveIntroCameraForward", 1.5f, 0.01f);
        Invoke("SwitchToRaceCamera", 7.0f);
        Invoke("StartRace", 12.1f);
        Invoke("HideStartMessage", 14.5f);
    }


    void ChangeIntroImageToRandomColor()
    {
        Color randColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        introImage.color = randColor;
    }

    void HideIntroImage()
    {
        introImage.gameObject.SetActive(false);
    }

    void MoveIntroCameraForward()
    {
        introCamera.gameObject.transform.position -= rotationObject.transform.forward * Time.deltaTime * introCameraMoveSpeed;
    }
    void SwitchToRaceCamera() 
    {
        regularRaceCamera[GameManager.GetCurrentActiveCarIndex()].gameObject.SetActive(true);
        introCamera.gameObject.SetActive(false);
        introMusic.Stop();
        regRacingHUD.SetActive(true); 
        engineRevSound.Play();
        pulseAnimator.enabled = false;
        countDownObj.SetActive(true);
    }

    void StartRace()
    {
        countDownObj.SetActive(false);
        startRaceObj.SetActive(true);
        startAudio.Play();
    }

    void HideStartMessage()
    {
        startRaceObj.SetActive(false);
        CarFactory.EnableAICars();
        Events.RaceStarted?.Invoke();
    }
}
