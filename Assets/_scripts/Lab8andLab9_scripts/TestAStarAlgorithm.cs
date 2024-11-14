using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStarAlgorithm : MonoBehaviour
{
    void Start()
    {
        // Graph with Euclidean distance
        Graph g = new Graph(HeuristicStrategy.EuclideanDistance);
        g.AddVertexForAStarWithPosition('A', new Vector3(0, 4, 0), new Dictionary<char, int> { { 'B', 4 }, { 'X', 20 } });
        g.AddVertexForAStarWithPosition('B', new Vector3(4, 4, 0), new Dictionary<char, int> { { 'A', 4 }, { 'C', 4 } });
        g.AddVertexForAStarWithPosition('C', new Vector3(8, 4, 0), new Dictionary<char, int> { { 'B', 4 }, { 'Z', 4 } });
        g.AddVertexForAStarWithPosition('X', new Vector3(0, 0, 0), new Dictionary<char, int> { { 'A', 20 }, { 'W', 4 }, { 'Y', 4 } });
        g.AddVertexForAStarWithPosition('Y', new Vector3(4, 0, 0), new Dictionary<char, int> { { 'X', 4 }, { 'Z', 6 } });
        g.AddVertexForAStarWithPosition('Z', new Vector3(8, 0, 0), new Dictionary<char, int> { { 'C', 4 }, { 'Y', 4 } });
        g.AddVertexForAStarWithPosition('W', new Vector3(12, 0, 0), new Dictionary<char, int> { { 'X', 4 } });

        List<char> path = g.ShortestPathViaAStar('A', 'W');
        Debug.Log("Path from A to W using A* with Euclidean: " + (path != null ? string.Join(" -> ", path) : "No path found"));
    }
}
