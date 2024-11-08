using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();

    public void AddVertex(char vertex, Dictionary<char, int> edges)
    {
        vertices[vertex] = edges;
    }

    public List<char> ShortestPathViaDijkstra(char start, char finish)
    {
        List<char> path = new List<char>();
        var distances = new Dictionary<char, int>();
        var previous = new Dictionary<char, char>();
        var pending = new List<char>();

        // Step 1: Initialize distances and pending vertices
        foreach (var v in vertices)
        {
            distances[v.Key] = int.MaxValue;
            previous[v.Key] = '\0';
            pending.Add(v.Key);
        }
        distances[start] = 0;

        // Step 2: Process the graph
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

        // Step 3: Reconstruct the shortest path
        char current = finish;
        while (current != '\0')
        {
            path.Insert(0, current);
            current = previous[current];
        }

        return path;
    }

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
