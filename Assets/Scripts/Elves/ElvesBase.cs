using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elves", menuName = "Elves/Creat new Elf")]
public class ElvesBase : ScriptableObject
{
    //[SerializeField] 使私有变量在Inspector中可见并可编辑
    [SerializeField] int id;
    [SerializeField] string elfName;

    //[TextArea]
    //[SerializeField] string description;

    [Header("贴图")]
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;

    [Header("属性")]
    [SerializeField] Type type1;
    [SerializeField] Type type2;

    [Header("种族值")]
    [Range(0, 100)] public int Hp;
    [Range(0, 100)] public int Attack;
    [Range(0, 100)] public int Defense;
    [Range(0, 100)] public int SpAttack;
    [Range(0, 100)] public int SpDefense;
    [Range(0, 100)] public int Speed;

    [Header("可学习技能")]
    [SerializeField] List<LearnableSkill> learnableSkills;

    public int Id
    {
        get { return id; }
    }
    public string ElfName
    {
        get { return elfName; }
    }

    public Sprite LeftSprite
    {
        get { return leftSprite; }
    }
    public Sprite RightSprite
    {
        get { return rightSprite; }
    }

    public Type Type1
    {
        get { return type1; }
    }
    public Type Type2
    {
        get { return type2; }
    }

    public int BaseStatsHp
    {
        get { return Hp; }
    }
    public int BaseStatsAttack
    {
        get { return Attack; }
    }
    public int BaseStatsDefense
    {
        get { return Defense; }
    }
    public int BaseStatsSpAttack
    {
        get { return SpAttack; }
    }
    public int BaseStatsSpDefense
    {
        get { return SpDefense; }
    }
    public int BaseStatsSpeed
    {
        get { return Speed; }
    }

    public List<LearnableSkill> LearnableSkills
    {
        get { return learnableSkills; }
    }
}

[System.Serializable]

//创建可学习技能类，包括技能和等级
public class LearnableSkill
{
    [SerializeField] SkillBase skill;
    [SerializeField] int level;

    public SkillBase SkillBase
    {
        get { return skill; }
    }

    public int Level
    {
        get { return level; }
    }
}

