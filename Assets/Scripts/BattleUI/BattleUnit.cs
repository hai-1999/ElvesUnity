using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] ElvesBase baseElves;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Elves elf { get; set; }

    Image image;
    Vector3 orginalPos;
    Color orginalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        orginalPos = image.transform.localPosition;
        orginalColor = image.color;
    }

    public void SetUp()
    {
        elf = new Elves(baseElves, level);//创建新的elf
        if (isPlayerUnit)
            image.sprite = elf.baseElf.LeftSprite;
        else
            image.sprite = elf.baseElf.RightSprite;

        ElvesEnterAnimation();
    }

    //精灵进入动画
    public void ElvesEnterAnimation()
    {
        image.color = orginalColor;

        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, orginalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, orginalPos.y);

        image.transform.DOLocalMoveX(orginalPos.x, 1f);
    }

    //精灵攻击动画
    public void ElvesAttackAnimation()
    {
        var sequence = DOTween.Sequence();//定义动画序列
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(orginalPos.x + 50f, 0.25f));//0.25s内右移50单位
        else
            sequence.Append(image.transform.DOLocalMoveX(orginalPos.x - 50f, 0.25f));//0.25s内左移50单位

        sequence.Append(image.transform.DOLocalMoveX(orginalPos.x, 0.25f));//0.25s内返回初始位置
    }

    //精灵受击动画
    public void ElvesHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));//0.1s内变灰
        sequence.Append(image.DOColor(orginalColor, 1f));//1s内变回初始颜色
    }

    //精灵退场动画
    public void ElvesFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(orginalPos.y-150f, 0.25f));//0.25s内下移150单位
        sequence.Join(image.DOFade(0f, 0.5f));//0.5f内消失
    }
}
