using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 0;
    public int totalLevels = 3; //maksymalna liczba poziomow
    public SceneFader sceneFader;

    public void LoadNextLevel()
    {
        currentLevel++;

        if (currentLevel <= totalLevels)
        
        {
            sceneFader.FadeToLevel(GetSceneName(currentLevel)); //ladowanie nastepnego
        }
    }

    public void LoadEndScreen()
    {
        sceneFader.FadeToLevel("EndScreen");
    }

    public void LoadLevel(int level)
    {
        if (level <= totalLevels && level >= 0) 
        {
            currentLevel = level; //currentlevel to ten zaladowany
            sceneFader.FadeToLevel(GetSceneName(level));
        }
        else if (level > totalLevels)
        {
            LoadEndScreen();
        }
        else
        {
            Debug.LogError("Nieprawidłowy numer levelu: " + level);
        }
    }

    private string GetSceneName(int level)
    {
        switch (level)
        {
            case 0: return "StartScreen";
            case 1: return "FirstScene";
            case 2: return "SecondSceneNEW";
            case 3: return "ThirdSceneNEW";
            default: return "StartScreen";
        }
    }
}