using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Artificialintelligence;

namespace Corridors
{
    public class Wall : MonoBehaviour
    {
        public int number;
        public WallCollisionResult collisionResult = WallCollisionResult.Nothing;

        private void Start()
        {
            if (collisionResult == WallCollisionResult.Score || collisionResult == WallCollisionResult.Victory)
            {
                GetComponent<Collider2D>().isTrigger = true;
                gameObject.layer = Layers.ScoreWall;
            }
        }

        public Wall(WallCollisionResult collisionResult)
        {
            this.collisionResult = collisionResult;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IntelligentEntity>().Gene != null)
            {
                CollisionResult(collision);
                // Debug.Log("Goal " + name + " registered " + collision);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnTriggerEnter2D(collision.collider);
        }

        private void CollisionResult(Collider2D other)
        {
            var entity = other.GetComponent<IntelligentEntity>();
            switch (collisionResult)
            {
                case WallCollisionResult.Victory:
                    if (entity.Fitness < number)
                    {
                        entity.Fitness += 1;
                        entity.Victory();
                    }
                    break;
                case WallCollisionResult.Defeat:
                    entity.Defeat();
                    break;
                case WallCollisionResult.Score:
                    if (entity.Fitness < number)
                    {
                        entity.Fitness = number;
                    }
                    break;
            }
        }
    }
}