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

    //控制血条变化
    public void SetHp(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f, 1f);//
    }
}
