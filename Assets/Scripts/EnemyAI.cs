using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy,
    }

    State state;

    float timer;

    void Awake()
    {
        state = State.WaitingForTurn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitingForTurn:
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;

            case State.Busy:
                break;
        }
    }

    void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    void TurnChange()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    bool TryTakeEnemyAction(Action onEnemyActionComplete)
    {
        foreach (Unit enemy in UnitManager.Instance.GetEnemyUnits())
        {
            if (TryTakeEnemyAction(enemy, onEnemyActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    bool TryTakeEnemyAction(Unit enemy, Action onEnemyActionComplete)
    {
        EnemyAIAction bestEnemyAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction action in enemy.GetActions())
        {
            if (!enemy.CanTakeAction(action))
            {
                continue;
            }

            if (bestEnemyAction == null)
            {
                bestEnemyAction = action.GetBestEnemyAction();
                bestBaseAction = action;
            }
            else
            {
                EnemyAIAction testAction = action.GetBestEnemyAction();
                if (testAction != null && testAction.actionValue > bestEnemyAction.actionValue)
                {
                    bestEnemyAction = testAction;
                    bestBaseAction = action;
                }
            }
        }

        if (bestEnemyAction != null && enemy.TryTakingAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAction.gridPosition, onEnemyActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChange -= TurnChange;
    }
}
