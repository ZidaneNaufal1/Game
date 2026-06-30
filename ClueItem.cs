using UnityEngine;

public class ClueItem : MonoBehaviour
{
    [Header("Clue Data")]
    public string clueName = "Catatan Sejarah";

    [TextArea(2, 4)]
    public string questionText = "Pertanyaan pilihan ganda di sini.";

    public string[] answers = new string[3];

    [Tooltip("0 = jawaban pertama, 1 = jawaban kedua, 2 = jawaban ketiga")]
    public int correctAnswerIndex = 0;

    [Header("Book Object")]
    public GameObject bookObjectToHide;

    private bool hasFactRead = false;
    private bool isSolved = false;
    private bool quizCleared = false;

    public bool HasFactRead
    {
        get { return hasFactRead; }
    }

    public bool IsSolved
    {
        get { return isSolved; }
    }

    public bool QuizCleared
    {
        get { return quizCleared; }
    }

    public void MarkFactRead()
    {
        hasFactRead = true;
        Debug.Log("Funfact untuk " + clueName + " sudah dibaca.");
    }

    public void OnQuizCorrect()
    {
        if (isSolved) return;

        quizCleared = true;

        Debug.Log("Jawaban benar! Buku/kunci ditemukan: " + clueName);

        Collect();
    }

    public void Collect()
    {
        if (isSolved) return;

        isSolved = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectClue(clueName);
        }

        if (bookObjectToHide != null)
        {
            bookObjectToHide.SetActive(false);
        }
    }
}