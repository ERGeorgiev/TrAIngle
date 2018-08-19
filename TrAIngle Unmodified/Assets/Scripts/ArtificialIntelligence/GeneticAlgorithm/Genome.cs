using System;
using System.Collections.Generic;
using System.Linq;

namespace Artificialintelligence.Genetic
{
    public class Genome<T>
    {
        public Gene<T> Elite;
        public List<Gene<T>> Genes { get; private set; }
        public int Size
        {
            get { return Genes.Capacity; }
            set
            {
                if (value <= 0)
                {
                    Genes.Clear();
                    Genes.Capacity = 0;
                }
                else if (value >= Genes.Capacity)
                {
                    Genes.Capacity = value;
                }
                else
                {
                    Genes.RemoveRange(value, Genes.Count - value);
                    Genes.Capacity = value;
                }
            }
        }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }
        public static Random random = new Random();

        private float totalFitness;
        private readonly int geneSize;
        private readonly System.Func<T, T> generateGene;
        public GenomeSettings settings;
        public GenomeFunctions<T> functions;

        public Genome(int genomeSize, int geneSize, GenomeFunctions<T> functions, GenomeSettings settings = null)
        {
            if (settings == null)
                this.settings = new GenomeSettings(genomeSize);
            else
                this.settings = settings;
            this.Generation = 1;
            this.Genes = new List<Gene<T>>(genomeSize);
            this.geneSize = geneSize;
            this.functions = functions;

            for (int i = 0; i < Genes.Capacity; i++)
            {
                Genes.Add(new Gene<T>(geneSize, functions, settings));
            }
        }

        public void Reproduce()
        {
            List<Gene<T>> newGenes = new List<Gene<T>>(Genes.Capacity);

            newGenes.AddRange(GetElites());
            foreach (var gene in newGenes)
                gene.Fitness = 0;
            newGenes.AddRange(GenerateGenes(crossover: false, count: Genes.Capacity - settings.elitism));

            Genes = newGenes;
            Generation++;
        }

        public List<Gene<T>> GetElites(int elitismOverride = 0)
        {
            List<Gene<T>> elites = new List<Gene<T>>();
            if (Genes.Count < 1) return elites;

            if (elitismOverride != 0)
                elites.Capacity = elitismOverride;
            else
                elites.Capacity = settings.elitism;

            FitnessScaling<T>.ScaleFitness(Genes, settings.scalingType);
            Genes.Sort();

            Elite = new Gene<T>(Genes[0]);
            for (int i = 0; i < elites.Capacity && i < Genes.Count; i++)
                elites.Add(new Gene<T>(Genes[i]));

            return elites;
        }

        private List<Gene<T>> GenerateGenes(bool crossover, int count)
        {
            List<Gene<T>> generatedGenes = new List<Gene<T>>();
            if (count < 1) return generatedGenes;
            generatedGenes.Capacity = count;

            if (crossover == true)
            {
                int pairs = (int)Math.Ceiling((double)generatedGenes.Capacity / 2);
                for (int i = 0; i < pairs; i++)
                    generatedGenes.AddRange(Crossover()); // Generates 2 children
                if (generatedGenes.Count > count)
                    generatedGenes.Remove(generatedGenes.Last());
            }
            else
            {
                for (int i = 0; i < generatedGenes.Capacity; i++)
                    generatedGenes.Add(new Gene<T>(geneSize, functions, settings, initGenes: true));
            }

            return generatedGenes;
        }

        private List<Gene<T>> Crossover()
        {
            Gene<T> parentA = GetParent();
            Gene<T> parentB = GetParent();
            List<Gene<T>> children = parentA.Crossover(parentB);

            foreach (Gene<T> child in children)
            {
                child.Mutate();
            }

            return children;
        }

        private Gene<T> GetParent()
        {
            return Select<T>.Gene(Genes, settings.selectionType);
        }
    }
}