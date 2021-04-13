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

    void Start(){
        _stepManager = GetComponent<StepManager>();
        ImageChoicePanel = GameObject.Find("ImageChoicePanel");
        VideoChoicePanel = GameObject.Find("VideoChoicePanel");
        OXChoicePanel = GameObject.Find("OXChoicePanel");
        ImageChoicePanel.SetActive(false);
        VideoChoicePanel.SetActive(false);
        OXChoicePanel.SetActive(false);
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
                int Count = 0;
                foreach (Image _obj in _option_img){
                    _UIcs = _obj.gameObject.GetComponent<UIChoiceSetting>();
                    _UIcs.SetFlagID(CurrentID);
                    _UIcs.SetChoiceSprite(_choices._flagchoice_image[i]._option[Count]);
                    Count++;
                    if (_obj.sprite == _choices._flagchoice_image[i]._option[0]) _UIcs.SetFlagAns(true);
                    else _UIcs.SetFlagAns(false);
                }
                AlreadyGet = true;
                break;
            }
        }

        for (int i = 0; i < _choices._flagchoice_video.Count && AlreadyGet == false; i++){
            if (_choices._flagchoice_video[i].FlagID == CurrentID){
                VideoChoicePanel.SetActive(true);//開啟面板
                int Count = 0;
                foreach (RawImage _obj in _option_rawImg){
                    _UIcs = _obj.gameObject.GetComponent<UIChoiceSetting>();
                    _UIcs.SetFlagID(CurrentID);
                    _UIcs.SetChoiceVideo(_choices._flagchoice_video[i]._option[Count], _choices._flagchoice_video[i]._vp[Count]);
                    Count++;
                    //for (int j = 0; j < _choices._flagchoice_video[i]._option.Count; j++) { _UIcs.SetChoiceVideo(_choices._flagchoice_video[i]._option[j], _choices._flagchoice_video[i]._vp[j]); }
                    //_cs.SetOutlineEffect(true);
                    if (_obj.texture == _choices._flagchoice_video[i]._option[0]) _UIcs.SetFlagAns(true);
                    else _UIcs.SetFlagAns(false);
                }
                AlreadyGet = true;
                break;
            }
        }

        //後續如果OX的圖片固定的話就可移除部分程式碼
        for (int i = 0; i < _choices._flagchoice_OX.Count && AlreadyGet == false; i++){
            if (_choices._flagchoice_OX[i].FlagID == CurrentID){
                OXChoicePanel.SetActive(true);//開啟面板
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
                }
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



}
