using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int start = new Vector2Int(0, 1);
    private Vector2Int goal = new Vector2Int(4, 4);
    private Vector2Int next;
    private Vector2Int current;
    [SerializeField] private bool clickToGenerate = false;
    [SerializeField] private float probabilityForObstacles;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private int[,] grid = new int[,]
    {
        { 0, 1, 0, 0, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 0, 0, 1, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0 }
    };

    private void GenerateRandomGrid(int width, int height, float obstacleProbability)
    {
        obstacleProbability = probabilityForObstacles;
        //this keeps the prob between 0 and 1
        obstacleProbability = Mathf.Clamp01(obstacleProbability);

        // will populate grid once until you press again
        if (!clickToGenerate)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[y, x] = (Random.value < obstacleProbability) ? 1 : 0;
                }
            }

            clickToGenerate = true;
        }

        // Draw grid cells
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Vector3 cellPosition = new Vector3(x * width, 0, y * height);
                Gizmos.color = grid[y, x] == 1 ? Color.black : Color.white;
                Gizmos.DrawCube(cellPosition, new Vector3(width, 0.1f, height));
            }
        }

        // Draw path
        foreach (var step in path)
        {
            Vector3 cellPosition = new Vector3(step.x * width, 0, step.y * height);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(cellPosition, new Vector3(width, 0.1f, height));
        }

        // Draw start and goal
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(start.x * width, 0, start.y * height), new Vector3(width, 0.1f, height));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(goal.x * width, 0, goal.y * height), new Vector3(width, 0.1f, height));

        
    }
    public bool AddObstacle(Vector2Int position)
    {
        if (!IsInBounds(position))
        {
            Debug.Log("AddObstacle: position out of bounds");
            return false;
        }

        // Don't place an obstacle on the start or goal
        if (position == start || position == goal)
        {
            Debug.Log("AddObstacle: cannot place on start or goal");
            return false;
        }

        // Makes it so an obstacle wont place on an existing one
        if (grid[position.y, position.x] == 1)
        {
            Debug.Log($"AddObstacle: cell ({position.x},{position.y}) is already an obstacle.");
            return false;
        }

        grid[position.y, position.x] = 1;
        FindPath(start, goal);
        return true;
    }

    [ContextMenu("Generate Grid")]
    private void ContextRegenerateGrid()
    {
        // Generates a new grid
        clickToGenerate = false;
    }

    [ContextMenu("Clear Grid")]
    private void ContextClearGrid()
    {
        for (int y = 0; y < grid.GetLength(0); y++)
            for (int x = 0; x < grid.GetLength(1); x++)
                grid[y, x] = 0;

        clickToGenerate = true; // treat as generated so GenerateRandomGrid won't overwrite
        FindPath(start, goal);
    }

    [ContextMenu("Add Random obstacle")]
    private void ContextAddExampleObstacle()
    {
        AddObstacle(new Vector2Int(Random.Range(0, 4), Random.Range(0, 4)));
        
    }

    private void RandomizeStartAndGoal()
    {
        do
        {
            start = new Vector2Int(Random.Range(0, grid.GetLength(1)), Random.Range(0, grid.GetLength(0)));
        } while (grid[start.y, start.x] == 1);

        do
        {
            goal = new Vector2Int(Random.Range(0, grid.GetLength(1)), Random.Range(0, grid.GetLength(0)));
        } while (goal == start || grid[goal.y, goal.x] == 1);
    }

    private void OnDrawGizmos()
    {
        GenerateRandomGrid(10, 10, probabilityForObstacles);
        RandomizeStartAndGoal();
        FindPath(start, goal);
    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.x < grid.GetLength(1) && point.y >= 0 && point.y < grid.GetLength(0);
    }

    private void FindPath(Vector2Int start, Vector2Int goal)
    {
        path.Clear();

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                next = current + direction;

                if (IsInBounds(next) && grid[next.y, next.x] == 0 && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found.");
            return;
        }

        // Trace path from goal to start
        Vector2Int step = goal;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(start);
        path.Reverse();
    }
}
