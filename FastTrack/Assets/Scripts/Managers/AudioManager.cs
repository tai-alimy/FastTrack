using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource saveSlotSound = null;
    [SerializeField] AudioSource backButtonSound = null;
    [SerializeField] AudioSource okButtonSound = null;
    [SerializeField] AudioSource moneyButtonSound = null;
    [SerializeField] AudioSource rightLeftButtonSound = null;
    [SerializeField] AudioSource cancelButtonSound = null;



    public void PlaySaveSlotSound()
    {
        saveSlotSound.Play();
    }

    public void PlayBackButtonSound()
    {
        backButtonSound.Play();
    }

    public void PlayOkButtonSound()
    {
        okButtonSound.Play();
    }

    public void PlayMoneyButtonSound()
    {
        moneyButtonSound.Play();
    }

    public void PlayRightLeftButtonSound()
    {
        rightLeftButtonSound.Play();
    }

    public void PlayCancelButtonSound()
    {
        cancelButtonSound.Play();
    }




}
