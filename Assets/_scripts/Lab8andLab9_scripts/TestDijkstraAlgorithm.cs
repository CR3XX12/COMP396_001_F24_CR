using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDijkstraAlgorithm : MonoBehaviour
{
    void Start()
    {
        // Initialize the graph as before
        Graph g = new Graph();
        g.AddVertex('A', new Dictionary<char, int> { { 'B', 10 }, { 'C', 12 }, { 'D', 4 }, { 'E', 2 } });
        g.AddVertex('B', new Dictionary<char, int> { { 'C', 2 }, { 'D', 4 }, { 'E', 5 }, { 'F', 5 } });
        g.AddVertex('C', new Dictionary<char, int> { { 'B', 6 }, { 'F', 2 } });
        g.AddVertex('D', new Dictionary<char, int> { { 'B', 3 }, { 'E', 3 } });
        g.AddVertex('E', new Dictionary<char, int> { { 'B', 3 }, { 'D', 3 }, { 'F', 9 } });
        g.AddVertex('F', new Dictionary<char, int>());

        // Dijkstra's Algorithm Test
        List<char> shortestPath = g.ShortestPathViaDijkstra('A', 'F');
        Debug.Log("Shortest Path from A to F using Dijkstra's: " + string.Join(" -> ", shortestPath));

        // BFS Test
        List<char> bfsPath = g.BFS('A', 'F');
        Debug.Log("Path from A to F using BFS: " + (bfsPath != null ? string.Join(" -> ", bfsPath) : "No path found"));

        // DFS Test
        List<char> dfsPath = g.DFS('A', 'F');
        Debug.Log("Path from A to F using DFS: " + (dfsPath != null ? string.Join(" -> ", dfsPath) : "No path found"));
    }
}
