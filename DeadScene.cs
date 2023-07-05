using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScene : MonoBehaviour
{
    [SerializeField] GameObject gameClear;
    [SerializeField] GameObject dead;

    void Start()
    {
        if (DataManager.Instance.clearCheck == true)
            gameClear.SetActive(true);
        else
            dead.SetActive(true);

        StartCoroutine(ReStart());
    }

    IEnumerator ReStart()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Menu");
    }
}
