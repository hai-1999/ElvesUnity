using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyBag : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMember[] partyMembers;
    List<Elves> elves;

    public void Init()
    {
        partyMembers = GetComponentsInChildren<PartyMember>();
    }

    public void SetPartyData(List<Elves> elves)
    {
        this.elves = elves;

        for (int i = 0; i < partyMembers.Length; i++)
            if (i < elves.Count)
                partyMembers[i].SetData(elves[i]);
            else
                partyMembers[i].gameObject.SetActive(false);
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < elves.Count; i++)
        {
            if (i == selectedMember)
                partyMembers[i].SetSelected(true);
            else
                partyMembers[i].SetSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
