using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PreviewObject : MonoBehaviour
{
    public Building.Type needType;
    private bool needTypeFlag;

    //충돌한 오브젝트의 컬라이더
    private List<Collider> colliderList = new List<Collider>();
    //지상레이어(무시할레이어)
    [SerializeField] private int layerGround;
    private const int IGNORERAYCASTLAYER = 2;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (needType == Building.Type.Normal)
        {
            if (colliderList.Count > 0)
            {
                SetColor(red);
            }
            else
            {
                SetColor(green);
            }
        }
        else
        {
            if (colliderList.Count > 0 || !needTypeFlag)
            {
                SetColor(red);
            }
            else
            {
                SetColor(green);
            }
        }
    }

    private void SetColor(Material mat)
    {
        foreach(Transform child in this.transform)
        {
            var newMaterials = new Material[child.GetComponent<Renderer>().materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }
            child.GetComponent<Renderer>().materials = newMaterials;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Structure")
        {
            if(other.GetComponent<Building>().type != needType)
            {
                colliderList.Add(other);
            }
            else
            {
                needTypeFlag = true;
            }
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORERAYCASTLAYER)
                colliderList.Add(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Structure")
        {
            if (other.GetComponent<Building>().type != needType)
            {
                colliderList.Remove(other);
            }
            else
            {
                needTypeFlag = false;
            }
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORERAYCASTLAYER)
                colliderList.Remove(other);
        }
        
    }
    public bool isBuildable()
    {
        if(needType == Building.Type.Normal)
        {
            return colliderList.Count == 0;
        }
        else
        {
            return colliderList.Count == 0 && needTypeFlag;
        }
    }
}

