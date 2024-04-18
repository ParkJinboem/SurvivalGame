using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    // 이름
    public string craftName;
    public Sprite craftImage;
    public string crfatDesc;
    //필요한 아이템
    public string[] craftNeedItem;
    //필요한 아이템 갯수
    public int[] craftNeedItemCount;
    //실제 설치될 프리팹
    public GameObject prefab;
    //미리보기 프리팹
    public GameObject previewPrefab;
}

public class CraftManual : MonoBehaviour
{
    //상태변수
    private bool isActivated = false;
    private bool isPreviewActivated = false;
    //기본 베이스 UI
    [SerializeField] private GameObject baseUI;

    private int tabNumber = 0;
    private int page = 1;
    private int selectedSlotNumber;
    private Craft[] craft_SelectedTab;


    //모닥불용 탭
    [SerializeField] private Craft[] craftFire;
    //건축용 탭
    [SerializeField] private Craft[] craftBuild;
    //미리보기 프리팹을 담을 변수
    private GameObject preViewObj;
    //실제 생설된 오브젝트
    private GameObject prefabObj;
    //플레이어 위치
    [SerializeField] private Transform player;

    //RayCast 필요 선언
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    //필요한 UI Slot요소
    [SerializeField] private GameObject[] slotsObj;
    [SerializeField] private Image[] imageSlot;
    [SerializeField] private Text[] textSlotName;
    [SerializeField] private Text[] textSlotDesc;
    [SerializeField] private Text[] textSlotNeedItem;

    //필요한 컴포넌트
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        tabNumber = 0;
        page = 1;
        TabSlotSetting(craftFire);
    }

    public void SlotClick(int slotNubmer)
    {
        selectedSlotNumber = slotNubmer + (page - 1) * slotsObj.Length;

        if(!CheckIngredient())
        {
            return;
        }

        preViewObj = Instantiate(craft_SelectedTab[selectedSlotNumber].previewPrefab, player.position + player.forward, Quaternion.identity);
        prefabObj = craft_SelectedTab[selectedSlotNumber].prefab;
        isPreviewActivated = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        baseUI.SetActive(false);
    }

    private bool CheckIngredient()
    {
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            if (inventory.GetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i]) < craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i])
            {
                return false;
            }
        }

        return true;
    }

    private void UseIngredient()
    {
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            inventory.SetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i], craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i]);
        }
    }

    public void TabSetting(int tabNumber)
    {
        this.tabNumber = tabNumber;
        page = 1;

        switch(tabNumber)
        {
            case 0:
                TabSlotSetting(craftFire);
                break;
            case 1:
                TabSlotSetting(craftBuild);
                break;
        }
    }

    private void ClearSlot()
    {
        for (int i = 0; i < slotsObj.Length; i++)
        {
            imageSlot[i].sprite = null;
            textSlotName[i].text = "";
            textSlotDesc[i].text = "";
            textSlotNeedItem[i].text = "";
            slotsObj[i].SetActive(false);
        }
    }
    
    public void RightPageSetting()
    {
        if(page < (craft_SelectedTab.Length / slotsObj.Length) + 1)
        {
            page++;
        }
        else
        {
            page = 1;
        }
        TabSlotSetting(craft_SelectedTab);
    }

    public void LeftPageSetting()
    {
        if (page != 1)
        {
            page--;
        }
        else
        {
            page = (craft_SelectedTab.Length / slotsObj.Length) + 1;
        }
        TabSlotSetting(craft_SelectedTab);
    }

    private void TabSlotSetting(Craft[] craftTab)
    {
        ClearSlot();
        craft_SelectedTab = craftTab;

        int starSlotNumber = (page - 1) * slotsObj.Length; //4의 배수로 늘어남

        for (int i = starSlotNumber; i < craft_SelectedTab.Length; i++)
        {
            if(i == page * slotsObj.Length)
            {
                break;
            }
            slotsObj[i - starSlotNumber].SetActive(true);

            imageSlot[i - starSlotNumber].sprite = craftTab[i].craftImage;
            textSlotName[i - starSlotNumber].text = craftTab[i].craftName;
            textSlotDesc[i - starSlotNumber].text = craftTab[i].crfatDesc;

            for (int j = 0; j < craft_SelectedTab[i].craftNeedItem.Length; j++)
            {
                textSlotNeedItem[i - starSlotNumber].text += craft_SelectedTab[i].craftNeedItem[j];
                textSlotNeedItem[i - starSlotNumber].text += "x" + craft_SelectedTab[i].craftNeedItemCount[j] + "\n";
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if(isPreviewActivated)
        {
            PreViewPositionUpdate();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if(isPreviewActivated && preViewObj.GetComponent<PreviewObject>().isBuildable())
        {
            UseIngredient();
            Instantiate(prefabObj, preViewObj.transform.position, preViewObj.transform.rotation);
            Destroy(preViewObj);
            isActivated = false;
            isPreviewActivated = false;
            preViewObj = null;
            prefabObj = null;
        }
    }

    private void PreViewPositionUpdate()
    {
        if(Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    preViewObj.transform.Rotate(0, -90f, 0f);
                }
                else if(Input.GetKeyDown(KeyCode.E))
                {
                    preViewObj.transform.Rotate(0, +90f, 0f);
                }
                location.Set(Mathf.Round(location.x), Mathf.Round(location.y / 0.1f) * 0.1f, Mathf.Round(location.z));
                preViewObj.transform.position = location;
            }
        }
    }
    private void Cancel()
    {
        if(isPreviewActivated)
        {
            Destroy(preViewObj);
        }

        isActivated = false;
        isPreviewActivated = false;
        preViewObj = null;
        prefabObj = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        baseUI.SetActive(false);
    }

    private void Window()
    {
        if(!isActivated)
        {
            OpenWindow();
        }
        else
        {
            closeWindow();
        }
    }
    private void OpenWindow()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isActivated = true;
        baseUI.SetActive(true);
    }

    private void closeWindow()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isActivated = false;
        baseUI.SetActive(false);
    }
}
