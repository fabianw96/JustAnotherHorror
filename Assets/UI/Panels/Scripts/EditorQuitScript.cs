using UnityEditor;
using UnityEngine;

namespace UI.Panels.Scripts
{
    public class EditorQuitScript : MonoBehaviour
    {
        public static void QuitEditor()
        {
            EditorApplication.isPlaying = false;
        } 
    }
}
