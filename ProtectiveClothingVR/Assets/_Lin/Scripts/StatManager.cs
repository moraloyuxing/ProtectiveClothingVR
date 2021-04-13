using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour{

    UIManager _uiManager;
    ChoiceManager _choiceManager;
    StepManager _stepManager;

    void Awake(){
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _choiceManager = GameObject.Find("MissionManager").GetComponent<ChoiceManager>();
        _stepManager = GameObject.Find("MissionManager").GetComponent<StepManager>();
    }

    string FlagPercentage() {
        int _upper = _choiceManager.GetFlagCount();
        int _lower = _upper + _stepManager.GetTotalMistake();
        float _result = (_upper / _lower) * 100.0f;
        return _result.ToString() + "%";
    }

    string StepPercentage() {
        int _upper = _stepManager.GetTotalStep();
        int _lower = _upper + _stepManager.GetStepMistake();
        float _result = (_upper / _lower) * 100.0f;
        return _result.ToString() + "%";
    }

    void DEV_AppendDefaultsToReport_Practice(){
        CSVManager.AppendToModeCSV(
                new string[9] {
                    _uiManager.GetUserID(),    //用戶代碼 原"A1125"
                    _uiManager.GetMissionType(),//情境類型
                    _uiManager.GetDate(),  //受測日期與時間 原CSVManager.GetTimeStamp(),
                    _uiManager.GetTimer(),  //總測驗時間 原Random.Range(0,11).ToString()
                    _choiceManager.GetFlagCount().ToString(),  //檢核點數量 原Random.Range(0,11).ToString()
                     _choiceManager.GetFlagCount().ToString(),  //答對數 原Random.Range(0,11).ToString()
                    _stepManager.GetTotalMistake().ToString(),  //答錯數 原Random.Range(0,11).ToString()
                    FlagPercentage(),  //檢核點正確率
                    _stepManager.GetWrongContent()  //錯誤的檢核點名稱
                }
                , CSVManager.GetCurrentMode()
            );
    }

    void DEV_AppendDefaultsToReport_Quiz(){
        CSVManager.AppendToModeCSV(
                new string[10] {
                    _uiManager.GetUserID(),    //用戶代碼 原"A1125"
                    _uiManager.GetMissionType(),//情境類型
                    _uiManager.GetDate(),  //受測日期與時間 原CSVManager.GetTimeStamp(),
                    _uiManager.GetTimer(),  //總測驗時間 原Random.Range(0,11).ToString()
                    _choiceManager.GetFlagCount().ToString(),  //檢核點數量 原Random.Range(0,11).ToString()
                     _choiceManager.GetFlagCount().ToString(),  //答對數 原Random.Range(0,11).ToString()
                    _stepManager.GetTotalMistake().ToString(),  //答錯數 原Random.Range(0,11).ToString()
                    FlagPercentage(),  //檢核點正確率
                    _stepManager.GetWrongContent(),  //操作步驟(尚未定義)
                    StepPercentage()   //操作步驟正確率
                }
                , CSVManager.GetCurrentMode()
            );
    }

    void DEV_ResetReport(){
        CSVManager.CreateReport(CSVManager.GetCurrentMode());
        Debug.Log("<color=orange>The report has been reset...</color>");
    }
}
