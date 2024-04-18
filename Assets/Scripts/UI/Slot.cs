using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler 
{
    //획득한 아이템
    public Item item;
    //획득한 아이템의 갯수 
    public int itemCount;
    //아이템 이미지 
    public Image itemImage;

    //퀵슬롯 정보
    [SerializeField] private bool isQuickSlot;
    [SerializeField] private int quickSlotNumber;

    //필요한 컴포넌트
    [SerializeField] private Text textCount;
    [SerializeField] private GameObject countImage;
    private ItemEffectDatabase itemEffectDatabase;
    //인벤토리 영역
    [SerializeField] private RectTransform baseRect;
    //퀵슬롯 영역
    [SerializeField] private RectTransform quickSlotBaseRect;
    private InputNumber inputNumber;
    
    void Start()
    {
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        inputNumber = FindObjectOfType<InputNumber>();
    }

    //이미지 투명도 조절
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    //아이템 획득
    public void AddItem(Item item, int count = 1)
    {
        this.item = item;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if(this.item.itemType != Item.ItemType.Equipment)
        {
            countImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else
        {
            textCount.text = "0";
            countImage.SetActive(false);
        }
        SetColor(1);
    }

    public int GetQuickSlotNumber()
    {
        return quickSlotNumber;
    }

    //아이템 개수 조정
    public void SetSlotCount(int count)
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }
 
    //슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        textCount.text = "0";
        countImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //슬롯 아이템에 우클릭 
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                itemEffectDatabase.UseItem(item);
                if(item.itemType == Item.ItemType.Used)
                {
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
        //인벤토리끄고 아이템  버릴시 버그발생하여 위에것으로 대체_240304 박진범
        //if (item != null && Inventory.inventoryActivated)
        //{
        //    DragSlot.instance.dragSlot = this;
        //    DragSlot.instance.DragSetImage(itemImage);
        //    DragSlot.instance.transform.position = eventData.position;
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //인벤토리 영역 || 퀵슬롯 영역
        if (!((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin
            && DragSlot.instance.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin
            && DragSlot.instance.transform.localPosition.y < baseRect.rect.yMax)
        ||
        (DragSlot.instance.transform.localPosition.x > quickSlotBaseRect.rect.xMin
        && DragSlot.instance.transform.localPosition.x < quickSlotBaseRect.rect.xMax
        && DragSlot.instance.transform.localPosition.y + baseRect.localPosition.y > quickSlotBaseRect.rect.yMin + quickSlotBaseRect.transform.localPosition.y
        && DragSlot.instance.transform.localPosition.y + baseRect.localPosition.y < quickSlotBaseRect.rect.yMax + quickSlotBaseRect.transform.localPosition.y)))
        //if(!((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin 
        //    && DragSlot.instance.transform.localPosition.x < baseRect.rect.xMax
        //    && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin
        //    && DragSlot.instance.transform.localPosition.y < baseRect.rect.yMax)
        //    ||
        //    (DragSlot.instance.transform.localPosition.x > quickSlotBaseRect.rect.xMin
        //    && DragSlot.instance.transform.localPosition.x < quickSlotBaseRect.rect.xMax
        //    && DragSlot.instance.transform.localPosition.y > quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMax //퀵슬롯 최대값
        //    && DragSlot.instance.transform.localPosition.y < quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMin))) //퀵슬롯 최소값
        {
            if (DragSlot.instance.transform != null)
            {
                inputNumber.Call();
            }           
        }
        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }     
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
            //인벤토리 -> 퀵슬롯 or 퀵슬롯 -> 퀵슬롯
            if(isQuickSlot)
            {
                itemEffectDatabase.IsActivatedQuickSlot(quickSlotNumber);
            }
            //인벤토리 -> 인벤토리 or 퀵슬롯 -> 인벤토리
            else
            {
                //퀵슬롯 -> 인벤토리
                if(DragSlot.instance.dragSlot.isQuickSlot)
                {
                    itemEffectDatabase.IsActivatedQuickSlot(DragSlot.instance.dragSlot.quickSlotNumber);
                } 
            }
        }
    }

    private void ChangeSlot()
    {
        Item tempItem = item;
        int tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    //마우스가 슬롯에 들어갈때 발동
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            itemEffectDatabase.ShowToolTip(item, transform.position);
        }
    }
    //마우스가 슬롯에 나갈때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffectDatabase.HideToolTip();
    }
}
