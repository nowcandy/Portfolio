using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Material mat;

    [SerializeField] Renderer ren;

    [HideInInspector] public bool isOpen;

    [HideInInspector] public Animator animator;

    bool isDoorPower;

    void Start()
    {
        mat = ren.GetComponent<Renderer>().material;
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        DoorEnergy();
        DoorLight();
    }

    void DoorLight()
    {
        if (isOpen == false)
            mat.SetColor("_EmissionColor", Color.red);
        else
            mat.SetColor("_EmissionColor", Color.green);
    }

    void DoorEnergy()
    {
        if(GameMng.Instance.energy > 0)
        {
            if (isOpen == false)
            {
                if (isDoorPower == false)
                {
                    isDoorPower = true;
                    GameMng.Instance.usePower++;
                    GameMng.Instance.AddPower();
                }
                else
                    return;
            }
            else
            {
                if (isDoorPower == true)
                {
                    isDoorPower = false;
                    GameMng.Instance.usePower--;
                    GameMng.Instance.AddPower();
                }
                else
                    return;
            }
        }
        else
        {
            isDoorPower = false;
            isOpen = true;
            AudioMng.Instance.BgmOn("Door");
            animator.SetTrigger("Open");
            GameMng.Instance.usePower--;
            GameMng.Instance.AddPower();
        }
    }
}
