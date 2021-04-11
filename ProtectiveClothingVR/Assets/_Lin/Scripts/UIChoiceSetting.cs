using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChoiceSetting : MonoBehaviour{

    ChoiceManager _choiceManager;
    public bool FlagAns = false;
    public int FlagID = 0;

    void Awake(){
        _choiceManager = GameObject.Find("MissionManager").GetComponent<ChoiceManager>();
    }

    void Update(){
        
    }

    //還沒用到
    public bool GetFlagAns(){
        return FlagAns;
    }

    public void SetFlagAns(bool _state){
        FlagAns = _state;
    }

    public void SetFlagID(int _ID){
        FlagID = _ID;
    }

    //測試
    void OnMouseDown(){
        _choiceManager.CheckMatchChoice(FlagID, FlagAns);
    }

}
