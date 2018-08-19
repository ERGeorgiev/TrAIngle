using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EdsLibrary
{
    namespace Units
    {
        namespace Attributes
        {
            public class UnitAttributes : UnitStats
            {
                [Header("Unit Attributes:")]
                public int playerOwner = 0;
                public bool isDead = false;
                public bool canBeRevivedDefault = false;
                protected bool canBeRevived = false;
                [SerializeField]
                protected UnityEvent onStatusChange;
                [SerializeField]
                protected UnityEvent onDeath;

                protected void RefreshStatus()
                {
                    if (onStatusChange != null)
                    {
                        onStatusChange.Invoke();
                    }
                }
                protected void OnDeathTriggers()
                {
                    if (onDeath != null)
                    {
                        onDeath.Invoke();
                    }
                }
            }

            public class UnitStats : MonoBehaviour
            {
                [Header("Unit Stats:")]
                #region Health
                public float healthMax = 100;
                [SerializeField] // Since getter and setter cannot be accessed in the inspector, the private variable must be used
                [Tooltip("Cannot be increased above the maximum health.")] // This is done by the constructor of this class
                private float healthCurrent = 100;
                /// <summary>
                /// Cannot be increased above the maximum health.
                /// </summary>
                [SerializeField]
                public float HealthCurrent
                {
                    get { return healthCurrent; }
                    set
                    {
                        healthCurrent = Mathf.Clamp(value, 0f, healthMax);
                    }
                }
                #endregion
                #region Mana
                public float manaMax = 100;
                [SerializeField] // Since getter and setter cannot be accessed in the inspector, the private variable must be used
                [Tooltip("Cannot be increased above the maximum health.")] // This is done by the constructor of this class
                private float manaCurrent = 100;
                /// <summary>
                /// Cannot be increased above the maximum health.
                /// </summary>
                [SerializeField]
                public float ManaCurrent
                {
                    get { return manaCurrent; }
                    set
                    {
                        manaCurrent = Mathf.Clamp(value, 0f, manaMax);
                    }
                }
                #endregion

                public UnitStats()
                {
                    HealthCurrent = healthCurrent;
                    ManaCurrent = manaCurrent;
                }
            }

            #region Effects
            [System.Serializable]
            public class RespawnEvent : EffectEvent
            {
                public Transform spawnPoint;
            }
            [System.Serializable]
            public class DeathEvent : EffectEvent
            {

            }
            [System.Serializable]
            public class ReviveEvent : EffectEvent
            {

            }
            [System.Serializable]
            public class EffectEvent
            {
                public Transform effect;
            }
            #endregion
        }
    }
}