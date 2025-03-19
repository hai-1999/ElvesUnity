using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elves", menuName = "Elves/Creat new Elf")]
public class ElvesBase : ScriptableObject
{
    //[SerializeField] ʹ˽�б�����Inspector�пɼ����ɱ༭
    [SerializeField] int id;
    [SerializeField] string elfName;

    //[TextArea]
    //[SerializeField] string description;

    [Header("��ͼ")]
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;

    [Header("����")]
    [SerializeField] Type type1;
    [SerializeField] Type type2;

    [Header("����ֵ")]
    [Range(0, 100)] public int Hp;
    [Range(0, 100)] public int Attack;
    [Range(0, 100)] public int Defense;
    [Range(0, 100)] public int SpAttack;
    [Range(0, 100)] public int SpDefense;
    [Range(0, 100)] public int Speed;

    [Header("��ѧϰ����")]
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

//������ѧϰ�����࣬�������ܺ͵ȼ�
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

