using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Option : MonoBehaviour
{
    [SerializeField] GameObject option;
    [SerializeField] GameObject cctv;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider slider;

    public bool isActive;

    void Update()
    {
        Stop();
    }

    public void Volume()
    {
        if (slider.value == 0)
            audioMixer.SetFloat("Option", -80);
        else
            audioMixer.SetFloat("Option", -20 + slider.value * 40);
    }

    public void Stop()
    {
        if(cctv.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            audioMixer.SetFloat("Option", -80);
            option.SetActive(true);
            Time.timeScale = 0;
            isActive = true;
        }
    }

    public void Resume()
    {
        Volume();
        option.SetActive(false);
        isActive = false;
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
