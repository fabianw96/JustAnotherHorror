using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)    
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            DontDestroyOnLoad(this);
        }

    }
}