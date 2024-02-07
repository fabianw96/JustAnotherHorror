using System.Collections.Generic;
using Interactables;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;
        public List<Key> collectedKeys;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)    
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void AddKeyToList(Key key)
        {
            collectedKeys.Add(key);
        }
    }
}