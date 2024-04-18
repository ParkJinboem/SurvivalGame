using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //필요한 컴포넌트
    [SerializeField] private GameObject inventoryBase;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject quickSlotParent;
    [SerializeField] private QuickSlotcontroller quickSlotController;

    //인벤토리슬롯들
    [SerializeField] private Slot[] slots;
    [SerializeField] private Slot[] quickSlots;
    private bool isNotPut;
    private int slotNumber;

    // Start is called before the first frame update
    void Start()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        quickSlots = quickSlotParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventoryBase.SetActive(false);
        
    }

    public void AcquireItem(Item item, int count = 1)
    {
        //퀵슬롯에 아이템을 먼저채움
        putSlot(quickSlots, item, count);
        if(!isNotPut)
        {
            quickSlotController.IsActivatedQuickSlot(slotNumber);
        }
        //퀵슬롯에 아이템이 다차면 인벤토리에 채움
        if (isNotPut == true)
        {
            putSlot(slots, item, count);
        }
        if (isNotPut == true)
        {
            Debug.Log("퀵슬롯과 인벤토리가 꽉 찻습니다.");
        }
    }

    private void putSlot(Slot[] slots, Item item, int count)
    {
        if (Item.ItemType.Equipment != item.itemType && Item.ItemType.Kit != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == item.itemName)
                    {
                        slotNumber = i;
                        slots[i].SetSlotCount(count);
                        isNotPut = false;
                        return;
                    }
                }

            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item, count);
                isNotPut = false;
                return;
            }
        }
        isNotPut = true;
    }

    public int GetItemCount(string itemName)
    {
        int temp = SearchSlotItem(slots, itemName);

        return temp != 0 ? temp : SearchSlotItem(quickSlots, itemName);
    }

    private int SearchSlotItem(Slot[] slots, string itemName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (itemName == slots[i].item.itemName)
                {
                    return slots[i].itemCount;
                }
            }
        }
        return 0;
    }

    public void SetItemCount(string itemName, int itemCount)
    {
        if(!ItemCountAdjust(slots, itemName, itemCount))
        {
            ItemCountAdjust(quickSlots, itemName, itemCount);
        }
    }

    private bool ItemCountAdjust(Slot[] slots, string itemName, int itemCount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (itemName == slots[i].item.itemName)
                {
                    slots[i].SetSlotCount(-itemCount);
                    return true;
                }
            }
        }

        return false;
    }
}
