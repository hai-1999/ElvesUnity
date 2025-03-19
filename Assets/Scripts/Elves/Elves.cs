using System.Collections.Generic;
using UnityEngine;

public class Elves
{
    public ElvesBase baseElf { get; set; }
    public int level { get; set; }

    public int hp { get; set; }

    public List<Skill> skills { get; set; }

    public Elves(ElvesBase pbaseElf, int pLevel)
    {
        baseElf = pbaseElf;//导入基础数值
        level = pLevel;//设置等级
        hp = MaxHp;//初始血量等于最大血量

        //初始化技能
        skills = new List<Skill>();

        foreach (var skill in baseElf.LearnableSkills)
        {
            if (skill.Level <= level)
                skills.Add(new Skill(skill.SkillBase));

            if (skills.Count >= 4)//最多四个技能 
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
        hp -= Mathf.FloorToInt(finalDamage);

        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = critical,
            Fainted=false
        };

        if (hp<=0)//hp最小为0
        {
            hp = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Skill GetRandomSkill()
    {
        int r = Random.Range(0, skills.Count);
        return skills[r];      
    }
}


public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float Type { get; set; }
}
