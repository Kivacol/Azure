using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JellyCtrl : MonoBehaviour
{
    [Header("InputAct")]
    //控制器
    [SerializeField] private Vector2 moveInputValue;
    private PlayerInputActions controls;

    private JellyJump jellyJump;
    private JellyDash jellyDash;
    JellyWallJump jellyWallJump;

    void Awake()
    {
        jellyJump = gameObject.GetComponent<JellyJump>();
        jellyDash = gameObject.GetComponent<JellyDash>();
        jellyWallJump = GetComponent<JellyWallJump>();

        controls = new PlayerInputActions();

        controls.GamePad.Move.performed += ctx => moveInputValue = ctx.ReadValue<Vector2>();
        controls.GamePad.Move.canceled += ctx => moveInputValue = Vector2.zero;

        controls.GamePad.Jump.started += ctx => {jellyWallJump.WallJump(); jellyJump.Jump(); };
        controls.GamePad.Jump.canceled += ctx => jellyJump.isNotJump();

        controls.GamePad.Float.started += ctx => jellyJump.UseFloat();
        controls.GamePad.Float.canceled += ctx => jellyJump.RecoverFloat();

        controls.GamePad.Dash.started += ctx => jellyDash.UseDash();
    }

    // input要用的
    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

}
