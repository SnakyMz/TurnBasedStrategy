using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(MoveAction))]
[RequireComponent(typeof(ShootAction))]
public class Unit : MonoBehaviour
{
    [SerializeField] bool isEnemy = false;
    [SerializeField] int maxActionPoints = 3;
    [SerializeField] float deathLinger = 2;

    GridPosition gridPosition;
    HealthSystem healthSystem;
    BaseAction[] unitActions;
    MoveAction moveAction;
    SpinAction spinAction;

    int actionPoints = 0;

    void Awake()
    {
        actionPoints = maxActionPoints;
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        unitActions = GetComponents<BaseAction>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnChange;
        healthSystem.OnDeath += HandleDeath;
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

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public bool TryTakingAction(BaseAction action)
    {
        int actionCost = action.GetActionCost();
        if (actionPoints >= actionCost)
        {
            actionPoints -= actionCost;
            return true;
        }

        return false;
    }

    void TurnChange()
    {
        bool isPlayerTurn = TurnSystem.Instance.GetPlayerTurn();
        if ((isEnemy && !isPlayerTurn) || (!isEnemy && isPlayerTurn))
            actionPoints = maxActionPoints;
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
    }
}
