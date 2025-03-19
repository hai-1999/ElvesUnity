using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;

    [SerializeField] GameObject health;

    private Elves elf;
    public void SetData(Elves elf)
    {
        this.elf = elf;

        nameText.text = elf.baseElf.ElfName;
        levelText.text = "Lvl " + elf.level;

        health.transform.localScale = new Vector3((float)elf.hp / elf.MaxHp, 1f, 1f);
    }

    public IEnumerator UpdateHpSmooth()
    {
        float newHp = (float)elf.hp / elf.MaxHp;
        float curHp = health.transform.localScale.x;//��ǰHP
        float changeAmt = curHp - newHp;// HP�仯��

        while (curHp - newHp > Mathf.Epsilon)//HP�仯����Ϊ0
        {
            curHp -= changeAmt * Time.deltaTime;//��ǰHp���ͣ�һ֡ʱ�䣩
            health.transform.localScale = new Vector3(curHp, 1f);//Hp���ͺ�����

            yield return null;//��ͣ����һ֡
        }
        health.transform.localScale = new Vector3(newHp, 1f);
    }
}
