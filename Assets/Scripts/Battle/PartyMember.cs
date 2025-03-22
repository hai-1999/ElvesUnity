using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;

    [SerializeField] GameObject health;

    private Elves elf;
    public void SetData(Elves elf)
    {
        this.elf = elf;

        nameText.text = elf.BaseElf.ElfName;
        levelText.text = "Lvl " + elf.Level;

        health.transform.localScale = new Vector3((float)elf.HP / elf.MaxHp, 1f, 1f);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = Color.green;
        else
            nameText.color = Color.black;
    }
}
