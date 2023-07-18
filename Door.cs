using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] GameObject jumpScare;
    [SerializeField] GameObject bunny;

    [SerializeField] BoxCollider door;
    [SerializeField] BoxCollider barricade;

    public float chargeTime;
    public float maxChargeTime;

    public bool isCharge;

    void Update()
    {
        Charge();
    }

    void Charge()
    {
        if(GameMng.Instance.stage >= 5 && GameMng.Instance.isClearStage == false)
        {
            door.enabled = true;
            barricade.enabled = false;
            if (isCharge == false)
            {
                if (chargeTime < maxChargeTime)
                    chargeTime += Time.deltaTime;
                else
                {
                    jumpScare.SetActive(true);
                    if(GameMng.Instance.player.isDead == false)
                    {
                        AudioMng.Instance.BgmOn("Bunny");
                        GameMng.Instance.player.isDead = true;
                    }
                }
            }
            else
            {
                if (chargeTime > 0)
                    chargeTime -= Time.deltaTime * 3;
                else
                {
                    chargeTime = 0;
                }
            }

            bunny.transform.position = new Vector3(0, 0, -12 + chargeTime / 40);
            leftDoor.transform.position = new Vector3(0.35f + chargeTime / 85, leftDoor.transform.position.y, leftDoor.transform.position.z);
            rightDoor.transform.position = new Vector3(0f - chargeTime / 85, rightDoor.transform.position.y, rightDoor.transform.position.z);
        }
        else
        {
            barricade.enabled = true;
            door.enabled = false;
        }
    }
}
