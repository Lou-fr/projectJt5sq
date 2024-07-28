using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartAfterTimer : MonoBehaviour
{
#if !UNITY_SERVER
    [SerializeField] private Button btn;
    [SerializeField] GameObject welcome;
    [SerializeField] GameObject lng;
    float basetime = 7.5f;
    float timeRemaining;
    bool timerRunnig = false;
    TextMeshProUGUI temp_txt;
    string temp_content;
    private void Awake()
    {
    }
    private void OnDestroy()
    {
    }

    private void HandleButtonReactivate(float retry)
    {
        Button btn1 = GameObject.Find("Ack_sever").GetComponent<Button>();
        btn1.onClick.RemoveAllListeners();
        btn1.onClick.AddListener(btn_press);
        btn.interactable = false;
        temp_txt = btn.gameObject.GetComponent<TextMeshProUGUI>();
        temp_content = temp_txt.text;
        timeRemaining = basetime * retry;
        timerRunnig = true;
    }
    void btn_press()
    {
        GameObject.Find("ERRORSERVER").SetActive(false);
        welcome.SetActive(true);
        btn.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (timerRunnig)
        {
            if (timeRemaining > 0.1)
            {
                timeRemaining -= Time.deltaTime;
                temp_txt.text = temp_content + "(" + Mathf.FloorToInt(timeRemaining % 60) + "s)";
            }
            else
            {
                Debug.Log("Timer is out");
                temp_txt.text = temp_content;
                lng.SetActive(true);
                btn.interactable = true;
                timerRunnig = false;
            }
        }
    }
#endif
}
