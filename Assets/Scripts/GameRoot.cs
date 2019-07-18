﻿using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;

    public LodingPanel LoadingPanel;
    public DynamicPanel DynamicPanel;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        CleanUIRoot();
        CommonTool.Log("Link Start!");
        Init();
    }

    private void Init()
    {
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        ViewSvc view = GetComponent<ViewSvc>();
        view.InitSvc();


        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        PlayerOprateSys playerOpratete = GetComponent<PlayerOprateSys>();
        playerOpratete.InitSys();
        EntoSceneSys entoScene = GetComponent<EntoSceneSys>();
        entoScene.InitSys();
        login.EnterLogin();
    }

    private void CleanUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        DynamicPanel.SetPanelState();
    }

    public static void AddTips(string tips)
    {
        Instance.DynamicPanel.AddTips(tips);
    }

    private PlayerData playerData = null;

    public PlayerData PlayerData => playerData;

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.PlayerData;
    }

    public void SetPlayerName(string name)
    {
        playerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.coin = data.coin;
        playerData.guideid = data.id;
        playerData.level = data.lv;
        playerData.exp = data.exp;

    }

    public void SetPlayerDataByStrengthen(RspStrengthen data)
    {
        playerData.coin = data.coin;
        playerData.crystal = data.crystal;
        playerData.pa = data.pa;
        playerData.pd = data.pd;
        playerData.sa = data.sa;
        playerData.sd = data.sd;
        playerData.strenarr = data.strenarr;
    }
}