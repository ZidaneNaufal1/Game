using UnityEngine;

public class QuizStation : MonoBehaviour
{
    [Header("Linked Clue")]
    public ClueItem linkedClue;

    public void OpenQuizStation()
    {
        if (linkedClue == null)
        {
            Debug.LogWarning("Linked Clue belum diisi di QuizStation.");

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Quiz Station belum tersambung ke clue.");
            }

            return;
        }

        if (!linkedClue.HasFactRead)
        {
            Debug.Log("Funfact untuk quiz ini belum dibaca.");

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Cari dan baca funfact dulu sebelum menjawab quiz.");
            }

            return;
        }

        if (linkedClue.IsSolved)
        {
            Debug.Log("Quiz ini sudah selesai.");

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Buku/kunci ini sudah didapat. Cari buku berikutnya.");
            }

            return;
        }

        if (BookQuizUI.Instance != null)
        {
            BookQuizUI.Instance.OpenQuiz(linkedClue);
        }
        else
        {
            Debug.LogWarning("BookQuizUI belum ada di scene.");
        }
    }
}