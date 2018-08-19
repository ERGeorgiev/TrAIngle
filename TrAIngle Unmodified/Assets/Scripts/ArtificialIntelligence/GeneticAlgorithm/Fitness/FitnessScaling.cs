using System;
using System.Collections.Generic;

namespace Artificialintelligence.Genetic
{
    public class FitnessScaling<T>
    {
        public static void ScaleFitness(List<Gene<T>> genes, FitnessScalingType scalingType)
        {
            switch (scalingType)
            {
                case FitnessScalingType.SigmaTruncation:
                    SigmaTruncation(genes);
                    break;
                case FitnessScalingType.Rank:
                    Rank(genes);
                    break;
                case FitnessScalingType.None:
                    return;
            }
        }

        public static float GetFitnessAverage(List<Gene<T>> genes)
        {
            float fitnessAverage = 0;

            foreach (Gene<T> gene in genes)
                fitnessAverage += gene.Fitness;
            fitnessAverage /= genes.Count;

            return fitnessAverage;
        }

        public static float GetFitnessDeviation(List<Gene<T>> genes, float fitnessAverage)
        {
            float fitnessDeviation = 0;

            foreach (Gene<T> gene in genes)
                fitnessDeviation += Math.Abs(fitnessAverage - gene.Fitness);
            fitnessDeviation /= genes.Count;

            return fitnessDeviation;
        }

        private static float SigmaTruncation(float fitness, float fitnessAverage, float fitnessDeviation)
        {
            // Returns a number bigger than 0
            return Math.Max(fitness - (fitnessAverage - 2 * fitnessDeviation), 0);
        }

        private static void SigmaTruncation(List<Gene<T>> genes)
        {
            float fitnessAverage = GetFitnessAverage(genes);
            float fitnessDeviation = GetFitnessDeviation(genes, fitnessAverage);

            foreach (Gene<T> gene in genes)
                gene.Fitness = SigmaTruncation(gene.Fitness, fitnessAverage, fitnessDeviation);
        }

        private static void Rank(List<Gene<T>> genes)
        {
            genes.Sort();

            for (int i = 0; i < genes.Count; i++)
            {
                genes[i].Fitness = genes.Count - i;
            }
        }
    }
}