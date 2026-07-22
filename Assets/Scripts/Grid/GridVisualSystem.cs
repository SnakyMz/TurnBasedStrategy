using System.Collections.Generic;
using UnityEngine;

public class GridVisualSystem : MonoBehaviour
{
    [SerializeField] GameObject gridVisualPrefab;

    GridVisual[,] gridVisualArray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int width = LevelGrid.Instance.GetGridWidth();
        int length = LevelGrid.Instance.GetGridLenght();

        gridVisualArray = new GridVisual[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject grid = Instantiate(gridVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridVisualArray[x, z] = grid.GetComponent<GridVisual>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGridVisuals();
    }

    public void HideAllGridVisuals()
    {
        foreach (GridVisual grid in gridVisualArray)
        {
            grid.Hide();
        }
    }

    public void ShowGridVisuals(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition position in gridPositionList)
        {
            gridVisualArray[position.x, position.z].Show();
        }
    }

    void UpdateGridVisuals()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        HideAllGridVisuals();
        ShowGridVisuals(unit.GetMoveAction().GetValidActionGridPositionList());
    }
}
