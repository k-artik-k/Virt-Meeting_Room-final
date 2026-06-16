using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class VRInputFieldFocus : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_InputField field = GetComponent<TMP_InputField>();
        field.ActivateInputField();
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}