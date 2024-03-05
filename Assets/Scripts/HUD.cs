using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //필요한 컴포넌트
    [SerializeField] private GunController guncontroller;
    private Gun currentGun;

    //UI HUD
    [SerializeField] private GameObject bulletHUD;
    //총알 개수 UI
    [SerializeField] private Text[] textBullet;

    private void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = guncontroller.GetGun();
        textBullet[0].text = currentGun.carryBulletCount.ToString();
        textBullet[1].text = currentGun.reloadBulletCount.ToString();
        textBullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
