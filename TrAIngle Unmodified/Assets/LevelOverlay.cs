using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Corridors;

public class LevelOverlay : MonoBehaviour
{
    public List<Corridor> corridors;
    List<bool> dis = new List<bool>();

    private void OnDisable()
    {
        dis = new List<bool>();
        foreach (var corridor in corridors)
        {
            if (corridor == null || corridor.gameObject == null) continue;
            dis.Add(corridor.isActiveAndEnabled);
            corridor.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < corridors.Count && i < dis.Count; i++)
        {
            if (corridors[i] == null || corridors[i].gameObject == null) continue;
            corridors[i].gameObject.SetActive(dis[i]);
        }
    }
}
