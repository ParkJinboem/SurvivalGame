using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;
    [SerializeField] private Text privewText;
    [SerializeField] private Text inputText;
    [SerializeField] private InputField ifText;
    [SerializeField] private GameObject baseObj;
    [SerializeField] private ActionController player;

    private void Update()
    {
        if(activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OK();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }
    }
    public void Call()
    {
        baseObj.SetActive(true);
        activated = true;
        ifText.text = "";
        privewText.text = DragSlot.instance.dragSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        baseObj.SetActive(false);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        int number = 0;
        DragSlot.instance.SetColor(0);
        if (inputText.text != "")
        {
            if(CheckNumber(inputText.text))
            {
                number = int.Parse(inputText.text);
                if(number > DragSlot.instance.dragSlot.itemCount)
                {
                    number = DragSlot.instance.dragSlot.itemCount;
                }
                else
                {
                    number = 1;
                }
            }
        }
        else
        {
            number = int.Parse(privewText.text);
        }
        StartCoroutine(DropItemCoroutine(number));
    }

    IEnumerator DropItemCoroutine(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if(DragSlot.instance.dragSlot.item.itemPrefab != null)
            {
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, player.transform.position + player.transform.forward, Quaternion.identity);
            }
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f);
        }      

        //현재 아이템을 들고있고, 아이템의 모든 개수를 버리면 아이템 파괴
        if(int.Parse(privewText.text) == num)
        {
            if(QuickSlotcontroller.handItem != null)
            {
                Destroy(QuickSlotcontroller.handItem);
            }
        }
        DragSlot.instance.dragSlot = null;
        baseObj.SetActive(false);
        activated = false;
    }

    private bool CheckNumber(string argString)
    {
        char[] tempCharArray = argString.ToCharArray();
        bool isNumber = true;
        for (int i = 0; i < tempCharArray.Length; i++)
        {
            //유니코드 사용
            if (tempCharArray[i] >= 48 && tempCharArray[i] <= 57)
            {
                continue;
            }
            isNumber = false;
        }
        return isNumber;
    }
}
