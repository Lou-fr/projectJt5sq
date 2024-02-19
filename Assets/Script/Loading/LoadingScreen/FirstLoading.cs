using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GetLocal;

public class AsyncLoadinWorld : MonoBehaviour
{
    [SerializeField] private GameObject LogOut;
    [SerializeField] private GameObject Welcome;
    [SerializeField] private GameObject SwitchLanguage;
    [SerializeField] private TextMeshProUGUI loadingtext;
    [SerializeField] private Button PlayButton;
    [SerializeField] private GameObject sessionObject;
    void Start()
    {
        Application.targetFrameRate = 60;
        PlayButton.onClick.AddListener(OnButtonPress);
    }
    void OnButtonPress()
    {
        SwitchLanguage.SetActive(false); LogOut.SetActive(false); loadingtext.gameObject.SetActive(true); PlayButton.gameObject.SetActive(false); Welcome.SetActive(false);
        StartCoroutine(LoadScene());

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
            loadingtext.text = loadingtextLen + (asyncOperation.progress * 100 +10) + " %";
            if (asyncOperation.progress >=   0.9f)
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
}
