using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Light[] DoorLight;
    [SerializeField] GameObject[] robots;

    bool isFlicker;
    bool isFlicker2;

    void Update()
    {
        if (isFlicker == false)
            StartCoroutine(Flicker());

        if (isFlicker2 == false)
            StartCoroutine(Flicker2());

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator Flicker()
    {
        isFlicker = true;
        float ran = Random.Range(0, 3f);
        int temp = Random.Range(2, 5);
        yield return new WaitForSeconds(ran);
        DoorLight[0].enabled = true;
        robots[temp].SetActive(true);
        yield return new WaitForSeconds(ran * 2);
        DoorLight[0].enabled = false;
        robots[temp].SetActive(false);
        isFlicker = false;
    }
    IEnumerator Flicker2()
    {
        isFlicker2 = true;
        float ran = Random.Range(0, 3f);
        int temp = Random.Range(0, 2);
        yield return new WaitForSeconds(ran);
        DoorLight[1].enabled = true;
        robots[temp].SetActive(true);
        yield return new WaitForSeconds(ran * 2);
        DoorLight[1].enabled = false;
        robots[temp].SetActive(false);
        isFlicker2 = false;
    }
}
