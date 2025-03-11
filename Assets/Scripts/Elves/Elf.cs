using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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


    public bool TakeDamage(Skill skill, Elf attacker)//�˺����㣬�����Ƿ���
    {
        float modifiers = Random.Range(0.85f,1f);
        float a = (2 * attacker.level + 10) / 250f;
        float b = a * skill.Base.Power * ((float)attacker.Attack / Denfense) + 2;
        int damage = Mathf.FloorToInt(b * modifiers);

        hp -= damage;
        if(hp<=0)//hp��СΪ0
        {
            hp = 0;
            return true;           
        }
        return false;
    }

    public Skill GetRandomSkill()
    {
        int r = Random.Range(0, skills.Count);
        return skills[r];
        
    }
}




