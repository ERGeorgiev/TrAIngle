using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Artificialintelligence;
using Artificialintelligence.Genetic;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject machineLearningObject;
    MachineLearning machineLearning;
    [SerializeField] private int populationMin = 1;
    [SerializeField] private int populationMax = 100;
    [SerializeField] public GameObject populationObject;
    [SerializeField] public Text populationText;
    [SerializeField] public GameObject elitismObject;
    [SerializeField] public Text elitismText;
    [SerializeField] public GameObject mutationObject;
    [SerializeField] public GameObject selectionObject;
    [SerializeField] public GameObject crossoverObject;
    [SerializeField] public GameObject tipObject;
    [SerializeField] public Text tipText;

    private void Awake()
    {
        machineLearning = machineLearningObject.GetComponent<MachineLearning>();
    }

    public void IncreasePopulation()
    {
        machineLearning.population = Mathf.Min(machineLearning.population + 1, populationMax);
        populationText.text = machineLearning.population.ToString();
    }

    public void DecreasePopulation()
    {
        machineLearning.population = Mathf.Max(machineLearning.population - 1, populationMin);
        populationText.text = machineLearning.population.ToString();
    }

    public void IncreaseElitism()
    {
        machineLearning.settings.elitism = Mathf.Min(machineLearning.settings.elitism + 1, machineLearning.population);
        elitismText.text = machineLearning.settings.elitism.ToString();
    }

    public void DecreaseElitism()
    {
        machineLearning.settings.elitism = Mathf.Max(machineLearning.settings.elitism - 1, 0);
        elitismText.text = machineLearning.settings.elitism.ToString();
    }

    public void ChangeMutationType(int mutationType) { machineLearning.settings.mutationType = (MutationType)mutationType; }
    public void ChangeSelectionType(int selectionType) { machineLearning.settings.selectionType = (SelectionType)selectionType; }
    public void ChangeCrossoverType(int crossoverType) { machineLearning.settings.crossoverType = (CrossoverType)crossoverType; }

    public void Refresh()
    {
        populationText.text = machineLearning.population.ToString();
        elitismText.text = machineLearning.settings.elitism.ToString();
    }

    public void Level_1()
    {
        machineLearning.population = 0;
        machineLearning.settings.elitism = 0;
        machineLearning.settings.mutationRate = 0.05f;
        machineLearning.settings.mutationType = MutationType.Random;
        machineLearning.settings.reproductionType = ReproductionType.Generational;
        machineLearning.settings.scalingType = FitnessScalingType.None;
        machineLearning.settings.selectionType = SelectionType.Random;
        populationMax = 1;
        Refresh();
        populationObject.SetActive(true);
        elitismObject.SetActive(false);
        mutationObject.SetActive(false);
        selectionObject.SetActive(false);
        crossoverObject.SetActive(false);
        tipObject.SetActive(true);
        tipText.text = "Tap '+' to increase the population and '-' to decrease the population.\n\nPopulaton indicates the amount of triangles spawned.\n\nNew generations are created from the previous population, the first being totally random.\n\nNew generations rely on 'Crossover' and 'Mutation'.\nCrossover is when two parents from the previous population are chosen and mixed together to create a member of the new population.\nMutation occurs with a random chance for each member of the new generation, changing them by altering their weights.";
    }

    public void Level_2()
    {
        machineLearning.population = 0;
        machineLearning.settings.elitism = 0;
        machineLearning.settings.mutationRate = 0.05f;
        machineLearning.settings.mutationType = MutationType.Random;
        machineLearning.settings.reproductionType = ReproductionType.Generational;
        machineLearning.settings.scalingType = FitnessScalingType.None;
        machineLearning.settings.selectionType = SelectionType.Random;
        populationMax = 10;
        Refresh();
        populationObject.SetActive(true);
        elitismObject.SetActive(true);
        mutationObject.SetActive(false);
        selectionObject.SetActive(false);
        crossoverObject.SetActive(false);
        tipObject.SetActive(true);
        tipText.text = "Use Elitism to specify the number of best performers that make it to the next generation.\n\nHigh elitism can cause the neural network to stop improving.\n\nElitism cannot be higher than Population.";
    }

    public void Level_3()
    {
        machineLearning.population = 0;
        machineLearning.settings.elitism = 0;
        machineLearning.settings.mutationRate = 0.05f;
        machineLearning.settings.mutationType = MutationType.Random;
        machineLearning.settings.reproductionType = ReproductionType.Generational;
        machineLearning.settings.scalingType = FitnessScalingType.None;
        machineLearning.settings.selectionType = SelectionType.Random;
        populationMax = 20;
        Refresh();
        populationObject.SetActive(true);
        elitismObject.SetActive(true);
        mutationObject.SetActive(true);
        selectionObject.SetActive(true);
        crossoverObject.SetActive(true);
        tipObject.SetActive(false);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
