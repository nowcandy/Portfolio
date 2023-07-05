using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVLight : MonoBehaviour
{
    Hacking hac;

    public Light CCTV;
    public Light FlashLight;

    [HideInInspector] public Camera cam;
    [HideInInspector] public Color original;

    void Start()
    {
        hac = GetComponentInChildren<Hacking>();
        cam = GetComponent<Camera>();
        original = CCTV.color;
    }

    void Update()
    {
        Flash();
    }

    IEnumerator Shutter()
    {
        isShutter = true;
        FlashLight.enabled = true;
        hac.isLightOn = true;
        GameMng.Instance.energy -= 0.5f;
        AudioMng.Instance.BgmOn("FlashLight");
        yield return new WaitForSeconds(0.15f);
        hac.isLightOn = false;
        FlashLight.enabled = false;
        yield return new WaitForSeconds(10f);
        isShutter = false;
    }
    bool isShutter;
    void Flash()
    {
        if(hac.isBlack == false)
        {
            if (cam.enabled == true)
            {
                if (Input.GetKeyDown(KeyCode.Space) && isShutter == false)
                {
                    StartCoroutine(Shutter());
                }
            }
            else
            {
                FlashLight.enabled = false;
                hac.isLightOn = false;
            }
        }
    }
}
