using UnityEngine;

public class IntroUI : MonoBehaviour
{
    [Header("Intro Panel")]
    public GameObject introPanel;

    [Header("Player Control")]
    public MonoBehaviour playerController;

    [Header("Objective Text")]
    public GameObject objectiveTextObject;

    private bool introActive = true;

    void Start()
    {
        ShowIntro();
    }

    void Update()
    {
        if (!introActive) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            CloseIntro();
        }
    }

    void ShowIntro()
    {
        introActive = true;

        if (introPanel != null)
        {
            introPanel.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (objectiveTextObject != null)
        {
            objectiveTextObject.SetActive(false);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseIntro()
    {
        introActive = false;

        if (introPanel != null)
        {
            introPanel.SetActive(false);
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        if (objectiveTextObject != null)
        {
            objectiveTextObject.SetActive(false);
        }

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}