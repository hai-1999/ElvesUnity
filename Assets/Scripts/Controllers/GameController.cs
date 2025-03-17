using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{FreeRoam, Battle}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle()
    {
        state = GameState.Battle;//游戏状态设置为“战斗”
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        battleSystem.StartBattle();//战斗系统初始化
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;//游戏状态设置为“自由移动”
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(state==GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if(state==GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
