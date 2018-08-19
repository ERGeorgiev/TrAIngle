using UnityEngine;

namespace EdsLibrary
{
    namespace Units
    {
        namespace Extensions
        {
            public class UnitDecomposition : MonoBehaviour
            {
                [Tooltip("The unit's corpse will never be removed.")]
                public bool infiniteDecomposition = false;
                [Tooltip("The unit must not be decomposed so revival can take place.\nSet to -1 for never, 0 for instant.\nUse -2 to set it equal to the length of the death sound.")]
                public float duration = 0;
                private float end = 0;
                [Tooltip("If there's a decompose removal animation, specify the time it needs to finish.\nExample: An unit's corpse turns to sand for (2) seconds before removal.")]
                public float animationTime = 0;
                public Transform decompositionEffects;

                [HideInInspector]
                public Unit unit;

                // Update is called once per frame
                void Update()
                {
                    UpdateDecomposition();
                }

                void UpdateDecomposition()
                {
                    if (unit.isDead && !infiniteDecomposition)
                    {
                        float time = Time.time;
                        if (end == 0)
                        {
                            // Increment end by effectDuration so decomposition can't interrupt it by premature removal.
                            float effectDuration = EffectDestroyer.GetEffectLifespan(unit.death.effect.gameObject);
                            duration += effectDuration;
                            end = time + duration;
                            animationTime = end; // Animation starts right after the duration
                            end += animationTime; // Increment by animationTime so the animation has time to play.
                        }
                        else if (time > end)
                        {
                            // If decompositionAnimationTime == decompositionEnd, then only this condition will be executed, as that would mean animation time == 0;
                            end = 0;
                            unit.Kill(DeathType.Remove);
                        }
                        else if (time > animationTime)
                        {
                            unit.PlayAnimation(AnimationNames.Remove);
                        }
                    }
                    else if (end != 0)
                    {
                        end = 0;
                    }
                }
            }
        }
    }
}
