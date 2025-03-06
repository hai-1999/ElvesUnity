using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elves", menuName = "Elves/Creat new Elf")]
public class BaseElf : ScriptableObject
{
    //[SerializeField] 使私有变量在Inspector中可见并可编辑
    [SerializeField] string id;
    [SerializeField] string elfName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;

    //返回私有变量的值，方便后续调用
    public string Id
    {
        get { return id; }
    }
    public string ElfName
    {
        get { return elfName; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite LeftSprite
    {
        get { return leftSprite; }
    }
    public Sprite RightSprite
    {
        get { return rightSprite; }
    }


    [SerializeField] ElvesType type1;
    [SerializeField] ElvesType type2;

    public ElvesType Type1
    {
        get { return type1; }
    }
    public ElvesType Type2
    {
        get { return type2; }
    }

    //种族值
    [SerializeField] int baseStatsHp;
    [SerializeField] int baseStatsAttack;
    [SerializeField] int baseStatsDefense;
    [SerializeField] int baseStatsSpAttack;
    [SerializeField] int baseStatsSpDefense;
    [SerializeField] int baseStatsSpeed;

    public int BaseStatsHp
    {
        get { return baseStatsHp; }
    }
    public int BaseStatsAttack
    {
        get { return baseStatsAttack; }
    }
    public int BaseStatsDefense
    {
        get { return baseStatsDefense; }
    }
    public int BaseStatsSpAttack
    {
        get { return baseStatsSpAttack; }
    }
    public int BaseStatsSpDefense
    {
        get { return baseStatsSpDefense; }
    }
    public int BaseStatsSpeed
    {
        get { return baseStatsSpeed; }
    }


    [SerializeField] List<LearnableSkill> learnableSkills;

    public List<LearnableSkill> LearnableSkills
    {
        get { return learnableSkills; }
    }
}

[System.Serializable]

//创建可学习技能类，包括技能和等级
public class LearnableSkill
{
    [SerializeField] BaseSkill skill;
    [SerializeField] int level;

    public BaseSkill SkillBase
    {
        get { return skill; }
    }

    public int Level
    {
        get { return level; }
    }
}

//枚举 ELves和Skills的属性
public enum ElvesType
{ 
    None,
    普通,   
    火,
    水,
    草, 
    电, 
    冰,
    飞行,
    地面,
    战斗,
    超能,
    暗影,
    光,
    机械,
    龙,
}
