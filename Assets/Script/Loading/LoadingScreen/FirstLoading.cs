using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GetLocal;
using BleizEntertainment.Multiplayer;
using System.Threading.Tasks;
using System;

public class FirstLoading : MonoBehaviour
{
    [SerializeField] private GameObject LogOut;
    [SerializeField] private GameObject Welcome;
    [SerializeField] private GameObject SwitchLanguage;
    [SerializeField] private TextMeshProUGUI loadingtext;
    [SerializeField] private Button PlayButton;
    [SerializeField] private GameObject sessionObject;
    [SerializeField] private GameObject error;
    public static Action<float> ServerNotAvailaible = delegate { };
    float timesTry = 0;
    void Start()
    {
        Application.targetFrameRate = 60;
        PlayButton.onClick.AddListener(OnButtonPress);
    }
    private void Awake()
    {
        PlayerMultiplayerRequester.OnFinishrequest += HandleLoadingScene;
    }
    private void OnDestroy()
    {
        PlayerMultiplayerRequester.OnFinishrequest -= HandleLoadingScene;
    }

    private void HandleLoadingScene(string response)
    {
        timesTry++;
        if (response is "con_serv") { error.SetActive(true); GameObject t = GameObject.Find("Error_server");t.GetComponent<TextMeshProUGUI>().text = GetLocalal.GetString("StartScreen", "No_Session"); loadingtext.gameObject.SetActive(false); ServerNotAvailaible?.Invoke(timesTry); LogOut.SetActive(true); return; }
        timesTry = 0;
        StartCoroutine(LoadScene());
    }

    void OnButtonPress()
    {
        SwitchLanguage.SetActive(false); LogOut.SetActive(false); loadingtext.gameObject.SetActive(true); PlayButton.gameObject.SetActive(false); Welcome.SetActive(false);
        loadingtext.text = GetLocalal.GetString("StartScreen", "Con");
        PlayerMultiplayerRequester request = new PlayerMultiplayerRequester();
        request.RequestMultiplayer();
    }
    IEnumerator LoadScene()
    {
        yield return null;
        sessionObject.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Progression " + asyncOperation.progress);
        string loadingtextLen = GetLocalal.GetString("StartScreen", "Loading");
        while (!asyncOperation.isDone)
        {
            loadingtext.text = loadingtextLen + (asyncOperation.progress * 100 + 10) + " %";
            if (asyncOperation.progress >= 0.9f)
            {
#if UNITY_STANDALONE
                string startText = GetLocalal.GetString("StartScreen", "Start");
                loadingtext.text = startText;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }
#endif
#if UNITY_ANDROID || UNITY_IOS
                string startText = GetLocalal.GetString("StartScreen", "StartPhone");
                loadingtext.text = startText;
                if (Input.touchCount > 0)
                {
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
}

