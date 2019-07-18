﻿using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerOprateSys : BaseSystem
{
    public static PlayerOprateSys Instance;
    public bool IsPlayerControll;
    public ChaInfoPanel ChaInfoPanel;
    public TasksPanel TasksPanel;
    public MainPanel MainPanel;
    public DialogPanel DialogPanel;
    public StrengthenPanel StrengthenPanel;
    private bool isChaInfoOpen;
    private bool isTasksopen;
    private PlayerController playerController;
    private CameraController cameraController;
    private CharacterController characterController;
    private Transform chaCameraTrans;
    private int nowOpenPOPanel;
    private Vector3 chaCameraRotationOrigin;
    private Vector3 chaCameraPositionOrigin;
 
    private GuideCfg curGuideData;

    private NavMeshAgent navMeshAgent;


    public void InjectPOSysThings(PlayerController pc, CameraController cc)
    {
        playerController = pc;
        cameraController = cc;
        navMeshAgent = playerController.GetComponent<NavMeshAgent>();
        characterController = playerController.GetComponent<CharacterController>();
    }

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("PlayerOperateSys Connected");
    }

    public void SwitchPanel(BasePanel panel)
    {
        if (panel.IsOpen)
        {
            nowOpenPOPanel--;
            panel.SetPanelState(false);
            if (nowOpenPOPanel == 0)
            {
                SetMainCamreraRotateState();
                ViewSvc.Instance.SetCursorState(false);
            }
        }
        else
        {
            nowOpenPOPanel++;
            panel.SetPanelState();
            SetMainCamreraRotateState(false);
            ViewSvc.Instance.SetCursorState();
        }
    }

    public void OnSwitchChaInfoPanel(bool isOPen = true)
    {
        chaCameraTrans.gameObject.SetActive(isOPen);
    }

    private void Update()
    {
        if (IsPlayerControll)
        {
            if (Input.GetKeyDown(PlayerCfg.ChaInfoPanel))
            {
                SwitchPanel(ChaInfoPanel);
            }

            if (Input.GetKeyDown(PlayerCfg.TasksPanel))
            {
                SwitchPanel(TasksPanel);
            }
            if (Input.GetKeyDown(PlayerCfg.StrengthenPanel))
            {
                SwitchPanel(StrengthenPanel);
            }
        }

        if (isNavigate)
        {
            DetectIsArriveNavPos();
        }
    }

    public void SetMainCamreraRotateState(bool can = true)
    {
        cameraController.SetRotateState(can);
    }

    public void DisablePlayerControl()
    {
        playerController.enabled = false;
        cameraController.enabled = false;
    }

    public void EnablePlayerControl()
    {
        playerController.enabled = true;
        cameraController.enabled = true;
    }


    public void InitDefault()
    {
        //playerctrl
        playerController.Init();
        if (Camera.main != null) cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.Target = playerController.CameraPivot.transform;


        if (chaCameraTrans == null)
        {
            chaCameraTrans = GameObject.FindWithTag("ChaCamera").transform;
        }

        chaCameraTrans.gameObject.SetActive(false);
        SetStartChaCameraTrans();
    }

    public void SetPlayerMoveMobile(Vector2 dir)
    {
        playerController.InputDir = dir;
    }

    public void SetChaCameraRotate(float rotate)
    {
        var cameraPivot = playerController.ChaCameraRotatePivot.transform;
        chaCameraTrans.transform.RotateAround(cameraPivot.position, cameraPivot.up, rotate);
    }

    public void SetStartChaCameraTrans()
    {
        chaCameraRotationOrigin = chaCameraTrans.transform.localEulerAngles;
        chaCameraPositionOrigin = chaCameraTrans.transform.localPosition;
    }

    public void ResetChaCameraTrans()
    {
        chaCameraTrans.localEulerAngles = chaCameraRotationOrigin;
        chaCameraTrans.localPosition = chaCameraPositionOrigin;
    }


    private bool isNavigate;

    public bool IsNavigate => isNavigate;

    public void NavGuide(GuideCfg cfg)
    {
        if (cfg != null)
        {
            curGuideData = cfg;
        }

        navMeshAgent.enabled = true;
        if (curGuideData.npcid != -1)
        {
            float dis = Vector3.Distance(playerController.transform.position,
                curMapBaseInfo.NpcPosTrans[cfg.npcid].position);
            if (dis <= 0.5f)
            {
                isNavigate = false;
            }
            else
            {
                characterController.enabled = false;
                isNavigate = true;
                navMeshAgent.speed = Constans.PLyerMoveSpeed;
                navMeshAgent.SetDestination(curMapBaseInfo.NpcPosTrans[cfg.npcid].position);
            }
        }
    }

    public void CancelNavGuide()
    {
        GameRoot.AddTips("导航取消");
        StopNavSet();
    }

    private MapBaseInfo curMapBaseInfo;

    private void DetectIsArriveNavPos()
    {
        float dis = Vector3.Distance(playerController.transform.position,
            curMapBaseInfo.NpcPosTrans[curGuideData.npcid].position);
        if (dis <= 0.5f)
        {
            StopNavSet();
            SwitchPanel(DialogPanel);
        }
    }

    private void StopNavSet()
    {
        isNavigate = false;
        characterController.enabled = true;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
    }

    public void FreshMapBaseInfo()
    {
        curMapBaseInfo = GameObject.FindWithTag("MapRoot").GetComponent<MapBaseInfo>();
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.RspGuide;
        GameRoot.AddTips(Constans.Color("金币+", TxtColor.Red)
                         + curGuideData.coin + "  经验+" + curGuideData.exp);
        GameRoot.Instance.SetPlayerDataByGuide(data);
        MainPanel.FreshPanel();
        switch (curGuideData.actid)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
    public void EntoPLayerControll()
    {
        IsPlayerControll = true;
        MainPanel.SetPanelState();
        InitDefault();
        EnablePlayerControl();
        ViewSvc.Instance.SetCursorState(false);
        ViewSvc.Instance.AdjustDepthFieldFL(8f);

        FreshMapBaseInfo();
        //读取地图信息     TODO || Temp
       audioSvc.PlayBgAudio(Constans.BGCityHappy, true);
    }

    
    public GuideCfg GetCurMainLineData()
    {
        return curGuideData;
    }

    public void RspStrengthen(GameMsg msg)
    {
        int combatPowerPre = CommonTool.CalcuEvaluation(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrengthen(msg.RspStrengthen);
        int combatPowerNow = CommonTool.CalcuEvaluation(GameRoot.Instance.PlayerData);
        StrengthenPanel.FreshPanel();
        MainPanel.FreshPanel();
        audioSvc.PlayUIAudio(Constans.UISsuccess);
        GameRoot.AddTips(Constans.Color("战斗力",TxtColor.Green)+"提升了"+Constans.Color((combatPowerNow-combatPowerPre).ToString(),TxtColor.White));
        
    }
}