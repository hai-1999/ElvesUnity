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
        float curHp = health.transform.localScale.x;//当前HP
        float changeAmt = curHp - newHp;// HP变化量

        while (curHp - newHp > Mathf.Epsilon)//HP变化量不为0
        {
            curHp -= changeAmt * Time.deltaTime;//当前Hp降低（一帧时间）
            health.transform.localScale = new Vector3(curHp, 1f);//Hp降低后设置

            yield return null;//暂停等下一帧
        }
        health.transform.localScale = new Vector3(newHp, 1f);
    }
}
