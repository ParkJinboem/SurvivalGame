using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    //아이템 이름
    public string itemName;
    [TextArea]
    public string itemDesc;
    //아이템 타입
    public ItemType itemType;
    //아이템 이미지
    public Sprite itemImage;
    //아이템 프리팹
    public GameObject itemPrefab;
    // 무기 유형
    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }
}
