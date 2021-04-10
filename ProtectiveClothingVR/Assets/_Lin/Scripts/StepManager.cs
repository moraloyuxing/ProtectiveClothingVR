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
    ChoiceManager _choicemanager;
    FlagManager _flagManager;
    int CurrentMissionID = 0;

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
        CallVoiceHint();
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
        _allSteps._stepData[CurrentStep]._stepMistake.text = FlagMistake.ToString();
    }

    public void CallVoiceHint(){
        Button _ActivateBtn = _allSteps._stepData[CurrentStep]._voiceHint.GetComponent<Button>();
        _ActivateBtn.interactable = false;   //當前語音提示按鈕disable
        _flagManager.HintActivate(CurrentMissionID, _ActivateBtn);
    }
}
