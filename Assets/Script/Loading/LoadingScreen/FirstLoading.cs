using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using GetLocal;
using System.Threading.Tasks;
using System;
using UnityEngine.InputSystem.Controls;

public class FirstLoading : MonoBehaviour
{
#if !UNITY_SERVER
    [SerializeField] private GameObject LogOut;
    [SerializeField] private GameObject Welcome;
    [SerializeField] private GameObject SwitchLanguage;
    [SerializeField] private TextMeshProUGUI loadingtext;
    [SerializeField] private Button PlayButton;
    [SerializeField] private GameObject sessionObject;
    [SerializeField] private GameObject error;
    public static Action PreStartRelay = delegate { };
    public static Action StartTransport = delegate { };
    float timesTry = 0;
    void Start()
    {
        Application.targetFrameRate = 60;
        PlayButton.onClick.AddListener(OnButtonPress);
    }
    void Awake()
    {
        RelayHostManager.RelayInitiated += HandleLoadingScene;
    }
    public void HandleLoadingScene()
    {
        RelayHostManager.RelayInitiated -= HandleLoadingScene;
        StartCoroutine(LoadScene());
    }

    void OnButtonPress()
    {
        SwitchLanguage.SetActive(false); LogOut.SetActive(false); loadingtext.gameObject.SetActive(true); PlayButton.gameObject.SetActive(false); Welcome.SetActive(false);
        loadingtext.text = GetLocalal.GetString("StartScreen", "Con");
        PreStartRelay?.Invoke();
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Progression " + asyncOperation.progress);
        string loadingtextLen = GetLocalal.GetString("StartScreen", "Loading");
        while (!asyncOperation.isDone)
        {
            loadingtext.text = loadingtextLen + (asyncOperation.progress * 100 + 10) + " %";
            if(asyncOperation.progress < 0.9f)Debug.Log(loadingtext.text);
            if (asyncOperation.progress >= 0.9f)
            {
#if UNITY_STANDALONE
                string startText = GetLocalal.GetString("StartScreen", "Start");
                loadingtext.text = startText;
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    StartTransport?.Invoke();
                    asyncOperation.allowSceneActivation = true;
                }
#endif
#if UNITY_ANDROID || UNITY_IOS
                string startText = GetLocalal.GetString("StartScreen", "StartPhone");
                loadingtext.text = startText;
                if (Touchscreen.current.press.wasPressedThisFrame)
                {
                    StartTransport?.Invoke();
                    asyncOperation.allowSceneActivation = true;
                }
#endif
            }
            yield return null;
        }

    }
    async Task Waiter()
    {
        await Task.Delay(10);
    }
#endif
}

