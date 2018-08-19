using System.Collections.Generic;
using System.Linq;

namespace Artificialintelligence.Neural
{
    /// <summary>
    /// Initializes the neural network with random numbers
    /// </summary>
    /// <param name="layers">layers to the neural network</param>
    public class NeuralNetwork
    {
        public NeuronLayer inputLayer = new NeuronLayer();
        public List<NeuronLayer> hiddenLayers = new List<NeuronLayer>();
        public NeuronLayer outputLayer = new NeuronLayer();

        public NeuralNetwork() { }

        public NeuralNetwork(int inputs, int[] hiddenLayers, int outputs, bool initialiseLayers = true)
        {
            this.inputLayer.Capacity = inputs;
            NeuronLayer.InitSize(this.hiddenLayers, hiddenLayers);
            this.outputLayer.Capacity = outputs;
            if (initialiseLayers == true)
                NeuronLayer.Init(this.inputLayer, this.hiddenLayers, this.outputLayer);
        }

        public NeuralNetwork(int inputs, List<NeuronLayer> hiddenLayers, int outputs)
        {
            this.inputLayer = new NeuronLayer(inputs, 1);
            this.hiddenLayers = hiddenLayers;
            this.outputLayer = new NeuronLayer(outputs, this.hiddenLayers[hiddenLayers.Count - 1].Capacity);
        }


        /// <summary>
        /// Deep copy constructor
        /// </summary>
        /// <param name="copyNetwork">Network to deep copy</param>
        public NeuralNetwork(NeuralNetwork source)
        {
            this.inputLayer = new NeuronLayer(source.inputLayer);
            foreach (NeuronLayer layer in source.hiddenLayers)
            {
                this.hiddenLayers.Add(new NeuronLayer(layer));
            }
            this.outputLayer = new NeuronLayer(source.outputLayer);
        }

        /// <summary>
        /// Create neuron matrix by reference to a list of hidden neuron layers
        /// </summary>
        public static List<NeuronLayer> InitNeurons(List<NeuronLayer> neuronLayers)
        {
            List<NeuronLayer> newNeuronLayers = new List<NeuronLayer>();
            newNeuronLayers.Capacity = neuronLayers.Capacity;
            for (int layer = 0; layer < newNeuronLayers.Capacity; layer++)
            {
                newNeuronLayers.Add(new NeuronLayer(neuronLayers[layer]));
            }
            return newNeuronLayers;
        }

        /// <summary>
        /// Feed forward this neural network with a given input array
        /// </summary>
        /// <param name="inputs">Inputs to network</param>
        /// <returns></returns>
        public float[] FeedForward(float[] inputs)
        {
            NeuronLayer.FeedForward(inputs, inputLayer, hiddenLayers, outputLayer);

            return outputLayer.Inputs();
        }
    }
}