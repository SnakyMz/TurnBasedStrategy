using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    [SerializeField] GameObject gridDebugPrefab;

    GridSystem gridSystem;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one level Grid system" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugPrefabs(gridDebugPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.AddUnit(unit);
    }

    public Unit GetUnitAtGridPosition(GridPosition position)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        return gridObject.GetUnit();
    }

    public void RemoveUnitAtGridPosition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.ClearUnit();
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromPosition, GridPosition toPosition)
    {
        RemoveUnitAtGridPosition(fromPosition, unit);
        AddUnitAtGridPosition(toPosition, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public bool IsValidGridPosition(GridPosition position)
    {
        return gridSystem.IsValidGridPosition(position);
    }

    public bool HasAnyUnitOnPosition(GridPosition position)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        return gridObject.HasAnyUnit();
    }

    public int GetGridWidth()
    {
        return gridSystem.GetGridWidth();
    }

    public int GetGridLenght()
    {
        return gridSystem.GetGridLenght();
    }
}
