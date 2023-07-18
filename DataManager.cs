using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    // ---싱글톤으로 선언--- //
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }
    [SerializeField] TextMeshProUGUI continueText;
    [SerializeField] GameObject tutorialUI;
    [SerializeField] GameObject storyUI;

    [SerializeField] TextMeshProUGUI story;
    [SerializeField] TextMeshProUGUI manual;

    [SerializeField] int temp;

    bool isNewGame;
    public bool clearCheck;
    void Start()
    {
        LoadGameData();
    }

    void Update()
    {
        if(GameMng.Instance != null)
        {
            if (GameMng.Instance.player.isDead == true)
                clearCheck = false;
            else
                clearCheck = true;
        }
        ContinueCheck();
        Tutorial();
    }

    void Tutorial()
    {
        if (isNewGame == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))

            {
                if (temp == 0)
                {
                    storyUI.SetActive(true);
                    tutorialUI.SetActive(false);
                    story.enabled = true;
                    temp = 1;
                }
                else if (temp == 1)
                {
                    story.enabled = false;
                    manual.enabled = true;
                    temp = 2;
                }
                else
                    SceneManager.LoadScene("Game");
            }
        }
    }

    // --- 게임 데이터 파일이름 설정 ("원하는 이름(영문).json") --- //
    string GameDataFileName = "GameData.json";

    // --- 저장용 클래스 변수 --- //
    public Data data = new Data();

    void ContinueCheck()
    {
        if(continueText != null)
        {
            string filePath = Application.persistentDataPath + "/" + GameDataFileName;
            if (!File.Exists(filePath))
                continueText.color = new Color(1, 1, 1, 0.2f);
            else
                continueText.color = new Color(1, 1, 1, 1);
        }
    }

    public void Continue()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if (File.Exists(filePath))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void NewGame()
    {
        data.stage = 1;
        data.value = 0.5f;
        data.volume = 0;
        data.mouseSensetive = 250;
        data.mouseSensetiveValue = 0.5f;
        SaveGameData();
        tutorialUI.SetActive(true);
        isNewGame = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            // 저장된 파일 읽어오고 Json을 클래스 형식으로 전환해서 할당
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            print("불러오기 완료" + filePath);
        }
    }


    // 저장하기
    public void SaveGameData()
    {
        // 클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(filePath, ToJsonData);

        // 올바르게 저장됐는지 확인 (자유롭게 변형)
        print("저장 완료");
    }
}