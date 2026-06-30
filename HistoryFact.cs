using UnityEngine;

public class HistoryFact : MonoBehaviour
{
    [Header("Fact Data")]
    public string factTitle = "Catatan Sejarah";

    [TextArea(3, 8)]
    public string funFactText = "Isi funfact sejarah Indonesia di sini.";

    [Header("Linked Clue")]
    public ClueItem linkedClue;

    [Header("Object Settings")]
    public GameObject objectToHideAfterRead;

    private bool hasBeenRead = false;

    public void OpenFact()
    {
        if (hasBeenRead) return;

        if (HistoryFactUI.Instance != null)
        {
            HistoryFactUI.Instance.OpenFactPanel(this);
        }
        else
        {
            Debug.LogWarning("HistoryFactUI belum ada di Canvas.");
        }
    }

    public void FinishReading()
    {
        if (hasBeenRead) return;

        hasBeenRead = true;

        if (linkedClue != null)
        {
            linkedClue.MarkFactRead();
        }
        else
        {
            Debug.LogWarning("Linked Clue belum diisi di HistoryFact.");
        }

        Debug.Log("FUNFACT DIBACA: " + factTitle);

        if (objectToHideAfterRead != null)
        {
            objectToHideAfterRead.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}