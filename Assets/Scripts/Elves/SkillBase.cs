using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Elves/Creat new Skill")]

public class SkillBase : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] string skillName;

    [Header("����")]
    [SerializeField] Type type;
    [SerializeField] DamageType damageType;

    [Header("������ֵ")]
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    public string Id
    {
        get { return id; }
    }
    public string SkillName
    {
        get { return skillName; }
    }

    public Type Type
    {
        get { return type; }
    }

    public DamageType DamageType
    {
        get { return damageType; }
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

public enum DamageType
{
    ����,
    ����,
    �仯
}