using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    Unit unit;
    MeshRenderer meshRenderer;

    void Awake()
    {
        unit = GetComponentInParent<Unit>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += ToggleSelectedVisual;
        ToggleSelectedVisual(UnitActionSystem.Instance);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleSelectedVisual(UnitActionSystem unitActionSystem)
    {
        if (unit == unitActionSystem.GetSelectedUnit())
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
