using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class MoveAction : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float stoppingDistance = 0.1f;
    [SerializeField] int maxMoveRange = 4;

    Vector3 targetPosition;
    Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    public void Move(GridPosition targetPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
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

    public bool IsValidGridPosition(GridPosition position)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositionList();
        return validGridPositions.Contains(position);
    }
}
