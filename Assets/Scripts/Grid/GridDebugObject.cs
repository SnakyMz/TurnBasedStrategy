using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    GridObject gridObject;
    TextMeshPro gridText;

    void Start()
    {
        gridText = GetComponentInChildren<TextMeshPro>();
    }

    public void SetDebugObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    void Update()
    {
        gridText.text = gridObject.ToString();
    }
}
