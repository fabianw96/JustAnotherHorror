using System.Collections.Generic;
using Interactables;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager instance;

        public List<Key> collectedKeys;
        
        private void Awake()
        {
            if (instance != null && instance != this)    
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(this);
        }

        public void AddKeyToList(Key key)
        {
            collectedKeys.Add(key);
        }


    }
}