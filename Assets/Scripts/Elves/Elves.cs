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
        baseElf = pbaseElf;//���������ֵ
        level = pLevel;//���õȼ�
        hp = MaxHp;//��ʼѪ���������Ѫ��

        //��ʼ������
        skills = new List<Skill>();

        foreach (var skill in baseElf.LearnableSkills)
        {
            if (skill.Level <= level)
                skills.Add(new Skill(skill.SkillBase));

            if (skills.Count >= 4)//����ĸ����� 
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

    //�˺����㺯���������˺�����
    public DamageDetails TakeDamage(Skill skill, Elves attacker)
    {
        //�����˺�
        float baseDamage;
        if (skill.Base.DamageType == DamageType.����)
            baseDamage = skill.Base.Power * ((float)attacker.Attack / Defense) + 2;
        else if (skill.Base.DamageType == DamageType.����)
            baseDamage = skill.Base.Power * ((float)attacker.SpAttack / SpDefense) + 2;
        else
            baseDamage = 0;

        //�ȼ��ӳ�
        float level = (2 * attacker.level + 10) / 250f;

        //���Կ���
        float type = TypeChart.GetEffectiveness(skill.Base.Type, this.baseElf.Type1)
                   * TypeChart.GetEffectiveness(skill.Base.Type, this.baseElf.Type2);

        //�Ƿ񱩻�
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        //��ֵ����
        float modifiers = Random.Range(0.85f, 1f);

        //�����˺�
        float finalDamage = baseDamage * level * type * critical * modifiers;
        hp -= Mathf.FloorToInt(finalDamage);

        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = critical,
            Fainted=false
        };

        if (hp<=0)//hp��СΪ0
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
