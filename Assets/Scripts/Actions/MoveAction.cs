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

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "MOVE";
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
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
                GridPosition offsetGridPosition = new GridPosition(x, z) + unitGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(offsetGridPosition) || offsetGridPosition == unitGridPosition || LevelGrid.Instance.HasAnyUnitOnPosition(offsetGridPosition))
                {
                    continue;
                }
                validGridPositions.Add(offsetGridPosition);
            }
        }
        return validGridPositions;
    }
}
