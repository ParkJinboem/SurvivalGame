using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    //������ �̸�
    public string itemName;
    [TextArea]
    public string itemDesc;
    //������ Ÿ��
    public ItemType itemType;
    //������ �̹���
    public Sprite itemImage;
    //������ ������
    public GameObject itemPrefab;
    //ŰƮ ������
    public GameObject kitPrefab;
    //ŰƮ ������������
    public GameObject kitPreviewPrefab;
    // ���� ����
    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        Kit,
        ETC,
    }
}
