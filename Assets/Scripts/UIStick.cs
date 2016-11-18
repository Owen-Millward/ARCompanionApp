using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler{

    private Image baseImg;
    private Image joystickImg;

    public Vector3 input;

    private void Start(){

        baseImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnPointerDown(PointerEventData ped){

        OnDrag(ped);
    }

    public virtual void OnDrag(PointerEventData ped){

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseImg.rectTransform, ped.position, ped.pressEventCamera, out pos)){

            pos.x /= baseImg.rectTransform.sizeDelta.x;
            pos.y /= baseImg.rectTransform.sizeDelta.y;

            // Create input vector
            input = new Vector3(pos.x * 2, 0, pos.y * 2);
            if (input.magnitude > 1.0f) {
                input.Normalize();
            }

            // Move joystick image
            joystickImg.rectTransform.anchoredPosition = new Vector2(input.x * (baseImg.rectTransform.sizeDelta.x / 4),
                                                                     input.z * (baseImg.rectTransform.sizeDelta.y / 4));
        }
    }

    public virtual void OnPointerUp(PointerEventData ped){

        input = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

   public float getHorizontal(){

        input.x = Mathf.Round(input.x * 100f) / 100f;
        return input.x;
    }

    public float getVertical(){

        input.z = Mathf.Round(input.z * 100f) / 100f;
        return input.z;
    }

}