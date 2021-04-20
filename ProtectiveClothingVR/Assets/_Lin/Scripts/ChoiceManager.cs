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
    public List<RenderTexture> _option = new List<RenderTexture>();
    public List<VideoClip> _vp = new List<VideoClip>();
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
    UIChoiceSetting _UIcs;
    public Image[] _option_img = new Image[3];
    public RawImage[] _option_rawImg = new RawImage[2];
    public VideoPlayer[] _option_vp = new VideoPlayer[2];
    public Image[] _option_OX = new Image[2];
    GameObject ImageChoicePanel;
    GameObject VideoChoicePanel;
    GameObject OXChoicePanel;

    public RectTransform[] ImgPos = new RectTransform[3];
    public RectTransform[] VpPos = new RectTransform[2];
    public RectTransform[] OXPos = new RectTransform[2];
    Vector2[] Img_baseV2 = new Vector2[3];
    Vector2[] Vp_OX_baseV2 = new Vector2[2];
    public Vector2[] Img_baseModV2 = new Vector2[3];
    public Vector2[] Vp_OX_baseModV2 = new Vector2[2];

    public Vector2[] Img_ranV2 = new Vector2[3];
    public Vector2[] Vp_OX_ranV2 = new Vector2[2];
    public Vector2[] Img_ranModV2 = new Vector2[3];
    public Vector2[] Vp_OX_ranModV2 = new Vector2[2];
    bool StepPanelIn = false;
    bool CheckOnce = false;

    void Start(){
        _stepManager = GetComponent<StepManager>();
        ImageChoicePanel = GameObject.Find("ImageChoicePanel");
        VideoChoicePanel = GameObject.Find("VideoChoicePanel");
        OXChoicePanel = GameObject.Find("OXChoicePanel");
        for (int i = 0; i < 3; i++) {
            if (i < 2) {Vp_OX_baseV2[i] = VpPos[i].anchoredPosition;}
            Img_baseV2[i] = ImgPos[i].anchoredPosition;
        }
        ImageChoicePanel.SetActive(false);
        VideoChoicePanel.SetActive(false);
        OXChoicePanel.SetActive(false);
    }

    void Update(){
        if (CheckOnce == true){
            if (ImageChoicePanel.activeInHierarchy == true) { ImgChoiceState(StepPanelIn); }
            else if (VideoChoicePanel.activeInHierarchy == true) { VpChoiceState(StepPanelIn); }
            else if (OXChoicePanel.activeInHierarchy == true) { OXChoiceState(StepPanelIn); }
        }
    }

    public void SetGlowChoice(int _ID){
        bool AlreadyGet = false;
        /*前一個flag的所有選項復原*/
        if (_ID != 0){
            for (int i = 0; i < _choices._flagchoice_base.Count && AlreadyGet == false; i++){
                if (_choices._flagchoice_base[i].FlagID == CurrentID){
                    foreach (GameObject _obj in _choices._flagchoice_base[i]._option){_obj.GetComponent<ChoiceSetting>().SetOutlineEffect(false);}
                    _choices._flagchoice_base.Remove(_choices._flagchoice_base[i]); /*前一個flag資料從list移除*/
                    AlreadyGet = true;
                    break;
                }
            }

            for (int i = 0; i < _choices._flagchoice_image.Count && AlreadyGet == false; i++){
                if (_choices._flagchoice_image[i].FlagID == CurrentID){
                    foreach (Image _obj in _option_img){_obj.sprite = null;}
                    _choices._flagchoice_image.Remove(_choices._flagchoice_image[i]); /*前一個flag資料從list移除*/
                    ImageChoicePanel.SetActive(false);
                    AlreadyGet = true;
                    break;
                }
            }

            for (int i = 0; i < _choices._flagchoice_video.Count && AlreadyGet == false; i++){
                if (_choices._flagchoice_video[i].FlagID == CurrentID){
                    foreach (RawImage _obj in _option_rawImg){_obj.texture = null;}
                    foreach (VideoPlayer _obj in _option_vp) {
                        _obj.targetTexture = null;
                        _obj.clip = null;
                    }
                    _choices._flagchoice_video.Remove(_choices._flagchoice_video[i]); /*前一個flag資料從list移除*/
                    VideoChoicePanel.SetActive(false);
                    AlreadyGet = true;
                    break;
                }
            }

            for (int i = 0; i < _choices._flagchoice_OX.Count && AlreadyGet == false; i++){
                if (_choices._flagchoice_OX[i].FlagID == CurrentID){
                    foreach (Image _obj in _option_OX) { _obj.sprite = null; }
                    _choices._flagchoice_OX.Remove(_choices._flagchoice_OX[i]); /*前一個flag資料從list移除*/
                    OXChoicePanel.SetActive(false);
                    AlreadyGet = true;
                    break;
                }
            }
        }

        //更新ID並尋找下一檢核點
        CurrentID = _ID;
        AlreadyGet = false;

        for (int i = 0; i < _choices._flagchoice_base.Count && AlreadyGet == false; i++){
            if (_choices._flagchoice_base[i].FlagID == CurrentID){
                foreach (GameObject _obj in _choices._flagchoice_base[i]._option){
                    _cs = _obj.GetComponent<ChoiceSetting>();
                    _cs.SetFlagID(CurrentID);
                    _cs.SetOutlineEffect(true);
                    if (_obj == _choices._flagchoice_base[i]._option[0]) _cs.SetFlagAns(true);
                    else _cs.SetFlagAns(false);
                }
                AlreadyGet = true;
                break;
            }
        }

        for (int i = 0; i < _choices._flagchoice_image.Count && AlreadyGet == false; i++){
            if (_choices._flagchoice_image[i].FlagID == CurrentID){
                ImageChoicePanel.SetActive(true);//開啟面板
                CheckOnce = false;
                int Count = 0;
                foreach (Image _obj in _option_img){
                    _UIcs = _obj.gameObject.GetComponent<UIChoiceSetting>();
                    _UIcs.SetFlagID(CurrentID);
                    _UIcs.SetChoiceSprite(_choices._flagchoice_image[i]._option[Count]);
                    Count++;
                    if (_obj.sprite == _choices._flagchoice_image[i]._option[0]) _UIcs.SetFlagAns(true);
                    else _UIcs.SetFlagAns(false);
                }
                //調換位置
                int _ran = Random.Range(0, 3);
                int except = _ran;
                if (StepPanelIn == false) ImgPos[_ran].anchoredPosition = Img_baseV2[0];
                else {ImgPos[_ran].anchoredPosition = Img_baseModV2[0];}

                Img_ranV2[_ran] = Img_baseV2[0];
                Img_ranModV2[_ran] = Img_baseModV2[0];
                while (_ran == except) {_ran = Random.Range(0, 3);}
                if (StepPanelIn == false) ImgPos[_ran].anchoredPosition = Img_baseV2[1];
                else ImgPos[_ran].anchoredPosition = Img_baseModV2[1];

                Img_ranV2[_ran] = Img_baseV2[1];
                Img_ranModV2[_ran] = Img_baseModV2[1];
                for (int _lastOne = 0; _lastOne < 3; _lastOne++) {
                    if (_lastOne != _ran && _lastOne != except) {
                        if (StepPanelIn == false) ImgPos[_lastOne].anchoredPosition = Img_baseV2[2];
                        else ImgPos[_lastOne].anchoredPosition = Img_baseModV2[2];
                        Img_ranV2[_lastOne] = Img_baseV2[2];
                        Img_ranModV2[_lastOne] = Img_baseModV2[2];
                        break;
                    }
                }
                //StepPanel已在畫面內
                //if (StepPanelIn == true) {
                //    for (int k = 0; k < 3; k++) {ImgPos[i].anchoredPosition = Img_ranModV2[i];}
                //}
                CheckOnce = true;
                AlreadyGet = true;
                break;
            }
        }

        for (int i = 0; i < _choices._flagchoice_video.Count && AlreadyGet == false; i++){
            if (_choices._flagchoice_video[i].FlagID == CurrentID){
                VideoChoicePanel.SetActive(true);//開啟面板
                CheckOnce = false;
                int Count = 0;
                foreach (RawImage _obj in _option_rawImg){
                    _UIcs = _obj.gameObject.GetComponent<UIChoiceSetting>();
                    _UIcs.SetFlagID(CurrentID);
                    _UIcs.SetChoiceVideo(_choices._flagchoice_video[i]._option[Count], _choices._flagchoice_video[i]._vp[Count]);
                    Count++;
                    if (_obj.texture == _choices._flagchoice_video[i]._option[0]) _UIcs.SetFlagAns(true);
                    else _UIcs.SetFlagAns(false);
                }
                //調換位置
                int _ran = Random.Range(0, 2);
                if (_ran == 0) {
                    VpPos[0].anchoredPosition = Vp_OX_baseV2[0];
                    VpPos[1].anchoredPosition = Vp_OX_baseV2[1];
                    Vp_OX_ranV2[0] = VpPos[0].anchoredPosition;
                    Vp_OX_ranV2[1] = VpPos[1].anchoredPosition;
                    Vp_OX_ranModV2[0] = Vp_OX_baseModV2[0];
                    Vp_OX_ranModV2[1] = Vp_OX_baseModV2[1];
                }
                else if (_ran == 1) {
                    VpPos[0].anchoredPosition = Vp_OX_baseV2[1];
                    VpPos[1].anchoredPosition = Vp_OX_baseV2[0];
                    Vp_OX_ranV2[0] = VpPos[0].anchoredPosition;
                    Vp_OX_ranV2[1] = VpPos[1].anchoredPosition;
                    Vp_OX_ranModV2[0] = Vp_OX_baseModV2[1];
                    Vp_OX_ranModV2[1] = Vp_OX_baseModV2[0];
                }
                //StepPanel已在畫面內
                if (StepPanelIn == true){
                    for (int k = 0; k < 2; k++) { VpPos[i].anchoredPosition = Vp_OX_ranModV2[i]; }
                }
                CheckOnce = true;
                AlreadyGet = true;
                break;
            }
        }

        //後續如果OX的圖片固定的話就可移除部分程式碼
        for (int i = 0; i < _choices._flagchoice_OX.Count && AlreadyGet == false; i++){
            if (_choices._flagchoice_OX[i].FlagID == CurrentID){
                OXChoicePanel.SetActive(true);//開啟面板
                CheckOnce = false;
                int Count = 0;
                foreach (Image _obj in _option_OX){
                    _UIcs = _obj.gameObject.GetComponent<UIChoiceSetting>();
                    _UIcs.SetFlagID(CurrentID);
                    _UIcs.SetChoiceSprite(_choices._flagchoice_OX[i]._option[Count]);
                    Count++;
                    //for (int j = 0; j < _choices._flagchoice_OX[i]._option.Count; j++) { _UIcs.SetChoiceSprite(_choices._flagchoice_OX[i]._option[j]); }
                    //_cs.SetOutlineEffect(true);
                    if (_obj.sprite == _choices._flagchoice_OX[i]._option[0]) _UIcs.SetFlagAns(true);
                    else _UIcs.SetFlagAns(false);
                    OXPos[0].anchoredPosition = Vp_OX_baseV2[0];
                    OXPos[1].anchoredPosition = Vp_OX_baseV2[1];
                    Vp_OX_ranV2[0] = OXPos[0].anchoredPosition;
                    Vp_OX_ranV2[1] = OXPos[1].anchoredPosition;
                    Vp_OX_ranModV2[0] = Vp_OX_baseModV2[0];
                    Vp_OX_ranModV2[1] = Vp_OX_baseModV2[1];
                }
                //StepPanel已在畫面內
                if (StepPanelIn == true){
                    for (int k = 0; k < 2; k++) { OXPos[i].anchoredPosition = Vp_OX_ranModV2[i]; }
                }
                CheckOnce = true;
                AlreadyGet = true;
                break;
            }
        }
    }

    public void CheckMatchChoice(int _choiceID,bool _choiceAns) {
        if (_choiceID == CurrentID){
            if (_choiceAns) { _stepManager.FlagCorrect(); } //步驟內的選項正確
            else { _stepManager.FlagWrong(); }  //步驟內的選項錯誤
        }
        else { _stepManager.StepWrong();}
    }

    public int GetFlagCount() {
        int total = _choices._flagchoice_base.Count + _choices._flagchoice_image.Count + _choices._flagchoice_video.Count + _choices._flagchoice_OX.Count;
        return total;
    }

    public void StepPanelState(bool _state) {
        StepPanelIn = _state;
    }

    //StepPanel進入時的UIChoice調整
    void ImgChoiceState(bool _state) {
        if (_state == true){
            for (int i = 0; i < 3; i++) { ImgPos[i].anchoredPosition = Vector2.Lerp(ImgPos[i].anchoredPosition, Img_ranModV2[i], 0.48f); }
        }
        else {
            for (int i = 0; i < 3; i++) { ImgPos[i].anchoredPosition = Vector2.Lerp(ImgPos[i].anchoredPosition, Img_ranV2[i], 0.48f); }
        }
    }


    void VpChoiceState(bool _state){
        if (CheckOnce == false) CheckOnce = true;
        if (_state == true){
            for (int i = 0; i < 2; i++) { VpPos[i].anchoredPosition = Vector2.Lerp(VpPos[i].anchoredPosition, Vp_OX_ranModV2[i], 0.48f); }
        }
        else{
            for (int i = 0; i < 2; i++) { VpPos[i].anchoredPosition = Vector2.Lerp(VpPos[i].anchoredPosition, Vp_OX_ranV2[i], 0.48f); }
        }
    }

    void OXChoiceState(bool _state){
        if (_state == true){
            for (int i = 0; i < 2; i++) { OXPos[i].anchoredPosition = Vector2.Lerp(OXPos[i].anchoredPosition, Vp_OX_ranModV2[i], 0.48f); }
        }
        else{
            for (int i = 0; i < 2; i++) { OXPos[i].anchoredPosition = Vector2.Lerp(OXPos[i].anchoredPosition, Vp_OX_ranV2[i], 0.48f); }
        }
    }
}
