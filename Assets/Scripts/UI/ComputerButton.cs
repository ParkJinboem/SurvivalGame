using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ComputerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private ComputerKit computerKit;
    [SerializeField] private int buttonNum;
    public void OnPointerEnter(PointerEventData eventData)
    {
        computerKit.ShowToolTip(buttonNum);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        computerKit.HideToolTip();
    }


}
