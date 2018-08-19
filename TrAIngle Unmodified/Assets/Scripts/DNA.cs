using System;

public class DNA<T>
{
	public T[] Allele { get; private set; }
	public float Fitness { get; private set; }

	private Random random;
	private Func<T> getRandomGene;
	private Func<int, float> fitnessFunction;

	public DNA(int size, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
	{
		Allele = new T[size];
		this.random = random;
		this.getRandomGene = getRandomGene;
		this.fitnessFunction = fitnessFunction;

		if (shouldInitGenes)
		{
			for (int i = 0; i < Allele.Length; i++)
			{
				Allele[i] = getRandomGene();
			}
		}
	}

	public float CalculateFitness(int index)
	{
		Fitness = fitnessFunction(index);
		return Fitness;
	}

	public DNA<T> Crossover(DNA<T> otherParent)
	{
		DNA<T> child = new DNA<T>(Allele.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

		for (int i = 0; i < Allele.Length; i++)
		{
			child.Allele[i] = random.NextDouble() < 0.5 ? Allele[i] : otherParent.Allele[i];
		}

		return child;
	}

	public void Mutate(float mutationRate)
	{
		for (int i = 0; i < Allele.Length; i++)
		{
			if (random.NextDouble() < mutationRate)
			{
				Allele[i] = getRandomGene();
			}
		}
	}
}