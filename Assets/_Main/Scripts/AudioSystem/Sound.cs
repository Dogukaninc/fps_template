using UnityEngine;

namespace _Space_Shooter_Files.Scripts
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        [HideInInspector]
        public AudioSource source;
        public AudioClip clip;
        [Range(0,1f)]
        public float volume;
        [Range(0.1f, 3f)]
        public float pitch;
        public bool loop;
        public bool playOnAwake;
    }
}