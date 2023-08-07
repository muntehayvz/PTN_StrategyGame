using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
/*        BuildingFactory buildingFactory = GetComponent<BuildingFactory>();

        IBuilding barracks = buildingFactory.CreateBuilding(BuildingType.Barracks);
        IBuilding powerPlant = buildingFactory.CreateBuilding(BuildingType.PowerPlant);

        barracks.DisplayInfo();
        powerPlant.DisplayInfo();*/
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