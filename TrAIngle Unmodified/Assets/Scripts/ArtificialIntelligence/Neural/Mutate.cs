using System;
using System.Collections.Generic;
using Artificialintelligence.Genetic;

namespace Artificialintelligence.Neural
{
    public class Mutate
    {
        private static Random random = new Random();

        public static void NeuronLayer(Gene<NeuronLayer> geneA, float MutationRate, MutationType mutationType)
        {
            if (random.NextDouble() >= MutationRate)
                return;

            switch (mutationType)
            {
                case MutationType.Exchange:
                    Exchange(geneA);
                    break;
                case MutationType.Random:
                    Random(geneA);
                    break;
                case MutationType.Binary:
                    Binary(geneA);
                    break;
                default:
                    throw new NotImplementedException("More research is needed");
            }
        }

        private static void Exchange(Gene<NeuronLayer> geneA)
        {
            int posA = random.Next(0, geneA.Allele.Count);

            Exchange(geneA.Allele[posA]);
        }

        private static void Exchange(NeuronLayer layerA)
        {
            int posA = random.Next(0, layerA.Count);
            int posB = random.Next(0, layerA.Count);
            Neuron neuronA = layerA.neurons[posA];

            layerA.neurons[posA] = layerA.neurons[posB];
            layerA.neurons[posB] = neuronA;
        }

        private static void Binary(Gene<NeuronLayer> geneA)
        {
            int posA = random.Next(0, geneA.Allele.Count);

            Binary(geneA.Allele[posA]);
        }

        private static void Binary(NeuronLayer layerA)
        {
            int posA = random.Next(0, layerA.Count);
            int posB = random.Next(0, layerA.Count);
            Neuron neuronA = layerA.neurons[posA];

            layerA.neurons[posA] = layerA.neurons[posB];
            layerA.neurons[posB] = neuronA;
        }

        private static void Random(Gene<NeuronLayer> geneA)
        {
            int posA = random.Next(0, geneA.Allele.Count);

            Random(geneA.Allele[posA]);
        }

        private static void Random(NeuronLayer layerA)
        {
            int posA = random.Next(0, layerA.Count);

            layerA.neurons[posA].Mutate();
        }
    }
}
