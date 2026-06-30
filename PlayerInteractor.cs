using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    private HistoryFact nearbyFact;
    private GateExit nearbyGate;
    private QuizStation nearbyQuizStation;
    private CluePiece nearbyPiece;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (nearbyGate != null && GameManager.Instance != null && GameManager.Instance.HasAllClues())
            {
                nearbyGate.TryOpenGate();
                return;
            }

            if (nearbyPiece != null)
            {
                nearbyPiece.Collect();
                nearbyPiece = null;
                return;
            }

            if (nearbyQuizStation != null)
            {
                nearbyQuizStation.OpenQuizStation();
                return;
            }

            if (nearbyFact != null)
            {
                nearbyFact.OpenFact();
                return;
            }

            if (nearbyGate != null)
            {
                nearbyGate.TryOpenGate();
                return;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        CluePiece piece = other.GetComponent<CluePiece>();

        if (piece != null)
        {
            nearbyPiece = piece;
            Debug.Log("Tekan E untuk mengambil clue piece: " + piece.pieceName);
            return;
        }

        HistoryFact fact = other.GetComponent<HistoryFact>();

        if (fact != null)
        {
            nearbyFact = fact;
            Debug.Log("Tekan E untuk membaca funfact: " + fact.factTitle);
            return;
        }

        QuizStation quizStation = other.GetComponent<QuizStation>();

        if (quizStation != null)
        {
            nearbyQuizStation = quizStation;
            Debug.Log("Tekan E untuk membuka Quiz Station.");
            return;
        }

        GateExit gate = other.GetComponent<GateExit>();

        if (gate != null)
        {
            nearbyGate = gate;
            Debug.Log("Tekan E untuk membuka gerbang.");
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        CluePiece piece = other.GetComponent<CluePiece>();

        if (piece != null && piece == nearbyPiece)
        {
            nearbyPiece = null;
            Debug.Log("Menjauh dari clue piece.");
            return;
        }

        HistoryFact fact = other.GetComponent<HistoryFact>();

        if (fact != null && fact == nearbyFact)
        {
            nearbyFact = null;
            Debug.Log("Menjauh dari funfact.");
            return;
        }

        QuizStation quizStation = other.GetComponent<QuizStation>();

        if (quizStation != null && quizStation == nearbyQuizStation)
        {
            nearbyQuizStation = null;
            Debug.Log("Menjauh dari Quiz Station.");
            return;
        }

        GateExit gate = other.GetComponent<GateExit>();

        if (gate != null && gate == nearbyGate)
        {
            nearbyGate = null;
            Debug.Log("Menjauh dari gerbang.");
            return;
        }
    }
}