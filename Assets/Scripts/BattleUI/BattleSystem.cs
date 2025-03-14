using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//ö�� ��ս�����е�״̬
public enum BattleState {Start, PlayerActionSelection, PlayerSkillSelection, EnemySkillSelection, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;

    int currentAction;//��ǰ����
    int currentSkill;//��ǰ����

    private void Start()
    {
        dialogBox.EnableDialogText(true);//�Ի����ı��ɼ�

        dialogBox.EnableActionSelector(false);//����ѡ�������ɼ�
        dialogBox.EnableSkillSelector(false);//����ѡ�������ɼ�

        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        if (state == BattleState.PlayerActionSelection)
            HandleActionSelection();
        else if (state == BattleState.PlayerSkillSelection)
            HandleSkillSelection();
    }

    public IEnumerator SetupBattle()//ս��ϵͳ��ʼ��
    {
        enemyUnit.SetUp();
        enemyHud.SetData(enemyUnit.elf);

        playerUnit.SetUp();
        playerHud.SetData(playerUnit.elf);
        dialogBox.SetSkillNames(playerUnit.elf.skills);

        yield return dialogBox.TypeDialog($"һ��Ұ����{playerUnit.elf.baseElf.ElfName}�����ˣ���");

        PlayerAction();//��ʼ���ѡ����
    }

    void PlayerAction()
    {
        state = BattleState.PlayerActionSelection;//ս��״̬����Ϊ����Ҷ���ѡ��

        StartCoroutine(dialogBox.TypeDialog($"ѡ��һ����������"));
        dialogBox.EnableActionSelector(true);//����ѡ��������
    }

    void PlayerSkill()
    {
        state = BattleState.PlayerSkillSelection;//ս��״̬����Ϊ����Ҽ���ѡ��

        dialogBox.EnableDialogText(false);//�Ի����ı����ɼ�
        dialogBox.EnableActionSelector(false);//����ѡ�������ɼ�
        
        dialogBox.EnableSkillSelector(true);//����ѡ��������
    }

    IEnumerator PerformPlayerSKill()
    {
        state = BattleState.Busy;
        var skill = playerUnit.elf.skills[currentSkill];//ȷ��ѡ��ļ���

        yield return dialogBox.TypeDialog($"{playerUnit.elf.baseElf.ElfName}ʹ����{skill.Base.SkillName}��");
        playerUnit.PlayAttackAnimation();//��ҹ�������
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();//�����ܻ�����

        var damageDetails= enemyUnit.elf.TakeDamage(skill, playerUnit.elf);

        yield return enemyHud.UpdateHp();//���µ���hp
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.elf.baseElf.ElfName}�����ˣ�");
            enemyUnit.PlayFaintAnimation();//���˵��¶���
        }
        else
        {
            StartCoroutine(EnemySkill());//��ʼ����ѡ����
        }      
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical>1f)
            yield return dialogBox.TypeDialog("�����ˣ�");

        if (damageDetails.Type > 1)
            yield return dialogBox.TypeDialog("Ч����Ⱥ��");
        else if (damageDetails.Type < 1)
            yield return dialogBox.TypeDialog("Ч��һ�㣡");
    }

    IEnumerator EnemySkill()
    {
        state = BattleState.EnemySkillSelection;//ս��״̬����Ϊ�����˼���ѡ��

        var skill = enemyUnit.elf.GetRandomSkill();

        yield return dialogBox.TypeDialog($"{enemyUnit.elf.baseElf.ElfName}ʹ����{skill.Base.SkillName}��");
        enemyUnit.PlayAttackAnimation();//���˹�������
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();//����ܻ�����

        var damageDetails = playerUnit.elf.TakeDamage(skill, enemyUnit.elf);
        yield return playerHud.UpdateHp();//�������hp
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.elf.baseElf.ElfName}�����ˣ�");
            playerUnit.PlayFaintAnimation();//��ҵ��¶���
        }
        else
        {
            PlayerAction();//��ʼ���ѡ����
        }
    }

    void HandleActionSelection()//������ѡ��
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

        if (Input.GetKeyDown(KeyCode.Space))//�����ո�ȷ��
        {
            switch (currentAction)
            {
                case 0://����ѡ��
                    PlayerSkill();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3://����
                    break;
            }
        }
    }

    void HandleSkillSelection()//������ѡ��
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

        if (Input.GetKeyDown(KeyCode.Space))//�����ո�ȷ��
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            StartCoroutine(PerformPlayerSKill());//ִ�����ѡ����
        }
    }
}
