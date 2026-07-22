using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] int actionPoints = 3;
    GridPosition gridPosition;
    BaseAction[] unitActions;
    MoveAction moveAction;
    SpinAction spinAction;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        unitActions = GetComponents<BaseAction>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
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
}
