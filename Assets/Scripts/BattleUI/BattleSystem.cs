using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//枚举 对战过程中的状态
public enum BattleState {Start, PlayerActionSelection, PlayerSkillSelection, EnemySkillSelection, Busy}


public class BattleSystem : MonoBehaviour
{

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;

    int currentAction;//当前动作
    int currentSkill;//当前技能

    private void Start()
    {
        dialogBox.EnableDialogText(true);

        dialogBox.EnableActionSelector(false);
        dialogBox.EnableSkillSelector(false);

        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        if (state == BattleState.PlayerActionSelection)
            HandleActionSelection();
        else if (state == BattleState.PlayerSkillSelection)
            HandleSkillSelection();
    }

    public IEnumerator SetupBattle()//战斗系统初始化
    {
        enemyUnit.SetUp();
        enemyHud.SetData(enemyUnit.elf);

        playerUnit.SetUp();
        playerHud.SetData(playerUnit.elf);
        dialogBox.SetSkillNames(playerUnit.elf.skills);

        yield return dialogBox.TypeDialog($"一个野生的{playerUnit.elf.baseElf.ElfName}出现了！！");
        yield return new WaitForSeconds(1f);

        PlayerAction();//开始玩家选择动作
    }


    void PlayerAction()
    {
        state = BattleState.PlayerActionSelection;//战斗状态设置为“玩家动作选择”

        StartCoroutine(dialogBox.TypeDialog($"选择一个动作！！"));
        dialogBox.EnableActionSelector(true);//动作选择器开启

    }

    void PlayerSkill()
    {
        state = BattleState.PlayerSkillSelection;//战斗状态设置为“玩家技能选择”

        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(false);
        
        dialogBox.EnableSkillSelector(true);//技能选择器开启
    }


    void HandleActionSelection()//处理动作选择
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            if(currentAction < 3)
                ++currentAction;           
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            if(currentAction > 0)
                --currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentAction < 2)
                currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentAction > 1)
                currentAction -= 2;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))//按“空格”确定
        {
            switch (currentAction)
            {
                case 0://技能选择
                    PlayerSkill();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3://逃跑
                    break;
            }
        }
    }

    void HandleSkillSelection()//处理技能选择
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentSkill < playerUnit.elf.skills.Count-1)
                ++currentSkill;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentSkill > 0)
                --currentSkill;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentSkill < playerUnit.elf.skills.Count - 2)
                currentSkill += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentSkill > 1)
                currentSkill -=2;
        }
        dialogBox.UpdateSKillSelection(currentSkill, playerUnit.elf.skills[currentSkill]);
    }
}
