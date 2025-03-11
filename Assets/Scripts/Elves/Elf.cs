using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Elf
{
    public BaseElf baseElf { get; set; }
    public int level { get; set; }

    public int hp { get; set; }

    public List<Skill> skills { get; set; }

    public Elf(BaseElf pbaseElf, int pLevel)//Elf��ʼ��
    {
        baseElf = pbaseElf;//��ʼ����������
        level = pLevel;//��ʼ���ȼ�
        hp = MaxHp;//��ʼѪ���������Ѫ��

        skills = new List<Skill>();

        foreach (var skill in baseElf.LearnableSkills)//��ʼ������
        {
            if (skill.Level <= level)
            {
                skills.Add(new Skill(skill.SkillBase));
            }
            if (skills.Count >= 4)//����ĸ����� 
            {
                break;
            }
        }       
    }
    //
    public int MaxHp
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsHp * level) / 100f) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((baseElf.BaseStatsAttack * level) / 100f) + 5; }
    }

    public int Denfense
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

    public DamageDetails TakeDamage(Skill skill, Elf attacker)//�˺����㣬�����Ƿ���
    {

        float baseDamage = skill.Base.Power * ((float)attacker.Attack / Denfense) + 2;//�����˺�

        float level = (2 * attacker.level + 10) / 250f;//�ȼ��ӳ�
        float type = TypeChart.GetEffectiveness(skill.Base.Type, this.baseElf.Type1)
                   * TypeChart.GetEffectiveness(skill.Base.Type, this.baseElf.Type2);//���Կ���

        float critical = 1f;
        if (Random.value * 100f <= 6.25f)//�Ƿ񱩻�
            critical = 2f;

        float modifiers = Random.Range(0.85f, 1f);//��ֵ����

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
