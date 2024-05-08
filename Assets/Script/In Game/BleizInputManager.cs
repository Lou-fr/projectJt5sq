using UnityEngine;
using UnityEngine.InputSystem;

public class BleizInputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    
    public Vector2 LookInput { get; private set; } = Vector2.zero;

    public bool SpaceIsPressed {  get; private set; } =false;

    public float ZoomCameraInput { get; private set; } = 0.0f;
    public bool InvertScroll {get;private set;} = true;
    public bool MouseIsClicked {get; private set;}= false;
    public bool SprintIsPressed {get; private set;} = false;

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

        _input.player.LookPress.performed += MouseClicked;
        _input.player.LookPress.canceled += MouseClicked;

        _input.player.Sprint.performed += SprintClicked;
        _input.player.Sprint.canceled += SprintClicked;
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
        SpaceIsPressed = false;

        _input.player.LookPress.performed -= MouseClicked;
        _input.player.LookPress.canceled -= MouseClicked;

        _input.player.Sprint.performed -= SprintClicked;
        _input.player.Sprint.canceled -= SprintClicked;

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
        if(ctx.ReadValue<float>() is 1)SpaceIsPressed = true;
        else SpaceIsPressed=false;
    }
    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }
    private void MouseClicked(InputAction.CallbackContext ctx) 
    {
        if(ctx.ReadValue<float>() is 1)MouseIsClicked = true;
        else MouseIsClicked=false;
    }
    private void SprintClicked(InputAction.CallbackContext ctx)
    {
        if(ctx.ReadValue<float>() is 1)SprintIsPressed = true;
        else SprintIsPressed=false;
    }   
}
