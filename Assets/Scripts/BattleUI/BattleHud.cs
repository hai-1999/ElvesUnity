using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;

    public void SetData(Elf elf)
    {
        nameText.text = elf.baseElf.ElfName;
        levelText.text = "Lvl " + elf.level;
        hpBar.SetHp((float)elf.hp / elf.MaxHp);
    }
}
