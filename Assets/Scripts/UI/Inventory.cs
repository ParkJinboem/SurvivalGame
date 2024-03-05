using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //�ʿ��� ������Ʈ
    [SerializeField] private GameObject inventoryBase;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject quickSlotParent;

    //�κ��丮���Ե�
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
        //�����Կ� �������� ����ä��
        putSlot(quickSlots, item, count);
        //�����Կ� �������� ������ �κ��丮�� ä��
        if (isNotPut == true)
        {
            putSlot(slots, item, count);
        }
        if (isNotPut == true)
        {
            Debug.Log("�����԰� �κ��丮�� �� �����ϴ�.");
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
