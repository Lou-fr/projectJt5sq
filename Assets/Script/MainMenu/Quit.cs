using UnityEngine;

public class Quit : MonoBehaviour
{
    public void OnButtonPress()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
