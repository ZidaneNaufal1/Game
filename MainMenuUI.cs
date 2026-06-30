using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject howToPlayPanel;
    public GameObject creditsPanel;
    public GameObject difficultyPanel;

    [Header("Scene Settings")]
    public string gameplaySceneName = "Level_01_FogGrounds";

    void Start()
    {
        BackToMainMenu();

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    public void StartGame()
    {
        OpenDifficulty();
    }

    public void OpenDifficulty()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(true);
    }

    public void StartEasy()
    {
        SelectDifficultyAndStart("Easy");
    }

    public void StartNormal()
    {
        SelectDifficultyAndStart("Normal");
    }

    public void StartHard()
    {
        SelectDifficultyAndStart("Hard");
    }

    void SelectDifficultyAndStart(string difficultyName)
    {
        PlayerPrefs.SetString("GameDifficulty", difficultyName);
        PlayerPrefs.Save();

        Debug.Log("Difficulty dipilih: " + difficultyName);

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}