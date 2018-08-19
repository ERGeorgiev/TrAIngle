using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EdsLibrary
{
    namespace Audio
    {
        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)]
            public float volume = 0.7f;
            [Range(0.5f, 1.5f)]
            public float pitch = 1f;
            [Range(0f, 0.5f)]
            public float randomVolume = 0.1f;
            [Range(0f, 0.5f)]
            public float randomPitch = 0.1f;
            [HideInInspector]
            public AudioSource source;

            public void Play()
            {
                source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
                source.pitch = pitch * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
                source.Play();
            }

            public void SetSource(AudioSource source)
            {
                this.source = source;
                this.source.clip = clip;
            }
        }

        public class AudioManager : MonoBehaviour
        {
            public static AudioManager instance = null;

            [SerializeField]
            public AudioSourceExt Guns;
            public Sound[] sounds;
            public static Sound[] Sounds;

            void Awake()
            {
                if (!AssignInstance())
                    return;
                Sounds = sounds;
                foreach (Sound sound in sounds)
                {
                    sound.SetSource(gameObject.AddComponent<AudioSource>());
                }
            }

            bool AssignInstance()
            {
                if (instance != null)
                {
                    if (instance != this)
                    {
                        Destroy(this.gameObject);
                        return false;
                    }
                    return true;
                }
                else
                {
                    instance = this;
                    DontDestroyOnLoad(this);
                    return true;
                }
            }

            /// <summary>
            /// Find the given AudioSource based on its name.
            /// Not to be used on Awake().
            /// </summary>
            public static Sound FindSound(string name)
            {
                foreach (Sound sound in Sounds)
                {
                    if (sound.name == name)
                    {
                        return sound;
                    }
                }
                return null;
            }

            [System.Serializable]
            public class AudioSourceExt
            {
                
                public List<AudioSource> audioSources = new List<AudioSource>();
            }
        }
    }
}
