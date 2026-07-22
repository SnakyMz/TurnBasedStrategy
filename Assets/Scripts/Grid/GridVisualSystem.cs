using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GridVisualTypeMaterial
{
    public GridVisualType gridVisualType;
    public Material material;
}

public enum GridVisualType
{
    White,
    Red,
    Blue,
    Yellow
}

public class GridVisualSystem : MonoBehaviour
{
    [SerializeField] GameObject gridVisualPrefab;
    [SerializeField] List<GridVisualTypeMaterial> gridVisualTypeMaterials;

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

    void HideAllGridVisuals()
    {
        if (gridVisualArray.Length <= 0) return;
        foreach (GridVisual grid in gridVisualArray)
        {
            grid.Hide();
        }
    }

    void ShowGridVisuals(List<GridPosition> gridPositionList, GridVisualType type)
    {
        foreach (GridPosition position in gridPositionList)
        {

            gridVisualArray[position.x, position.z].Show(GetGridVisualMaterial(type));
        }
    }

    void UpdateGridVisuals()
    {
        BaseAction action = UnitActionSystem.Instance.GetSelectedAction();

        HideAllGridVisuals();

        GridVisualType gridVisualType;

        switch (action)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                break;
        }

        ShowGridVisuals(action.GetValidActionGridPositionList(), gridVisualType);
    }

    Material GetGridVisualMaterial(GridVisualType type)
    {
        foreach (GridVisualTypeMaterial materialType in gridVisualTypeMaterials)
        {
            if (materialType.gridVisualType == type)
            {
                return materialType.material;
            }
        }

        return null;
    }
}
