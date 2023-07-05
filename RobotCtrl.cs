using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotCtrl : MonoBehaviour
{
    Player player;
    Animator animator;
    NavMeshAgent nav;
    public enum Type { Honey, Cloudy, Kitty, RedBelly, Froggy };
    public Type type;

    public Vector3[] movePosition;
    [SerializeField] float moveTime;
    [SerializeField] CCTVLight camOn;
    [SerializeField] AudioSource walk;
    [SerializeField] GameObject jumpScare;
    [SerializeField] GameObject jack;

    public bool isCrazy;

    public int temp = 0;

    float tempTime;

    bool isSuffer;
    bool isDoor;
    bool isReset;
    bool isMove;
    bool isAttack;
    bool isDefense;
    bool isTemp;
    bool isWalk;
    bool isPlay;

    void Start()
    {
        moveTime -= GameMng.Instance.stage;
        player = GameMng.Instance.player;
        switch (type)
        {
            case Type.Froggy:
                nav = GetComponent<NavMeshAgent>();
                break;
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameEndCheck();
    }

    void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1);
        if(GameMng.Instance.aspect.activeSelf == true)
            animator.SetLookAtPosition(GameMng.Instance.aspect.transform.position);
    }

    void GameEndCheck()
    {
        if(player.isDead == false)
        {
            if(GameMng.Instance.isClearStage == false)
            {
                switch (type)
                {
                    case Type.Froggy:
                        if (GameMng.Instance.robot[3].isCrazy == true)
                            Adam();
                        else
                            transform.position = movePosition[0];
                        break;
                    case Type.Honey:
                        Catherine();
                        Algorithm();
                        break;
                    case Type.Kitty:
                        Algorithm();
                        break;
                    case Type.RedBelly:
                        Algorithm();
                        break;
                    case Type.Cloudy:
                        Robert();
                        break;
                }
            }
        }
    }

    void Adam()
    {
        if (isWalk == true && nav.enabled == true)
        {
            isWalk = false;
            walk.Play();
        }

        if (isReset == true)
        {
            isReset = false;
        }
        else if(isCrazy == true)
        {
            tempTime = 0;
            if (isTemp == false)
                StartCoroutine(Crazy());
        }
        else
        {
            tempTime += Time.deltaTime;
            if(tempTime >= moveTime)
            {
                tempTime = 0;
                int temp = Random.Range(1, movePosition.Length);
                transform.position = movePosition[temp];
            }    
        }
    }

    void Catherine()
    {
        if (isTemp == false && type == Type.Cloudy)
            StartCoroutine(Shame());
    }

    void Robert()
    {
        if (temp < 3)
        {
            if(camOn.cam.enabled == true)
            {
                tempTime = 0;
            }
            else
            {
                if (tempTime >= moveTime)
                {
                    tempTime = 0;
                    temp++;
                    transform.position = movePosition[temp];
                }
                else
                    tempTime += Time.deltaTime;
            }
        }
        else if(temp == 3)
        {
            int random = Random.Range(5, 11);
            jack.SetActive(true);
            transform.position = movePosition[random];
            temp = 4;
        }
        else
        {
            if(isCrazy == false)
            {
                GameMng.Instance.isPowerOff = true;
                isCrazy = true;
                AudioMng.Instance.BgmOn("PowerOff");
                for (int num = 0; num < GameMng.Instance.allLight.Length; num++)
                {
                    GameMng.Instance.allLight[num].GetComponent<Light>().enabled = false;
                    if (GameMng.Instance.allLight[num].GetComponent<Animator>() != null)
                        GameMng.Instance.allLight[num].GetComponent<Animator>().enabled = false;
                }
            }
            tempTime += Time.deltaTime;
            if(tempTime >= moveTime)
            {
                tempTime = 0;
                int random = Random.Range(5, 11);
                transform.position = movePosition[random];
            }
        }
    }

    void Algorithm()
    {
        if (isAttack == true && isDefense == false)
        {
            StartCoroutine(DoorStep());
        }

        if (isMove == false && isAttack == false)
            StartCoroutine(MoveAlgorithm());
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "DoorArea")
        {
            Button button = other.GetComponentInParent<Button>();
            isDoor = button.isOpen;
            switch (type)
            {
                case Type.Froggy:
                    if (isDoor == true)
                    {
                        jumpScare.SetActive(true);
                        if(player.isDead == false)
                            AudioMng.Instance.BgmOn("Cloudy");
                        walk.Stop();
                        nav.enabled = false;
                        transform.position = movePosition[0];
                        player.isDead = true;
                    }
                    else
                    {
                        AudioMng.Instance.BgmOn("Knocking");
                        walk.Stop();
                        GameMng.Instance.energy -= 5;
                        nav.enabled = false;
                        if (isReset == false)
                        {
                            transform.position = movePosition[0];
                            StopCoroutine(Crazy());
                            animator.SetBool("isRun", false);
                            isReset = true;
                            isCrazy = false;
                            isTemp = false;
                        }
                    }
                    break;
                case Type.Honey:
                    break;
                case Type.Kitty:
                    break;
                case Type.RedBelly:
                    break;
                case Type.Cloudy:
                    break;
            }
        }
        else if(other.tag == "CCTVArea")
        {
            CCTVLight cctv = other.GetComponentInParent<CCTVLight>();

            switch (type)
            {
                case Type.Honey:
                    if (cctv.cam.enabled == true)
                    {
                        if (isCrazy == true)
                            cctv.CCTV.color = Color.red;
                    }
                    else
                        cctv.CCTV.color = new Color(0.4f,0.75f,0.4f);
                    break;
                case Type.Cloudy:
                    if(cctv.cam.enabled == true)
                    {
                        if(isPlay == false)
                        {
                            isPlay = true;
                            AudioMng.Instance.BgmOn("Drop");
                        }
                    }
                    else
                    {
                        isPlay = false;
                        AudioMng.Instance.BgmOff("Drop");
                    }

                    if(cctv.FlashLight.enabled == true)
                    {
                        if(isSuffer == false && isCrazy == true)
                        {
                            AudioMng.Instance.BgmOff("Drop");
                            StartCoroutine(Suffer());
                            jack.SetActive(false);
                        }
                    }
                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "CCTVArea" && type == Type.Honey)
        {
            CCTVLight cctv = other.GetComponentInParent<CCTVLight>();
            cctv.CCTV.color = cctv.original;
        }
    }
    IEnumerator Suffer()
    {
        temp = 0;
        tempTime = 0;
        isSuffer = true;
        animator.SetBool("isScream", true);
        AudioMng.Instance.BgmOn("Joke");
        yield return new WaitForSeconds(3f);
        isSuffer = false;
        isCrazy = false;
        GameMng.Instance.isPowerOff = false;
        for (int num = 0; num < GameMng.Instance.allLight.Length; num++)
        {
            GameMng.Instance.allLight[num].GetComponent<Light>().enabled = true;
        }
        animator.SetBool("isScream", false);
        AudioMng.Instance.BgmOn("PowerOff");
        transform.position = movePosition[temp];
    }

    IEnumerator Crazy()
    {
        isTemp = true;
        nav.enabled = true;
        animator.SetTrigger("Screem");
        AudioMng.Instance.BgmOn("Crazy");
        yield return new WaitForSeconds(2.2f);
        isWalk = true;
        animator.SetBool("isRun", true);
        nav.SetDestination(player.transform.position);
    }

    IEnumerator Shame()
    {
        isTemp = true;
        int ranTime = Random.Range(5, 31);
        yield return new WaitForSeconds(ranTime * 2);
        isCrazy = true;
        yield return new WaitForSeconds(6);
        isCrazy = false;
        isTemp = false;
    }

    IEnumerator MoveAlgorithm()
    {
        int randomInt;
        isMove = true;
        yield return new WaitForSeconds(moveTime);
        if (GameMng.Instance.isClearStage == false)
        {
            randomInt = Random.Range(-1, 4);
            if (randomInt > 0)
                randomInt = 1;
        }
        else
            randomInt = 0;

        if (temp + randomInt >= 0 && temp + randomInt < movePosition.Length - 1)
        {
            temp += randomInt;
            if (temp <= 0)
                transform.position = movePosition[0];
            else
                transform.position = movePosition[temp];
            isMove = false;
        }
        else
        {
            if (temp + randomInt < 0)
            {
                temp = 0;
                transform.position = movePosition[temp];
                isMove = false;
            }
            else if(temp + randomInt >= movePosition.Length - 1)
            {
                temp = movePosition.Length - 1;
                transform.position = movePosition[temp];
                isAttack = true;
            }
        }
    }
    int count = 8;
    IEnumerator DoorStep()
    {
        isDefense = true;
        yield return new WaitForSeconds(count);
        int randomAttack = Random.Range(0, 2);
        if (randomAttack == 0)
        {
            isDefense = false;
            Debug.Log("기다립니다");
            count--;
        }
        else
        {
            if (isDoor == true)
            {
                switch (type)
                {
                    case Type.Honey:
                        AudioMng.Instance.BgmOn("Honey");
                        break;
                    case Type.Kitty:
                        AudioMng.Instance.BgmOn("Kitty");
                        break;
                    case Type.RedBelly:
                        AudioMng.Instance.BgmOn("RedBelly");
                        break;
                }
                jumpScare.SetActive(true);
                temp = 0;
                transform.position = movePosition[temp];
                Debug.Log("공격받았습니다");
                player.isDead = true;
            }
            else
            {
                count = 8;
                temp = 0;
                Debug.Log("돌아갔습니다");
                transform.position = movePosition[movePosition.Length - Random.Range(4, movePosition.Length - 2)];
                isAttack = false;
                isMove = false;
                isDefense = false;
            }
        }
    }
}
