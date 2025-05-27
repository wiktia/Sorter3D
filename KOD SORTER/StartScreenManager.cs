using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void StartGame()
    {   
        Debug.Log("StartGame called");
        SceneManager.LoadScene("FirstScene"); 
    }







    public void ExitGame()
    {
        Application.Quit(); // wyjście z gry 
    }

     public void ActivateEndScreenUI()
     
    {
        Cursor.lockState = CursorLockMode.None;  
        Cursor.visible = true;
        
        GameObject endScreenPanel = GameObject.Find("EndScreenPanel");
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);  
        }
    }
}
