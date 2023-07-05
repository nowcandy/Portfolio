using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CameraCtrl : MonoBehaviour
{
    Camera mainCam;
    Player player;

    public Camera[] cam;

    [HideInInspector] public bool isOpen;
    [HideInInspector] public bool openCCTV;

    bool isSee;

    public TextMeshProUGUI nameText;
    public GameObject staminaUI;
    public GameObject CCTVUI;

    void Start()
    {
        AudioMng.Instance.BgmOn("BackGround");
        mainCam = Camera.main;
        player = GameMng.Instance.player;
    }

    void Update()
    {
        GameOverCheck();
    }

    void Stamina()
    {
        if(mainCam.enabled == true)
        {
            if (player.stamina >= 3)
                staminaUI.SetActive(false);
            else
                staminaUI.SetActive(true);
        }
        else
            staminaUI.SetActive(false);
    }

    void GameOverCheck()
    {
        if (player.isDead == false)
        {
            CCTVCheck();
            MainCamCtrl();
            Stamina();
        }
        else
        {
            if (CCTVUI.activeSelf == true)
                CCTVUI.SetActive(false);

            for (int i = 0; i < cam.Length; i++)
            {
                cam[i].enabled = false;
            }
            mainCam.enabled = true;
        }
    }

    public void CCTVCtrl(int index)
    {
        CamCtrl(index);
    }

    public void CCTVName(string name)
    {
        nameText.text = name;
    }

    void CCTVCheck()
    {
        if(isOpen == true)
        {
            if (openCCTV == false)
            {
                AudioMng.Instance.BgmOff("BackGround");
                AudioMng.Instance.BgmOn("CamBackGround");
                CamCtrl(0);
                nameText.text = "RightCorrider";
                CCTVUI.SetActive(true);
                openCCTV = true;
            }
        }
    }

    public void CamCtrl(int num)
    {
        AudioMng.Instance.BgmOn("CamChange");
        for (int i = 0; i < cam.Length; i++)
        {
            cam[i].enabled = false;
        }

        if(isSee == false)
        {
            isSee = true;
            GameMng.Instance.usePower++;
            GameMng.Instance.AddPower();
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.isStop = true;
        mainCam.enabled = false;
        cam[num].enabled = true;
        GameMng.Instance.aspect.SetActive(true);
        GameMng.Instance.aspect.transform.position = cam[num].transform.position;
    }

    public void MainCamCtrl()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameMng.Instance.aspect.SetActive(false);
            if (mainCam.enabled == false)
            {
                for (int i = 0; i < cam.Length; i++)
                {
                    cam[i].enabled = false;
                }
                AudioMng.Instance.BgmOff("CamBackGround");
                AudioMng.Instance.BgmOn("BackGround");
                CCTVUI.SetActive(false);
                player.isStop = false;
                mainCam.enabled = true;
                isOpen = false;
                openCCTV = false;
                if (isSee == true)
                {
                    isSee = false;
                    GameMng.Instance.usePower--;
                    GameMng.Instance.AddPower();
                }
            }
            else
                return;
        }
    }
}
