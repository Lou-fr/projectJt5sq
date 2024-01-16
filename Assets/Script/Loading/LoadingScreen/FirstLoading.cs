using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GetLocal;

public class AsyncLoadinWorld : MonoBehaviour
{
    [SerializeField] GameObject LogOut;
    [SerializeField] GameObject Welcome;
    [SerializeField] GameObject SwitchLanguage;
    [SerializeField] TextMeshProUGUI loadingtext;
    [SerializeField] Button PlayButton;
    [SerializeField] TokenManager tokenmanager;
    [SerializeField] GameObject tokenObjectManager;
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
        tokenmanager.enabled = true;tokenObjectManager.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Progression " + asyncOperation.progress);
        string loadingtextLen = GetLocalal.GetString("StartScreen", "Loading");
        string startText = GetLocalal.GetString("StartScreen", "Start");
        while (!asyncOperation.isDone)
        {
            loadingtext.text = loadingtextLen + (asyncOperation.progress * 100 +10) + " %";
            if (asyncOperation.progress >=   0.9f)
            {
                loadingtext.text = startText;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }

            }
            yield return null;
        }
    }
}
