using System.Collections.Generic;

public class GridObject
{
    GridSystem gridSystem;
    GridPosition gridPosition;
    Unit unit = null;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString() + "\n" + unit;
    }

    public void AddUnit(Unit unit)
    {
        this.unit = unit;
    }

    public void ClearUnit()
    {
        unit = null;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public bool HasAnyUnit()
    {
        return unit != null;
    }
}
