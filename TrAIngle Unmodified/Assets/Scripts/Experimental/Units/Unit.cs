using System.Collections;
using UnityEngine;
using EdsLibrary.Units.Extensions;
using EdsLibrary.Units.Attributes;
using EdsLibrary;

namespace EdsLibrary
{
    namespace Units
    {
        public class Unit : UnitAttributes
        {
            [Header("Effects: ")]
            public RespawnEvent respawn;
            public ReviveEvent revive;
            public DeathEvent death;

            [Header("Extensions:")]
            public UnitDecomposition decomposition;
            public HeroStats hero;

            // CLASS-SPECIFIC
            [HideInInspector]
            public bool removed = false;
            private Animator animator;

            void Awake()
            {
                animator = GetComponent<Animator>();

                if (decomposition != null)
                    decomposition.unit = this;
            }

            void Start()
            {
                // Register hero in HeroContainer:
                if (this.IsHero() == true)
                {
                    if (PlayerManager.instance != null)
                        PlayerManager.instance.AddHero(this, this.playerOwner);
                }

                RefreshStatus();
            }

            public void PlayAnimation(AnimationNames animationName)
            {
                animator.SetBool(animationName.ToString(), true);
            }

            public void Damage(float damage)
            {
                if (!isDead)
                {
                    HealthCurrent -= damage;
                    RefreshStatus();
                    if (HealthCurrent <= 0)
                    {
                        Kill();
                    }
                }
            }

            public bool IsHero()
            {
                if (hero != null)
                    return true;
                else
                    return false;
            }

            public bool Respawn(Transform spawnPointCustom = null, bool noEffects = false)
            {
                if (isDead == true)
                {
                    // Assign spawn based on priority
                    Transform spawn;
                    if (spawnPointCustom != null)
                        spawn = spawnPointCustom;
                    else if (respawn.spawnPoint != null)
                        spawn = respawn.spawnPoint;
                    else
                    {
                        Debug.LogError("Attempt to respawn without spawn point.");
                        return false;
                    }

                    // Respawn the unit at the assigned spawn
                    if (!noEffects && respawn.effect != null)
                        Instantiate(respawn.effect, spawn.position, spawn.rotation, transform);

                    this.transform.position = spawn.position;
                    this.isDead = false;
                    this.canBeRevived = this.canBeRevivedDefault;
                    this.HealthCurrent = this.healthMax;
                    RefreshStatus();
                    return true;
                }
                else
                    Debug.LogError("Attempt to respawn a living hero.");
                return false;
            }

            public bool Revive(float seconds = 0, bool noEffects = false)
            {
                if (isDead && canBeRevived)
                {
                    if (!noEffects && revive.effect != null)
                        Instantiate(revive.effect, transform.position, transform.rotation, transform);
                    this.Respawn(this.transform, noEffects: false);
                    return true;
                }
                else
                    Debug.LogError("Attempt to revive a living unit.");
                return false;
            }

            bool Decomposes()
            {
                if (decomposition != null)
                {
                    if (decomposition.duration == 0)
                        return true;
                }
                return false;
            }

            public void Kill(params DeathType[] deathTypes)
            {
                DeathFlags deathFlags = new DeathFlags(deathTypes);

                if (deathFlags.destroy)
                    Destroy(this.gameObject);

                if (deathFlags.remove)
                {
                    StartCoroutine(Remove(0));
                    return;
                }

                if (canBeRevived == true) // Negate revival
                    canBeRevived = !deathFlags.noRevive;

                if (!isDead)
                {
                    float removalDelay = 0;
                    isDead = true;
                    HealthCurrent = 0;

                    PlayAnimation(AnimationNames.Death);
                    if (!deathFlags.noEffects)
                    {
                        if (death.effect != null)
                        {
                            Instantiate(death.effect, transform.position, death.effect.rotation);
                            removalDelay += EffectDestroyer.GetEffectLifespan(death.effect.gameObject);
                        }
                    }
                    if (!Decomposes())
                        StartCoroutine(Remove(removalDelay));
                    OnDeathTriggers();
                }
            }

            private IEnumerator Remove(float delay = 0)
            {
                yield return new WaitForSeconds(delay);
                if (IsHero())
                {
                    canBeRevived = false;
                    removed = true;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }

    }
}
