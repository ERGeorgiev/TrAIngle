using System.Collections.Generic;
using System;

namespace Artificialintelligence.Genetic
{
    public class Gene<T> : System.IComparable<Gene<T>>
    {
        public List<T> Allele { get; private set; }
        private float fitness = 0;
        public float Fitness
        {
            get { return fitness; }
            set
            {
                fitness = Math.Max(0, value);
            }
        }

        private readonly GenomeFunctions<T> functions;
        private readonly GenomeSettings settings;

        public Gene(int size, GenomeFunctions<T> functions, GenomeSettings settings, bool initGenes = true)
        {
            this.settings = settings;
            this.functions = functions;
            this.Allele = new List<T>(size);
            this.Fitness = 0;

            if (initGenes == true) InitGenes();
        }

        public Gene(Gene<T> source)
        {
            this.settings = source.settings;
            this.functions = source.functions;
            this.Allele = new List<T>(source.Allele.Count);
            this.Allele = functions.allelesDeepCopy(source.Allele);
            this.Fitness = 0;
        }

        public List<Gene<T>> Crossover(Gene<T> partner)
        {
            return functions.crossoverGenes(this, partner, settings.crossoverType);
        }

        public void Mutate()
        {
            functions.mutateGenes(this, settings.mutationRate, settings.mutationType);
        }

        private void InitGenes()
        {
            Allele.Clear();
            for (int i = Allele.Capacity - 1; i < Allele.Capacity; i++)
            {
                Allele.Add(functions.generateGene(default(T)));
            }
            for (int i = 1; i < Allele.Capacity; i++)
            {
                Allele.Add(functions.generateGene(Allele[i - 1]));
            }
        }

        int System.IComparable<Gene<T>>.CompareTo(Gene<T> other)
        {
            if (Fitness > other.Fitness)
                return -1;
            else if (Fitness < other.Fitness)
                return 1;
            else
                return 0;
        }

        public override string ToString()
        {
            if (typeof(T) == typeof(char))
            {
                return new string((char[])(object)Allele);
            }
            else
            {
                string result = "";
                foreach(var x in Allele)
                {
                    result += x.ToString() + "; ";
                }
                return result;
            }
        }
    }
}