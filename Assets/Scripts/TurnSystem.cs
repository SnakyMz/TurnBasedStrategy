using System;
using TMPro;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event Action OnTurnChange;

    [SerializeField] TextMeshProUGUI turnNumberText;
    int turnNumber = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one turn system" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turnNumberText.text = "TURN " + turnNumber;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextTurn()
    {
        turnNumber++;
        turnNumberText.text = "TURN " + turnNumber;
        OnTurnChange?.Invoke();
        UnitActionSystem.Instance.UpdateActionPointsUI();
    }
}
