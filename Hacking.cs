using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Hacking : MonoBehaviour
{
    [SerializeField] GameObject black;
    [SerializeField] RawImage video;
    [SerializeField] VideoPlayer player;
    [SerializeField] Light CCTV;
    [SerializeField] Image ghost;

    [HideInInspector] public bool isHacking;
    [HideInInspector] public bool isActive;
    [HideInInspector] public bool isLightOn;
    [HideInInspector] public bool isBlack;

    [HideInInspector] public float hackingTime;

    void Update()
    {
        Emp();
        Active();
    }
    bool isBgm;
    void Active()
    {
        if (GetComponentInParent<Camera>().enabled == true)
        {
            isActive = true;
            if (isBlack == false)
            {
                if(GameMng.Instance.isPowerOff == false)
                    CCTV.enabled = true;
            }
            else if (isBgm == true)
            {
                AudioMng.Instance.BgmOn("HackingCam");
                video.enabled = true;
                player.Play();
                isBgm = false;
            }
        }
        else
        {
            if(isBgm == false)
            {
                AudioMng.Instance.BgmOff("HackingCam");
                player.Stop();
                video.enabled = false;
                isBgm = true;
            }
            if (CCTV.enabled == true)
                CCTV.enabled = false;
            isActive = false;
        }
    }

    void Emp()
    {
        if (isHacking == true)
        {
            StartCoroutine(EMP());
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Robot" && isBlack == false)
        {
            RobotCtrl robot = other.gameObject.GetComponent<RobotCtrl>();
            if(robot.isCrazy == true)
            {
                if (isActive == true)
                {
                    if (robot.type == RobotCtrl.Type.Honey)
                    {
                        hackingTime += Time.deltaTime;
                        if (hackingTime >= 3f)
                        {
                            Debug.Log("해킹당했습니다");
                            hackingTime = 0;
                            isHacking = true;
                            isBlack = true;
                        }
                    }
                }
                else
                {
                    hackingTime = 0;
                }
            }
        }

        if(other.tag == "Robot" && isLightOn == true && GameMng.Instance.robot[3].isCrazy == true)
        {
            RobotCtrl robot = other.gameObject.GetComponent<RobotCtrl>();
            if (robot.type == RobotCtrl.Type.Froggy)
                robot.isCrazy = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Robot")
        {
            RobotCtrl robot = other.gameObject.GetComponent<RobotCtrl>();
            if (robot.type == RobotCtrl.Type.Froggy && robot.isCrazy == true)
            {
                robot.isCrazy = false;
            }
        }
    }

    IEnumerator EMP()
    {
        ghost.enabled = true;
        isHacking = false;
        yield return new WaitForSeconds(1f);
        CCTV.enabled = false;
        black.SetActive(true);
        ghost.enabled = false;
        yield return new WaitForSeconds(20.0f);
        CCTV.enabled = true;
        isBlack = false;
        black.SetActive(false);
    }
}
