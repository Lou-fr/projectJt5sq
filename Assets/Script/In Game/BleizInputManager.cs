using UnityEngine;
using UnityEngine.InputSystem;

public class BleizInputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    
    public Vector2 LookInput { get; private set; } = Vector2.zero;

    public float SpaceIsPressed {  get; private set; } =0.0f;

    public float ZoomCameraInput { get; private set; } = 0.0f;

    InputMaster _input = null;
    void Awake()
    {
        RebindSaveLoad.uppdateBinding += handleLoadbidings;
    }
    void OnDestroy()
    {
        RebindSaveLoad.uppdateBinding -= handleLoadbidings;
    }

    private void handleLoadbidings()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            _input.LoadBindingOverridesFromJson(rebinds);
    }

    private void OnEnable()
    {
        _input = new InputMaster();
        _input.player.Enable();
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            _input.LoadBindingOverridesFromJson(rebinds);

        _input.player.movement.performed += SetMove;
        _input.player.movement.canceled += SetMove;

        _input.player.PlayerZoom.performed += SetZoom;
        _input.player.PlayerZoom.canceled += SetZoom;

        _input.player.Jump.performed += SetSpace;
        _input.player.Jump.canceled += SetSpace;

        _input.player.Look.performed += SetLook;
        _input.player.Look.canceled += SetLook;

    }
    private void OnDisable()
    {
        _input.player.movement.performed -= SetMove;
        _input.player.movement.canceled -= SetMove;
        MoveInput = Vector2.zero;

        _input.player.PlayerZoom.performed -= SetZoom;
        _input.player.PlayerZoom.canceled -= SetZoom;

        _input.player.Look.performed -= SetLook;
        _input.player.Look.canceled -= SetLook;
        LookInput = Vector2.zero;

        _input.player.Jump.performed -= SetSpace;
        _input.player.Jump.canceled -= SetSpace;
        SpaceIsPressed = 0f;
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
    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }
}
