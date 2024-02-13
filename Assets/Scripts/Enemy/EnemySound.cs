using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemySound : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private List<AudioClip> clipList;
        private AudioClip _clip;
        
        public void PlayFootsteps()
        {
            //TODO play footstep sounds here
            _clip = clipList[Random.Range(0, clipList.Count)];

            source.clip = _clip;
            source.volume = Random.Range(0.5f, 0.65f);
            source.pitch = Random.Range(0.5f, 0.8f);
            source.Play();


        }
    }
}
