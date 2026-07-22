using System;
using System.Collections.Generic;
using UnityEngine;


public class ShootAction : BaseAction
{
    enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    [SerializeField] Animator animator;
    [SerializeField] GameObject bulletProjectilePrefab;
    [SerializeField] Transform gunPoint;
    [SerializeField] int shootDamage = 4;
    [SerializeField] int maxShootRange = 6;
    [SerializeField] float shootingCooloff = 0.5f;
    [SerializeField] float rotateSpeed = 15f;

    State state;
    Unit targetUnit;

    float stateTimer;

    bool canShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        HandleState();
    }

    void HandleState()
    {
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                if (stateTimer <= 0)
                {
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                }
                break;

            case State.Shooting:
                if (canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                if (stateTimer <= 0)
                {
                    state = State.Cooloff;
                    float cooloffStateTime = 0.5f;
                    stateTimer = cooloffStateTime;
                }
                break;

            case State.Cooloff:
                if (stateTimer <= 0)
                {
                    ActionComplete();
                }
                break;
        }
    }

    public override string GetActionName()
    {
        return "SHOOT";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int x = -maxShootRange; x <= maxShootRange; x++)
        {
            for (int z = -maxShootRange; z <= maxShootRange; z++)
            {
                int circleDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (circleDistance > maxShootRange + 1) continue;

                GridPosition rangeGridPosition = new GridPosition(x, z) + unitGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(rangeGridPosition) || !LevelGrid.Instance.HasAnyUnitOnPosition(rangeGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(rangeGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositions.Add(rangeGridPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition position, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(position);
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShoot = true;
        ActionStart(onActionComplete);
    }

    void Shoot()
    {
        animator.SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletProjectilePrefab, gunPoint.position, Quaternion.identity);
        Vector3 shootPoint = targetUnit.GetWorldPosition();
        shootPoint.y = gunPoint.transform.position.y;
        bullet.GetComponent<Projectile>().Setup(shootPoint);
        targetUnit.Damage(shootDamage);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetTargetCountAtPosition(GridPosition position)
    {
        return GetValidActionGridPositionList(position).Count;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 5 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalize()) * 5),
        };
    }
}
