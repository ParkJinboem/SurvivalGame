using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerToolTip : MonoBehaviour
{
    [SerializeField] private GameObject baseUI;

    [SerializeField] private Text kintName;
    [SerializeField] private Text kitDes;
    [SerializeField] private Text kitNeedItem;

    public void ShowToolTip(string kitName, string kitDes, string[] needItem, int[] needItemNumber)
    {
        baseUI.SetActive(true);
        this.kintName.text = kitName;
        this.kitDes.text = kitDes;

        for (int i = 0; i < needItem.Length; i++)
        {
            kitNeedItem.text += needItem[i];
            kitNeedItem.text += " x " + needItemNumber[i].ToString() + "\n";
        }
    }

    public void HideToolTip()
    {
        baseUI.SetActive(false);
        kintName.text = "";
        kitDes.text = "";
        kitNeedItem.text = "";
    }
}
