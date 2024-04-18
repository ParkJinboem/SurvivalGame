using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class QuickSlotcontroller : MonoBehaviour
{
    //퀵 슬롯들
    [SerializeField] private Slot[] quickSlots;
    //퀵 슬롯쿨타임
    [SerializeField] private Image[] coolTimeImage;
    //퀼슬롯의 부모 객체
    [SerializeField] private Transform parent;
    //선택된 퀵슬롯 0~7
    private int selectedSlot;

    //아이템이 위치할 손 끝
    [SerializeField] private Transform itemPos;
    //손에 든 아이템
    public static GameObject handItem;

    //쿨타임 내용
    [SerializeField] private float coolTime;
    private float currentCoolTime;
    private bool isCooltime;

    //애니메이션 등장 내용
    [SerializeField] private float appeartime;
    private float curretnAppeartime;
    private bool isAppear;

    //필요한 컴포넌트
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
        //선택된 슬롯으로 이미지 이동
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
        StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "맨손"));

        if(item != null)
        {
            StartCoroutine(HandItemCoroutine(item));
        }
    }
    IEnumerator HandItemCoroutine(Item item)
    {
        HandController.isActiviate = false;
        //무기교체의 마지막 함수가 실행될때까지 대기
        yield return new WaitUntil(() => HandController.isActiviate);
        if(item.itemType == Item.ItemType.Kit)
        {
            HandController.currentKit = item;
        }
        handItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, itemPos.position, itemPos.rotation);
        handItem.GetComponent<Rigidbody>().isKinematic = true;
        handItem.GetComponent<BoxCollider>().enabled = false;
        handItem.tag = "Untagged";
        handItem.layer = 9; //무기로 레이어 변경
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
