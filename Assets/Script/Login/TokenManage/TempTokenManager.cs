using GetLocal;
using Login.Library.Resonses;
using System;
using TMPro;
using TokenManage;
using UnityEngine;
using UnityEngine.Networking;

namespace TempTokenManager
{

    public class TokenManager : MonoBehaviour
    {
        [SerializeField] GameObject authmanager;
        [SerializeField] LogInManager LogInManager;
        [SerializeField] TextMeshProUGUI _Username;
        [SerializeField] GameObject ConnectionMenu;
        [SerializeField] GameObject LogoutButton;
        [SerializeField] GameObject PlayButton;
        [SerializeField] GameObject ErrorTokenPopup;
        [SerializeField] GameObject LogOutConfirm;
        [SerializeField][HideInInspector] AuthResponse token;
        void Start()
        {
            token = TokenLoader.LoadToken();
            if (token != null)
            {
                var (succes, _username, ResponseCode) = TokenVerification.VerifToken();
                if (succes == true)
                {
                    string temp = GetLocalal.GetString("StartScreen", "Welcome");
                    _Username.text = temp + _username;
                    _Username.gameObject.SetActive(true);
                    ConnectionMenu.SetActive(false); LogoutButton.SetActive(true); PlayButton.SetActive(true);
                }
                else
                {
                    if (ResponseCode == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogWarning("Token no more valid");
                        ConnectionMenu.SetActive(false); ErrorTokenPopup.SetActive(true); _Username.gameObject.SetActive(false); this.enabled = false;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnLogoutButton()
        {
            ConnectionMenu.SetActive(false); LogoutButton.SetActive(false); PlayButton.SetActive(false); LogOutConfirm.SetActive(true); _Username.gameObject.SetActive(false);

        }
        public void OnAcknowledgeButton()
        {
            TokenLoader.UnloadToken();
            ConnectionMenu.SetActive(true); ErrorTokenPopup.SetActive(false); authmanager.SetActive(true);
        }
        public void OnLogOutConfirmButton()
        {
            TokenLoader.UnloadTempToken();
            TokenLoader.UnloadToken();
            ConnectionMenu.SetActive(true); LogOutConfirm.SetActive(false); authmanager.SetActive(true); LogInManager.enabled = true;
        }
        public void OnLogoutCancelButton()
        {
            LogoutButton.SetActive(true); PlayButton.SetActive(true); LogOutConfirm.SetActive(false); _Username.gameObject.SetActive(true);
        }
    }
}