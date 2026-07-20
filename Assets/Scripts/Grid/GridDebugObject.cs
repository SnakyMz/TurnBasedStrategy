using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    GridObject gridObject;
    TMP_Text gridText;

    void Start()
    {
        gridText = GetComponentInChildren<TMP_Text>();
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
