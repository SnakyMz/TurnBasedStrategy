using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event Action<BaseAction> OnActionStart;
    public static event Action OnActionEnd;

    // delegate
    protected Action onActionComplete;
    protected Unit unit;

    protected bool isActive = false;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public Unit GetUnit()
    {
        return unit;
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

        OnActionStart?.Invoke(this);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnActionEnd?.Invoke();
    }

    public abstract void TakeAction(GridPosition position, Action onActionComplete);

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual bool IsValidGridPosition(GridPosition position)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositionList();
        return validGridPositions.Contains(position);
    }

    public EnemyAIAction GetBestEnemyAction()
    {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
        List<GridPosition> validGridPositions = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validGridPositions)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActions.Add(enemyAIAction);
        }

        if (enemyAIActions.Count > 0)
        {
            enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => (b.actionValue - a.actionValue));
            return enemyAIActions[0];
        }
        else
        {
            return null;
        }
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
