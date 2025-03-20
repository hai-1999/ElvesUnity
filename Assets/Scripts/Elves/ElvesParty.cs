using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Í¬ÐÐ¾«Áé
public class ElvesParty : MonoBehaviour
{
    [SerializeField] List<Elves> elves;

    private void Start()
    {
        foreach (var elf in elves)
        {
            elf.Init();
        }
    }

    public Elves GeteHealthyElves()
    {
        return elves.Where(x => x.HP > 0).FirstOrDefault();
    }
}
