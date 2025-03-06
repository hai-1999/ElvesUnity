using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elves", menuName = "Elves/Creat new Elf")]
public class BaseElf : ScriptableObject
{
    //[SerializeField] ʹ˽�б�����Inspector�пɼ����ɱ༭
    [SerializeField] string id;
    [SerializeField] string elfName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;

    //����˽�б�����ֵ�������������
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

    //����ֵ
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

//������ѧϰ�����࣬�������ܺ͵ȼ�
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

//ö�� ELves��Skills������
public enum ElvesType
{ 
    None,
    ��ͨ,   
    ��,
    ˮ,
    ��, 
    ��, 
    ��,
    ����,
    ����,
    ս��,
    ����,
    ��Ӱ,
    ��,
    ��е,
    ��,
}
