using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhiteBoard : MonoBehaviour
{
    [SerializeField] string[] tutorial;
    [SerializeField] TextMeshPro board;

    void Update()
    {
        Tutorial();
    }

    void Tutorial()
    {
        int count = GameMng.Instance.stage;
        board.text = tutorial[count];
    }
}
