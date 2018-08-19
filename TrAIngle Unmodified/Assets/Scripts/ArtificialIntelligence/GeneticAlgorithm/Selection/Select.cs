using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artificialintelligence.Genetic
{
    class Select<T>
    {
        private static Random random = new Random();

        public static Gene<T> Gene(List<Gene<T>> entities, SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.Random:
                    return Random(entities);
                case SelectionType.RouletteWheel:
                    return RouletteWheel(entities);
                case SelectionType.Tournament:
                    return Tournament(entities);
                default:
                    return null;
            }
        }

        public static Gene<T> Random(List<Gene<T>> entities)
        {
            if (entities.Count <= 0)
                return null;

            int index = random.Next(0, entities.Count);
            return entities[index];
        }

        public static Gene<T> RouletteWheel(List<Gene<T>> entities)
        {
            if (entities.Count <= 0)
                return default(Gene<T>);

            float totalScore = 0;
            Dictionary<Gene<T>, float> pieLedger = new Dictionary<Gene<T>, float>();

            foreach (Gene<T> entity in entities)
            {
                totalScore += entity.Fitness;
                pieLedger.Add(entity, totalScore);
            }

            float chosen = (float)random.NextDouble() * totalScore;
            foreach (Gene<T> entity in pieLedger.Keys)
            {
                if (pieLedger[entity] >= chosen)
                    return entity;
            }

            return pieLedger.Last().Key;
        }

        public static Gene<T> Tournament(List<Gene<T>> entities)
        {
            if (entities.Count <= 0)
                return default(Gene<T>);

            Gene<T> entityA = Random(entities);
            Gene<T> entityB = Random(entities);

            if (entityA.Fitness >= entityB.Fitness)
                return entityA;
            else
                return entityB;
        }
    }
}
