using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class SpinAction : BaseAction
{
    float totalSpinHappened = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        totalSpinHappened += spinAmount;
        if (totalSpinHappened >= 360)
        {
            totalSpinHappened = 0;
            ActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "SPIN";
    }

    public override int GetActionCost()
    {
        return 2;
    }

    public override void TakeAction(GridPosition position, Action onActionComplete)
    {
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitPosition = unit.GetGridPosition();
        return new List<GridPosition>
        {
            unitPosition
        };
    }
}
