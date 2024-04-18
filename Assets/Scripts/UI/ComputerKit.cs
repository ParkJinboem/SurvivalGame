using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kit
{
    public string kitName;
    public string kitDescription;
    public string[] needItemName;
    public int[] needItemNumber;
    public GameObject kitPrefab;
}

public class ComputerKit : MonoBehaviour
{
    [SerializeField] private Kit[] kits;
    //������ ������ ��ġ
    [SerializeField] private Transform itemAppear;
    [SerializeField] private GameObject baseUI;
    //�ߺ��������
    private bool isCraft = false;
    //���� �Ѡ�����
    public bool isPowerOn = false;
    //�ʿ��� ������Ʈ
    private Inventory inventory;
    [SerializeField] private ComputerToolTip toolTip;

    private AudioSource audio;
    [SerializeField] private AudioClip btnSound;
    [SerializeField] private AudioClip beepSound;
    [SerializeField] private AudioClip activatedSound;
    [SerializeField] private AudioClip outputSound;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventory = FindObjectOfType<Inventory>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isPowerOn)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PowerOff();
            }
        }
    }

    public void PowerOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPowerOn = true;
        baseUI.SetActive(true);
    }

    private void PowerOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPowerOn = false;
        toolTip.HideToolTip();
        baseUI.SetActive(false);
    }
    public void ShowToolTip(int btnNum)
    {
        toolTip.ShowToolTip(kits[btnNum].kitName, kits[btnNum].kitDescription, kits[btnNum].needItemName, kits[btnNum].needItemNumber);
    }
    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }

    private void PlaySE(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }

    public void ClickButton(int slotNumber)
    {
        PlaySE(btnSound);
        if (!isCraft)
        {
            //��� üũ
            if(!CheckIngredient(slotNumber))
            {
                return;
            }
            isCraft = true;
            //��� ���
            UseIngredient(slotNumber);
            
            //��� ����
            StartCoroutine(CraftCoroutine(slotNumber));
        }
    }

    private bool CheckIngredient(int slotNumber)
    {
        for (int i = 0; i < kits[slotNumber].needItemName.Length; i++)
        {
            if (inventory.GetItemCount(kits[slotNumber].needItemName[i]) < kits[slotNumber].needItemNumber[i])
            {
                PlaySE(beepSound);
                return false;
            }
        }
        return true;
    }
    private void UseIngredient(int slotNumber)
    {
        for (int i = 0; i < kits[slotNumber].needItemName.Length; i++)
        {
            inventory.SetItemCount(kits[slotNumber].needItemName[i], kits[slotNumber].needItemNumber[i]);
        }
    }

    IEnumerator CraftCoroutine(int slotNumber)
    {
        PlaySE(activatedSound);
        yield return new WaitForSeconds(3f);
        PlaySE(outputSound);
        Instantiate(kits[slotNumber].kitPrefab, itemAppear.position, Quaternion.identity);
        isCraft = false;

    }
}
