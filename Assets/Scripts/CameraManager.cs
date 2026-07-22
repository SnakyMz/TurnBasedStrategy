using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineCamera actionCamera;
    [SerializeField] CinemachineTargetGroup targetGroup;

    Unit actionUnit;
    Unit targetUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionCamera.gameObject.SetActive(false);
        BaseAction.OnActionStart += HandleCameraAction;
        BaseAction.OnActionEnd += HideActionCamera;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowActionCamera()
    {
        targetGroup.AddMember(actionUnit.transform, 1, 0.5f);
        targetGroup.AddMember(targetUnit.transform, 0.5f, 0.5f);
        actionCamera.gameObject.SetActive(true);
    }

    void HideActionCamera()
    {
        if (targetGroup.IsEmpty) return;
        targetGroup.RemoveMember(actionUnit.transform);
        targetGroup.RemoveMember(targetUnit.transform);
        actionCamera.gameObject.SetActive(false);
    }

    void HandleCameraAction(BaseAction action)
    {
        switch (action)
        {
            case ShootAction shootAction:
                actionUnit = shootAction.GetUnit();
                targetUnit = shootAction.GetTargetUnit();
                ShowActionCamera();
                break;
        }
    }

    void OnDestroy()
    {
        BaseAction.OnActionStart -= HandleCameraAction;
        BaseAction.OnActionEnd -= HideActionCamera;
    }
}
