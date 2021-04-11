using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum QuestionType {
    Base,Image,Video,OX
}

[System.Serializable]
public class FlagChoice_base{
    public int FlagID;  //編號規則：步驟順位 + 檢核點順位
    public QuestionType _type;
    public List<GameObject> _option = new List<GameObject>();
}

[System.Serializable]
public class FlagChoice_Image {
    public int FlagID;  //編號規則：步驟順位 + 檢核點順位
    public QuestionType _type;
    public List<Sprite> _option = new List<Sprite>();
}

[System.Serializable]
public class FlagChoice_Video{
    public int FlagID;  //編號規則：步驟順位 + 檢核點順位
    public QuestionType _type;
    public List<RawImage> _option = new List<RawImage>();
    public List<VideoPlayer> _vp = new List<VideoPlayer>();
}

[System.Serializable]
public class FlagChoice_OX{
    public int FlagID;  //編號規則：步驟順位 + 檢核點順位
    public QuestionType _type;
    public List<Sprite> _option = new List<Sprite>();
}

[System.Serializable]
public class AllChoice{
    public List<FlagChoice_base> _flagchoice_base = new List<FlagChoice_base>();
    public List<FlagChoice_Image> _flagchoice_image = new List<FlagChoice_Image>();
    public List<FlagChoice_Video> _flagchoice_video = new List<FlagChoice_Video>();
    public List<FlagChoice_OX> _flagchoice_OX = new List<FlagChoice_OX>();
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
            for (int i = 0; i < _choices._flagchoice_base.Count; i++){
                if (_choices._flagchoice_base[i].FlagID == CurrentID){
                    foreach (GameObject _obj in _choices._flagchoice_base[i]._option){
                        if (_obj.GetComponent<ChoiceSetting>() == null) {/*UI處理*/}
                        else{
                            _obj.GetComponent<ChoiceSetting>().SetOutlineEffect(false);
                        }
                    }
                    _choices._flagchoice_base.Remove(_choices._flagchoice_base[i]);
                    break;
                }
            }
            /*前一個flag資料從list移除*/
        }
        CurrentID = _ID;
        for (int i = 0; i < _choices._flagchoice_base.Count; i++){
            if (_choices._flagchoice_base[i].FlagID == CurrentID){
                foreach (GameObject _obj in _choices._flagchoice_base[i]._option){
                    if (_obj.GetComponent<ChoiceSetting>() == null) {/*UI處理*/}
                    else{
                        _cs = _obj.GetComponent<ChoiceSetting>();
                        _cs.SetFlagID(CurrentID);
                        _cs.SetOutlineEffect(true);
                        if (_obj == _choices._flagchoice_base[i]._option[0]) _cs.SetFlagAns(true);
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
