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

    public IEnumerator SetHpSmooth(float newHp)
    {
        float curHp = health.transform.localScale.x;//��ǰHP
        float changeAmt = curHp - newHp;// HP�仯��
        while (curHp-newHp>Mathf.Epsilon)//HP�仯����Ϊ0
        {
            curHp -= changeAmt * Time.deltaTime;//��ǰHp���ͣ�һ֡ʱ�䣩
            health.transform.localScale = new Vector3(curHp, 1f);//Hp���ͺ�����

            yield return null;//��ͣ����һ֡
        }
        health.transform.localScale = new Vector3(newHp, 1f);

    }

}
