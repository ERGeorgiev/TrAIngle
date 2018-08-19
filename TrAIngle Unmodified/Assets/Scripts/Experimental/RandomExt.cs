using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdsLibrary
{
    namespace Math
    {
        public class RandomExt : MonoBehaviour
        {
            ///<summary>
            ///Randomizes variable x from -x to +x using a factor.
            ///</summary>
            public static float Oscillation(float x, float factor)
            {
                float rnd = Random.Range(1 - factor, 1 + factor);
                return x * rnd;
            }

            ///<summary>
            ///Randomizes a vector based on degrees.
            ///</summary>
            public static Vector3 Oscillation(Vector3 vector, float degrees)
            {
                float factor = degrees / 180f;
                float magn = vector.magnitude;
                Vector3 target = vector.normalized;
                Vector3 osc = new Vector2(Random.Range(-factor, factor), Random.Range(-factor, factor));
                target = magn * (target + osc);
                return target;
            }

            ///<summary>
            ///Randomizes a vector based on degrees.
            ///</summary>
            public static Vector2 Oscillation(Vector2 vector, float degrees)
            {
                float factor = degrees / 180f;
                float magn = vector.magnitude;
                Vector2 target = vector.normalized;
                Vector2 osc = new Vector2(Random.Range(-factor, factor), Random.Range(-factor, factor));
                target = magn * (target + osc);
                return target;
            }
        }
    }
}
