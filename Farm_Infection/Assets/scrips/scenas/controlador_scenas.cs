using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controlador_scenas : MonoBehaviour
{
    
    public void FarmInfectionNivel()
    {
        SceneManager.LoadScene("Farm_Infection");
    }
    public void DeleteScene()
    {
        SceneManager.LoadScene("DeleteScene");
    }
    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
