using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemEffect
{
    public string itemName; // 아이템 이름(키값)
    public string[] part; // 부위
    public int[] num; // 수치
}
public class ItemEffectDatabase : MonoBehaviour
{
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    [SerializeField] private ItemEffect[] itemEffects;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.")]
    [SerializeField] private StatusController playerStatus;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private SlotToolTip slotToolTip;
    [SerializeField] private QuickSlotcontroller quickSlotcontroller;

    //퀵슬롯 징검다리
    public void IsActivatedQuickSlot(int num)
    {
        quickSlotcontroller.IsActivatedQuickSlot(num);
    }

    //슬롯 툴팁 징검다리
    public void ShowToolTip(Item item, Vector3 pos)
    {
        slotToolTip.ShowToolTip(item, pos);
    }

    //슬롯 툴팁 징검다리
    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }

    public void UseItem(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            for(int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case HP:
                                playerStatus.IncreaseHP(itemEffects[i].num[j]);
                                break;
                            case SP:
                                playerStatus.IncreaseSP(itemEffects[i].num[j]);
                                break;
                            case DP:
                                playerStatus.IncreaseDP(itemEffects[i].num[j]);
                                break;
                            case HUNGRY:
                                playerStatus.IncreaseHungry(itemEffects[i].num[j]);
                                break;
                            case THIRSTY:
                                playerStatus.IncreaseThirsty(itemEffects[i].num[j]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위를 적용시키려고 하고 있습니다.");
                                break;
                        }
                        Debug.Log($"{item.itemName}을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("일치하는 itemName 없습니다.");
        }
    }
}
