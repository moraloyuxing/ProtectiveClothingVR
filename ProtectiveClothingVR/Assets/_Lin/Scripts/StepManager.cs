using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StepText {
    public Image _stepArrow;
    public Text _stepTitle;
    public Text _stepFlag;
    public Text _stepMistake;
    public GameObject _voiceHint;
}

[System.Serializable]
public class AllSteps {
    public List<StepText> _stepData = new List<StepText>();
}

public class StepManager : MonoBehaviour {

    int CurrentStep = 0;    //步驟
    public AllSteps _allSteps;
    int[] _flagCount;
    int FlagProgress = 0;   //檢核點
    int FlagMistake = 0;
    int TotalMistake = 0;
    int StepMistake = 0;
    ChoiceManager _choicemanager;
    FlagManager _flagManager;
    int CurrentMissionID = 0;
    string WrongContent = "";

    void Awake() {
        _choicemanager = GetComponent<ChoiceManager>();
        _flagManager = GetComponent<FlagManager>();
        _flagCount = new int[_allSteps._stepData.Count];
        for (int i = 0; i < _allSteps._stepData.Count; i++) {
            _allSteps._stepData[i]._stepArrow.enabled = false;
            _allSteps._stepData[i]._voiceHint.SetActive(false);
            _allSteps._stepData[i]._stepTitle.color = Color.gray;
            _allSteps._stepData[i]._stepFlag.color = Color.gray;
            _allSteps._stepData[i]._stepMistake.color = Color.gray;
            char c = _allSteps._stepData[i]._stepFlag.text[3];
            _flagCount[i] = int.Parse(c.ToString());
        }
        _allSteps._stepData[0]._stepArrow.enabled = true;
        _allSteps._stepData[0]._voiceHint.SetActive(true);
        _allSteps._stepData[0]._stepTitle.color = Color.yellow;
        _allSteps._stepData[0]._stepFlag.color = Color.yellow;
        _allSteps._stepData[0]._stepMistake.color = Color.yellow;
    }

    void Start(){
        CurrentMissionID = CurrentStep * 10 + FlagProgress;
        _choicemanager.SetGlowChoice(CurrentMissionID);
        //CallVoiceHint();
    }

    public void FlagCorrect() {
        FlagProgress++;
        _allSteps._stepData[CurrentStep]._stepFlag.text = "(" + FlagProgress + "/" + _flagCount[CurrentStep] + ")";
        if (FlagProgress == _flagCount[CurrentStep]) {
            FlagProgress = 0;   //檢核點歸零
            _allSteps._stepData[CurrentStep]._stepArrow.enabled = false;
            _allSteps._stepData[CurrentStep]._voiceHint.SetActive(false);
            _allSteps._stepData[CurrentStep]._stepTitle.color = Color.white;
            _allSteps._stepData[CurrentStep]._stepFlag.color = Color.white;
            _allSteps._stepData[CurrentStep]._stepMistake.color = Color.white;
            FlagMistake = 0;
            CurrentStep++;  //步驟推進
            if (CurrentStep < _allSteps._stepData.Count){
                _allSteps._stepData[CurrentStep]._stepArrow.enabled = true;
                _allSteps._stepData[CurrentStep]._voiceHint.SetActive(true);
                _allSteps._stepData[CurrentStep]._stepTitle.color = Color.yellow;
                _allSteps._stepData[CurrentStep]._stepFlag.color = Color.yellow;
                _allSteps._stepData[CurrentStep]._stepMistake.color = Color.yellow;
            }
            else if (CurrentStep >= _allSteps._stepData.Count) {
                Debug.Log("完成所有任務");
            }
        }
        CurrentMissionID = CurrentStep * 10 + FlagProgress;
        _choicemanager.SetGlowChoice(CurrentMissionID);
        CallVoiceHint();
    }

    public void FlagWrong() {
        FlagMistake++;
        TotalMistake++;
        _allSteps._stepData[CurrentStep]._stepMistake.text = FlagMistake.ToString();

        //抓出此步驟的內容
        if (TotalMistake == 1) WrongContent = _flagManager.GetFlagContent(CurrentMissionID);
        else WrongContent = WrongContent + " / " + _flagManager.GetFlagContent(CurrentMissionID);
    }

    public void StepWrong() {
        StepMistake++;
        Debug.Log("步驟錯誤(此訊息來自)StepManager");
    }

    public void CallVoiceHint(){
        Button _ActivateBtn = _allSteps._stepData[CurrentStep]._voiceHint.GetComponent<Button>();
        _ActivateBtn.interactable = false;   //當前語音提示按鈕disable
        _flagManager.HintActivate(CurrentMissionID, _ActivateBtn);
    }

    public int GetTotalMistake() {
        return TotalMistake;
    }

    public string GetWrongContent() {
        return WrongContent;
    }

    public int GetStepMistake() {
        return StepMistake;
    }

    public int GetTotalStep() {
        return _allSteps._stepData.Count;
    }

}
