using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    SoulMemberUI[] memberSlots;
    List<Enemy> enemys;

    public void Init(){
        memberSlots = GetComponentsInChildren<SoulMemberUI>();
    }

    public void SetSoulData(List<Enemy> enemys){
        this.enemys = enemys;

        for(int i = 0; i< memberSlots.Length;i++){
            if(i<enemys.Count){ memberSlots[i].SetData(enemys[i]);}
            else {memberSlots[i].gameObject.SetActive(false);}
        }

        messageText.text = "Choose the soul!";
    }

    public void UpdateMemberSelection (int selectedMember){
        for(int i = 0 ; i< enemys.Count;i++){
            if(i == selectedMember){
                memberSlots[i].SetSelected(true);
            }else memberSlots[i].SetSelected(false);
        }
    }

    public void SetMeassageText(string message){
        messageText.text = message;
    }
}
