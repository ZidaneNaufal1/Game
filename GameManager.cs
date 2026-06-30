using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Clue / Key Settings")]
    public int totalClues = 3;
    public int collectedClues = 0;

    [Header("Monster Settings")]
    public GameObject monsterObject;
    public Transform monsterSpawnPoint;
    public int cluesNeededToSpawnMonster = 1;

    [Header("Random Monster Spawn")]
    public bool useRandomMonsterSpawn = true;
    public Transform[] randomMonsterSpawnPoints;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Win UI")]
    public GameObject winPanel;

    [Header("Player Control")]
    public MonoBehaviour playerController;

    private bool monsterSpawned = false;
    private bool isGameOver = false;
    private bool isWin = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (monsterObject != null)
        {
            monsterObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if ((isGameOver || isWin) && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void CollectClue(string clueName)
    {
        if (isGameOver || isWin) return;

        collectedClues++;

        Debug.Log("BUKU/KUNCI DIDAPAT: " + clueName);
        Debug.Log("Progress kunci gerbang: " + collectedClues + " / " + totalClues);

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective("Objective: Buku/kunci ditemukan " + collectedClues + " / " + totalClues + ". Cari funfact berikutnya.");
        }

        if (collectedClues >= cluesNeededToSpawnMonster && !monsterSpawned)
        {
            SpawnMonster();
        }

        if (collectedClues >= totalClues)
        {
            Debug.Log("SEMUA BUKU/KUNCI TERKUMPUL! Gerbang keluar sekarang bisa dibuka.");

            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Semua kunci terkumpul. Pergi ke gerbang keluar.");
            }
        }
    }

    public bool HasAllClues()
    {
        return collectedClues >= totalClues;
    }

    void SpawnMonster()
    {
        monsterSpawned = true;

        if (monsterObject == null)
        {
            Debug.LogWarning("Monster Object belum diisi di GameManager.");
            return;
        }

        Transform selectedSpawnPoint = GetRandomMonsterSpawnPoint();

        if (selectedSpawnPoint != null)
        {
            monsterObject.transform.position = selectedSpawnPoint.position;
            monsterObject.transform.rotation = selectedSpawnPoint.rotation;

            Debug.Log("Monster spawn random di: " + selectedSpawnPoint.name);
        }
        else if (monsterSpawnPoint != null)
        {
            monsterObject.transform.position = monsterSpawnPoint.position;
            monsterObject.transform.rotation = monsterSpawnPoint.rotation;

            Debug.Log("Monster spawn di spawn point default.");
        }
        else
        {
            Debug.LogWarning("Tidak ada Monster Spawn Point yang diisi.");
        }

        monsterObject.SetActive(true);

        MonsterChaseAI monsterAI = monsterObject.GetComponent<MonsterChaseAI>();

        if (monsterAI != null)
        {
            monsterAI.isChasing = true;
        }
        else
        {
            Debug.LogWarning("MonsterChaseAI belum ada di object Monster.");
        }

        Debug.Log("Monster muncul dari kabut dan mulai mengejar player!");
    }

    Transform GetRandomMonsterSpawnPoint()
    {
        if (!useRandomMonsterSpawn)
        {
            return monsterSpawnPoint;
        }

        if (randomMonsterSpawnPoints == null || randomMonsterSpawnPoints.Length == 0)
        {
            return monsterSpawnPoint;
        }

        int validCount = 0;

        for (int i = 0; i < randomMonsterSpawnPoints.Length; i++)
        {
            if (randomMonsterSpawnPoints[i] != null)
            {
                validCount++;
            }
        }

        if (validCount == 0)
        {
            return monsterSpawnPoint;
        }

        Transform[] validPoints = new Transform[validCount];
        int index = 0;

        for (int i = 0; i < randomMonsterSpawnPoints.Length; i++)
        {
            if (randomMonsterSpawnPoints[i] != null)
            {
                validPoints[index] = randomMonsterSpawnPoints[i];
                index++;
            }
        }

        int randomIndex = Random.Range(0, validPoints.Length);
        return validPoints[randomIndex];
    }

    public void AlertMonsterFromWrongAnswer()
    {
        if (isGameOver || isWin) return;

        Debug.Log("Jawaban salah! Monster ter-alert.");

        if (!monsterSpawned)
        {
            SpawnMonster();
        }

        if (monsterObject != null)
        {
            MonsterChaseAI monsterAI = monsterObject.GetComponent<MonsterChaseAI>();

            if (monsterAI != null)
            {
                monsterAI.isChasing = true;
            }
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective("Objective: Jawaban salah. Kabur dari monster!");
        }
    }

    public void AlertMonsterFromEchoSense()
    {
        if (isGameOver || isWin) return;

        Debug.Log("Echo Sense menarik perhatian monster!");

        if (!monsterSpawned)
        {
            SpawnMonster();
        }

        if (monsterObject != null)
        {
            MonsterChaseAI monsterAI = monsterObject.GetComponent<MonsterChaseAI>();

            if (monsterAI != null)
            {
                monsterAI.isChasing = true;
            }
            else
            {
                Debug.LogWarning("MonsterChaseAI belum ada di object Monster.");
            }
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective("Objective: Echo Sense menarik perhatian monster. Tetap bergerak!");
        }
    }

    public void GameOver()
    {
        if (isGameOver || isWin) return;

        isGameOver = true;

        Debug.Log("GAME OVER! Player tertangkap monster.");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void WinGame()
    {
        if (isGameOver || isWin) return;

        isWin = true;

        Debug.Log("YOU ESCAPED! Player berhasil keluar dari kabut.");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (monsterObject != null)
        {
            monsterObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}