using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject parentObject;

    public Image building;

    [Range(1,100)]
    public int gridX, gridY;

    [SerializeField]
    private float buildingCurrentSizeX, buildingCurrentSizeY;

    [SerializeField]
    private float defaultPositionX, defaultPositionY;

    private void Start()
    {
        buildingCurrentSizeX = building.GetComponent<RectTransform>().sizeDelta.x;
        buildingCurrentSizeY = building.GetComponent<RectTransform>().sizeDelta.y;

        defaultPositionX = building.GetComponent<RectTransform>().anchoredPosition.x;
        defaultPositionY = building.GetComponent<RectTransform>().anchoredPosition.y;

        GridSystem();
    }

    void GridSystem()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = (gridY - 1); j >= 0; j--)
            {
                Image image = Instantiate(building, parentObject.transform);
                image.GetComponent<RectTransform>().anchoredPosition = new Vector3(defaultPositionX + (buildingCurrentSizeX * i), defaultPositionY - (buildingCurrentSizeY * j), 0);

                var buildingName = new StringBuilder();
                buildingName.Append("Building");
                buildingName.Append(i);
                buildingName.Append("_");
                buildingName.Append(j);

                image.name = buildingName.ToString();
            }
        }   
    }
}


/*public static GameManager instance;
public static bool isGameStarted = false;
public static bool isGameEnded = false;
public static bool isGameAlreadyOpen = false;
public GameObject StartScreen, FinishScreen;
public GameObject StartPanel;

//SCORE
[SerializeField] TextMeshProUGUI scoreText;

//LEVEL
public static int levelCount;
public List<GameObject> Levels = new List<GameObject>();
public GameObject levelText;

void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
}

void Start()
{
    isGameStarted = false;
    isGameEnded = false;
    if (isGameAlreadyOpen == true)
    {
        StartPanel.SetActive(false);
        OnLevelStarted();
    }
    LoadLevel();
}
void Update()
{
    levelText.GetComponent<TextMeshProUGUI>().SetText("Level: " + (levelCount + 1).ToString());
}
public void NextLevel()
{
    levelCount++;
    PlayerPrefs.SetInt("LevelNo", levelCount);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

public void RestartLevel()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

void LoadLevel()
{
    levelCount = PlayerPrefs.GetInt("LevelNo", 0);

    if (levelCount > Levels.Count - 1 || levelCount < 0)
    {
        levelCount = 0;
        PlayerPrefs.SetInt("LevelNo", levelCount);
    }
    Instantiate(Levels[levelCount], Vector3.zero, Quaternion.identity);
}

public void OnLevelStarted()
{
    isGameAlreadyOpen = true;
    isGameStarted = true;
    StartScreen.SetActive(false);
}

public void OnLevelCompleted()
{
    FinishScreen.SetActive(true);
}*/