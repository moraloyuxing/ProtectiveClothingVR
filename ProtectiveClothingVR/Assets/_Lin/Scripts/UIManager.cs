using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour{

    //IconBar & 下轄面板相關宣告---Start
    public Button[] _Iconbar = new Button[3];   //0為SettingPanel；1為StepPanel；2為StatPanel

    //SettingPanel宣告---Start
    private GameObject SettingPanel;
    private MusicManager _musicmanager;
    public Text[] _volumeText;
    private bool[] _showpanel =new bool[3]; //0為SettingPanel；1為StepPanel；2為StatPanel
    private bool[] _latestVolumeState;
    public Button[] _volumeModifyBtn;
    public Image[] _volumeIOBtn;
    public Sprite[] _volumeImg;
    public Image[] _languageIOBtn;
    public Sprite[] _languageImg;
    //SettingPanel宣告---End

    //StepPanel宣告---Start
    Animator _stepPanelAnim;
    ChoiceManager _choiceManager;
    //StepPanel宣告---End

    //StatPanel宣告---Start
    private GameObject StatPanel;
    Text Stat_UserID;
    Text Stat_MissionType;
    Text Stat_Date;
    Text Stat_Timer;
    int[] _timer = new int[3] { 0,0,0};  //0為秒；1為分；2為時
    //StatPanel宣告---End
    //IconBar & 下轄面板相關宣告---End
    void Awake(){
        _stepPanelAnim = GameObject.Find("StepPanel").GetComponent<Animator>();
        _choiceManager = GameObject.Find("MissionManager").GetComponent<ChoiceManager>();
        StatPanel = GameObject.Find("StatPanel");
        Stat_UserID = GameObject.Find("Value_UserID").GetComponent<Text>();
        Stat_MissionType = GameObject.Find("Value_MissionType").GetComponent<Text>();
        Stat_Date = GameObject.Find("Value_Date").GetComponent<Text>();
        Stat_Timer = GameObject.Find("Value_Timer").GetComponent<Text>();
        StatPanel.SetActive(false);
        SettingPanel = GameObject.Find("SettingPanel");
        SettingPanel.SetActive(false);
        _musicmanager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        _latestVolumeState = new bool[_volumeText.Length];
        for (int i = 0; i < _latestVolumeState.Length; i++) _latestVolumeState[i] = true;
    }

    void Start(){
        InitialSettingPanel();//加一個setting狀態(從globalmanager存取)
        InitialStatPanel();
        InvokeRepeating("Timer", 1.0f, 1.0f);
    }

    void Timer() {
        _timer[0]++;
        if (_timer[0] == 60) {
            _timer[0] = 0;
            _timer[1]++;
        }

        if (_timer[1] == 60) {
            _timer[1] = 0;
            _timer[2]++;
        }

        Stat_Timer.text = _timer[2] + ":" + _timer[1] + ":" + _timer[0];
    }

    //IconBar宣告---Start
    public void IO_SettingPanel(){
        _showpanel[0] = !_showpanel[0];
        if (_showpanel[0] == true){
            SettingPanel.SetActive(true);
            for (int i = 0; i < _Iconbar.Length; i++) if (i != 0) _Iconbar[i].interactable = false;
        }
        else{
            SettingPanel.SetActive(false);
            for (int i = 0; i < _Iconbar.Length; i++) _Iconbar[i].interactable = true;
        }
    }

    public void IO_StepPanel(){
        _showpanel[1] = !_showpanel[1];
        _choiceManager.StepPanelState(_showpanel[1]);
        if (_showpanel[1] == true){_stepPanelAnim.Play("StepPanel_SlideIn");}
        else{_stepPanelAnim.Play("StepPanel_SlideOut");}
    }

    public void IO_StatPanel(){
        _showpanel[2] = !_showpanel[2];
        if (_showpanel[2] == true){
            StatPanel.SetActive(true);
            for (int i = 0; i < _Iconbar.Length; i++) if (i != 2) _Iconbar[i].interactable = false;
        }
        else{
            StatPanel.SetActive(false);
            for (int i = 0; i < _Iconbar.Length; i++) _Iconbar[i].interactable = true;
        }
    }
    //IconBar宣告---End

    //SettingPanel宣告---Start
    void InitialSettingPanel() {
        for (int i = 0; i < _volumeText.Length; i++) {
            //_volumeText[i].text = ((int)(GlobalManager._volume[i]*100.0f)).ToString();  //音量text
            _volumeText[i].text = GlobalManager._vol[i].ToString();  //音量text
            _musicmanager.SetVolume(i, GlobalManager._vol[i]);   //真實音量(0~1)設定

            //是否靜音(含按鈕interactabletext顏色)
            if (GlobalManager._volumeState[i] == true){
                _latestVolumeState[i] = true;
                _musicmanager.volumeIO(i, _latestVolumeState[i]);

                //調整icon呈現等
                _volumeIOBtn[i].sprite = _volumeImg[0];
                _volumeModifyBtn[i].interactable = true;
                _volumeModifyBtn[i + 3].interactable = true;
                _volumeText[i].color = Color.white;
            }
            else {
                _latestVolumeState[i] = false;
                _musicmanager.volumeIO(i, _latestVolumeState[i]);

                //調整icon呈現等
                _volumeIOBtn[i].sprite = _volumeImg[1];
                _volumeModifyBtn[i].interactable = false;
                _volumeModifyBtn[i + 3].interactable = false;
                _volumeText[i].color = Color.gray;
            }
        }

        //語言勾選的icon
        if (GlobalManager.LanguageType == 0){
            _languageIOBtn[0].sprite = _languageImg[0];
            _languageIOBtn[1].sprite = _languageImg[1];
        }
        else {
            _languageIOBtn[0].sprite = _languageImg[1];
            _languageIOBtn[1].sprite = _languageImg[0];
        }
    }

    public void VolumeModify(int _type) {
        if (_type < 3){
            GlobalManager._vol[_type] -=1;
            _volumeText[_type].text = GlobalManager._vol[_type].ToString();
            if(GlobalManager._vol[_type]==0) _volumeModifyBtn[_type].interactable = false;
            if (GlobalManager._vol[_type] < 100) _volumeModifyBtn[_type + 3].interactable = true;
            _musicmanager.ModifyVolume(_type, GlobalManager._vol[_type]);
            _musicmanager.CallSoundEffect();
        }
        else {
            GlobalManager._vol[_type-3]+=1;
            _volumeText[_type-3].text = GlobalManager._vol[_type-3].ToString();
            if (GlobalManager._vol[_type-3] == 100) _volumeModifyBtn[_type].interactable = false;
            if (GlobalManager._vol[_type-3] > 0) _volumeModifyBtn[_type - 3].interactable = true;
            _musicmanager.ModifyVolume(_type-3, GlobalManager._vol[_type-3]);
            _musicmanager.CallSoundEffect();
        }
    }

    public void SetVolumeState(int _type) {
        if (_latestVolumeState[_type] == true) {
            _latestVolumeState[_type] = false;
            _musicmanager.volumeIO(_type, _latestVolumeState[_type]);

            //調整icon呈現等
            _volumeIOBtn[_type].sprite = _volumeImg[1];
            _volumeModifyBtn[_type].interactable = false;
            _volumeModifyBtn[_type+3].interactable = false;
            _volumeText[_type].color = Color.gray;
        }
        else {
            _latestVolumeState[_type] = true;
            _musicmanager.volumeIO(_type, _latestVolumeState[_type]);

            //調整icon呈現等
            _volumeIOBtn[_type].sprite = _volumeImg[0];
            if(GlobalManager._vol[_type]>0)_volumeModifyBtn[_type].interactable = true;
            if(GlobalManager._vol[_type]<100)_volumeModifyBtn[_type + 3].interactable = true;
            _volumeText[_type].color = Color.white;
            _musicmanager.CallSoundEffect();
        }
    }

    public void SetLanguage(int _type) {
        if (_type == 0) {
            _languageIOBtn[0].sprite = _languageImg[0];
            _languageIOBtn[1].sprite = _languageImg[1];
            Debug.Log("切換為中文版本");//切換文本
            GlobalManager.LanguageType = 0;
        }
        else {
            _languageIOBtn[0].sprite = _languageImg[1];
            _languageIOBtn[1].sprite = _languageImg[0];
            Debug.Log("切換為英文版本");//切換文本
            GlobalManager.LanguageType = 1;
        }
    }

    public void QuitMission() {
        Debug.Log("退出任務");
        GlobalManager.UserID = "";
        GlobalManager.Date = "";
        GlobalManager.MissionType = 0;
        SceneManager.LoadSceneAsync(0);
        //叫出二次確認的panel
    }
    //SettingPanel宣告---End

    //StatPanel宣告---Start
    void InitialStatPanel() {
        Stat_UserID.text = GlobalManager.UserID;
        Stat_Date.text = GlobalManager.Date;
        if (GlobalManager.MissionType == 1)Stat_MissionType.text = "穿戴防護衣";
        else Stat_MissionType.text = "脫除防護衣";
    }
    //StatPanel宣告---End

    //輸出資料相關---Start
    public string GetUserID() {
        return Stat_UserID.text;
    }

    public string GetMissionType() {
        return Stat_MissionType.text;
    }

    public string GetDate(){
        return Stat_Date.text;
    }

    public string GetTimer(){
        return Stat_Timer.text;
    }
    //輸出資料相關---End

}
