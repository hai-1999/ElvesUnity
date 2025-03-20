using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Elves
{
    [SerializeField] ElvesBase baseElf;
    [SerializeField] int level;

    public ElvesBase BaseElf {
        get { 
            return baseElf; 
        }
    }
    public int Level {
         get { 
            return level; 
         }
    }

    public int HP { get; set; }

    public List<Skill> Skills { get; set; }

    public void Init()
    {
        this.HP = MaxHp;

        //初始化技能
        this.Skills = new List<Skill>();

        foreach (var skill in baseElf.LearnableSkills)
        {
            if (skill.Level <= level)
                Skills.Add(new Skill(skill.SkillBase));

            if (Skills.Count >= 4)//最多四个技能 
                break;
        }       
    }
    
    public int MaxHp
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsHp * level) / 100f) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsAttack * level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsDefense * level) / 100f) + 5; }
    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsSpAttack * level) / 100f) + 5; }
    }

    public int SpDefense
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsSpDefense * level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsSpeed * level) / 100f) + 5; }
    }

    //伤害计算函数，返回伤害详情
    public DamageDetails TakeDamage(Skill skill, Elves attacker)
    {
        //基础伤害
        float baseDamage;
        if (skill.Base.DamageType == DamageType.物理)
            baseDamage = skill.Base.Power * ((float)attacker.Attack / Defense) + 2;
        else if (skill.Base.DamageType == DamageType.特殊)
            baseDamage = skill.Base.Power * ((float)attacker.SpAttack / SpDefense) + 2;
        else
            baseDamage = 0;

        //等级加成
        float level = (2 * attacker.level + 10) / 250f;

        //属性克制
        float type = TypeChart.GetEffectiveness(skill.Base.Type, this.baseElf.Type1)
                   * TypeChart.GetEffectiveness(skill.Base.Type, this.baseElf.Type2);

        //是否暴击
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        //数值修正
        float modifiers = Random.Range(0.85f, 1f);

        //最终伤害
        float finalDamage = baseDamage * level * type * critical * modifiers;
        HP -= Mathf.FloorToInt(finalDamage);

        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = critical,
            Fainted=false
        };

        if (HP<=0)//hp最小为0
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Skill GetRandomSkill()
    {
        int r = Random.Range(0, Skills.Count);
        return Skills[r];      
    }
}


public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float Type { get; set; }
}
