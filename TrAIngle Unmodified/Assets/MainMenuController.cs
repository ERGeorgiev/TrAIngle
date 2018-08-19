using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public List<GameObject> howItWorks;

    private void Awake()
    {
        DisplayMainMenu();
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
        foreach (GameObject how in howItWorks)
        {
            how.SetActive(false);
        }
    }

    public void DisplayHowItWorks()
    {
        mainMenu.SetActive(false);
        foreach (GameObject how in howItWorks)
        {
            how.SetActive(false);
        }
        DisplayHowItWorks_next();
    }

    public void DisplayHowItWorks_next()
    {
        for (int i = 0; i < howItWorks.Count - 1; i++)
        {
            if (howItWorks[i].activeInHierarchy == true)
            {
                howItWorks[i].SetActive(false);
                howItWorks[i + 1].SetActive(true);
                return;
            }
        }
        howItWorks[0].SetActive(true);
    }

    public void DisplayHowItWorks_previous()
    {
        for (int i = howItWorks.Count - 1; i > 0; i--)
        {
            if (howItWorks[i].activeInHierarchy == true)
            {
                howItWorks[i].SetActive(false);
                howItWorks[i - 1].SetActive(true);
                break;
            }
        }
    }

    public void DisplayInfo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
