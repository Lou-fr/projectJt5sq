using UnityEngine;
using UnityEngine.InputSystem;

public class BleizInputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    
    public float LookInput { get; private set; } = 0.0f;

    public float SpaceIsPressed {  get; private set; } =0.0f;

    public float ZoomCameraInput { get; private set; } = 0.0f;

    InputMaster _input = null;
    private void OnEnable()
    {
        _input = new InputMaster();
        _input.player.Enable();

        _input.player.movement.performed += SetMove;
        _input.player.movement.canceled += SetMove;

        _input.player.PlayerZoom.performed += SetZoom;
        _input.player.PlayerZoom.canceled += SetZoom;

        _input.player.Jump.performed += SetSpace;
        _input.player.Jump.canceled += SetSpace;

    }
    private void OnDisable()
    {
        _input.player.movement.performed -= SetMove;
        _input.player.movement.canceled -= SetMove;

        _input.player.PlayerZoom.performed -= SetZoom;
        _input.player.PlayerZoom.canceled -= SetZoom;

        _input.player.Jump.performed -= SetSpace;
        _input.player.Jump.canceled -= SetSpace;
        _input.player.Disable();
    }
    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    private void SetZoom(InputAction.CallbackContext ctx)
    {
        ZoomCameraInput = ctx.ReadValue<float>();
    }
    private void SetSpace(InputAction.CallbackContext ctx)
    {
        SpaceIsPressed = ctx.ReadValue<float>();
    }
}
