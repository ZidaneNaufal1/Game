using UnityEngine;
using TMPro;

public class BookQuizUI : MonoBehaviour
{
    public static BookQuizUI Instance;

    [Header("UI Panel")]
    public GameObject quizPanel;

    [Header("Texts")]
    public TMP_Text titleText;
    public TMP_Text factText;
    public TMP_Text questionText;
    public TMP_Text optionText;
    public TMP_Text instructionText;

    private ClueItem currentClue;
    private GameObject objectToHideAfterClose;

    private bool isQuizOpen = false;
    private bool isFactOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (quizPanel != null)
        {
            quizPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isFactOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                ClosePanel();
            }

            return;
        }

        if (!isQuizOpen) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Answer(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Answer(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Answer(2);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void OpenFact(string factTitle, string factContent, GameObject objectToHide)
    {
        currentClue = null;
        objectToHideAfterClose = objectToHide;

        isFactOpen = true;
        isQuizOpen = false;

        if (quizPanel != null)
        {
            quizPanel.SetActive(true);
        }

        titleText.text = factTitle;
        factText.text = factContent;
        questionText.text = "";
        optionText.text = "";
        instructionText.text = "Tekan E / ESC untuk menutup. Setelah itu cari buku quiz.";

        if (ObjectiveUI.Instance != null && ObjectiveUI.Instance.objectiveText != null)
        {
            ObjectiveUI.Instance.objectiveText.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        Debug.Log("PANEL FUNFACT MUNCUL: " + factTitle);
    }

    public void OpenQuiz(ClueItem clue)
    {
        currentClue = clue;
        objectToHideAfterClose = null;

        isQuizOpen = true;
        isFactOpen = false;

        if (quizPanel != null)
        {
            quizPanel.SetActive(true);
        }

        titleText.text = "Quiz Sejarah";
        factText.text = "Jawab berdasarkan funfact yang sudah kamu baca.";
        questionText.text = clue.questionText;

        optionText.text =
            "1. " + clue.answers[0] + "\n" +
            "2. " + clue.answers[1] + "\n" +
            "3. " + clue.answers[2];

        instructionText.text = "Tekan 1 / 2 / 3 untuk menjawab. Tekan ESC untuk menutup.";

        if (ObjectiveUI.Instance != null && ObjectiveUI.Instance.objectiveText != null)
        {
            ObjectiveUI.Instance.objectiveText.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    void Answer(int index)
    {
        if (currentClue == null) return;

        if (index == currentClue.correctAnswerIndex)
        {
            Debug.Log("Jawaban benar! Buku/kunci ditemukan.");

            Time.timeScale = 1f;

            currentClue.OnQuizCorrect();

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Buku/kunci ditemukan. Cari funfact berikutnya.");
            }

            ClosePanel();
        }
        else
        {
            Debug.Log("Jawaban salah! Monster mendengar kesalahanmu...");

            Time.timeScale = 1f;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AlertMonsterFromWrongAnswer();
            }

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Jawaban salah. Kabur dari monster!");
            }

            ClosePanel();
        }
    }

    void ClosePanel()
    {
        bool wasFactOpen = isFactOpen;

        isQuizOpen = false;
        isFactOpen = false;
        currentClue = null;

        if (quizPanel != null)
        {
            quizPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        if (ObjectiveUI.Instance != null && ObjectiveUI.Instance.objectiveText != null)
        {
            ObjectiveUI.Instance.objectiveText.gameObject.SetActive(true);
        }

        if (wasFactOpen)
        {
            if (objectToHideAfterClose != null)
            {
                objectToHideAfterClose.SetActive(false);
            }

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Funfact terbaca. Cari buku quiz.");
            }
        }

        objectToHideAfterClose = null;
    }
}