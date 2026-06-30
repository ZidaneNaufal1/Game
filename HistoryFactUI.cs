using UnityEngine;
using TMPro;

public class HistoryFactUI : MonoBehaviour
{
    public static HistoryFactUI Instance;

    [Header("History Fact UI")]
    public GameObject historyFactPanel;
    public TextMeshProUGUI historyTitleText;
    public TextMeshProUGUI historyContentText;
    public TextMeshProUGUI historyInstructionText;

    private HistoryFact currentFact;
    private bool isOpen = false;

    // INI PENTING:
    // Supaya tombol E yang membuka panel tidak langsung menutup panel di frame yang sama.
    private float closeDelay = 0.25f;
    private float closeTimer = 0f;
    private bool canClose = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (historyFactPanel != null)
        {
            historyFactPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!isOpen) return;

        if (!canClose)
        {
            closeTimer += Time.unscaledDeltaTime;

            if (closeTimer >= closeDelay)
            {
                canClose = true;
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            CloseFactPanel();
        }
    }

    public void OpenFactPanel(HistoryFact fact)
    {
        if (fact == null) return;

        currentFact = fact;
        isOpen = true;

        closeTimer = 0f;
        canClose = false;

        if (historyFactPanel != null)
        {
            historyFactPanel.SetActive(true);
        }

        if (historyTitleText != null)
        {
            historyTitleText.text = fact.factTitle;
        }

        if (historyContentText != null)
        {
            historyContentText.text = fact.funFactText;
        }

        if (historyInstructionText != null)
        {
            historyInstructionText.text = "Tekan E / ESC untuk menutup";
        }

        Debug.Log("PANEL FUNFACT MUNCUL: " + fact.factTitle);
    }

    public void CloseFactPanel()
    {
        if (!isOpen) return;

        isOpen = false;
        canClose = false;
        closeTimer = 0f;

        if (historyFactPanel != null)
        {
            historyFactPanel.SetActive(false);
        }

        if (currentFact != null)
        {
            currentFact.FinishReading();
            currentFact = null;
        }
    }
}