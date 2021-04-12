using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class UIChoiceSetting : MonoBehaviour,IPointerClickHandler{

    ChoiceManager _choiceManager;
    Image _choiceSprite;
    RawImage _rawImage;
    VideoPlayer _vPlayer;
    public bool FlagAns = false;
    public int FlagID = 0;

    void Awake(){
        _choiceManager = GameObject.Find("MissionManager").GetComponent<ChoiceManager>();
        if(GetComponent<Image>() != null)_choiceSprite = GetComponent<Image>();
        if (GetComponent<RawImage>() != null) _rawImage = GetComponent<RawImage>();
        if (GetComponent<VideoPlayer>() != null) _vPlayer = GetComponent<VideoPlayer>();
    }

    void Update(){
        
    }

    //還沒用到
    public bool GetFlagAns(){
        return FlagAns;
    }

    public void SetFlagAns(bool _state){
        FlagAns = _state;
    }

    public void SetFlagID(int _ID){
        FlagID = _ID;
    }

    public void SetChoiceSprite(Sprite _sprite) {
        _choiceSprite.sprite = _sprite;
    }

    public void SetChoiceVideo(RenderTexture _raw,VideoClip _vp) {
        _rawImage.texture = _raw;
        _vPlayer.clip = _vp;
        _vPlayer.targetTexture = _raw;
    }

    //測試
    public void OnPointerClick(PointerEventData pointerEventData) {
        _choiceManager.CheckMatchChoice(FlagID, FlagAns);
    }
}
