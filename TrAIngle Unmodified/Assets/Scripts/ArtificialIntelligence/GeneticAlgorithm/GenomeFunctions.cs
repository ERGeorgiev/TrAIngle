using System;
using System.Collections.Generic;

namespace Artificialintelligence.Genetic
{
    public class GenomeFunctions<T>
    {
        public Func<T, T> generateGene;
        public Func<Gene<T>, Gene<T>, CrossoverType, List<Gene<T>>> crossoverGenes;
        public Action<Gene<T>, float, MutationType> mutateGenes;
        public Func<List<T>, List<T>> allelesDeepCopy;
        //public readonly Func<Gene<T>, CrossoverType, List<Gene<T>>> mutateGenes;

        public GenomeFunctions
            (
            Func<T, T> generateGene, 
            Func<Gene<T>, Gene<T>, CrossoverType, List<Gene<T>>> crossoverGenes,
            Action<Gene<T>, float, MutationType> mutateGenes,
            Func<List<T>, List<T>> allelesDeepCopy)
        {
            this.generateGene = generateGene;
            this.crossoverGenes = crossoverGenes;
            this.mutateGenes = mutateGenes;
            this.allelesDeepCopy = allelesDeepCopy;
        }
    }
}