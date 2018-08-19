using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdsLibrary
{
    namespace Math
    {
        public class VectorExt : MonoBehaviour
        {
            ///<summary>
            ///Returns a directional vector with an optional exact distance.
            ///</summary>
            public static Vector3 DirectionalVector(Vector3 start, Vector3 end, float distance = 1)
            {
                return (((end - start).normalized) * distance) + start;
            }
        }
    }
}