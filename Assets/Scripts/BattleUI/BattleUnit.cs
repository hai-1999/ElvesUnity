using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] BaseElf baseElves;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Elf elves { get; set; }

    public void SetUp()
    {
        elves = new Elf(baseElves, level);//通过baseElves和level创建新的Elves
        if (isPlayerUnit)
            GetComponent<Image>().sprite = elves.baseElf.LeftSprite;
        else
            GetComponent<Image>().sprite = elves.baseElf.RightSprite;
    }
}
