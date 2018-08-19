using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Corridors
{
    public class Corridor : MonoBehaviour, IWorldBounds2D
    {
        private const float size = 10f;
        private const float width_sidewall = 1f * size;
        private const float width_startwall = 0.5f * size;
        private const float width_endwall = 0.5f * size;
        private const float width_goalwall = 0.25f * size;
        private const float length = 5f * size;
        public Vector2 boundsMin = new Vector2(0, 0);
        public Vector2 boundsMax = new Vector2(0, 0);
        private Color color_sidewall = Color.white;
        private Color color_startwall = Color.grey;
        private Color color_endwall = Color.blue;
        private Color color_goalwall = new Color(0, 1, 0, 0.25f);
        private List<Wall> walls;
        
        public List<Segment> corridors;

        private void Awake()
        {
            walls = new List<Wall>(corridors.Count);
            CreateCorridor();   
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        [System.Serializable]
        public class Segment
        {
            public Direction direction = Direction.Up;
            public int length = 1;
        }

        private void CreateStartWall()
        {
            List<Vector2> positions = new List<Vector2>(corridors.Capacity);
            Vector2 pointLeft = new Vector2(-length / 2, 0);
            Vector2 pointRight = new Vector2(length / 2, 0);

            positions.Add(pointLeft);
            positions.Add(pointRight);

            AddWall("Start Wall", width_startwall, positions, color_startwall, WallCollisionResult.Defeat);
        }

        private void CreateCorridor()
        {
            List<Vector2> positionsLeft = new List<Vector2>(corridors.Capacity);
            List<Vector2> positionsRight = new List<Vector2>(corridors.Capacity);
            Vector2 pointLeft = new Vector2(-length / 2, 0);
            Vector2 pointRight = new Vector2(length / 2, 0);

            positionsLeft.Add(pointLeft);
            positionsRight.Add(pointRight);

            bool addGoal = true;
            for (int i = 1; i <= corridors[0].length; i++)
            {
                // Skip goal on last corridor last length
                if ((corridors.Count == 1) && (i == corridors[0].length))
                    addGoal = false;
                AddTurn(positionsLeft, positionsRight, corridors[0].direction, corridors[0].direction, addGoal);
            }
            for (int index = 1; index < corridors.Count; index++)
            {
                // Skip goal on last, so it will add EndWall, but dont skip if length is not 1
                if ((index == corridors.Count - 1) && (corridors[index].length == 1))
                    addGoal = false;
                else
                    addGoal = true;
                AddTurn(positionsLeft, positionsRight, corridors[index - 1].direction, corridors[index].direction, addGoal);
                for (int i = 1; i < corridors[index].length; i++)
                {
                    // Skip goal on last corridor last length
                    if ((index == corridors.Count - 1) && (i == corridors[index].length - 1))
                        addGoal = false;
                    AddTurn(positionsLeft, positionsRight, corridors[index].direction, corridors[index].direction, addGoal);
                }
            }

            CreateStartWall();
            AddWall("Left Wall", width_sidewall, positionsLeft, color_sidewall, WallCollisionResult.Defeat);
            AddWall("Right Wall", width_sidewall, positionsRight, color_sidewall, WallCollisionResult.Defeat);
            CreateEndWall(positionsLeft.Last(), positionsRight.Last());
        }

        private void CreateEndWall(Vector2 pointA, Vector2 pointB)
        {
            List<Vector2> positions = new List<Vector2>(corridors.Capacity)
            {
                pointA,
                pointB
            };

            AddWall("End Wall", width_endwall, positions, color_endwall, WallCollisionResult.Victory);
        }

        private void CreateGoalWall(float width, Vector2 pointA, Vector2 pointB)
        {
            List<Vector2> positions = new List<Vector2>(corridors.Capacity)
            {
                pointA,
                pointB
            };

            AddWall("Goal Wall", width, positions, color_goalwall, WallCollisionResult.Score);
        }

        private GameObject AddWall(string name, float width, List<Vector2> positions, Color color, WallCollisionResult wallCollisionResult)
        {
            GameObject wall_object = new GameObject(name);
            LineRenderer renderer = AddRenderer(width, wall_object, color);

            wall_object.transform.parent = transform;

            renderer.positionCount = positions.Count;
            for (int position = 0; position < renderer.positionCount; position++)
            {
                renderer.SetPosition(position, new Vector2(positions[position].x, positions[position].y));
            }

            AddCollider(wall_object, width, positions);

            Wall wall = wall_object.AddComponent<Wall>();
            wall.collisionResult = wallCollisionResult;
            walls.Add(wall);
            wall.number = walls.Count;

            return wall_object;
        }

        private LineRenderer AddRenderer(float width, GameObject destination, Color color)
        {
            LineRenderer renderer = destination.AddComponent<LineRenderer>();

            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.startColor = color;
            renderer.endColor = color;
            renderer.widthMultiplier = width;

            return renderer;
        }

        private void AddTurn(List<Vector2> positionsLeft, List<Vector2> positionsRight, Direction previousDirection, Direction direction, bool addGoal = true)
        {
            Vector3 pointLeft = new Vector3(positionsLeft[positionsLeft.Count - 1].x, positionsLeft[positionsLeft.Count - 1].y);
            Vector3 pointRight = new Vector3(positionsRight[positionsRight.Count - 1].x, positionsRight[positionsRight.Count - 1].y);

            switch (direction)
            {
                case Direction.Up:
                    switch (previousDirection)
                    {
                        case Direction.Up:
                            pointLeft.y += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointRight.y += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                        case Direction.Down:
                            positionsLeft.RemoveAt(positionsLeft.Count - 1);
                            positionsRight.RemoveAt(positionsRight.Count - 1);
                            break;
                        case Direction.Left:
                            pointLeft.x -= length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointLeft.y += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            break;
                        case Direction.Right:
                            pointRight.x += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            pointRight.y += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (previousDirection)
                    {
                        case Direction.Up:
                            positionsLeft.RemoveAt(positionsLeft.Count - 1);
                            positionsRight.RemoveAt(positionsRight.Count - 1);
                            break;
                        case Direction.Down:
                            pointLeft.y -= length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointRight.y -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                        case Direction.Left:
                            pointRight.x -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            pointRight.y -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                        case Direction.Right:
                            pointLeft.x += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointLeft.y -= length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (previousDirection)
                    {
                        case Direction.Up:
                            pointRight.y += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            pointRight.x -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                        case Direction.Down:
                            pointLeft.y -= length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointLeft.x -= length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            break;
                        case Direction.Left:
                            pointLeft.x -= length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointRight.x -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                        case Direction.Right:
                            positionsLeft.RemoveAt(positionsRight.Count - 1);
                            positionsLeft.RemoveAt(positionsRight.Count - 1);
                            pointRight.y += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            pointRight.x -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (previousDirection)
                    {
                        case Direction.Up:
                            pointLeft.y += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointLeft.x += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            break;
                        case Direction.Down:
                            pointRight.y -= length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            pointRight.x += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                        case Direction.Left:
                            positionsRight.RemoveAt(positionsRight.Count - 1);
                            positionsRight.RemoveAt(positionsRight.Count - 1);
                            pointLeft.y += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointLeft.x += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            break;
                        case Direction.Right:
                            pointLeft.x += length;
                            positionsLeft.Add(new Vector3(pointLeft.x, pointLeft.y));
                            pointRight.x += length;
                            positionsRight.Add(new Vector3(pointRight.x, pointRight.y));
                            break;
                    }
                    break;
            }
            if (addGoal == true)
                CreateGoalWall(width_goalwall, positionsLeft.Last(), positionsRight.Last());

            UpdateBounds(pointLeft);
            UpdateBounds(pointRight);
        }

        public Collider2D AddCollider(GameObject destination, float width, List<Vector2> positions)
        {
            var collider = gameObject.GetComponent<EdgeCollider2D>();

            collider = destination.AddComponent<EdgeCollider2D>();
            collider.points = positions.ToArray();
            collider.edgeRadius = width / 2;

            return collider;
        }

        private void UpdateBounds(Vector2 target)
        {
            if (target.x < 0 && target.x < boundsMin.x)
                boundsMin.x = target.x;
            else if (target.x > 0 && target.x > boundsMax.x)
                boundsMax.x = target.x;
            if (target.y < 0 && target.y < boundsMin.y)
                boundsMin.y = target.y;
            else if (target.y > 0 && target.y > boundsMax.y)
                boundsMax.y = target.y;
        }

        public Vector3 WorldPosition(float z)
        {
            Vector3 position = new Vector3();

            position = (boundsMin + boundsMax) / 2;
            position.z = z;

            return position;
        }

        public Vector2 WorldBoundsSizeVector()
        {
            Vector2 size = new Vector2(0, 0);

            size.x = Mathf.Abs(boundsMin.x) + Mathf.Abs(boundsMax.x);
            size.y = Mathf.Abs(boundsMin.y) + Mathf.Abs(boundsMax.y);

            return size;
        }
    }
}