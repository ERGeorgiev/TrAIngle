using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFocus2D : MonoBehaviour
{
    public IWorldBounds2D PreviousTarget { get; private set; }
    private IWorldBounds2D targetLock;
    public IWorldBounds2D TargetLock
    {
        get { return targetLock; }
        set
        {
            PreviousTarget = targetLock;
            targetLock = value;
        }
    }
    public Camera Camera { get; private set; }

    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    public void Focus(IWorldBounds2D bounds)
    {
        if (bounds != null)
        {
            Adjust(bounds);
            transform.position = bounds.WorldPosition(transform.position.z);
        }
    }

    private void Adjust(IWorldBounds2D bounds)
    {
        Camera.orthographicSize = bounds.WorldBoundsSizeVector().magnitude / 1.75f;
    }
    
    void FixedUpdate()
    {
        if (targetLock != null)
        {
            transform.position = targetLock.WorldPosition(transform.position.z);
        }
    }
}