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
    
    public static void Load(Scenes scene, LoadSceneMode mode)
    {
        SceneManager.LoadScene(scene.ToString(), mode);
    }

    public static void Unload(Scenes scenes)
    {
        SceneManager.UnloadSceneAsync(scenes.ToString());
    }
}
