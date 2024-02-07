using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes
    {
        MainMenu,
        SampleScene,
        GameOver,
        PauseMenu,
        SettingsMenu,
    }
    
    public static void Load(Scenes scenes, LoadSceneMode mode)
    {
        SceneManager.LoadScene(scenes.ToString(), mode);
    }

    public static void Unload(Scenes scenes)
    {
        SceneManager.UnloadSceneAsync(scenes.ToString());
    }
}
