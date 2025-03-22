using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    //[SerializeField] Color highlightColor;


    [SerializeField] Text dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] List<Text> actionTexts;

    [SerializeField] GameObject skillSelector;
    [SerializeField] List<Text> skillTexts;
    [SerializeField] GameObject skillDetail;


    [SerializeField] Text ppText;
    [SerializeField] Text typeText;


    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f /30);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)//动作选择器可视化
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableSkillSelector(bool enabled)//技能选择器可视化
    {
        skillSelector.SetActive(enabled);
        skillDetail.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for(int i=0; i<actionTexts.Count;++i)
        {
            if (i == selectedAction)
                actionTexts[i].color = Color.blue;
            else
                actionTexts[i].color = Color.black;
        }
    }

    public void UpdateSKillSelection(int selectedSkill, Skill skill)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedSkill)
                skillTexts[i].color = Color.blue;
            else
               skillTexts[i].color = Color.black;
        }

        ppText.text=$"PP {skill.PP }/{skill.Base.PP}";
        typeText.text =skill.Base.Type.ToString();
    }

    public void  SetSkillNames(List<Skill> skills)
    {
        for(int i =0;i<skillTexts.Count;i++)
        {
            if (i < skills.Count)
                skillTexts[i].text = skills[i].Base.SkillName;
            else
                skillTexts[i].text = "-";
        }
    }
}
