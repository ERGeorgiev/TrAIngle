using System.Collections;
using UnityEngine;
using EdsLibrary.Units;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public Unit playerHero;
    public Transform playerPrefab;
    public float spawnDelay = 2;
    public AudioSource respawnSound;

    public static GameManager instance;

    bool respawning = false;

    void Awake ()
    {
        //if (!AssignInstance())
        //    return;
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

    public IEnumerator _respawnPlayer()
    {
        respawning = true;
        if (respawnSound != null)
        {
            respawnSound.Play();
        }
        yield return new WaitForSeconds(spawnDelay);
        if (playerHero.Respawn() == false)
        {
            EndGame();
        }
        respawning = false;
    }

    void Update()
    {
        if (playerHero.isDead)
        {
            if (respawning == false)
            {
                PlayerManager.Player player = PlayerManager.instance.players[playerHero.playerOwner];
                if (player.livesRemaining > 0)
                {
                    player.livesRemaining--;
                    StartCoroutine(_respawnPlayer());
                }
                else
                {
                    EndGame();
                }
            }
        }
    }

	public static void killUnit (Unit _unit)
    {
        instance._killUnit(_unit);
    }

    public void _killUnit(Unit _unit)
    {
        Destroy(_unit.gameObject);
        if (_unit.IsHero())
        {
            //cameraShake.Shake(0.25f, 0.3f);
            instance.StartCoroutine(instance._respawnPlayer());
        }
    }

    public void EndGame()
    {
        gameOverUI.SetActive(true);
    }
}
