using UnityEngine;

public class GateExit : MonoBehaviour
{
    [Header("Gate Settings")]
    public GameObject gateObject;
    public string lockedMessage = "Gerbang masih terkunci. Cari semua clue dulu.";
    public string openMessage = "Gerbang terbuka! Kamu berhasil keluar.";

    private bool isOpen = false;

    public void TryOpenGate()
    {
        if (isOpen) return;

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager belum ada di scene.");
            return;
        }

        if (GameManager.Instance.HasAllClues())
        {
            OpenGate();
        }
        else
        {
            Debug.Log(lockedMessage);
        }
    }

    void OpenGate()
    {
        isOpen = true;

        Debug.Log(openMessage);

        if (gateObject != null)
        {
            gateObject.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.WinGame();
        }
    }
}