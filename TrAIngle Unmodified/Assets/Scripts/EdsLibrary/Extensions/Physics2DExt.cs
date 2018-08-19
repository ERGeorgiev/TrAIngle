using UnityEngine;

namespace Extensions
{
    public class Physics2DExt
    {
        public static float Raycast(Transform transform, float raycast_distance, int raycast_layerMask, float angle)
        {
            float dist = raycast_distance;
            
            Vector3 direction = transform.TransformDirection(MathfExt.DegreeToVector2(angle));
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, raycast_distance, raycast_layerMask);
            if (raycast.collider != null)
                dist = raycast.distance;
            Debug.DrawRay(transform.position, direction * raycast.distance, Color.red);

            return dist;
        }

        public static float[] Raycast(Transform transform, float raycast_distance, int raycast_layerMask, params float[] angles)
        {
            float[] dist = new float[angles.Length];

            for (int i = 0; i < angles.Length; i++)
                dist[i] = Raycast(transform, raycast_distance, raycast_layerMask, angles[i]);

            return dist;
        }

        public static float[] RaycastReflect(Transform transform, float raycast_distance, int raycast_layerMask, params float[] angles)
        {
            float[] dist = new float[angles.Length];

            for (int i = 0; i < angles.Length; i++)
                dist[i] = Raycast(transform, raycast_distance, raycast_layerMask, angles[i]) - Raycast(transform, raycast_distance, raycast_layerMask, (180 - angles[i]));

            return dist;
        }
    }
}
