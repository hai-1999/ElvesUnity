using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    //private void Start()
    //{
    //    health.transform.localScale = new Vector3(0.5f, 1f);
    //}

    //����Ѫ���仯
    public void SetHp(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f, 1f);//
    }
}
