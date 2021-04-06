using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour{

    public StepManager _stepManager;

    void Update(){
        //csv輸出測試
        if (Input.GetKeyDown(KeyCode.Q)) CSVManager.SetCurrentMode(0);
        if (Input.GetKeyDown(KeyCode.W)) CSVManager.SetCurrentMode(1);
        if (Input.GetKeyDown(KeyCode.E)) DEV_AppendDefaultsToReport();
        if (Input.GetKeyDown(KeyCode.R)) DEV_ResetReport();

        //步驟面板測試
        if (Input.GetKeyDown(KeyCode.A)) _stepManager.FlagCorrect();
        if (Input.GetKeyDown(KeyCode.S)) _stepManager.FlagWrong();
    }

    void DEV_AppendDefaultsToReport(){
        CSVManager.AppendToModeCSV(
                new string[7] {
                    "A1125",    //用戶代碼
                    CSVManager.GetTimeStamp(),  //受測日期與時間
                    Random.Range(0,11).ToString(),  //總測驗時間
                    Random.Range(0,11).ToString(),  //檢核點數量
                    Random.Range(0,11).ToString(),  //答對數
                    Random.Range(0,11).ToString(),  //答錯數
                    Random.Range(0,11).ToString(),  //檢核點正確率
                }
                , CSVManager.GetCurrentMode()
            );
    }

    void DEV_ResetReport(){
        CSVManager.CreateReport(CSVManager.GetCurrentMode());
        Debug.Log("<color=orange>The report has been reset...</color>");
    }


}
