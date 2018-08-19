using System.Collections.Generic;
using System;

namespace Artificialintelligence.Neural
{
    public class Neuron
    {
        public float input;
        public List<float> weights;
        public const float maxWeight = 1;
        private static Random random = new Random();

        public Neuron()
        {
            this.input = 0;
            this.weights = new List<float>();
        }

        public Neuron(int weights, bool randomWeights = false)
        {
            this.input = 0;
            this.weights = new List<float>();
            this.weights.Capacity = weights;
            for (int weight = 0; weight < this.weights.Capacity; weight++)
            {
                if (randomWeights == true)
                    this.weights.Add(RandomWeight());
                else
                    this.weights.Add(maxWeight);
            }
        }

        public Neuron(int weights, float setWeights)
        {
            this.input = 0;
            this.weights = new List<float>();
            this.weights.Capacity = weights;
            for (int weight = 0; weight < this.weights.Capacity; weight++)
            {
                this.weights.Add(setWeights);
            }
        }

        public Neuron(Neuron source)
        {
            this.input = 0;
            this.weights = new List<float>(source.weights);
        }

        public float RandomWeight()
        {
            // -1 to 1
            return (float)(2 * maxWeight * MachineLearning.Random.NextDouble() - maxWeight);
        }

        public void Randomise()
        {
            for (int weight = 0; weight < weights.Count; weight++)
            {
                weights[weight] = RandomWeight();
            }
        }

        public void Reset(float value = 0)
        {
            for (int weight = 0; weight < weights.Count; weight++)
            {
                weights[weight] = value;
            }
        }

        public void Input(List<Neuron> neurons)
        {
            float output = 0.25f; // ??????????????????????????????

            for (int i = 0; i < neurons.Count; i++)
            {
                output += neurons[i].input * weights[i];
            }

            input = (float)System.Math.Tanh(output);
        }

        public void SetWeights(float value)
        {
            for (int i = 0; i < weights.Count; i++)
            {
                weights[i] = value;
            }
        }

        public void FlipWeights()
        {
            for (int i = 0; i < weights.Count; i++)
            {
                FlipWeight(weights[i]);
            }
        }

        private void FlipWeight(float weight)
        {
            weight *= -1;
        }

        public void Mutate()
        {
            int randomWeight = random.Next(0, weights.Count);
            weights[randomWeight] = RandomWeight();
        }
    }
}
