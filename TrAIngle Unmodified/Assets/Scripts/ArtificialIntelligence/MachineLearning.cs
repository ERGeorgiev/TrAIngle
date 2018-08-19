using System.Collections.Generic;
using UnityEngine;
using Artificialintelligence.Genetic;
using Artificialintelligence.Neural;

namespace Artificialintelligence
{
    public class MachineLearning : MonoBehaviour
    {
        [Header("Genetic Algorithm")]
        [SerializeField] private GameObject entityPrefab;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 startOrientation;
        [SerializeField] public int population = 1;
        [SerializeField] public GenomeSettings settings;
        [SerializeField] public LevelController levelController;
        private const int inputs = 3;
        private const int hiddenLayers = 2;
        private const int outputs = 1;

        private bool stopped = true;

        public bool IsTraining
        {
            get
            {
                foreach (IntelligentEntity entity in entities.Values)
                {
                    if (entity.IsInactive == false)
                        return true;
                }
                return false;
            }
        }

        private Dictionary<GameObject, IntelligentEntity> entities = new Dictionary<GameObject, IntelligentEntity>();
        private Genome<NeuronLayer> genome;
        private static readonly System.Random random = new System.Random();
        public static System.Random Random { get { return random; } }

        private void Awake()
        {
            startOrientation = startOrientation + startOrientation;
            // Begin();
        }

        public void Stop()
        {
            stopped = true;
            Clear();
        }

        public void Begin()
        {
            stopped = false;
            GenomeFunctions<NeuronLayer> functions = new GenomeFunctions<NeuronLayer>(GenerateHiddenLayer, Crossover.NeuronLayer, Mutate.NeuronLayer, NeuronLayer.DeepCopy);
            genome = new Genome<NeuronLayer>(population, hiddenLayers, functions, settings);
            Spawn();
        }

        private void Clear()
        {
            foreach (GameObject entity in entities.Keys)
                Destroy(entity);
            entities.Clear();
        }
        
        private void Spawn()
        {
            for (int gene = 0; gene < genome.Genes.Count; gene++)
                GenerateTriangle(genome.Genes[gene], gene);
            levelController.UpdateGeneration(genome.Generation);
        }

        private void Respawn()
        {
            Clear();
            Spawn();
        }

        private GameObject GenerateTriangle(Gene<NeuronLayer> gene, int index)
        {
            GameObject obj = Instantiate(entityPrefab, startPosition, Quaternion.FromToRotation(startPosition, startOrientation), gameObject.transform);
            obj.name = "Triangle " + index;
            Triangle triangle = obj.GetComponent<Triangle>();
            NeuralNetwork net = new NeuralNetwork(inputs, gene.Allele, outputs);
            entities.Add(obj, triangle);

            triangle.Init(gene, net);

            return obj;
        }

        private static NeuronLayer GenerateHiddenLayer(NeuronLayer previousLayer)
        {
            int size = 10;
            if (previousLayer == default(NeuronLayer))
                return new
                    NeuronLayer(size, inputs, randomWeights: true);
            else
                return new NeuronLayer(size, previousLayer.Capacity, randomWeights: true);
        }

        public int VictoriousCount()
        {
            int count = 0;
            foreach (IntelligentEntity entity in entities.Values)
            {
                if (entity.IsVictorious == true)
                    count++;
            }
            return count;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (IsTraining != true && stopped == false)
            {
                genome.Size = population;
                levelController.UpdateVictorious(VictoriousCount());
                if (levelController.Continue == false)
                {
                    Stop();
                }
                else if (genome.Size != 0)
                {
                    genome.Reproduce();
                    Respawn();
                }
            }
        }

        //private float CalculateFitness(int index)
        //{
        //    float score = 0;
        //    NeuralNetwork network = GetComponent<NeuralNetwork>();
        //    Gene<char> genome = ga.Population[index];

        //    for (int i = 0; i < genome.Allele.Count; i++)
        //    {
        //        if (genome.Allele[i] == targetString[i])
        //        {
        //            score += 1;
        //        }
        //    }

        //    score /= targetString.Length;

        //    score = (Mathf.Pow(2, score) - 1) / (2 - 1);
        //    // Exponential 0 to 1.
        //    //https://math.stackexchange.com/questions/384613/exponential-function-with-values-between-0-and-1-for-x-values-between-0-and-1

        //    return score;
        //}
    }
}