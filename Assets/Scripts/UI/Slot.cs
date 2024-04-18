using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler 
{
    //ȹ���� ������
    public Item item;
    //ȹ���� �������� ���� 
    public int itemCount;
    //������ �̹��� 
    public Image itemImage;

    //������ ����
    [SerializeField] private bool isQuickSlot;
    [SerializeField] private int quickSlotNumber;

    //�ʿ��� ������Ʈ
    [SerializeField] private Text textCount;
    [SerializeField] private GameObject countImage;
    private ItemEffectDatabase itemEffectDatabase;
    //�κ��丮 ����
    [SerializeField] private RectTransform baseRect;
    //������ ����
    [SerializeField] private RectTransform quickSlotBaseRect;
    private InputNumber inputNumber;
    
    void Start()
    {
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        inputNumber = FindObjectOfType<InputNumber>();
    }

    //�̹��� ���� ����
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    //������ ȹ��
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

    //������ ���� ����
    public void SetSlotCount(int count)
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }
 
    //���� �ʱ�ȭ
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
        //���� �����ۿ� ��Ŭ�� 
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
        //�κ��丮���� ������  ������ ���׹߻��Ͽ� ���������� ��ü_240304 ������
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
        //�κ��丮 ���� || ������ ����
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
        //    && DragSlot.instance.transform.localPosition.y > quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMax //������ �ִ밪
        //    && DragSlot.instance.transform.localPosition.y < quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMin))) //������ �ּҰ�
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
            //�κ��丮 -> ������ or ������ -> ������
            if(isQuickSlot)
            {
                itemEffectDatabase.IsActivatedQuickSlot(quickSlotNumber);
            }
            //�κ��丮 -> �κ��丮 or ������ -> �κ��丮
            else
            {
                //������ -> �κ��丮
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

    //���콺�� ���Կ� ���� �ߵ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            itemEffectDatabase.ShowToolTip(item, transform.position);
        }
    }
    //���콺�� ���Կ� ������ �ߵ�
    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffectDatabase.HideToolTip();
    }
}
