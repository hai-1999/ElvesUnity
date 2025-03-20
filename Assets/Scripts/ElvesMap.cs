using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvesMap : MonoBehaviour
{
    [SerializeField] List<Elves> WildElves;

    public Elves GetRandomWildElves()
    {
       var wildElf = WildElves[Random.Range(0,WildElves.Count)];
        wildElf.Init();
        return wildElf;
    }
}
