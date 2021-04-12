using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalManager{
    public static int MissionType = 0;  //0為預設；1為穿戴防護衣；2為脫除防護衣
    public static int LanguageType = 0; //0為中文 ; 1為英文
    public static float[] _volume = new float[3] { 0.5f,0.5f,0.5f};   //0為BGM；1為SE；2為VoiceHint，介於0~1
    public static bool[] _volumeState = new bool[3] { true,true,true};    //0為BGM；1為SE；2為Voice
    public static int[] _vol = new int[3] { 50, 50, 50 };
    public static string UserID;
    public static string Date;
}
