using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event Action OnSelectedUnitChanged;

    [SerializeField] GameObject actionButtonPrefab;
    [SerializeField] TextMeshProUGUI actionPointsUI;
    [SerializeField] Transform actionPanel;
    [SerializeField] GameObject busyPanel;
    [SerializeField] Unit selectedUnit;
    [SerializeField] LayerMask unitLayerMask;

    BaseAction selectedAction;

    bool isBusy = false;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one unit action system" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        busyPanel.SetActive(false);
        SetSelectedUnit(selectedUnit);
        UpdateActionPointsUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBusy) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleUnitAction();
    }

    bool TryHandleUnitSelection()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit) return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    void HandleUnitAction()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidGridPosition(mouseGridPosition)) return;

            if (!selectedUnit.TryTakingAction(selectedAction)) return;

            UpdateActionPointsUI();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            SetBusy();
        }
    }

    void UpdateActionPointsUI()
    {
        actionPointsUI.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

    void CreateActionPanel()
    {
        foreach (Transform child in actionPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (BaseAction action in selectedUnit.GetActions())
        {
            GameObject actionButton = Instantiate(actionButtonPrefab, actionPanel);
            actionButton.GetComponent<ActionButton>().SetButton(action);
        }
    }

    void UpdateActionPanel()
    {
        foreach (Transform button in actionPanel)
        {
            button.GetComponent<ActionButton>().UpdateSelectedButton();
        }
    }

    public void SetBusy()
    {
        isBusy = true;
        busyPanel.SetActive(true);
    }

    public void ClearBusy()
    {
        isBusy = false;
        busyPanel.SetActive(false);
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        UpdateActionPointsUI();
        CreateActionPanel();
        SetSelectedAction(selectedUnit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke();
    }

    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
        UpdateActionPanel();
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
