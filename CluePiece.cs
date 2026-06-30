using UnityEngine;

public class CluePiece : MonoBehaviour
{
    [Header("Piece Data")]
    public string pieceName = "Clue Piece";
    public CluePieceGroup group;

    private bool isCollected = false;

    public void Collect()
    {
        if (isCollected) return;

        isCollected = true;

        Debug.Log("Mengambil " + pieceName);

        if (group != null)
        {
            group.CollectPiece(this);
        }
        else
        {
            Debug.LogWarning("CluePiece belum punya group.");
            gameObject.SetActive(false);
        }
    }
}