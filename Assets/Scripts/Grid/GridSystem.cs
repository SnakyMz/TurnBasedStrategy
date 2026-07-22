using UnityEngine;

public class GridSystem
{
    int width;
    int length;
    float cellsize;

    GridObject[,] gridObjectArray;

    public GridSystem(int width, int length, float cellsize)
    {
        this.width = width;
        this.length = length;
        this.cellsize = cellsize;

        gridObjectArray = new GridObject[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition position)
    {
        return new Vector3(position.x, 0f, position.z) * cellsize;
    }

    public GridPosition GetGridPosition(Vector3 position)
    {
        return new GridPosition(
            Mathf.RoundToInt(position.x / cellsize),
            Mathf.RoundToInt(position.z / cellsize)
        );
    }

    public void CreateDebugPrefabs(GameObject debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject debugObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObject.GetComponent<GridDebugObject>();
                gridDebugObject.SetDebugObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition position)
    {
        return gridObjectArray[position.x, position.z];
    }

    public bool IsValidGridPosition(GridPosition position)
    {
        return position.x >= 0 && position.z >= 0 && position.x < width && position.z < length;
    }

    public int GetGridWidth()
    {
        return width;
    }

    public int GetGridLenght()
    {
        return length;
    }
}
