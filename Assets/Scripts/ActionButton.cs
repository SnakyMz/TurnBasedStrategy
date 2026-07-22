using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject selectedImage;

    BaseAction buttonAction;

    public void SetButton(BaseAction action)
    {
        buttonAction = action;
        buttonText.text = action.GetActionName();

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });
    }

    public void UpdateSelectedButton()
    {
        selectedImage.SetActive(buttonAction == UnitActionSystem.Instance.GetSelectedAction());
    }
}
