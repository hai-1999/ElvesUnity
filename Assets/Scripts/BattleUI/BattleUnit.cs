using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] BaseElf baseElves;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Elf elf { get; set; }

    public void SetUp()
    {
        elf = new Elf(baseElves, level);//ͨ��baseElves��level�����µ�Elves
        if (isPlayerUnit)
            GetComponent<Image>().sprite = elf.baseElf.LeftSprite;
        else
            GetComponent<Image>().sprite = elf.baseElf.RightSprite;
    }
}
