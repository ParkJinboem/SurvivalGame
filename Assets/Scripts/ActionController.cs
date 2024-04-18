using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ActionController : MonoBehaviour
{
    //습득 가능한 최대 거리
    [SerializeField] private float range;
    //아이템습득 가능할 시 true
    private bool pickActivated = false;
    //고기 해체 가능할 시 true
    private bool dissloveActivated = false;
    //고기 해체 중에는 true
    private bool isDissloving = false;
    //불을 근접해서 바라 볼시 true
    private bool fireLookActivated= false;
    //컴퓨터를 바라 볼시 true
    private bool lookComputer = false;

    //충돌체 정보 저장
    private RaycastHit hitInfo;
    //아이템 레이어에만 반응하도록 레리어마스크 설정
    [SerializeField] private LayerMask layerMask;
    //필요한 컴포넌트
    [SerializeField] private Text actionText;
    [SerializeField] private Inventory inventory;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private QuickSlotcontroller quickSlotcontroller;
    [SerializeField] private ComputerKit computerKit;
    //고기해체
    [SerializeField] private Transform meatDissolveTool;
    //고기해체 시 소리 재생
    [SerializeField] string soundMeat;

    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckAction();
            CanPickUp();
            CanMeat();
            CanDropFire();
            CanComputerPowerOn();
        }
    }

    private void CanPickUp()
    {
        if(pickActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다");
                inventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CanComputerPowerOn()
    {
        if (lookComputer)
        {
            if (hitInfo.transform != null)
            {
                if(!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)
                {
                    hitInfo.transform.GetComponent<ComputerKit>().PowerOn();
                    InfoDisappear();
                }

            }
        }
    }
    private void CanMeat()
    {
        if (dissloveActivated)
        {
            if ((hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal") && hitInfo.transform.GetComponent<Animal>().isDead && !isDissloving)
            {
                isDissloving = true;
                InfoDisappear();
                //고기 해체 실시
                StartCoroutine(MeatCoroutine());
            }
        }
    }

    private void CanDropFire()
    {
        if(fireLookActivated)
        {
            if(hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())
            {
                Slot selectedSlot = quickSlotcontroller.GetSelectedSlot();
                if(selectedSlot.item != null)
                {
                    DropAnItem(selectedSlot);
                }
            }
        }
    }

    private void DropAnItem(Slot selectedSlot)
    {
        switch(selectedSlot.item.itemType)
        {
            case Item.ItemType.Used:
                if(selectedSlot.item.itemName.Contains("고기"))
                {
                    Instantiate(selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);
                    quickSlotcontroller.DecreaseSelectedItem();
                }
                break;
            case Item.ItemType.Ingredient:
                break;
        }
    }

    private void CheckAction()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            else if(hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")
            {
                MeatInfoAppear();
            }
            else if (hitInfo.transform.tag == "Fire")
            {
                FireInfoAppear();
            }
            else if(hitInfo.transform.tag == "Computer")
            {
                ComputerInfoAppear();
            }
            else
            {
                InfoDisappear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void Reset()
    {
        pickActivated = false;
        dissloveActivated = false;
        fireLookActivated = false;
        lookComputer = false;
    }

    IEnumerator MeatCoroutine()
    {
        WeaponManager.isChangeWeapon = true;
        WeaponSway.isActivated = false;
        WeaponManager.currentWeaponAnim.SetTrigger("WeaponOut");
        PlayerController.isActivated = false;
        yield return new WaitForSeconds(0.2f);
        WeaponManager.currentWeapon.gameObject.SetActive(false);
        meatDissolveTool.gameObject.SetActive(true);
        SoundManager.instance.PlaySE(soundMeat);
        yield return new WaitForSeconds(1.8f);
        //고기 아이템 획득
        inventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().itemNumber);

        WeaponManager.currentWeapon.gameObject.SetActive(true);
        meatDissolveTool.gameObject.SetActive(false);
        PlayerController.isActivated = true;
        WeaponSway.isActivated = true;
        WeaponManager.isChangeWeapon = false;
        isDissloving = false;

    }

    private void ItemInfoAppear()
    {
        Reset();
        pickActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void MeatInfoAppear()
    {
        if(hitInfo.transform.GetComponent<Animal>().isDead)
        {
            Reset();
            dissloveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";
        }
    }
    private void FireInfoAppear()
    {
        Reset();
        fireLookActivated = true;


        if (hitInfo.transform.GetComponent<Fire>().GetIsFire())
        {
            actionText.gameObject.SetActive(true);
            actionText.text = "선택된 아이템 불에 넣기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void ComputerInfoAppear()
    {
        if(!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)
        {
            Reset();
            lookComputer = true;
            actionText.gameObject.SetActive(true);
            actionText.text = " 컴퓨터 가동 " + "<color=yellow>" + "(E)" + "</color>";
        }
    }
    private void InfoDisappear()
    {
        Reset();
        actionText.gameObject.SetActive(false);
    }   
}
