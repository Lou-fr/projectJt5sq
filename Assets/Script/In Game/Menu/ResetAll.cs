using UnityEngine.InputSystem;
using UnityEngine;

public class ResetAll : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public void ResetBindings()
    {
        foreach(InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        PlayerPrefs.DeleteKey("rebinds");
    }
}
