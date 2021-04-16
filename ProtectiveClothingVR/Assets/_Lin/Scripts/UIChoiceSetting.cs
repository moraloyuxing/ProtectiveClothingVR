using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class UIChoiceSetting : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler{

    ChoiceManager _choiceManager;
    Image _choiceSprite;
    RawImage _rawImage;
    VideoPlayer _vPlayer;
    public bool FlagAns = false;
    public int FlagID = 0;
    bool _scaling = false;
    RectTransform _rect;
    Vector2 TestScaling = new Vector2(0.48f, 0.48f);

    void Awake(){
        _rect = GetComponent<RectTransform>();
        _choiceManager = GameObject.Find("MissionManager").GetComponent<ChoiceManager>();
        if(GetComponent<Image>() != null)_choiceSprite = GetComponent<Image>();
        if (GetComponent<RawImage>() != null) _rawImage = GetComponent<RawImage>();
        if (GetComponent<VideoPlayer>() != null) _vPlayer = GetComponent<VideoPlayer>();
    }

    void Update(){
        if (_scaling == true) _rect.localScale = Vector2.Lerp(_rect.localScale, TestScaling,0.4f);
        else _rect.localScale = Vector2.Lerp(_rect.localScale, new Vector2(0.4f,0.4f), 0.4f);
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

    //測試(之後更替為VR手把的接口)
    public void OnPointerClick(PointerEventData pointerEventData) {
        _choiceManager.CheckMatchChoice(FlagID, FlagAns);
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        _scaling = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData){
        _scaling = false;
    }
}
