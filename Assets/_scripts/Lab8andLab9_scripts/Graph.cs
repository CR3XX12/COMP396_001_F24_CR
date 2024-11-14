using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeuristicStrategy { EuclideanDistance, ManhattanDistance, DictionaryDistance };

public class Graph
{
    private Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();
    private HeuristicStrategy strategy;
    private Dictionary<char, Vector3> verticesData = new Dictionary<char, Vector3>();

    public Graph(HeuristicStrategy strategy = HeuristicStrategy.EuclideanDistance)
    {
        this.strategy = strategy;
    }

    // Method to add vertices for general usage
    public void AddVertex(char vertex, Dictionary<char, int> edges)
    {
        vertices[vertex] = edges;
    }

    // Method to add vertices with positions for A*
    public void AddVertexForAStarWithPosition(char vertex, Vector3 pos, Dictionary<char, int> edges)
    {
        vertices[vertex] = edges;
        verticesData[vertex] = pos;
    }

    // Method to add vertices with heuristic values (for DictionaryDistance heuristic)
    public void AddVertexForAStarWithHeuristic(char vertex, float heuristic, Dictionary<char, int> edges)
    {
        vertices[vertex] = edges;
        verticesData[vertex] = new Vector3(heuristic, 0, 0); // Store heuristic in x-component
    }

    // Distance calculation methods
    public float EuclideanDistance(Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1, v2);
    }

    public float ManhattanDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y) + Mathf.Abs(v1.z - v2.z);
    }

    public float GoalDistanceEstimate(char node, char finish)
    {
        switch (strategy)
        {
            case HeuristicStrategy.EuclideanDistance:
                return EuclideanDistance(verticesData[node], verticesData[finish]);
            case HeuristicStrategy.ManhattanDistance:
                return ManhattanDistance(verticesData[node], verticesData[finish]);
            case HeuristicStrategy.DictionaryDistance:
                return verticesData[node].x;  // Assume heuristic stored in x-component
            default:
                return 0;
        }
    }

    // Dijkstra’s shortest path method
    public List<char> ShortestPathViaDijkstra(char start, char finish)
    {
        List<char> path = new List<char>();
        var distances = new Dictionary<char, int>();
        var previous = new Dictionary<char, char>();
        var pending = new List<char>();

        foreach (var v in vertices)
        {
            distances[v.Key] = int.MaxValue;
            previous[v.Key] = '\0';
            pending.Add(v.Key);
        }
        distances[start] = 0;

        while (pending.Count > 0)
        {
            pending.Sort((x, y) => distances[x].CompareTo(distances[y]));
            var u = pending[0];
            pending.Remove(u);

            if (u == finish) break;

            foreach (var neighbor in vertices[u])
            {
                var alt = distances[u] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = u;
                }
            }
        }

        char current = finish;
        while (current != '\0')
        {
            path.Insert(0, current);
            current = previous[current];
        }

        return path;
    }

    // A* Algorithm
    public List<char> ShortestPathViaAStar(char start, char finish)
    {
        List<char> path = new List<char>();
        var previous = new Dictionary<char, char>();
        var gScore = new Dictionary<char, float>();
        var fScore = new Dictionary<char, float>();
        var openSet = new List<char>();
        var closedSet = new HashSet<char>();

        gScore[start] = 0;
        fScore[start] = GoalDistanceEstimate(start, finish);
        previous[start] = '-';
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            openSet.Sort((x, y) => fScore[x].CompareTo(fScore[y]));
            var current = openSet[0];

            if (current == finish)
            {
                while (current != '-')
                {
                    path.Insert(0, current);
                    current = previous[current];
                }
                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in vertices[current].Keys)
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeGScore = gScore[current] + vertices[current][neighbor];

                if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor]) continue;

                previous[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GoalDistanceEstimate(neighbor, finish);
            }
        }

        return null; // No path found
    }

    // BFS Algorithm
    public List<char> BFS(char start, char goal)
    {
        Queue<char> queue = new Queue<char>();
        Dictionary<char, bool> explored = new Dictionary<char, bool>();
        Dictionary<char, char> parent = new Dictionary<char, char>();

        queue.Enqueue(start);
        explored[start] = true;

        while (queue.Count > 0)
        {
            char current = queue.Dequeue();

            if (current == goal)
            {
                List<char> path = new List<char>();
                while (current != '\0')
                {
                    path.Insert(0, current);
                    current = parent.ContainsKey(current) ? parent[current] : '\0';
                }
                return path;
            }

            foreach (var neighbor in vertices[current].Keys)
            {
                if (!explored.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    explored[neighbor] = true;
                    parent[neighbor] = current;
                }
            }
        }

        return null; // No path found
    }

    // DFS Algorithm
    public List<char> DFS(char start, char goal)
    {
        Stack<char> stack = new Stack<char>();
        Dictionary<char, bool> visited = new Dictionary<char, bool>();
        Dictionary<char, char> parent = new Dictionary<char, char>();

        stack.Push(start);
        visited[start] = true;

        while (stack.Count > 0)
        {
            char current = stack.Pop();

            if (current == goal)
            {
                List<char> path = new List<char>();
                while (current != '\0')
                {
                    path.Insert(0, current);
                    current = parent.ContainsKey(current) ? parent[current] : '\0';
                }
                return path;
            }

            foreach (var neighbor in vertices[current].Keys)
            {
                if (!visited.ContainsKey(neighbor))
                {
                    stack.Push(neighbor);
                    visited[neighbor] = true;
                    parent[neighbor] = current;
                }
            }
        }

        return null; // No path found
    }
}
