using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField] private GameObject baseObj;

    [SerializeField] Text itemName;
    [SerializeField] Text itemDesc;
    [SerializeField] Text itemHowtoUsed;

    public void ShowToolTip(Item item, Vector3 pos)
    {
        baseObj.SetActive(true);
        Debug.Log(pos);
        pos += new Vector3(baseObj.GetComponent<RectTransform>().rect.width * 0.5f, -baseObj.GetComponent<RectTransform>().rect.height * 0.5f, 0);
        baseObj.transform.position = pos;
        this.itemName.text = item.itemName;
        this.itemDesc.text = item.itemDesc;
        if(item.itemType == Item.ItemType.Equipment)
        {
            this.itemHowtoUsed.text = "우클릭 - 장착";
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            this.itemHowtoUsed.text = "우클릭 - 먹기";
        }
        else
        {
            this.itemHowtoUsed.text = "";
        }
    }

    public void HideToolTip()
    {
        baseObj.SetActive(false);
    }
}
