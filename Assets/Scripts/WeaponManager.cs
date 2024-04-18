using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;
    //���繫��
    public static Transform currentWeapon;
    //���繫�� �ִϸ��̼�
    public static Animator currentWeaponAnim;
    //���� ������ Ÿ��
    [SerializeField] private string currentWeaponType;

    //���� ��ü ������
    [SerializeField] private float changeWeaponDelayTime;
    [SerializeField] private float changeWeaponEndDelayTime;

    [SerializeField] Gun[] guns;
    [SerializeField] CloseWeapon[] hands;
    [SerializeField] CloseWeapon[] axes;
    [SerializeField] CloseWeapon[] pickaxes;

    //���� ���� ��ų���
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    //�ʿ� ������Ʈ
    [SerializeField] private GunController guncontroller;
    [SerializeField] private HandController handcontroller;
    [SerializeField] private AxeController axecontroller;
    [SerializeField] private PickaxeController pickaxecontroller;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }
  
    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("WeaponOut");
        yield return new WaitForSeconds(changeWeaponDelayTime);
        CancelPreWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                guncontroller.CancelFineSight();
                guncontroller.CancelReload();
                GunController.isActiviate = false;
                break;
            case "HAND":
                HandController.isActiviate = false;
                if(HandController.currentKit != null)
                {
                    handcontroller.Cancel();
                }
                if(QuickSlotcontroller.handItem != null)
                {
                    Destroy(QuickSlotcontroller.handItem);
                }
                break;
            case "AXE":
                AxeController.isActiviate = false;
                break;
            case "PICKAXE":
                PickaxeController.isActiviate = false;
                break;
        }
    }

    private void WeaponChange(string type, string name)
    {
        if(type == "GUN")
        {
            guncontroller.GunChange(gunDictionary[name]);
        }
        else if(type =="HAND")
        {
            handcontroller.CloseWeaponChange(handDictionary[name]);
        }
        else if (type == "AXE")
        {
            axecontroller.CloseWeaponChange(axeDictionary[name]);
        }
        else if (type == "PICKAXE")
        {
            pickaxecontroller.CloseWeaponChange(pickaxeDictionary[name]);
        }
    }
}
