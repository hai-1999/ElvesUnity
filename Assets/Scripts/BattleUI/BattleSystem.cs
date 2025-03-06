using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;


    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    private void Start()
    {
        SetUpBattle();
    }

    public void SetUpBattle()
    {
        enemyUnit.SetUp();
        enemyHud.SetData(enemyUnit.elves);

        playerUnit.SetUp();
        playerHud.SetData(playerUnit.elves);
    }
}
