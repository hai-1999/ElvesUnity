using System;
using System.Collections;
using UnityEngine;

//ö�� ��ս�����е�״̬
public enum BattleState {Start, ActionSelection, SkillSelection, PerformSkill, PartySelection, Busy, BattleOver}

public class BattleSystem : MonoBehaviour
{
    [Header("����UI")]
    [SerializeField] BattleUnit enemyUnit;
    [Header("���UI")]
    [SerializeField] BattleUnit playerUnit;
    [Header("�Ի���")]
    [SerializeField] BattleDialogBox dialogBox;
    [Header("ͬ�б���")]
    [SerializeField] PartyBag PartyBag;

    public event Action<bool> OnBattleOver;

    BattleState state;

    int currentAction;//��ǰ����
    int currentSkill;//��ǰ����
    int currentMember;//��ǰ��Ա

    ElvesParty elvesParty;
    Elves wildElves;

    public void StartBattle(ElvesParty elvesParty, Elves elves)
    {
        this.elvesParty = elvesParty;
        this.wildElves = elves;

        dialogBox.EnableDialogText(true);//�Ի����ı��ɼ�

        dialogBox.EnableActionSelector(false);//����ѡ�������ɼ�
        dialogBox.EnableSkillSelector(false);//����ѡ�������ɼ�
        PartyBag.gameObject.SetActive(false);

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()//ս��ϵͳ��ʼ��
    {
        enemyUnit.SetUp(wildElves);//����Ұ��elf

        PartyBag.Init();
        playerUnit.SetUp(elvesParty.GeteHealthyElves());//��ͬ��elves��ѡ��һ��������elf

        dialogBox.SetSkillNames(playerUnit.elf.Skills);

        yield return dialogBox.TypeDialog($"һ��Ұ����{enemyUnit.elf.BaseElf.ElfName}�����ˣ���");

        ActionSelection();//��ʼ���ѡ����
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

    //������ѡ��
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

        currentAction = Mathf.Clamp(currentAction, 0, 3);//���Ƶ�ǰ������0��3֮��

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))//����J��ȷ��
        {
            switch (currentAction)
            {
                case 0://����ѡ��
                    SkillSelection();
                    break;
                case 1:
                    break;
                case 2:
                    OpenPartyBag();
                    break;
                case 3://����
                    break;
            }
        }
    }

    //������ѡ��
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

        currentAction = Mathf.Clamp(currentAction, 0, playerUnit.elf.Skills.Count - 1);//���Ƶ�ǰ������0����������֮��

        dialogBox.UpdateSKillSelection(currentSkill, playerUnit.elf.Skills[currentSkill]);

        if (Input.GetKeyDown(KeyCode.Space))//����J��ȷ��
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            StartCoroutine(RunPlayerSkill());//ִ�����ѡ����
        }
        else if (Input.GetKeyDown(KeyCode.X))//����K������
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            ActionSelection();
        }
    }

    //����elfѡ��
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

        currentMember = Mathf.Clamp(currentMember, 0, elvesParty.Elves.Count - 1);//���Ƶ�ǰ��Ա��0��elves����֮��

        PartyBag.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var newMember = elvesParty.Elves[currentMember];
            if (newMember.HP <= 0)
            {
                PartyBag.SetMessageText("�޷�ѡ���Ѿ����µľ��飡");
                return;
            }
            else if (newMember == playerUnit.elf)
            {
                PartyBag.SetMessageText("��ǰ�����Ѿ��ڳ��ϣ�");
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
        state = BattleState.ActionSelection;//ս��״̬����Ϊ����Ҷ���ѡ��

        dialogBox.SetDialog($"ѡ��һ����������");
        dialogBox.EnableActionSelector(true);//����ѡ��������
    }

    void SkillSelection()
    {
        state = BattleState.SkillSelection;//ս��״̬����Ϊ����Ҽ���ѡ��

        dialogBox.EnableDialogText(false);//�Ի����ı����ɼ�
        dialogBox.EnableActionSelector(false);//����ѡ�������ɼ�
        
        dialogBox.EnableSkillSelector(true);//����ѡ��������
    }

    void OpenPartyBag()
    {
        state = BattleState.PartySelection; //ս��״̬����Ϊ��ͬ��elfѡ��   
        PartyBag.SetPartyData(elvesParty.Elves);
        PartyBag.gameObject.SetActive(true);
    }

    //ִ����Ҽ���
    IEnumerator RunPlayerSkill()
    {
        state = BattleState.PerformSkill;

        var skill = playerUnit.elf.Skills[currentSkill];//ȷ��ѡ��ļ���
                                                        
        yield return RunSkill(playerUnit, enemyUnit, skill);//ִ�м���

        if(state==BattleState.PerformSkill)
            StartCoroutine(RunEnemySkill());//��ʼ����ѡ����
    }

    //ִ�е��˼���
    IEnumerator RunEnemySkill()
    {
        state = BattleState.PerformSkill;//ս��״̬����Ϊ�����˼���ѡ��

        var skill = enemyUnit.elf.GetRandomSkill();

        if (state == BattleState.PerformSkill)
            yield return RunSkill(enemyUnit, playerUnit, skill);//ִ�е��˼���

        ActionSelection();//��ʼ���ѡ����
    }

    //ִ�м���
    IEnumerator RunSkill(BattleUnit sourceUnit,BattleUnit targetUnit,Skill skill)
    {
        skill.PP--;

        yield return dialogBox.TypeDialog($"{sourceUnit.elf.BaseElf.ElfName}ʹ����{skill.Base.SkillName}��");
        sourceUnit.ElvesAttackAnimation();//��ҹ�������
        yield return new WaitForSeconds(1f);

        targetUnit.ElvesHitAnimation();//�����ܻ�����

        var damageDetails = targetUnit.elf.TakeDamage(skill, sourceUnit.elf);

        yield return targetUnit.Hud.UpdateHpSmooth();//���µ���hp
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.elf.BaseElf.ElfName}�����ˣ�");
            targetUnit.ElvesFaintAnimation();//���˵��¶���

            yield return new WaitForSeconds(1f);

            CheckForBattleOver(targetUnit);
        }
    }
    //����Ƿ�ս������
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
            yield return dialogBox.TypeDialog("�����ˣ�");

        if (damageDetails.Type > 1)
            yield return dialogBox.TypeDialog("Ч����Ⱥ��");
        else if (damageDetails.Type < 1)
            yield return dialogBox.TypeDialog("Ч��һ�㣡");
    }

    //����Elf
    IEnumerator SwitchElf(Elves newElf)
    {
        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}������");
        playerUnit.ElvesFaintAnimation();
        yield return new WaitForSeconds(2f);

        playerUnit.SetUp(newElf);//��ͬ��elves��ѡ��һ��������elf

        dialogBox.SetSkillNames(newElf.Skills);
        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}�����ɣ�");

        StartCoroutine(RunEnemySkill());//��ʼ����ѡ����
    }
}
