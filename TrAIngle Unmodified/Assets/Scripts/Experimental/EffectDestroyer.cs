using System.Collections;
using UnityEngine;

namespace EdsLibrary
{
    public class EffectDestroyer : MonoBehaviour
    {
        // allows a particle system to exist for a specified duration,
        // then shuts off emission, and waits for all particles to expire
        // before destroying the gameObject

        private bool earlyStop = false;

        void Awake()
        {
        }

        private IEnumerator Start()
        {
            float timeExpire = 0;
            timeExpire = GetEffectLifespan(gameObject) + Time.time;

            while (Time.time < timeExpire && !earlyStop)
            {
                yield return null;
            }

            // turn off emission
            var systems = GetComponentsInChildren<ParticleSystem>();
            foreach (var system in systems)
            {
                var emission = system.emission;
                emission.enabled = false;
            }
            BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);

            Destroy(gameObject);
        }

        public void Stop()
        {
            // stops the particle system early
            earlyStop = true;
        }

        public static float GetEffectLifespan(GameObject effect)
        {
            float totalLifetime = 0;
            float maxLifetime = 0;
            float duration = 0;
            ParticleSystem[] systems;
            AudioSource[] audioSources;

            // find out the maximum lifetime and duration of any particles in this effect
            systems = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (var system in systems)
            {
                maxLifetime = Mathf.Max(system.main.startLifetime.constant, maxLifetime);
                maxLifetime = Mathf.Max(system.main.startLifetime.constantMax, maxLifetime);
                duration = Mathf.Max(system.main.duration + system.main.startDelay.constant, duration);
                duration = Mathf.Max(system.main.duration + system.main.startDelay.constantMax, duration);
            }
            totalLifetime = maxLifetime + duration;

            audioSources = effect.GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                totalLifetime = Mathf.Max(audioSource.clip.length, totalLifetime);
            }

            return totalLifetime;
        }
    }
}
