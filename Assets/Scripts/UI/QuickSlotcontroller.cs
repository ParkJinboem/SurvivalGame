using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class QuickSlotcontroller : MonoBehaviour
{
    //�� ���Ե�
    [SerializeField] private Slot[] quickSlots;
    //�� ������Ÿ��
    [SerializeField] private Image[] coolTimeImage;
    //�������� �θ� ��ü
    [SerializeField] private Transform parent;
    //���õ� ������ 0~7
    private int selectedSlot;

    //�������� ��ġ�� �� ��
    [SerializeField] private Transform itemPos;
    //�տ� �� ������
    public static GameObject handItem;

    //��Ÿ�� ����
    [SerializeField] private float coolTime;
    private float currentCoolTime;
    private bool isCooltime;

    //�ִϸ��̼� ���� ����
    [SerializeField] private float appeartime;
    private float curretnAppeartime;
    private bool isAppear;

    //�ʿ��� ������Ʈ
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private WeaponManager weaponManager;
    private Animator anim;

    void Start()
    {
        quickSlots = parent.GetComponentsInChildren<Slot>();
        anim = GetComponent<Animator>();
        selectedSlot = 0;
    }

    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
        AppearCalc();
    }
    private void AppearReset()
    { 
        curretnAppeartime = appeartime;
        isAppear = true;
        anim.SetBool("Appear", isAppear);

    }
    private void AppearCalc()
    {
        if(Inventory.inventoryActivated)
        {
            AppearReset();
        }
        else
        {
            if (isAppear)
            {
                curretnAppeartime -= Time.deltaTime;
                if (curretnAppeartime <= 0)
                {
                    isAppear = false;
                    anim.SetBool("Appear", isAppear);
                }
            }
        }
      
    }
    private void CoolTimeReset()
    {
        currentCoolTime = coolTime;
        isCooltime = true;
    }

    private void CoolTimeCalc()
    {
        if(isCooltime)
        {
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < coolTimeImage.Length; i++)
            {
                coolTimeImage[i].fillAmount = currentCoolTime / coolTime;
            }
            if(currentCoolTime <= 0)
            {
                isCooltime = false;
            }
        }
    }

    private void TryInputNumber()
    {
        if(!isCooltime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeSlot(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeSlot(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeSlot(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeSlot(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ChangeSlot(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ChangeSlot(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ChangeSlot(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ChangeSlot(7);
            }
        }
        
    }

    public void IsActivatedQuickSlot(int num)
    {
        if(selectedSlot == num)
        {
            Execute();
            return;
        }
        if(DragSlot.instance != null)
        {
            if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
            {
                Execute();
                return;
            }
        }
    }

    private void ChangeSlot(int num)
    {
        SelectedSlot(num);
        Execute();
    }

    private void SelectedSlot(int num)
    {
        selectedSlot = num;
        //���õ� �������� �̹��� �̵�
        selectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void Execute()
    {
        CoolTimeReset();
        AppearReset();
        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
            {
                StartCoroutine(weaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            }
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used || quickSlots[selectedSlot].item.itemType == Item.ItemType.Kit)
            {
                ChangeHand(quickSlots[selectedSlot].item);
            }
            else
            {
                ChangeHand();
            }
        }
        else
        {
            ChangeHand();
        }
    }

    private void ChangeHand(Item item = null)
    {
        StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "�Ǽ�"));

        if(item != null)
        {
            StartCoroutine(HandItemCoroutine(item));
        }
    }
    IEnumerator HandItemCoroutine(Item item)
    {
        HandController.isActiviate = false;
        //���ⱳü�� ������ �Լ��� ����ɶ����� ���
        yield return new WaitUntil(() => HandController.isActiviate);
        if(item.itemType == Item.ItemType.Kit)
        {
            HandController.currentKit = item;
        }
        handItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, itemPos.position, itemPos.rotation);
        handItem.GetComponent<Rigidbody>().isKinematic = true;
        handItem.GetComponent<BoxCollider>().enabled = false;
        handItem.tag = "Untagged";
        handItem.layer = 9; //����� ���̾� ����
        handItem.transform.SetParent(itemPos);
    }

    public void DecreaseSelectedItem()
    {
        CoolTimeReset();
        AppearReset();
        quickSlots[selectedSlot].SetSlotCount(-1);
        if(quickSlots[selectedSlot].itemCount <= 0)
        {
            Destroy(handItem);
        }
    }

    public bool GetIsCoolTime()
    {
        return isCooltime;
    }

    public Slot GetSelectedSlot()
    {
        return quickSlots[selectedSlot];
    }
}
