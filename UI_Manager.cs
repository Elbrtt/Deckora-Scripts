using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
   
    Image cdPanel;

    void Start(){
        
    }

    public void LoadAfterGameScene(){
        SceneManager.LoadScene("AfterGamePlay_Menu");
    }

    public void LoadMainMenuScene(){
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCamDetection(){
        SceneManager.LoadScene("CamDetection");
    }


    public void Exit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #else
            Application.Quit(); 
        #endif
    }

    void Update()
    {
        
    }
}
