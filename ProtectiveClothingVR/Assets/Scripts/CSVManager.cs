using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class CSVManager {
    private static int CurrentMode = -1;
    private static string reportDirectoryName = "Report";
    private static string[] ModeFileName = new string[2] { "PracticeMode.csv", "QuizMode.csv" };//0為練習模式；1為測驗模式
    private static string CellSeparator = ",";
    private static string[] reportHeaders = new string[7]{
        "用戶代碼",
        "受測日期與時間",
        "總測驗時間",
        "檢核點數量",
        "答對數",
        "答錯數",
        "檢核點正確率"
    };

    private static Queue<string> TempOldReport = new Queue<string>();
    static bool NeedModify = false;

    #region Interactions
    public static void AppendToModeCSV(string[] strings,int _mode) {
        VerifyDirectory(_mode);
        VerifyFile(_mode);

        //確認標頭擋是否存在
        using (StreamReader sr = new StreamReader(GetFilePath(_mode), true)){
            string line = sr.ReadLine();
            if (line != "用戶代碼,受測日期與時間,總測驗時間,檢核點數量,答對數,答錯數,檢核點正確率" || line == null) {NeedModify = true;}
        }

        if (NeedModify) {
            using (StreamReader sr = new StreamReader(GetFilePath(_mode), true)){
                bool EndofFile = false;
                while (!EndofFile){
                    string line = sr.ReadLine();
                    if (line == null){
                        EndofFile = true;
                        break;
                    }
                    TempOldReport.Enqueue(line);
                }
            }

            using (var stream = new FileStream(GetFilePath(_mode), FileMode.Truncate)){}

            using (StreamWriter sw = new StreamWriter(GetFilePath(_mode), true, Encoding.UTF8)){
                //sw.WriteLine("用戶代碼,受測日期與時間,總測驗時間,檢核點數量,答對數,答錯數,檢核點正確率");
                string finalString = "";
                for (int i = 0; i < reportHeaders.Length; i++)
                {
                    if (finalString != "")
                    {
                        finalString += CellSeparator;
                    }
                    finalString += reportHeaders[i];
                }
                sw.WriteLine(finalString);
                while (TempOldReport.Count > 0) { sw.WriteLine(TempOldReport.Dequeue()); }
            }
            NeedModify = false;
        }

        //真正寫入最新資料
        using (StreamWriter sw = new StreamWriter(GetFilePath(_mode), true, Encoding.UTF8)){
            string finalString = "";
            for (int i = 0; i < strings.Length; i++){
                if (finalString != ""){
                    finalString += CellSeparator;
                }
                finalString += strings[i];
            }
            sw.WriteLine(finalString);
        }
    }

    public static void CreateReport(int _mode) {
        using (StreamWriter sw = new StreamWriter(GetFilePath(_mode), true, Encoding.UTF8)) {
            string finalString = "";
            for (int i = 0; i < reportHeaders.Length; i++) {
                if (finalString != "") {
                    finalString += CellSeparator;
                }
                finalString += reportHeaders[i];
            }
            sw.WriteLine(finalString);
        }
    }
    #endregion

    #region Operations
    static void VerifyDirectory(int _mode) {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
    }

    static void VerifyFile(int _mode) {
        string file = GetFilePath(_mode);
        if (!File.Exists(file)) {
            CreateReport(_mode);
        }
    }
    #endregion

    #region Queries
    static string GetDirectoryPath() {
        return Application.dataPath + "/" + reportDirectoryName;
    }

    static string GetFilePath(int _mode) {
        return GetDirectoryPath() + "/" + ModeFileName[_mode];
    }

    public static void SetCurrentMode(int _mode) { CurrentMode = _mode; }
    public static int GetCurrentMode() { return CurrentMode; }
    public static string GetTimeStamp() {return System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");}
    #endregion
}
