﻿using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class BattleControllPanel : BasePanel
{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirHandle;
    public Text txtLv;
    public Image imgHp;
    public Text txtHp;
    public Text txtName;
    public Transform expPrgsTrans;
    public Image impPower;
    public Text txtPower;


    public Image imgSk1CD;
    public Text txtSk1CD;
    public Image imgSk2CD;
    public Text txtSk2CD;
    public Image imgSk3CD;
    public Text txtSk3CD;


    private Vector2 startPos;
    private Vector2 defaultPos;
    private float pointDis;

    public Vector2 currentDir = Vector2.zero;

    public override void Init()
    {
        base.Init();
        RegisterUIEvents();
        sk1CDTime = resSvc.GetSkillCfg(101).CDTime / 1000.0f;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        pointDis = Screen.height * 1.0f / Constans.ScreenStandardHeight * Constans.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirHandle, false);
        FreshPanel();
    }

    public void FreshPanel()
    {
        PlayerData playerData = GameRoot.Instance.PlayerData;
        float totalHp = 800;
        int nowHp = playerData.HP;
//        SetText(txtHp, (nowHp / totalHp));
        SetText(txtHp, nowHp + "/" + totalHp);
        imgHp.fillAmount = nowHp / totalHp;
        var maxPower = CommonTool.GetPowerLimit(playerData.Level);
        impPower.fillAmount = 1.0f * playerData.Power / maxPower;
        SetText(txtPower, playerData.Power + "/" + maxPower);
        float posX = imgHp.fillAmount * imgHp.GetComponent<RectTransform>().sizeDelta.x;

        txtHp.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        SetText(txtLv, "Lv:" + playerData.Level);
        SetText(txtName, playerData.Name);
        //exp
        float expPrgVal = playerData.Exp * 1.0f / CommonTool.GetExpUpValue(playerData.Level) * 100;
        int index = (int) expPrgVal / 10;
        for (int i = 0; i < expPrgsTrans.childCount; i++)
        {
            Image img = expPrgsTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0.01f;
            }
        }

//        float globalRate = 1.0f * Constans.ScreenStandardHeight / Screen.height;
        float width = Screen.width * Constans.GloableScreenRate;
        GridLayoutGroup gridLayoutGroup = expPrgsTrans.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2((width - 10 * 9) / 10f, 18);
    }

    protected override void RegisterUIEvents()
    {
        OnClickDown(imgTouch.gameObject, eventDate =>
        {
            startPos = eventDate.position;
            SetActive(imgDirHandle);
            imgDirBg.transform.position = eventDate.position;
        });
        OnCLickUp(imgTouch.gameObject, eventData =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirHandle, false);
            imgDirHandle.transform.localPosition = Vector2.zero;
            currentDir = Vector2.zero;
            BattleSys.Instance.SetSelfPlayerMoveMobileDir(currentDir);
            //    MainCitySys.Instance.SetPlayerMoveMobile(Vector2.zero);
        });
        OnClickDrag(imgTouch.gameObject, eventData =>
        {
            Vector2 dir = eventData.position - startPos;
            float len = dir.magnitude;
            Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
            imgDirHandle.transform.position = startPos + clampDir;
            currentDir = dir.normalized;
            BattleSys.Instance.SetSelfPlayerMoveMobileDir(currentDir);
            //     MainCitySys.Instance.SetPlayerMoveMobile(dir);
        });
    }

    public void OnClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }

    private bool isSk1CD;
    private float sk1CDTime;
    private float sk1RunCDTime;
    private float sk1FillCount;

    public void OnClickSkill1()
    {
        if (isSk1CD) return;
        BattleSys.Instance.ReqReleaseSkill(1);
        isSk1CD = true;
        SetActive(imgSk1CD);
        imgSk1CD.fillAmount = 1;
        sk1RunCDTime = (int) sk1CDTime;
        SetText(txtSk1CD, sk1RunCDTime);
    }

    public void OnClickSkill2()
    {
    }


    public void OnClickSkill3()
    {
    }

    //Test
    public void ClickResetCfgs()
    {
        resSvc.Reset();
    }

    private float secondOneCountSk1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickSkill1();
        }

        float deltaTime = Time.deltaTime;
        if (isSk1CD)
        {
            #region sk1

            sk1FillCount += deltaTime;
            if (sk1FillCount >= sk1CDTime)
            {
                isSk1CD = false;
                sk1FillCount = 0;
                SetActive(imgSk1CD, false);
            }
            else
            {
                imgSk1CD.fillAmount = 1 - sk1FillCount / sk1CDTime;
            }

            secondOneCountSk1 += deltaTime;
            if (secondOneCountSk1 >= 1)
            {
                secondOneCountSk1 -= 1;
                sk1RunCDTime -= 1;

                SetText(txtSk1CD, sk1RunCDTime);
            }
        }

        #endregion
    }
}