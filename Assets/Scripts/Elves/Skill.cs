using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill 
{
    public BaseSkill Base {  get;  set; }
    public int PP {  get; set; }

    public Skill(BaseSkill pBase )
    {
        Base = pBase;
        PP = pBase.PP;
    }
}
