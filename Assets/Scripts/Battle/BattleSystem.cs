using System;
using System.Collections;
using UnityEngine;

//枚举 对战过程中的状态
public enum BattleState {Start, ActionSelection, SkillSelection, PerformSkill, PartySelection, Busy, BattleOver}

public class BattleSystem : MonoBehaviour
{
    [Header("敌人UI")]
    [SerializeField] BattleUnit enemyUnit;
    [Header("玩家UI")]
    [SerializeField] BattleUnit playerUnit;
    [Header("对话框")]
    [SerializeField] BattleDialogBox dialogBox;
    [Header("同行背包")]
    [SerializeField] PartyBag PartyBag;

    public event Action<bool> OnBattleOver;

    BattleState state;

    int currentAction;//当前动作
    int currentSkill;//当前技能
    int currentMember;//当前成员

    ElvesParty elvesParty;
    Elves wildElves;

    public void StartBattle(ElvesParty elvesParty, Elves elves)
    {
        this.elvesParty = elvesParty;
        this.wildElves = elves;

        dialogBox.EnableDialogText(true);//对话框文本可见

        dialogBox.EnableActionSelector(false);//动作选择器不可见
        dialogBox.EnableSkillSelector(false);//技能选择器不可见
        PartyBag.gameObject.SetActive(false);

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()//战斗系统初始化
    {
        enemyUnit.SetUp(wildElves);//生成野生elf

        PartyBag.Init();
        playerUnit.SetUp(elvesParty.GeteHealthyElves());//从同行elves中选择一个健康的elf

        dialogBox.SetSkillNames(playerUnit.elf.Skills);

        yield return dialogBox.TypeDialog($"一个野生的{enemyUnit.elf.BaseElf.ElfName}出现了！！");

        ActionSelection();//开始玩家选择动作
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
            HandleActionSelection();
        else if (state == BattleState.SkillSelection)
            HandleSkillSelection();
        else if (state == BattleState.PartySelection)
            HandlePartySelection();
    }

    //处理动作选择
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.S))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);//限制当前动作在0到3之间

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))//按“J”确定
        {
            switch (currentAction)
            {
                case 0://技能选择
                    SkillSelection();
                    break;
                case 1:
                    break;
                case 2:
                    OpenPartyBag();
                    break;
                case 3://逃跑
                    break;
            }
        }
    }

    //处理技能选择
    void HandleSkillSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentSkill;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentSkill;
        else if (Input.GetKeyDown(KeyCode.S))
            currentSkill += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentSkill -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, playerUnit.elf.Skills.Count - 1);//限制当前技能在0到技能数量之间

        dialogBox.UpdateSKillSelection(currentSkill, playerUnit.elf.Skills[currentSkill]);

        if (Input.GetKeyDown(KeyCode.Space))//按“J”确定
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            StartCoroutine(RunPlayerSkill());//执行玩家选择技能
        }
        else if (Input.GetKeyDown(KeyCode.X))//按“K”返回
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            ActionSelection();
        }
    }

    //处理elf选择
    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.S))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, elvesParty.Elves.Count - 1);//限制当前成员在0到elves数量之间

        PartyBag.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var newMember = elvesParty.Elves[currentMember];
            if (newMember.HP <= 0)
            {
                PartyBag.SetMessageText("无法选择已经倒下的精灵！");
                return;
            }
            else if (newMember == playerUnit.elf)
            {
                PartyBag.SetMessageText("当前精灵已经在场上！");
                return;
            }

            PartyBag.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchElf(newMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            PartyBag.gameObject.SetActive(false);
            ActionSelection();
        }
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;//战斗状态设置为“玩家动作选择”

        dialogBox.SetDialog($"选择一个动作！！");
        dialogBox.EnableActionSelector(true);//动作选择器开启
    }

    void SkillSelection()
    {
        state = BattleState.SkillSelection;//战斗状态设置为“玩家技能选择”

        dialogBox.EnableDialogText(false);//对话框文本不可见
        dialogBox.EnableActionSelector(false);//动作选择器不可见
        
        dialogBox.EnableSkillSelector(true);//技能选择器开启
    }

    void OpenPartyBag()
    {
        state = BattleState.PartySelection; //战斗状态设置为“同行elf选择”   
        PartyBag.SetPartyData(elvesParty.Elves);
        PartyBag.gameObject.SetActive(true);
    }

    //执行玩家技能
    IEnumerator RunPlayerSkill()
    {
        state = BattleState.PerformSkill;

        var skill = playerUnit.elf.Skills[currentSkill];//确定选择的技能
                                                        
        yield return RunSkill(playerUnit, enemyUnit, skill);//执行技能

        if(state==BattleState.PerformSkill)
            StartCoroutine(RunEnemySkill());//开始敌人选择动作
    }

    //执行敌人技能
    IEnumerator RunEnemySkill()
    {
        state = BattleState.PerformSkill;//战斗状态设置为“敌人技能选择”

        var skill = enemyUnit.elf.GetRandomSkill();

        if (state == BattleState.PerformSkill)
            yield return RunSkill(enemyUnit, playerUnit, skill);//执行敌人技能

        ActionSelection();//开始玩家选择动作
    }

    //执行技能
    IEnumerator RunSkill(BattleUnit sourceUnit,BattleUnit targetUnit,Skill skill)
    {
        skill.PP--;

        yield return dialogBox.TypeDialog($"{sourceUnit.elf.BaseElf.ElfName}使用了{skill.Base.SkillName}！");
        sourceUnit.ElvesAttackAnimation();//玩家攻击动画
        yield return new WaitForSeconds(1f);

        targetUnit.ElvesHitAnimation();//敌人受击动画

        var damageDetails = targetUnit.elf.TakeDamage(skill, sourceUnit.elf);

        yield return targetUnit.Hud.UpdateHpSmooth();//更新敌人hp
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.elf.BaseElf.ElfName}倒下了！");
            targetUnit.ElvesFaintAnimation();//敌人倒下动画

            yield return new WaitForSeconds(1f);

            CheckForBattleOver(targetUnit);
        }
    }
    //检测是否战斗结束
    void CheckForBattleOver(BattleUnit fiantedUnit)
    {
        if (fiantedUnit.IsPlayerUnit)
        {
            var nextElf = elvesParty.GeteHealthyElves();
            if (nextElf != null)
                OpenPartyBag();
            else
                BattleOver(false);
        }
        else
            BattleOver(true);
    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        OnBattleOver(won);
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical>1f)
            yield return dialogBox.TypeDialog("暴击了！");

        if (damageDetails.Type > 1)
            yield return dialogBox.TypeDialog("效果拔群！");
        else if (damageDetails.Type < 1)
            yield return dialogBox.TypeDialog("效果一般！");
    }

    //交换Elf
    IEnumerator SwitchElf(Elves newElf)
    {
        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}回来！");
        playerUnit.ElvesFaintAnimation();
        yield return new WaitForSeconds(2f);

        playerUnit.SetUp(newElf);//从同行elves中选择一个健康的elf

        dialogBox.SetSkillNames(newElf.Skills);
        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}出来吧！");

        StartCoroutine(RunEnemySkill());//开始敌人选择动作
    }
}
