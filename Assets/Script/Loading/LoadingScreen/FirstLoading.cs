using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadinWorld : MonoBehaviour
{
    public GameObject LogOut;
    public TextMeshProUGUI loadingtext;
    public Button PlayButton;
    void Start()
    {
        PlayButton.onClick.AddListener(OnButtonPress);
    }
    void OnButtonPress()
    {
        LogOut.SetActive(false);
        loadingtext.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(false);
        StartCoroutine(LoadScene());

    }
    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Progression " + asyncOperation.progress);
        while (!asyncOperation.isDone)
        {
            loadingtext.text = "Loading progress " + (asyncOperation.progress * 100 +10) + " %";
            if (asyncOperation.progress >=   0.9f)
            {
                loadingtext.text = "Appuiez sur la bare espace pour commencez";
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }

            }
            yield return null;
        }
    }
}
