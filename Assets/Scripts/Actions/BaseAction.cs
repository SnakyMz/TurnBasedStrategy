using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action onActionComplete;
    protected Unit unit;

    protected bool isActive = false;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public virtual int GetActionCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
    }

    public abstract void TakeAction(GridPosition position, Action onActionComplete);

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual bool IsValidGridPosition(GridPosition position)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositionList();
        return validGridPositions.Contains(position);
    }
}
