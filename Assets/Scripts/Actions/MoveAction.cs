using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class MoveAction : BaseAction
{
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float stoppingDistance = 0.1f;
    [SerializeField] int maxMoveRange = 4;

    Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            ActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public override string GetActionName()
    {
        return "MOVE";
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveRange; x <= maxMoveRange; x++)
        {
            for (int z = -maxMoveRange; z <= maxMoveRange; z++)
            {
                int circleDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (circleDistance > maxMoveRange + 1) continue;

                GridPosition rangeGridPosition = new GridPosition(x, z) + unitGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(rangeGridPosition) || rangeGridPosition == unitGridPosition || LevelGrid.Instance.HasAnyUnitOnPosition(rangeGridPosition))
                {
                    continue;
                }
                validGridPositions.Add(rangeGridPosition);
            }
        }
        return validGridPositions;
    }
}
