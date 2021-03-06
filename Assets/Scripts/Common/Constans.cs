﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Constans
{
 /// <summary>
 /// 场景加载名字
 /// </summary>
 public const string SceneMain = "SceneMainCity";

 public const int MainCityMapPreId = 1000;
 public const int MainCityMapPlayId = 1001;
/// <summary>
/// 背景音乐
/// </summary>
 public const string BGLogin = "bgLogin";
public const string BGCityHappy = "bgCityHappy";
 /// <summary>
 /// ui点击音效
 /// </summary>
 public const string UILoginBtn = "uiLoginBtn";
 public const string UIClickBtn = "uiClickBtn";
 public const string UISsuccess = "uiSuc";

 /// <summary>
 /// 人物音效
 /// </summary>
 public const string PlayerHurtAss = "hurtass";
 
 /// <summary>
 /// screen
 /// </summary>
 public const int ScreenStandardWidth = 1920;
 public const int ScreenStandardHeight = 1080;


 
 public static float GloableScreenRate=> 1.0f * ScreenStandardHeight / Screen.height;
 
 public const int ScreenOPDis = 140;

 /// <summary>
 /// move speed
 /// </summary>
 public const int PLyerMoveSpeed =7;
 public const float PlayerJumpHeight = 20f;
 public const int MonsterMoveSpeed = 3;
 public const int CamRotateSpeed = 15;

 /// <summary>
 /// 角色运动平滑
 /// </summary>
 public const float AccelerSpeed = 5;
 public const float AccelerHPSpeed = 0.35f;
 public const float RotateSmooth = 0.05f;
 
/// <summary>
/// 动画状态机参数
/// </summary>
 public const float BlendIdle = 0;
 public const float  BlendMove = 1;
 public const int ActionDefault = -1;
 public const int ActionBorn = 0;
 public const int ActionDie =100;
 public const int ActionHit =101;

 /// <summary>
 /// 动画时长
 /// </summary>
 public const int DieAniLength = 2300;

 /// <summary>
 /// Cam
 /// </summary>
 public const float CamClampUp = 60;
 public const float CamClampDown = -30;


/// <summary>
/// PanelOperate
/// </summary>
 public const float ChaInFoPanelRotateSpeed = 3.5f;

/// <summary>
/// UI COLOR
/// </summary>
//public const string MainlineTaskColor = "#e91e63";
//public const string SidelineTaskColor = "#
public const string StrenStarDarkColor = "#A6A6A6";
public const string StrenStarLightColor = "#0063FF";
public const string ChatPanelTypeNonSelected = "#3DA8FF";
public const string ChatPanelTypeSelected = "#01f901";



/// <summary>
/// string color
/// </summary>
private const string ColorRed = "<color=#ba574e>";
private const string ColorWhite = "<color=#00ff00ff>";
private const string ColorBlue = "<color=#76b9ed>";
private const string ColorGreen = "<color=#59DE3E>";
private const string ColorEnd = "</color>";

/// <summary>
/// ComboSpace
/// </summary>
public const int ComboSpace = 2000; 

public static string Color(string str, TxtColor c)
{
 string result = "";
 switch (c)
 {
  case TxtColor.Red:
   result = ColorRed + str + ColorEnd;
   break;
  case TxtColor.Blue:
   result = ColorBlue + str + ColorEnd;
   break;
  case TxtColor.Green:
   result =  ColorGreen+ str + ColorEnd;
   break;  case TxtColor.White:
   result = ColorWhite + str + ColorEnd;
   break;
 }
 return result;
}

/// <summary>
/// 物理
/// </summary>
public const float Gravity = 9.8f;
}
public enum  TxtColor
{Red,
 Blue,
 White,
 Green
}

public enum DamageType
{
 AD=1
 ,
 AP=2
}

public enum EntityType
{
 None,
 Player,
 Monster
}

public enum EntityState
{
 None,
 ButyState,
}

public enum MonsterType
{
 None,
 Normal=1,
 Boss=2
}
