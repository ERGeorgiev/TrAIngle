using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Corridors;
using Artificialintelligence;
using UnityEngine.SceneManagement;
using EdsLibrary.Audio;

public class LevelController : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] public LevelUIController ui;
    public Corridor corridorLevel1;
    public Corridor corridorLevel2;
    public Corridor corridorLevel3;
    public MachineLearning machineLearning;
    [HideInInspector] public int curentGeneration;
    [HideInInspector] public int maxGenerations;
    [HideInInspector] public int victoriousGeneration;
    [HideInInspector] public int[] victoriousGenerationGoal;
    [HideInInspector] private int currentLevel = 0;

    private void Awake()
    {
        victoriousGenerationGoal = new int[4] { 0, 1, 5, 1 };
    }

    void Start ()
    {
        NextLevel();
	}

    public bool Continue
    {
        get
        {
            if (curentGeneration < maxGenerations)
            {
                if (victoriousGeneration >= victoriousGenerationGoal[currentLevel])
                {
                    Victory();
                    return false;
                }
                else
                    return true;
            }
            else
            {
                Defeat();
                return false;
            }
        }
    }

    public void Defeat()
    {
        machineLearning.Stop();
        ui.ShowIntroduction("Level " + currentLevel, "Failed! But you can try again!");
    }

    public void Victory()
    {
        ui.ShowVictory("Level " + currentLevel, "Completed successfully! Well done!");
        AudioManager.FindSound("goal").Play();
    }

    public void NextLevel()
    {
        currentLevel++;
        LoadLevel(currentLevel);
    }

    public void Retry()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        switch(level)
        {
            case 1:
                IntroLevel1();
                break;
            case 2:
                IntroLevel2();
                break;
            case 3:
                IntroLevel3();
                break;
            case 4:
                Finish();
                break;
        }
    }

    public void Finish()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void UpdateGeneration(int generation)
    {
        victoriousGeneration = 0;
        curentGeneration = generation;
        ui.SetGenerations(curentGeneration, maxGenerations);
    }

    public void UpdateVictorious(int victoriousGeneration)
    {
        this.victoriousGeneration = victoriousGeneration;
    }

    public void IntroLevel1()
    {
        ui.ShowIntroduction("Level " + currentLevel, "Objective: Create a single triangle and wait for it to pass through.\n\nAfter closing this, press Setup and increase the Population, close Setup and then a triangle will spawn.\n\nGreen lines represent 'Goals' that award every triangle that touches them.\n\nBlue lines represent the finish line.");
        corridorLevel1.gameObject.SetActive(true);
        corridorLevel2.gameObject.SetActive(false);
        corridorLevel3.gameObject.SetActive(false);
        mainCamera.GetComponent<CameraFocus2D>().Focus(corridorLevel1);
        maxGenerations = 100;
        machineLearning.Begin();
        ui.LoadLevel1();
    }

    public void IntroLevel2()
    {
        ui.ShowIntroduction("Level " + currentLevel, "Objective: 5 triangles must make it to the end of the maze.\n\nYou have 25 generations with maximum 10 population.\n\nNew setting unlocked in setup: Elitism\n\n");
        corridorLevel1.gameObject.SetActive(false);
        corridorLevel2.gameObject.SetActive(true);
        corridorLevel3.gameObject.SetActive(false);
        mainCamera.GetComponent<CameraFocus2D>().Focus(corridorLevel2);
        maxGenerations = 25;
        ui.LoadLevel2();
        machineLearning.Begin();
    }

    public void IntroLevel3()
    {
        ui.ShowIntroduction("Level " + currentLevel, "Objective: 1 triangle must make it to the end of the maze.\n\nYou have 10 generations with maximum 20 population.\n\nAll setting unlocked in setup!");
        corridorLevel1.gameObject.SetActive(false);
        corridorLevel2.gameObject.SetActive(false);
        corridorLevel3.gameObject.SetActive(true);
        mainCamera.GetComponent<CameraFocus2D>().Focus(corridorLevel3);
        maxGenerations = 10;
        ui.LoadLevel3();
        machineLearning.Begin();
    }
}
