using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    //풀 체력
    [SerializeField] private int hp;
    //파괴 대기 시간
    [SerializeField] private float destoryTime;
    //폭발력 세기
    [SerializeField] private float force;

    // 타격효과
    [SerializeField] GameObject effectPrefab;

    [SerializeField] Item itemLeaf;
    [SerializeField] int leafCount;
    private Inventory inventory;

    [SerializeField] Rigidbody[] rigidbodys;
    [SerializeField] BoxCollider[] boxColiiders;

    [SerializeField] private string hitSound;

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        rigidbodys = this.transform.GetComponentsInChildren<Rigidbody>();
        boxColiiders = this.transform.GetComponentsInChildren<BoxCollider>();
    }

    public void Damage()
    {
        hp--;
        Hit();
        if(hp <= 0)
        {
            Destrucition();
        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hitSound);

        GameObject clone = Instantiate(effectPrefab, transform.position + Vector3.up, Quaternion.identity);

        Destroy(clone, destoryTime);
    }

    private void Destrucition()
    {
        inventory.AcquireItem(itemLeaf, leafCount);
        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].useGravity = true;
            rigidbodys[i].AddExplosionForce(force, transform.position, 1f);
            boxColiiders[i].enabled = true;
        }

        Destroy(this.gameObject, destoryTime);
    }
}
