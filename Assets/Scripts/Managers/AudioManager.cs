using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        [SerializeField] public AudioSource playerAudio;

        private void Awake()
        {
            //singleton
            if (Instance != null && Instance != this)    
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
