﻿using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionSys : BaseSystem
{
    public static MissionSys Instance;
    public MissionPanel MissionPanel;
    public bool IsIn = false;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        CommonTool.Log("BattleSystem Connected");
    }

    public void EnterMission()
    {
        IsIn = true;
        OpenMissionPanel();
    }

    public void OpenMissionPanel()
    {
        MissionPanel.SetPanelState(true);
    }

    public override void SwitchPanel(BasePanel panel, int force = 0)
    {
        base.SwitchPanel(panel, force);
        BaseSwitchPanel(panel);
    }

    public void ExitMission()
    {
        IsIn = false;
        MissionPanel.SetPanelState(false);
    }

    public void RspMission(GameMsg msg)
    {
        var data = msg.RspMission;
        GameRoot.Instance.SetPlayerDataByMissionStart(data);

        ExitMission();
        MainSys.Instance.ExitMainCity();
        BattleSys.Instance.EntoBattle(data.MID);
    }
}