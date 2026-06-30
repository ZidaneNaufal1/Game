using UnityEngine;
using System.Reflection;

public class DifficultyRuntimeController : MonoBehaviour
{
    [Header("Monster AI")]
    public MonoBehaviour monsterAI;

    [Header("Monster Speed Settings")]
    public float easyMonsterSpeed = 3.0f;
    public float normalMonsterSpeed = 4.2f;
    public float hardMonsterSpeed = 5.4f;

    [Header("Hard Mode")]
    public bool hardAlwaysChaseAfterFirstKey = true;

    private string difficulty = "Normal";

    void Start()
    {
        difficulty = PlayerPrefs.GetString("GameDifficulty", "Normal");
        ApplyMonsterDifficulty();

        Debug.Log("Difficulty aktif: " + difficulty);
    }

    void Update()
    {
        if (difficulty != "Hard") return;
        if (!hardAlwaysChaseAfterFirstKey) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.collectedClues < 1) return;

        ForceMonsterChase();
    }

    void ApplyMonsterDifficulty()
    {
        if (monsterAI == null)
        {
            Debug.LogWarning("Monster AI belum diisi di DifficultyRuntimeController.");
            return;
        }

        float targetSpeed = normalMonsterSpeed;

        if (difficulty == "Easy")
        {
            targetSpeed = easyMonsterSpeed;
        }
        else if (difficulty == "Normal")
        {
            targetSpeed = normalMonsterSpeed;
        }
        else if (difficulty == "Hard")
        {
            targetSpeed = hardMonsterSpeed;
        }

        SetFloatIfExists("moveSpeed", targetSpeed);
        SetFloatIfExists("speed", targetSpeed);
        SetFloatIfExists("chaseSpeed", targetSpeed);
        SetFloatIfExists("monsterSpeed", targetSpeed);
        SetFloatIfExists("runSpeed", targetSpeed);

        Debug.Log("Monster speed difficulty diset ke: " + targetSpeed);
    }

    void ForceMonsterChase()
    {
        SetBoolIfExists("isChasing", true);
        SetBoolIfExists("isHunting", true);
        SetBoolIfExists("isActive", true);
    }

    void SetFloatIfExists(string fieldName, float value)
    {
        if (monsterAI == null) return;

        System.Type type = monsterAI.GetType();

        FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field != null && field.FieldType == typeof(float))
        {
            field.SetValue(monsterAI, value);
            return;
        }

        PropertyInfo property = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (property != null && property.PropertyType == typeof(float) && property.CanWrite)
        {
            property.SetValue(monsterAI, value);
        }
    }

    void SetBoolIfExists(string fieldName, bool value)
    {
        if (monsterAI == null) return;

        System.Type type = monsterAI.GetType();

        FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field != null && field.FieldType == typeof(bool))
        {
            field.SetValue(monsterAI, value);
            return;
        }

        PropertyInfo property = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (property != null && property.PropertyType == typeof(bool) && property.CanWrite)
        {
            property.SetValue(monsterAI, value);
        }
    }
}