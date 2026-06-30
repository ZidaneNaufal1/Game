using UnityEngine;
using System.Reflection;

public class EchoSense : MonoBehaviour
{
    [Header("Input")]
    public KeyCode echoKey = KeyCode.Q;

    [Header("UI Arrow")]
    public GameObject screenArrow;
    public float markerDuration = 4f;
    public float screenEdgePadding = 170f;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Sequential Targets")]
    public Transform[] historyTargets;
    public Transform[] quizTargets;
    public Transform exitTarget;

    [Header("Difficulty Echo Settings")]
    public float easyCooldown = 0f;
    public float normalCooldown = 6f;
    public float hardCooldown = 12f;
    public int hardEchoUsesAfterFirstKey = 3;

    private Transform currentTarget;
    private float markerTimer = 0f;
    private float cooldownTimer = 0f;
    private bool echoActive = false;

    private string difficulty = "Normal";
    private int hardEchoUsesLeft = 3;

    void Start()
    {
        difficulty = PlayerPrefs.GetString("GameDifficulty", "Normal");
        hardEchoUsesLeft = hardEchoUsesAfterFirstKey;

        if (screenArrow != null)
        {
            screenArrow.SetActive(false);
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.ShowObjective(false);
        }

        Debug.Log("Echo Sense difficulty aktif: " + difficulty);
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(echoKey))
        {
            TryActivateEchoSense();
        }

        if (echoActive)
        {
            markerTimer -= Time.deltaTime;

            if (currentTarget != null)
            {
                UpdateScreenArrow();
            }

            if (markerTimer <= 0f)
            {
                HideEchoSense();
            }
        }
    }

    void TryActivateEchoSense()
    {
        if (cooldownTimer > 0f)
        {
            ShowTemporaryMessage("Objective: Echo Sense masih cooldown.");
            return;
        }

        if (IsHardModeAfterFirstKey() && hardEchoUsesLeft <= 0)
        {
            ShowTemporaryMessage("Objective: Echo Sense sudah habis di mode Hard.");
            return;
        }

        ActivateEchoSense();
    }

    void ActivateEchoSense()
    {
        currentTarget = FindSequentialTarget();

        if (currentTarget != null && screenArrow != null)
        {
            screenArrow.SetActive(true);
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.ShowObjective(true);

            if (currentTarget == null)
            {
                ObjectiveUI.Instance.SetObjective("Objective: Echo Sense tidak menemukan target.");
            }
            else
            {
                ObjectiveUI.Instance.SetObjective(GetEchoObjectiveText(currentTarget));
            }
        }

        ApplyEchoDifficultyCost();
        AttractMonsterAfterFirstKey();

        markerTimer = markerDuration;
        echoActive = true;
    }

    void ApplyEchoDifficultyCost()
    {
        if (difficulty == "Easy")
        {
            cooldownTimer = easyCooldown;
        }
        else if (difficulty == "Normal")
        {
            cooldownTimer = normalCooldown;
        }
        else if (difficulty == "Hard")
        {
            cooldownTimer = hardCooldown;

            if (GameManager.Instance != null && GameManager.Instance.collectedClues >= 1)
            {
                hardEchoUsesLeft--;

                if (ObjectiveUI.Instance != null)
                {
                    ObjectiveUI.Instance.SetObjective(GetEchoObjectiveText(currentTarget) + " Sisa Echo: " + hardEchoUsesLeft + " / " + hardEchoUsesAfterFirstKey + ".");
                }
            }
        }
    }

    bool IsHardModeAfterFirstKey()
    {
        if (difficulty != "Hard") return false;
        if (GameManager.Instance == null) return false;

        return GameManager.Instance.collectedClues >= 1;
    }

    void ShowTemporaryMessage(string message)
    {
        currentTarget = null;

        if (screenArrow != null)
        {
            screenArrow.SetActive(false);
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.ShowObjective(true);
            ObjectiveUI.Instance.SetObjective(message);
        }

        markerTimer = 2f;
        echoActive = true;
    }

    void HideEchoSense()
    {
        echoActive = false;

        if (screenArrow != null)
        {
            screenArrow.SetActive(false);
        }

        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.ShowObjective(false);
        }

        currentTarget = null;
    }

    Transform FindSequentialTarget()
    {
        int progress = 0;

        if (GameManager.Instance != null)
        {
            progress = GameManager.Instance.collectedClues;
        }

        progress = Mathf.Clamp(progress, 0, 3);

        if (progress >= 3)
        {
            return exitTarget;
        }

        Transform historyTarget = GetTargetFromArray(historyTargets, progress);

        if (historyTarget != null && historyTarget.gameObject.activeInHierarchy)
        {
            return historyTarget;
        }

        Transform quizTarget = GetTargetFromArray(quizTargets, progress);

        if (quizTarget != null && quizTarget.gameObject.activeInHierarchy)
        {
            return quizTarget;
        }

        if (exitTarget != null)
        {
            return exitTarget;
        }

        return null;
    }

    Transform GetTargetFromArray(Transform[] targets, int index)
    {
        if (targets == null) return null;
        if (index < 0) return null;
        if (index >= targets.Length) return null;

        return targets[index];
    }

    string GetEchoObjectiveText(Transform target)
    {
        int progress = 0;

        if (GameManager.Instance != null)
        {
            progress = GameManager.Instance.collectedClues;
        }

        progress = Mathf.Clamp(progress, 0, 3);

        if (progress >= 3)
        {
            return "Objective: Echo menunjukkan arah gerbang keluar.";
        }

        Transform historyTarget = GetTargetFromArray(historyTargets, progress);
        Transform quizTarget = GetTargetFromArray(quizTargets, progress);

        if (target == historyTarget)
        {
            return "Objective: Echo menunjukkan buku sejarah ke-" + (progress + 1) + ".";
        }

        if (target == quizTarget)
        {
            return "Objective: Echo menunjukkan buku quiz ke-" + (progress + 1) + ".";
        }

        return "Objective: Ikuti arah Echo Sense.";
    }

    void AttractMonsterAfterFirstKey()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.collectedClues < 1) return;

        MethodInfo echoAlertMethod = GameManager.Instance.GetType().GetMethod("AlertMonsterFromEchoSense");

        if (echoAlertMethod != null)
        {
            echoAlertMethod.Invoke(GameManager.Instance, null);
        }
        else
        {
            MethodInfo wrongAnswerMethod = GameManager.Instance.GetType().GetMethod("AlertMonsterFromWrongAnswer");

            if (wrongAnswerMethod != null)
            {
                wrongAnswerMethod.Invoke(GameManager.Instance, null);
            }
        }
    }

    void UpdateScreenArrow()
    {
        if (screenArrow == null) return;
        if (currentTarget == null) return;

        Camera cam = Camera.main;

        if (cameraTransform != null)
        {
            Camera foundCamera = cameraTransform.GetComponent<Camera>();

            if (foundCamera != null)
            {
                cam = foundCamera;
            }
        }

        if (cam == null) return;

        RectTransform arrowRect = screenArrow.GetComponent<RectTransform>();
        Canvas canvas = screenArrow.GetComponentInParent<Canvas>();

        if (arrowRect == null || canvas == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        if (canvasRect == null) return;

        Vector3 viewportPoint = cam.WorldToViewportPoint(currentTarget.position);

        bool targetBehindCamera = viewportPoint.z < 0f;

        Vector2 direction = new Vector2(viewportPoint.x - 0.5f, viewportPoint.y - 0.5f);

        if (targetBehindCamera)
        {
            direction *= -1f;
        }

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector2.up;
        }

        direction.Normalize();

        Vector2 canvasSize = canvasRect.rect.size;

        Vector2 arrowPosition = new Vector2(
            direction.x * ((canvasSize.x * 0.5f) - screenEdgePadding),
            direction.y * ((canvasSize.y * 0.5f) - screenEdgePadding)
        );

        arrowRect.anchoredPosition = arrowPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        arrowRect.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}