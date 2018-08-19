using System;

namespace Artificialintelligence.Genetic
{
    [Serializable]
    public class GenomeSettings
    {
        public int elitism;
        public float mutationRate;
        public FitnessScalingType scalingType;
        public ReproductionType reproductionType;
        public SelectionType selectionType;
        public MutationType mutationType;
        public CrossoverType crossoverType;

        public GenomeSettings
            (
            int elitism, 
            float mutationRate, 
            FitnessScalingType scalingType,
            ReproductionType reproductionType,
            SelectionType selectionType,
            MutationType mutationType,
            CrossoverType crossoverType
            )
        {
            this.elitism = elitism;
            this.mutationRate = mutationRate;
            this.scalingType = scalingType;
            this.reproductionType = reproductionType;
            this.selectionType = selectionType;
            this.mutationType = mutationType;
            this.crossoverType = crossoverType;

        }

        public GenomeSettings(int genomeSize)
        {
            this.elitism = Math.Max(1, genomeSize / 10);
            this.mutationRate = 0.01f;
            this.scalingType = FitnessScalingType.SigmaTruncation;
            this.reproductionType = ReproductionType.Generational;
            this.selectionType = SelectionType.RouletteWheel;
            this.mutationType = MutationType.Exchange;
            this.crossoverType = CrossoverType.SinglePoint;
        }
    }
}