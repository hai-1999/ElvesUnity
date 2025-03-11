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

    public IEnumerator SetHpSmooth(float newHp)
    {
        float curHp = health.transform.localScale.x;//当前HP
        float changeAmt = curHp - newHp;// HP变化量
        while (curHp-newHp>Mathf.Epsilon)//HP变化量不为0
        {
            curHp -= changeAmt * Time.deltaTime;//当前Hp降低（一帧时间）
            health.transform.localScale = new Vector3(curHp, 1f);//Hp降低后设置

            yield return null;//暂停等下一帧
        }
        health.transform.localScale = new Vector3(newHp, 1f);

    }

}
