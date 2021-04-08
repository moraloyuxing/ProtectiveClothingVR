using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour{

    int BuildIndex;

    //MenuPanel宣告---Start
    private Text HeaderText;
    private string[] _title = new string[3] {
        "輸入用戶代碼",
        "選擇情境",
        "選擇模式"
    };

    public GameObject[] StepState = new GameObject[3];

    //Step1_UserID相關
    public Sprite[] UserIDCount = new Sprite[10];
    public Image[] UserIDArea;
    private Button Btn_Back;
    private Button Btn_Enter;
    private string UserIDcheck;
    int OnType = -1;
    //int QualifyRange; ->UserID是否存在(目前暫定為四位數，介於0000至9999);
    //MenuPanel宣告---End

    //IconBar & SettingPanel宣告---Start
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
    //IconBar & SettingPanel宣告---End

    //StepPanel宣告---Start
    Animator _stepPanelAnim;
    //StepPanel宣告---End

    void Awake(){
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (BuildIndex == 0){
            HeaderText = GameObject.Find("StepTitle").GetComponent<Text>();
            Btn_Back = GameObject.Find("Button_BackSpace").GetComponent<Button>();
            Btn_Enter = GameObject.Find("Button_Enter").GetComponent<Button>();
        }

        else if (BuildIndex == 1 || BuildIndex == 2) {
            _stepPanelAnim = GameObject.Find("StepPanel").GetComponent<Animator>();
        }

        SettingPanel = GameObject.Find("SettingPanel");
        SettingPanel.SetActive(false);
        _musicmanager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        _latestVolumeState = new bool[_volumeText.Length];
        for (int i = 0; i < _latestVolumeState.Length; i++) _latestVolumeState[i] = true;
    }

    void Start(){
        InitialSettingPanel();//加一個setting狀態(從globalmanager存取)
    }

    //UserID相關---Start
    //輸入數字(keyboard)
    public void EnterCount(int _count) {
        if (OnType < UserIDArea.Length) {
            OnType++;
            UserIDArea[OnType].enabled = true;
            UserIDArea[OnType].sprite = UserIDCount[_count];
            UserIDcheck = UserIDcheck + _count.ToString();
            Btn_Back.interactable = true;
            if (OnType == UserIDArea.Length-1) Btn_Enter.interactable = true;
        }
    }

    //倒退
    public void UserID_BackSpace() {
        UserIDArea[OnType].sprite = null;
        UserIDArea[OnType].enabled = false;
        OnType--;
        UserIDcheck = UserIDcheck.Remove(UserIDcheck.Length - 1);
        Btn_Enter.interactable = false;
        if (OnType<0) Btn_Back.interactable = false;
    }

    //完成並偵測是否有此ID(先以Debug顯示結果)
    public void CheckUserID() {
        int RangeCheck = int.Parse(UserIDcheck);
        //if (RangeCheck > 0 && RangeCheck < 9999) Debug.Log("登入成功");
        if (RangeCheck > 0 && RangeCheck < 6999) {
            Debug.Log("登入成功");//此行用以測試登入失敗的場合
            StepState[0].SetActive(false);
            StepState[1].SetActive(true);
            HeaderText.text = _title[1];
        }
        else Debug.Log("登入失敗，請確認用戶代碼是否正確");
    }
    //UserID相關---End

    //Menu相關---Start
    public void MenuSelect(int _menu) {
        if (_menu == 0) Debug.Log("進入操作教學(呼叫影片或跳轉場景)");
        else {
            GlobalManager.MissionType = _menu;// _menu ==1為穿戴；_menu ==2為脫除，預計用Global存(方便後續輸出csv參照)
            StepState[1].SetActive(false);
            StepState[2].SetActive(true);
            HeaderText.text = _title[2];
        }
    }
    //Menu相關---End

    //Mode相關---Start
    public void ModeSelect(int _mode) {
        //Scenemanager載入_mode的場景
        if (GlobalManager.MissionType == 1){
            if (_mode==0) Debug.Log("進入穿戴防護衣的練習模式");
            else if (_mode==1) Debug.Log("進入穿戴防護衣的測驗模式");
        }

        else if (GlobalManager.MissionType == 2) {
            if (_mode == 0) Debug.Log("進入脫除防護衣的練習模式");
            else if (_mode == 1) Debug.Log("進入脫除防護衣的測驗模式");
        }
        StepState[2].SetActive(false);
        StepState[0].SetActive(true);
        HeaderText.text = _title[0];
        SceneManager.LoadSceneAsync(1);
    }
    //Mode相關---End

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

    public void SettingPanelIO(){
        _showpanel[0] = !_showpanel[0];
        if (_showpanel[0] == true) SettingPanel.SetActive(true);
        else SettingPanel.SetActive(false);
    }
    //public void CallSettingPanel(int _type) {
    //    if (_type < _showpanel.Length) {
    //        for (int i = 0; i < _showpanel.Length; i++) {
    //            //if(_showpanel[i] == true)已開啟的panel滑出
    //            _showpanel[i] = false;
    //        }           
    //    }
    //    _showpanel[_type] = true;
    //    _musicmanager.CallSoundEffect();
    //}

    public void VolumeModify(int _type) {
        if (_type < 3){
            GlobalManager._vol[_type] -=1;
            _volumeText[_type].text = GlobalManager._vol[_type].ToString();
            //_volumeText[_type].text = _musicmanager.GetVolume(_type).ToString();
            _musicmanager.ModifyVolume(_type, GlobalManager._vol[_type]);
            //_musicmanager.ModifyVolume(_type, -0.01f);
            _musicmanager.CallSoundEffect();
        }
        else {
            GlobalManager._vol[_type-3]+=1;
            _volumeText[_type-3].text = GlobalManager._vol[_type-3].ToString();
            //_volumeText[_type-3].text = _musicmanager.GetVolume(_type-3).ToString();
            _musicmanager.ModifyVolume(_type-3, GlobalManager._vol[_type-3]);
            //_musicmanager.ModifyVolume(_type-3, 0.01f);
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
            _volumeModifyBtn[_type].interactable = true;
            _volumeModifyBtn[_type + 3].interactable = true;
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
        SceneManager.LoadSceneAsync(0);
        //叫出二次確認的panel
    }

    public void QuitVR() {
        Debug.Log("退出遊戲");
    }

    //SettingPanel宣告---End

    //StepPanel宣告---Start
    public void StepPanelIO(){
        _showpanel[1] = !_showpanel[1];
        if (_showpanel[1] == true) _stepPanelAnim.Play("StepPanel_SlideIn");
        else _stepPanelAnim.Play("StepPanel_SlideOut");
    }
    //StepPanel宣告---End

}
