using System;
using System.Collections.Generic;
using Artificialintelligence.Genetic;

namespace Artificialintelligence.Neural
{
    public class Crossover
    {
        public static float ratio = 0.7f;
        private static Random random = new Random();

        public static List<Gene<NeuronLayer>> NeuronLayer(Gene<NeuronLayer> geneA, Gene<NeuronLayer> geneB, CrossoverType crossoverType)
        {
            switch(crossoverType)
            {
                case CrossoverType.SinglePoint:
                    return SinglePoint(geneA, geneB);
                case CrossoverType.MultiPoint:
                    return MultiPoint(geneA, geneB);
                case CrossoverType.OrderBased:
                    return OrderBased(geneA, geneB);
                default:
                    throw new NotImplementedException("More research is needed");
            }
        }

        private static List<Gene<NeuronLayer>> SinglePoint(Gene<NeuronLayer> geneA, Gene<NeuronLayer> geneB)
        {
            Gene<NeuronLayer> childA = new Gene<NeuronLayer>(geneA);
            Gene<NeuronLayer> childB = new Gene<NeuronLayer>(geneB);

            int maxLayers = Math.Min(childA.Allele.Count, childA.Allele.Count);
            for (int i = 0; i < maxLayers; i++)
            {
                if ((float)random.NextDouble() >= ratio)
                    continue;
                SinglePoint(childA.Allele[i], geneB.Allele[i]);
                SinglePoint(childB.Allele[i], geneA.Allele[i]);
            }

            return new List<Gene<NeuronLayer>>()
            {
                childA,
                childB
            };
        }

        private static void SinglePoint(NeuronLayer layerA, NeuronLayer layerB)
        {
            int maxNeurons = Math.Min(layerA.Count, layerB.Count);
            int position = random.Next(0, maxNeurons);

            for (int i = position; i < maxNeurons; i++)
            {
                layerA.neurons[i] = new Neuron(layerB.neurons[i]);
            }
        }

        private static List<Gene<NeuronLayer>> MultiPoint(Gene<NeuronLayer> geneA, Gene<NeuronLayer> geneB)
        {
            Gene<NeuronLayer> childA = new Gene<NeuronLayer>(geneA);
            Gene<NeuronLayer> childB = new Gene<NeuronLayer>(geneB);

            int maxLayers = Math.Min(childA.Allele.Count, childA.Allele.Count);
            for (int i = 0; i < maxLayers; i++)
            {
                if ((float)random.NextDouble() >= ratio)
                    continue;
                MultiPoint(childA.Allele[i], geneB.Allele[i]);
                MultiPoint(childB.Allele[i], geneA.Allele[i]);
            }

            return new List<Gene<NeuronLayer>>()
            {
                childA,
                childB
            };
        }

        private static void MultiPoint(NeuronLayer layerA, NeuronLayer layerB)
        {
            int maxNeurons = Math.Min(layerA.Count, layerB.Count);
            int positionA = random.Next(0, maxNeurons);
            int positionB = random.Next(positionA, maxNeurons);

            for (int i = positionA; i <= positionB; i++)
            {
                layerA.neurons[i] = new Neuron(layerB.neurons[i]);
            }
        }

        private static List<Gene<NeuronLayer>> OrderBased(Gene<NeuronLayer> geneA, Gene<NeuronLayer> geneB)
        {
            Gene<NeuronLayer> childA = new Gene<NeuronLayer>(geneA);
            Gene<NeuronLayer> childB = new Gene<NeuronLayer>(geneB);

            int maxLayers = Math.Min(childA.Allele.Count, childA.Allele.Count);
            for (int i = 0; i < maxLayers; i++)
            {
                if ((float)random.NextDouble() >= ratio)
                    continue;
                OrderBased(childA.Allele[i], geneB.Allele[i]);
                OrderBased(childB.Allele[i], geneA.Allele[i]);
            }

            return new List<Gene<NeuronLayer>>()
            {
                childA,
                childB
            };
        }

        private static void OrderBased(NeuronLayer layerA, NeuronLayer layerB)
        {
            int maxNeurons = Math.Min(layerA.Count, layerB.Count);

            for (int i = 0; i < maxNeurons; i++)
            {
                if ((float)random.NextDouble() >= 0.5f)
                    continue;
                layerA.neurons[i] = new Neuron(layerB.neurons[i]);
            }
        }
    }
}
