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

    //인벤토리슬롯들
    [SerializeField] private Slot[] slots;
    [SerializeField] private Slot[] quickSlots;
    private bool isNotPut;

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
        inventoryBase.SetActive(true);
    }


    private void CloseInventory()
    {
        inventoryBase.SetActive(false);
    }

    public void AcquireItem(Item item, int count = 1)
    {
        //퀵슬롯에 아이템을 먼저채움
        putSlot(quickSlots, item, count);
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
        if (Item.ItemType.Equipment != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == item.itemName)
                    {
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
}
