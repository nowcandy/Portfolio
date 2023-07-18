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
    [SerializeField] Slider volume;
    [SerializeField] Slider mouseSensetive;

    [SerializeField] GameObject sensetive;
    [SerializeField] GameObject menu;

    float value;
    float mouseValue;

    bool isStop;
    public bool isActive;

    void Update()
    {
        Stop();
        if(isStop == true)
            audioMixer.SetFloat("Option", -80);
        else
        {
            volume.value = DataManager.Instance.data.value;
            mouseSensetive.value = DataManager.Instance.data.mouseSensetiveValue;
            audioMixer.SetFloat("Option", DataManager.Instance.data.volume);
            GameMng.Instance.player.mouseSensitive = mouseSensetive.value * 500;
        }
    }

    public void Volume()
    {
        if (volume.value == 0)
        {
            DataManager.Instance.data.volume = -80;
            value = 0;
        }
        else
        {
            DataManager.Instance.data.volume = -20 + volume.value * 40;
            value = volume.value;
        }
        DataManager.Instance.data.value = value;
        DataManager.Instance.SaveGameData();
    }

    public void MouseSensetive()
    {
        mouseValue = mouseSensetive.value;
        DataManager.Instance.data.mouseSensetiveValue = mouseValue;
        DataManager.Instance.data.mouseSensetive = mouseSensetive.value * 500;
        DataManager.Instance.SaveGameData();
    }

    public void Stop()
    {
        if(cctv.activeSelf == false && Input.GetKeyDown(KeyCode.Escape) && GameMng.Instance.isClearStage == false)
        {
            isStop = true;
            audioMixer.SetFloat("Option", -80);
            AudioMng.Instance.BgmPause("BackGround");
            if(GameMng.Instance.stage == 1)
                AudioMng.Instance.BgmPause("Tutorial");
            option.SetActive(true);
            menu.SetActive(true);
            Time.timeScale = 0;
            isActive = true;
        }
    }

    public void Sensetive()
    {
        menu.SetActive(false);
        sensetive.SetActive(true);
    }

    public void Resume()
    {
        if(sensetive.activeSelf == true)
            sensetive.SetActive(false);
        Volume();
        MouseSensetive();
        isStop = false;
        AudioMng.Instance.BgmOn("BackGround");
        if (GameMng.Instance.stage == 1 && GameMng.Instance.energy > 92.6f)
            AudioMng.Instance.BgmOn("Tutorial");
        option.SetActive(false);
        isActive = false;
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
