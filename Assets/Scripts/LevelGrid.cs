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

    public List<Unit> GetUnitListAtGridPosition(GridPosition position)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.RemoveUnit(unit);
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
}
