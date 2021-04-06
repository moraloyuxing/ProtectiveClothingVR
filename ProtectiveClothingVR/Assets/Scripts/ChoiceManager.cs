using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FlagChoice{
    public int FlagID;  //編號規則：步驟順位 + 檢核點順位
    public List<GameObject> _mat = new List<GameObject>();
}

[System.Serializable]
public class AllChoice{
    public List<FlagChoice> _flagchoice = new List<FlagChoice>();
}

public class ChoiceManager : MonoBehaviour{

    public AllChoice _choices;
    StepManager _stepManager;
    int CurrentID = 0;
    ChoiceSetting _cs;

    void Start(){
        _stepManager = GetComponent<StepManager>();
    }

    public void SetGlowChoice(int _ID){
        if (_ID != 0){
            /*前一個flag的所有選項的rend復原*/
            for (int i = 0; i < _choices._flagchoice.Count; i++){
                if (_choices._flagchoice[i].FlagID == CurrentID){
                    foreach (GameObject _obj in _choices._flagchoice[i]._mat){
                        if (_obj.GetComponent<ChoiceSetting>() == null) {/*UI處理*/}
                        else{
                            _obj.GetComponent<ChoiceSetting>().SetOutlineEffect(false);
                        }
                    }
                    _choices._flagchoice.Remove(_choices._flagchoice[i]);
                    break;
                }
            }
            /*前一個flag資料從list移除*/
        }
        CurrentID = _ID;
        for (int i = 0; i < _choices._flagchoice.Count; i++){
            if (_choices._flagchoice[i].FlagID == CurrentID){
                foreach (GameObject _obj in _choices._flagchoice[i]._mat){
                    if (_obj.GetComponent<ChoiceSetting>() == null) {/*UI處理*/}
                    else{
                        _cs = _obj.GetComponent<ChoiceSetting>();
                        _cs.SetFlagID(CurrentID);
                        _cs.SetOutlineEffect(true);
                        if (_obj == _choices._flagchoice[i]._mat[0]) _cs.SetFlagAns(true);
                        else _cs.SetFlagAns(false);
                    }
                }
                break;
            }
        }
    }

    public void CheckMatchChoice(int _choiceID,bool _choiceAns) {

        if (_choiceID == CurrentID){
            if (_choiceAns) { _stepManager.FlagCorrect(); } //步驟內的選項正確
            else { _stepManager.FlagWrong(); }  //步驟內的選項錯誤
        }

        else {Debug.Log("步驟錯誤另計");}
    }



}
