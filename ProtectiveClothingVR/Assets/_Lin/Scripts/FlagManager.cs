using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class FlagManager : MonoBehaviour{
    int BuildIndex;
    string ReadPath_CSV;
    int RowCount = 0;
    Dictionary<int, string> FlagDic = new Dictionary<int, string>();
    Dictionary<int, string> VoiceHintDic = new Dictionary<int, string>();
    Animator _voiceHintPanel;
    Text _voiceHintText;

    AudioSource _voiceHintAudio;
    float _voiceTimer;
    string VoicePath;
    Button _btnToCall;
    StepManager _stepManager;
    bool FirstFlag = true;

    void Awake(){
        _stepManager = GetComponent<StepManager>();
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (BuildIndex == 1){
            VoicePath = Application.streamingAssetsPath + "/Voice_PutOn";
            ReadPath_CSV = Application.streamingAssetsPath + "/PutOn_FlagName.csv";
        }
        else if (BuildIndex == 2){
            VoicePath = Application.streamingAssetsPath + "/Voice_TakeOff";
            ReadPath_CSV = Application.streamingAssetsPath + "/TakeOff_FlagName.csv";
        }
        ReadCSVFile(ReadPath_CSV);
        _voiceHintPanel = GameObject.Find("VoiceHintPanel").GetComponent<Animator>();
        _voiceHintText = GameObject.Find("VoiceHintText").GetComponent<Text>();
        _voiceHintAudio = GameObject.Find("VoiceHint").GetComponent<AudioSource>();

        DirectoryInfo dir = new DirectoryInfo(VoicePath);
        FileInfo[] info = dir.GetFiles("*wav");
        foreach (FileInfo f in info){
            string VoiceID = f.Name.Remove(f.Name.Length - 4);
            VoiceHintDic.Add(int.Parse(VoiceID), f.Name);
        }
    }

    void Update(){
        if (Time.time > 3.0f && FirstFlag == true) {
            FirstFlag = false;
            _stepManager.CallVoiceHint();
        }


        if ( _voiceTimer >0.0f){
            _voiceTimer = _voiceTimer - Time.deltaTime;

            if (_voiceTimer <= 0.0f){
                _voiceHintPanel.Play("HintPanel_FadeOut"); //語音播畢以後呼叫文字面板的動畫淡出
                _btnToCall.interactable = true; //最後將語音提示的icon重新enable
            }
        }

    }

    void ReadCSVFile(string filePath){
        StreamReader sReader = new StreamReader(filePath);
        bool EndofFile = false;
        while (!EndofFile){
            string line = sReader.ReadLine();
            if (line == null){
                EndofFile = true;
                break;
            }
            if (RowCount != 0){
                var data_values = line.Split(',');
                FlagDic.Add(int.Parse(data_values[0]), data_values[1]);
            }
            RowCount++;
        }
    }

    string GetFlagContent(int _flagID){
        if ((FlagDic.ContainsKey(_flagID) == true)) return FlagDic[_flagID];
        else return "Error!";
    }

    string GetFlagVoiceHint(int _flagID){
        if (VoiceHintDic.ContainsKey(_flagID) == true) return VoiceHintDic[_flagID];
        else return "Error!";
    }

    IEnumerator PlayHintClip(int _flagID){
        string FileName = VoicePath + "/" + GetFlagVoiceHint(_flagID);
        UnityWebRequest url = new UnityWebRequest(FileName);
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(FileName, AudioType.WAV);
        yield return www.SendWebRequest();
        _voiceHintAudio.clip = DownloadHandlerAudioClip.GetContent(www);
        _voiceTimer = _voiceHintAudio.clip.length;
        _voiceHintAudio.Play();
    }

    public void HintActivate(int _flagID, Button _btn){
        _voiceHintPanel.Play("HintPanel_FadeIn");   //呼叫上方文字面板的動畫
        _voiceHintText.text = GetFlagContent(_flagID);  //替換文字內容(任務代碼當key抓出dic的string置入)
        _btnToCall = _btn;
        StartCoroutine(PlayHintClip(_flagID));//語音提示的檔案path需分流，檔名以任務代碼命名
    }
}