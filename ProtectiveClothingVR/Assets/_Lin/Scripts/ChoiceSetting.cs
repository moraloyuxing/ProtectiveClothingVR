using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceSetting : MonoBehaviour{

    Outline _outlineEffect;
    ChoiceManager _choiceManager;
    public bool FlagAns = false;
    public int FlagID = 0;
    MeshRenderer _mat;

    Vector3 mOffset;
    float mZcoord;

    void Awake(){
        _outlineEffect = GetComponent<Outline>();
        _outlineEffect.enabled = false;
        _choiceManager = GameObject.Find("MissionManager").GetComponent<ChoiceManager>();
        _mat = GetComponent<MeshRenderer>();
        mZcoord = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void Update(){
        //可做調色、粗細、透明度
    }

    public void SetOutlineEffect(bool _state) {
        if (_state == true) {
           // _outlineEffect.enabled = true;
            _mat.materials[0].SetFloat("Vector1_20EB6C2C",2.0f);
        } 
        else {
            //_outlineEffect.enabled = false;
            _mat.materials[0].SetFloat("Vector1_20EB6C2C", Mathf.Infinity);
        }
       
    }

    //還沒用到
    public bool GetFlagAns() {
        return FlagAns;
    }

    public void SetFlagAns(bool _state) {
        FlagAns = _state;
    }

    public void SetFlagID(int _ID) {
        FlagID = _ID;
    }

    //測試
    void OnMouseDown(){
        //_choiceManager.CheckMatchChoice(FlagID,FlagAns);
        if (FlagAns == false) _choiceManager.CheckMatchChoice(FlagID, FlagAns);
        else {
            //觸發TriggerZone
            Debug.Log("this?");
            _choiceManager.IO_TriggerZone(FlagID,true);
        }
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag(){
        transform.position = GetMouseWorldPos() + mOffset;
    }

    void OnMouseUp(){
        _choiceManager.IO_TriggerZone(FlagID, false);
    }

    Vector3 GetMouseWorldPos() {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZcoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "TriggerZone" && FlagAns == true) {
            _choiceManager.SetTriggerZoneCounter();
            other.gameObject.SetActive(false);
        }
    }

}
