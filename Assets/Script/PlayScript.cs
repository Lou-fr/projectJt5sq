using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        Debug.Log("Verfication des fichier");
        Debug.Log("Connexion au serveur");
        Debug.Log("Chargement des graphiques");
        Debug.Log("Lancement !");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
