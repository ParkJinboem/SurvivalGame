using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemEffect
{
    public string itemName; // ������ �̸�(Ű��)
    public string[] part; // ����
    public int[] num; // ��ġ
}
public class ItemEffectDatabase : MonoBehaviour
{
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    [SerializeField] private ItemEffect[] itemEffects;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� �����մϴ�.")]
    [SerializeField] private StatusController playerStatus;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private SlotToolTip slotToolTip;
    [SerializeField] private QuickSlotcontroller quickSlotcontroller;

    //������ ¡�˴ٸ�
    public void IsActivatedQuickSlot(int num)
    {
        quickSlotcontroller.IsActivatedQuickSlot(num);
    }

    //���� ���� ¡�˴ٸ�
    public void ShowToolTip(Item item, Vector3 pos)
    {
        slotToolTip.ShowToolTip(item, pos);
    }

    //���� ���� ¡�˴ٸ�
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
                                Debug.Log("�߸��� Status ������ �����Ű���� �ϰ� �ֽ��ϴ�.");
                                break;
                        }
                        Debug.Log($"{item.itemName}�� ����߽��ϴ�.");
                    }
                    return;
                }
            }
            Debug.Log("��ġ�ϴ� itemName �����ϴ�.");
        }
    }
}
