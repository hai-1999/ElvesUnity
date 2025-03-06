using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Elves/Creat new Skill")]

public class BaseSkill : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] string skillName;

    [TextArea]
    [SerializeField] string description;

    public string Id
    {
        get { return id; }
    }
    public string SkillName
    {
        get { return skillName; }
    }

    public string Description
    {
        get { return description; }
    }

    [SerializeField] ElvesType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    public ElvesType Type
    {
        get { return type; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public int PP
    {
        get { return pp; }
    }

}
