using System.Collections.Generic;
using UnityEngine;

namespace EdsLibrary
{
    namespace Units
    {
        public class PlayerManager : MonoBehaviour
        {
            public static PlayerManager instance = null;

            public List<Player> players = new List<Player>();
            [System.Serializable]
            public class Player
            {
                public int lives = 1;
                [HideInInspector]
                public int livesRemaining = 1;
                public List<Unit> heroes = new List<Unit>();
            }

            void Awake()
            {
                foreach (Player player in players)
                {
                    // There's a need to check for a null item as it might be null in the inspector
                    player.heroes.RemoveAll(item => item == null);
                }
                AssignInstance();
            }

            void AssignInstance()
            {
                if (instance != null)
                {
                    if (instance != this)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    instance = this;
                    DontDestroyOnLoad(this);
                }
            }

            public Unit FindHero(HeroNames name, int playerNumber = 1)
            {
                Unit target = null;
                if (name == HeroNames.Unnamed)
                {
                    Debug.LogError("Unnamed hero error in FindHero()");
                    return null;
                }
                // No known player:
                if (playerNumber == -1)
                {
                    foreach (Player player in players)
                    {
                        foreach (Unit unit in player.heroes)
                        {
                            if (HeroNameMatch(unit, name))
                                target = unit;
                            break;
                        }
                        if (target != null)
                            break;
                    }
                }
                else
                {
                    if (players.Count > playerNumber)
                    {
                        foreach (Unit unit in players[playerNumber].heroes)
                        {
                            if (HeroNameMatch(unit, name))
                                target = unit;
                            break;
                        }
                    }
                    else
                        Debug.LogErrorFormat("Player index {0} out of array range {1}.", playerNumber, players.Count);
                }
                return target;
            }

            bool HeroNameMatch(Unit unit, HeroNames matchName)
            {
                if (unit.IsHero())
                {
                    HeroNames statName = unit.hero.heroName;
                    if (matchName == statName)
                        return true;
                }
                else
                    Debug.LogWarning("Unit is not hero, but it should be.");
                return false;
            }
            
            /// <summary>
            /// Adds hero to the player's hero array.
            /// Not to be used on Awake(), as variable instance won't be assigned.
            /// </summary>
            /// <param name="hero">Hero to add.</param>
            /// <param name="player">Owner index of the added hero.</param>
            public void AddHero(Unit hero, int player)
            {
                if (hero != null)
                {
                    while (player >= players.Count)
                    {
                        players.Add(new Player());
                    }
                    if (players.Count > player)
                    {
                        if (players[player].heroes.Count != 0)
                        {
                            if (players[player].heroes.Find(item => item.hero.heroName == hero.hero.heroName) == null)
                            {
                                players[player].heroes.Add(hero);
                            }
                            else
                                Debug.LogError("Trying to double-add a hero");
                        }
                        else
                            players[player].heroes.Add(hero);
                    }
                    else
                        Debug.LogError("Player index out of array range.");
                }
                else Debug.LogError("Cannot assign hero number to a null hero.");
            }
        }
    }
}
