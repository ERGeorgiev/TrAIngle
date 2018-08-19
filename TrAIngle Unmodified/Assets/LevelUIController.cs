using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Artificialintelligence;

public class LevelUIController : MonoBehaviour
{
    [Header("Top Bar")]
    public GameObject TopBar_Level;
    public List<GameObject> TopBar_Buttons;
    public GameObject TopBar_Generation;
    [Header("Introduction")]
    public GameObject Introduction;
    public GameObject IntroductionTitle;
    public GameObject IntroductionDescription;
    [Header("Victory")]
    public GameObject Victory;
    public GameObject VictoryTitle;
    public GameObject VictoryDescription;
    [Header("Defeat")]
    public GameObject Defeat;
    public GameObject DefeatTitle;
    public GameObject DefeatDescription;
    [Header("Setup")]
    public GameObject Setup;
    [SerializeField] GameObject machineLearningObject;
    private MachineLearning machineLearning;
    [Header("Settings")]
    public Settings settings;

    private void Awake()
    {
        machineLearning = machineLearningObject.GetComponent<MachineLearning>();
    }

    public void ShowIntroduction(string title, string description)
    {
        TopBar_Level.GetComponent<Text>().text = title;
        DisableTopbar();
        Introduction.SetActive(true);
        IntroductionTitle.GetComponent<Text>().text = title;
        IntroductionDescription.GetComponent<Text>().text = description;
    }

    public void HideIntroduction()
    {
        EnableTopbar();
        Introduction.SetActive(false);
    }

    public void ShowVictory(string title, string description)
    {
        DisableTopbar();
        Victory.SetActive(true);
        VictoryTitle.GetComponent<Text>().text = title;
        VictoryDescription.GetComponent<Text>().text = description;
    }

    public void HideVictory()
    {
        EnableTopbar();
        Victory.SetActive(false);
    }

    public void ShowDefeat(string title, string description)
    {
        DisableTopbar();
        Defeat.SetActive(true);
        DefeatTitle.GetComponent<Text>().text = title;
        DefeatDescription.GetComponent<Text>().text = description;
    }

    public void HideDefeat()
    {
        EnableTopbar();
        Defeat.SetActive(false);
    }

    private void EnableTopbar()
    {
        foreach (var button in TopBar_Buttons)
        {
            button.GetComponent<Button>().enabled = true;
        }
    }

    private void DisableTopbar()
    {
        foreach (var button in TopBar_Buttons)
        {
            button.GetComponent<Button>().enabled = false;
        }
    }

    public void SetGenerations(int count, int maxGenerations)
    {
        TopBar_Generation.GetComponent<Text>().text = "Generation: " + count + "/" + maxGenerations;
    }

    public void ShowSetup()
    {
        machineLearning.Stop();
        Setup.SetActive(true);
    }

    public void HideSetup()
    {
        machineLearning.Begin();
        Setup.SetActive(false);
    }
    
    public void LoadLevel1()
    {
        settings.gameObject.SetActive(true);
        settings.Level_1();
        settings.gameObject.SetActive(false);
    }

    public void LoadLevel2()
    {
        settings.gameObject.SetActive(true);
        settings.Level_2();
        settings.gameObject.SetActive(false);
    }

    public void LoadLevel3()
    {
        settings.gameObject.SetActive(true);
        settings.Level_3();
        settings.gameObject.SetActive(false);
    }
}
