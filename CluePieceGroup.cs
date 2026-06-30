using UnityEngine;

public class CluePieceGroup : MonoBehaviour
{
    [Header("Main Clue")]
    public ClueItem sourceClue;

    [Header("Pieces")]
    public CluePiece[] pieces;

    private int collectedPieces = 0;
    private bool isActivated = false;
    private bool isCompleted = false;

    void Start()
    {
        SetPiecesActive(false);
    }

    public void ActivatePieces()
    {
        if (isCompleted) return;

        isActivated = true;
        collectedPieces = 0;

        SetPiecesActive(true);

        Debug.Log("Clue pieces muncul. Cari semua potongan clue di sekitar area ini.");

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective("Objective: Cari semua potongan clue di sekitar Quiz Station.");
        }
    }

    public void CollectPiece(CluePiece piece)
    {
        if (!isActivated || isCompleted) return;

        collectedPieces++;

        Debug.Log("Clue piece terkumpul: " + collectedPieces + " / " + pieces.Length);

        piece.gameObject.SetActive(false);

        if (collectedPieces >= pieces.Length)
        {
            CompleteGroup();
        }
    }

    void CompleteGroup()
    {
        isCompleted = true;

        Debug.Log("Semua clue piece terkumpul. Clue utama berhasil didapat.");

        if (sourceClue != null)
        {
            sourceClue.Collect();
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective("Objective: Clue utama didapat. Cari buku sejarah berikutnya.");
        }
    }

    void SetPiecesActive(bool active)
    {
        foreach (CluePiece piece in pieces)
        {
            if (piece != null)
            {
                piece.gameObject.SetActive(active);
            }
        }
    }
}