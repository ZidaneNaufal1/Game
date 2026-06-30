using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance;

    [Header("Objective Text")]
    public TextMeshProUGUI objectiveText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowObjective(false);
    }

    public void SetObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = newObjective;
        }

        Debug.Log(newObjective);
    }

    public void ShowObjective(bool show)
    {
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(show);
        }
    }
}