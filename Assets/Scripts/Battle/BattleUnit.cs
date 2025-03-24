using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    //[SerializeField] ElvesBase baseElves;
    [SerializeField] BattleHud hud;
    [SerializeField] bool isPlayerUnit;

    public BattleHud Hud
    {
        get { return hud; }
    }

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public Elves elf { get; set; }

    Image image;
    Vector3 orginalPos;
    Color orginalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        orginalPos = transform.localPosition;
        orginalColor = image.color;
    }

    public void SetUp(Elves elf)
    {
        this.elf = elf;//�����µ�elf
        if (isPlayerUnit)
            image.sprite = this.elf.BaseElf.LeftSprite;
        else
            image.sprite = this.elf.BaseElf.RightSprite;

        hud.SetData(elf);//����hud����

        ElvesEnterAnimation();
    }

    //������붯��
    public void ElvesEnterAnimation()
    {
        image.color = orginalColor;

        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, orginalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, orginalPos.y);

        image.transform.DOLocalMoveX(orginalPos.x, 1f);
    }

    //���鹥������
    public void ElvesAttackAnimation()
    {
        var sequence = DOTween.Sequence();//���嶯������
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(orginalPos.x + 50f, 0.25f));//0.25s������50��λ
        else
            sequence.Append(image.transform.DOLocalMoveX(orginalPos.x - 50f, 0.25f));//0.25s������50��λ

        sequence.Append(image.transform.DOLocalMoveX(orginalPos.x, 0.25f));//0.25s�ڷ��س�ʼλ��
    }

    //�����ܻ�����
    public void ElvesHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));//0.1s�ڱ��
        sequence.Append(image.DOColor(orginalColor, 1f));//1s�ڱ�س�ʼ��ɫ
    }

    //�����˳�����
    public void ElvesFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(orginalPos.y-150f, 0.25f));//0.25s������150��λ
        sequence.Join(image.DOFade(0f, 0.5f));//0.5f����ʧ
    }
}
