using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonComputer : MonoBehaviour
{
    [SerializeField] private InputActionReference _look,_press;
#if UNITY_STANDALONE
    private void Update()
    {
        if (_press.action.IsPressed())
        {
            _look.action.Enable();
        }
        else
        {
            _look.action.Disable();
        }
    }
#endif
}