using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMng : MonoBehaviour
{
    public static GameMng Instance;
    public Player player;

    [HideInInspector] public bool isEnd;
    [HideInInspector] public bool isClearStage;

    public float energy;
    public float stagePower;
    public float time = 0;

    public int usePower;
    public int stage;

    public Image[] usePowerUI;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI mText;

    public GameObject clearImage;

    public int count = 1;
    float endTime = 0.5f;

    public bool isPowerOff;

    bool isPowerDown;
    bool isBgm;

    public GameObject[] allLight;
    public RobotCtrl[] robot;
    public Animal[] animal;

    [SerializeField] Light lantern;
    [SerializeField] Light jumpLight;
    [SerializeField] Animator jumpAnimator;

    public GameObject aspect;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        AddPower();
        animal[0].RandomMove();
        animal[1].RandomMove();
        allLight = GameObject.FindGameObjectsWithTag("Light");
        for(int num = 0; num < robot.Length; num++)
        {
            robot[num].enabled = false;
        }
    }
    bool isPlay;
    void Update()
    {
        stage = DataManager.Instance.data.stage;
        if (stage == 1 && isPlay == false)
        {
            isPlay = true;
            AudioMng.Instance.BgmOn("Tutorial");
        }
        stagePower = 0.15f + 0.025f * stage;
        dayText.text = stage + "";
        GameEndCheck();
    }
    void GameEndCheck()
    {
        if (player.isDead == false)
        {
            Difficulty();
            Energy();
            StageMng();
        }
        else
        {
            StartCoroutine(Dead());
        }
    }

    void Difficulty()
    {
        int count;
        if (stage <= 4)
        {
            count = stage;
        }
        else
            count = 4;

        for (int i = 0; i < count; i++)
        {
            robot[i].enabled = true;
        }
    }

    public void AddPower()
    {
        for (int i = 0; i < usePowerUI.Length; i++)
        {
            usePowerUI[i].enabled = false;
        }

        for (int i = 0; i < usePower; i++)
        {
            usePowerUI[i].enabled = true;
        }
    }

    void StageMng()
    {
        if(isClearStage == false)
        {
            if (stage < 7)
            {
                if (count == 1)
                    mText.text = "AM";
                else
                    mText.text = "PM";

                time += Time.deltaTime;
                if (time / 40 >= count)
                {
                    count++;
                    timeText.text = count - 1 + "";
                }

                if (count == 7)
                {
                    StartCoroutine(StageClear());
                }
            }
            else
            {
                GameClear();
            }
        }
    }

    void GameClear()
    {
        if(isEnd == false)
        {
            DataManager.Instance.data.Stage(6);
            DataManager.Instance.SaveGameData();
            isEnd = true;
            SceneManager.LoadScene("Dead");
        }
    }

    void Energy()
    {
        if(isClearStage == false)
        {
            if (energy <= 0f)
            {
                for (int num = 0; num < robot.Length; num++)
                {
                    robot[num].transform.position = robot[num].movePosition[0];
                    robot[num].enabled = false;
                }

                if (allLight[0].activeSelf == true)
                {
                    for (int num = 0; num < allLight.Length; num++)
                    {
                        allLight[num].SetActive(false);
                    }
                    AudioMng.Instance.BgmOn("PowerOff");
                    lantern.enabled = false;
                }

                energy = 0;
                if(isPowerDown == false)
                    StartCoroutine(PowerDown());
            }
            else
            {
                int power;
                if (usePower == 1)
                    power = 2;
                else
                    power = usePower;
                energy -= 1 * (power * 0.5f) * Time.deltaTime * stagePower;
                powerText.text = (int)energy + "%";
            }
        }
    }

    IEnumerator Dead()
    {
        jumpAnimator.enabled = true;
        jumpAnimator.SetBool("isJumpScare", true);
        jumpLight.enabled = true;
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Dead");
    }

    IEnumerator PowerDown()
    {
        if (isBgm == false)
            AudioMng.Instance.BgmOn("EnergyDown");
        isBgm = true;
        isPowerDown = true;
        yield return new WaitForSeconds(5.0f - endTime);
        endTime += endTime;
        int temp = Random.Range(0, 2);
        if (temp == 0)
            isPowerDown = false;
        else
        {
            AudioMng.Instance.BgmOff("EnergyDown");
            player.isDead = true;
        }
    }

    IEnumerator StageClear()
    {
        count = 1;
        for (int num = 0; num < robot.Length; num++)
        {
            robot[num].temp = 0;
            robot[num].GetComponent<Animator>().SetTrigger("Standing");
            robot[num].transform.position = robot[num].movePosition[0];
        }
        if(stage != 6)
            animal[stage].RandomMove();
        player.transform.position = new Vector3(0, 1, -5);
        timeText.text = 12 + "";
        isClearStage = true;
        stage++;
        energy = 100f;
        time = 0f;
        clearImage.SetActive(true);
        DataManager.Instance.data.Stage(stage);
        DataManager.Instance.SaveGameData();
        AudioMng.Instance.BgmOn("ClearClock");
        yield return new WaitForSeconds(4f);
        AudioMng.Instance.BgmOn("Clear");
        yield return new WaitForSeconds(4f);
        clearImage.SetActive(false);
        isClearStage = false;
    }
}
