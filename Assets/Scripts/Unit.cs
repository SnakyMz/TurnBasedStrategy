using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(MoveAction))]
[RequireComponent(typeof(HealAction))]
[RequireComponent(typeof(ShootAction))]
public class Unit : MonoBehaviour
{
    public static event Action<Unit> OnUnitSpawn;
    public static event Action<Unit> OnUnitDeath;

    [SerializeField] TextMeshProUGUI actionPointUI;
    [SerializeField] bool isEnemy = false;
    [SerializeField] int maxActionPoints = 3;
    [SerializeField] float deathLinger = 2;

    GridPosition gridPosition;
    HealthSystem healthSystem;
    BaseAction[] unitActions;
    MoveAction moveAction;
    HealAction healAction;
    ShootAction shootAction;

    int actionPoints = 0;

    void Awake()
    {
        actionPoints = maxActionPoints;
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        healAction = GetComponent<HealAction>();
        shootAction = GetComponent<ShootAction>();
        unitActions = GetComponents<BaseAction>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateActionPointUI();
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnChange;
        healthSystem.OnDeath += HandleDeath;
        OnUnitSpawn?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public BaseAction[] GetActions()
    {
        return unitActions;
    }

    public float GetHealthNormalize()
    {
        return healthSystem.GetHealthNormalize();
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public HealAction GetHealAction()
    {
        return healAction;
    }

    public ShootAction GetShootAction()
    {
        return shootAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public bool CanTakeAction(BaseAction action)
    {
        int actionCost = action.GetActionCost();
        if (actionPoints >= actionCost) return true;
        else return false;

    }

    public bool TryTakingAction(BaseAction action)
    {
        if (CanTakeAction(action))
        {
            actionPoints -= action.GetActionCost();
            UpdateActionPointUI();
            return true;
        }

        return false;
    }

    void TurnChange()
    {
        bool isPlayerTurn = TurnSystem.Instance.IsPlayerTurn();
        if ((isEnemy && !isPlayerTurn) || (!isEnemy && isPlayerTurn))
        {
            actionPoints = maxActionPoints;
            UpdateActionPointUI();
        }
    }

    void UpdateActionPointUI()
    {
        actionPointUI.text = actionPoints.ToString();
    }

    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    void HandleDeath()
    {
        LevelGrid.Instance.ClearUnitAtGridPosition(gridPosition);
        StartCoroutine(DestroyDelay(deathLinger));
    }

    IEnumerator DestroyDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChange -= TurnChange;
        healthSystem.OnDeath -= HandleDeath;
        OnUnitDeath?.Invoke(this);
    }
}
