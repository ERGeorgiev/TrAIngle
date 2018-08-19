using System.Collections.Generic;
using System;
using System.Linq;
using Artificialintelligence.Genetic;

namespace Artificialintelligence.Neural
{
    public class NeuronLayer
    {
        public List<Neuron> neurons;

        public int Capacity
        {
            get { return neurons.Capacity; }
            set { neurons.Capacity = value; }
        }
        public int Count
        {
            get { return neurons.Count; }
        }

        public NeuronLayer()
        {
            this.neurons = new List<Neuron>();
        }

        public NeuronLayer(int size, NeuronLayer previousLayer = null, bool randomWeights = false) : this(size, previousLayer.neurons.Capacity, randomWeights) { }

        public NeuronLayer(int size, int previousLayer_size, bool randomWeights = false)
        {
            this.neurons = new List<Neuron>();
            neurons.Capacity = size;
            for (int neuron = 0; neuron < neurons.Capacity; neuron++)
            {
                neurons.Add(new Neuron(previousLayer_size, randomWeights)); // Adding neurons with weights count equal to the neurons in the last layer
            }
        }

        public NeuronLayer(NeuronLayer source) : this()
        {
            foreach (var neuron in source.neurons)
            {
                neurons.Add(new Neuron(neuron));
            }
        }

        public static List<NeuronLayer> GenerateHidden(int inputs, int[] hidden, bool randomWeights = false)
        {
            List<NeuronLayer> neuronLayers = new List<NeuronLayer>(hidden.Length);
            
            for (int layer = 0; layer < hidden.Length; layer++)
            {
                if (layer == 0)
                    neuronLayers.Add(new NeuronLayer(hidden[layer], inputs, randomWeights));
                else
                    neuronLayers.Add(new NeuronLayer(hidden[layer], hidden[layer - 1], randomWeights));
            }

            return neuronLayers;
        }

        public void Randomise()
        {
            foreach(var neuron in neurons)
            {
                neuron.Randomise();
            }
        }

        public void Reset(float value = 0)
        {
            foreach (var neuron in neurons)
            {
                neuron.Reset(value);
            }
        }

        public static List<NeuronLayer> DeepCopy(List<NeuronLayer> neuronLayers)
        {
            return neuronLayers.ConvertAll(layer => layer.Clone());
        }

        public static NeuronLayer DeepCopy(NeuronLayer neuronLayer)
        {
            return new NeuronLayer(neuronLayer);
        }

        public NeuronLayer Clone()
        {
            return new NeuronLayer(this);
        }

        public float[] Inputs()
        {
            List<float> inputs = new List<float>(neurons.Count);

            foreach (Neuron neuron in neurons)
            {
                inputs.Add(neuron.input);
            }

            return inputs.ToArray();
        }

        public static void FeedForward(float[] inputs, NeuronLayer inputLayer, List<NeuronLayer> hiddenLayers, NeuronLayer outputLayer)
        {
            inputLayer.InputDirect(inputs);

            inputLayer.FeedForward(hiddenLayers[0]);
            int layer = 0;
            for (layer = 0; layer < hiddenLayers.Count - 1; layer++)
            {
                hiddenLayers[layer].FeedForward(hiddenLayers[layer + 1]);
            }
            hiddenLayers[layer].FeedForward(outputLayer);
        }

        public void FeedForward(NeuronLayer destination)
        {
            foreach (Neuron neuron in destination.neurons)
            {
                neuron.Input(this.neurons);
            }
        }

        public void InputDirect(float[] inputs)
        {
            if (inputs.Length > neurons.Count)
                throw new ArgumentOutOfRangeException("Incorrect number of inputs in input layer.");

            for (int i = 0; i < inputs.Length; i++)
                neurons[i].input = inputs[i];
        }

        public static int[] Layers(NeuronLayer input, List<NeuronLayer> hiddenLayers, NeuronLayer output)
        {
            int[] layers = new int[hiddenLayers.Count + 2];

            layers[0] = input.Capacity;
            int layer = 1;
            for (layer = 1; layer < layers.Length; layer++)
            {
                layers[layer] = hiddenLayers[layer].Capacity;
            }
            layers[layer - 1] = output.Capacity;

            return layers;
        }

        public static int[] Layers(List<NeuronLayer> neuronLayers)
        {
            int[] layers = new int[neuronLayers.Count];

            for (int layer = 0; layer < layers.Length; layer++)
            {
                layers[layer] = neuronLayers[layer].Capacity;
            }

            return layers;
        }

        public static void InitSize(List<NeuronLayer> neuronLayers, int[] layers)
        {
            neuronLayers.Capacity = layers.Length;
            for (int layer = 0; layer < layers.Length; layer++)
            {
                if (neuronLayers.Count - 1 < layer)
                {
                    neuronLayers.Add(new NeuronLayer());
                }
                neuronLayers[layer].Capacity = layers[layer];
            }
        }

        public void Init(int weights, bool randomWeights = false)
        {
            for (int neuron = 0; neuron < Capacity; neuron++)
            {
                neurons.Add(new Neuron(weights, randomWeights));
            }
        }

        public static void Init(NeuronLayer inputLayer, List<NeuronLayer> hiddenLayers, NeuronLayer outputLayer)
        {
            inputLayer.Init(1);
            int layer = 0;
            for (layer = 0; layer < hiddenLayers.Capacity; layer++)
            {
                if (layer == 0)
                    hiddenLayers[layer].Init(inputLayer.Capacity, randomWeights: true);
                else
                    hiddenLayers[layer].Init(hiddenLayers[layer - 1].Capacity, randomWeights: true);
            }
            outputLayer.Init(hiddenLayers[layer - 1].Capacity, randomWeights: true);
        }
    }
}
