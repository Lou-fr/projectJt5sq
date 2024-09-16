using UnityEngine;

public class LoadingBetweenPosition : MonoBehaviour
{
    private GameObject loadingScreen;
    bool IsPositionSetup;
    bool IsMenuReady;
    void Start()
    {
        Sensivity_Controller.MenuReady += handleMenuReady;
        loadingScreen = this.gameObject;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Sensivity_Controller.MenuReady -= handleMenuReady;

    }

    private void handleMenuReady()
    {
        IsMenuReady = true;
        handleRemoveLoadingScreen();
    }

    private void handlePositionSetup()
    {
        IsPositionSetup = true;
        handleRemoveLoadingScreen();
    }

    private void handleRemoveLoadingScreen()
    {
        if (IsMenuReady && IsPositionSetup) loadingScreen.SetActive(false);
    }
}
