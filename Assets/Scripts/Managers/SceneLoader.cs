using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoader : MonoBehaviour
    {
        public enum Scenes
        {
            MainMenu,
            SampleScene,
            GameOver,
            GameWin
        }
    
        public static void Load(Scenes scenes, LoadSceneMode mode)
        {
            SceneManager.LoadScene(scenes.ToString(), mode);
        }

        public static void Unload(Scenes scenes)
        {
            SceneManager.UnloadSceneAsync(scenes.ToString());
        }

        public static void LoadGameOver()
        {
            SceneManager.LoadScene((int)Scenes.GameOver);
        }
        
        public static void LoadGameWin()
        {
            SceneManager.LoadScene((int)Scenes.GameWin);
        }
    }
}
