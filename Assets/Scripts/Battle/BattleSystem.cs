using System;
using System.Collections;
using UnityEngine;

//ö�� ��ս�����е�״̬
public enum BattleState {Start, ActionSelection, SkillSelection, EnemySkill, PartySelection, Busy}

public class BattleSystem : MonoBehaviour
{
    [Header("����UI")]
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [Header("���UI")]
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
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
        enemyHud.SetData(enemyUnit.elf);

        PartyBag.Init();

        playerUnit.SetUp(elvesParty.GeteHealthyElves());//��ͬ��elves��ѡ��һ��������elf
        playerHud.SetData(playerUnit.elf);

        dialogBox.SetSkillNames(playerUnit.elf.Skills);

        yield return dialogBox.TypeDialog($"һ��Ұ����{enemyUnit.elf.BaseElf.ElfName}�����ˣ���");

        PlayerAction();//��ʼ���ѡ����
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

    void PlayerAction()
    {
        state = BattleState.ActionSelection;//ս��״̬����Ϊ����Ҷ���ѡ��

        dialogBox.SetDialog($"ѡ��һ����������");
        dialogBox.EnableActionSelector(true);//����ѡ��������
    }

    void PlayerSkill()
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
        // PartyBag.OnSelected += OnSelectedMember;
    }

    IEnumerator PerformPlayerSkill()
    {
        state = BattleState.Busy;
        var skill = playerUnit.elf.Skills[currentSkill];//ȷ��ѡ��ļ���
        skill.PP--;

        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}ʹ����{skill.Base.SkillName}��");
        playerUnit.ElvesAttackAnimation();//��ҹ�������
        yield return new WaitForSeconds(1f);

        enemyUnit.ElvesHitAnimation();//�����ܻ�����

        var damageDetails= enemyUnit.elf.TakeDamage(skill, playerUnit.elf);

        yield return enemyHud.UpdateHpSmooth();//���µ���hp
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.elf.BaseElf.ElfName}�����ˣ�");
            enemyUnit.ElvesFaintAnimation();//���˵��¶���

            yield return new WaitForSeconds(1f);
            OnBattleOver(true);
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
        state = BattleState.EnemySkill;//ս��״̬����Ϊ�����˼���ѡ��

        var skill = enemyUnit.elf.GetRandomSkill();

        yield return dialogBox.TypeDialog($"{enemyUnit.elf.BaseElf.ElfName}ʹ����{skill.Base.SkillName}��");
        enemyUnit.ElvesAttackAnimation();//���˹�������
        yield return new WaitForSeconds(1f);

        playerUnit.ElvesHitAnimation();//����ܻ�����

        var damageDetails = playerUnit.elf.TakeDamage(skill, enemyUnit.elf);
        yield return playerHud.UpdateHpSmooth();//�������hp
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}�����ˣ�");
            playerUnit.ElvesFaintAnimation();//��ҵ��¶���

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            PlayerAction();//��ʼ���ѡ����
        }
    }

    //������ѡ��
    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.D))
            ++currentAction;           
        else if(Input.GetKeyDown(KeyCode.A))
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
                    PlayerSkill();
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
            currentSkill -=2;

        currentAction = Mathf.Clamp(currentAction, 0, playerUnit.elf.Skills.Count - 1);//���Ƶ�ǰ������0����������֮��

        dialogBox.UpdateSKillSelection(currentSkill, playerUnit.elf.Skills[currentSkill]);

        if (Input.GetKeyDown(KeyCode.Space))//����J��ȷ��
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            StartCoroutine(PerformPlayerSkill());//ִ�����ѡ����
        }
        else if(Input.GetKeyDown(KeyCode.X))//����K������
        {
            dialogBox.EnableSkillSelector(false);
            dialogBox.EnableDialogText(true);

            PlayerAction();
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
            PlayerAction();
        }
    }
    IEnumerator SwitchElf(Elves newElf)
    {
        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}������");
        playerUnit.ElvesFaintAnimation();
        yield return new WaitForSeconds(2f);

        playerUnit.SetUp(newElf);//��ͬ��elves��ѡ��һ��������elf
        playerHud.SetData(newElf);

        dialogBox.SetSkillNames(newElf.Skills);
        yield return dialogBox.TypeDialog($"{playerUnit.elf.BaseElf.ElfName}�����ɣ�");

        StartCoroutine(EnemySkill());//��ʼ����ѡ����
    }
}
