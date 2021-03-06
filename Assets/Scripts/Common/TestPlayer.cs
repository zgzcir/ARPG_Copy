﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class TestPlayer : Controller
{
    private Transform camTrans;
    private CameraController cameraController;
    private Vector3 camOffSet;
    public CharacterController CharacterController;

    private float targetBlend;
    private float currentBlend;
    public GameObject EffectSkill1;

    private float currentVelocity;

    public Transform RayCastPoint;


    public GameObject CameraPivot; //改成transform
    public Transform ChaCameraRotatePivot;

    private float targetRotation;


    private bool isJump = false;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private static readonly int IsJump = Animator.StringToHash("IsJump");

    public void Start()
    {
        if (Camera.main != null) camTrans = Camera.main.transform;
        cameraController = camTrans.GetComponent<CameraController>();
    }

    private float margin = 0.1f;

    private bool IsGrounded()
    {
        return Physics.Raycast(RayCastPoint.position, -Vector3.up, margin);
    }

    private bool isGrounded = true;
    private static readonly int Action = Animator.StringToHash("Action");

    private void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        #region kb input

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //******************************************///////////////////////////////////////////
////        float u = 0;
////      
////
////            if (CharacterController.isGrounded)
////            {
////                if (Input.GetKeyDown(PlayerCfg.Jump))
////                {                    cameraController.SetJumpState();
////                    u = Constans.PlayerJumpHeight;
////                    Ani.SetBool(IsJump, true);
////                }
////                else
////                {
////                    Ani.SetBool(IsJump, false);
////                    cameraController.SetJumpState(false);
////
////                }
////            }
////        
////        u -= Constans.Gravity * Time.deltaTime;
        //******************************************///////////////////////////////////////////

        //todo
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        InputDir = input.normalized;


        if (isMove)
        {
            if (MainSys.Instance != null && MainSys.Instance.IsNavigate)
            {
                MainSys.Instance.CancelNavGuide();
            }

            SetBlend(Constans.BlendMove);
            SetDir();
            SetMove();
        }
        else
        {
            SetBlend(Constans.BlendIdle);
        }

        #endregion

        if (MainSys.Instance != null && MainSys.Instance.IsNavigate)
        {
            SetBlend(Constans.BlendMove);
        }


        if (!currentBlend.Equals(targetBlend))
        {
            UpdateMixBlend();
        }
    }

    private void SetDir()
    {
        targetRotation = Mathf.Atan2(InputDir.x, InputDir.y) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                    ref currentVelocity, Constans.RotateSmooth);
    }

    private void SetMove()
    {
        CharacterController.Move(Time.deltaTime * Constans.PLyerMoveSpeed * transform.forward);
    }

    private void SetJump()
    {
        CharacterController.Move(Time.deltaTime * Constans.PlayerJumpHeight * transform.up);
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constans.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constans.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constans.AccelerSpeed * Time.deltaTime;
        }

        Ani.SetFloat(Blend, currentBlend);
    }

    public void CLickSkill1Button()
    {
        Ani.SetInteger(Action, 1);
        EffectSkill1.gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.9f);
        Ani.SetInteger(Action, -1);
        EffectSkill1.gameObject.SetActive(false);
    }
}