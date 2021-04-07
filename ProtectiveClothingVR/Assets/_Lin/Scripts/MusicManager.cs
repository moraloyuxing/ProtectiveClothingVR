using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour{

    private AudioSource BGM;
    private AudioSource SoundEffect;
    private AudioSource VoiceTutorial;
    public AudioSource[] _audio;   //  0:BGM ; 1:SoundEffect ; 2:VoiceTutorial
    private float[] _latestvolume;

    void Awake(){
        //BGM = transform.Find("BGM").GetComponent<AudioSource>();
        //SoundEffect = transform.Find("SoundEffect").GetComponent<AudioSource>();
        //VoiceTutorial = transform.Find("VoiceTutorial").GetComponent<AudioSource>();
        //_audio[0] = transform.Find("BGM").GetComponent<AudioSource>();
        //_audio[1] = transform.Find("SoundEffect").GetComponent<AudioSource>();
        //_audio[2] = transform.Find("VoiceTutorial").GetComponent<AudioSource>();
        _latestvolume = new float[_audio.Length];
    }

    public void CallSoundEffect() {
        _audio[1].Play();
    }

    public int GetVolume(int _type) {
        return (int)(_audio[_type].volume * 100.0f);
    }

    public void ModifyVolume(int _type,int _num) {
        //_audio[_type].volume += _num;
        _audio[_type].volume = _num * 0.01f;
        _latestvolume[_type] = _audio[_type].volume;
        GlobalManager._volume[_type] = _latestvolume[_type];
    }

    public void volumeIO(int _type,bool _state) {
        if (_state == true){_audio[_type].volume = _latestvolume[_type];}
        else {_audio[_type].volume = 0.0f;}
        GlobalManager._volumeState[_type] = _state;
    }

    public void SetVolume(int _type,int _vol) {
        _audio[_type].volume = _vol*0.01f;
        //_audio[_type].volume = _vol;
        _latestvolume[_type] = _audio[_type].volume;
        //GlobalManager._volume[_type] = _latestvolume[_type];
    }

}
