using UnityEngine;
using System.Collections.Generic;

public class GraphNode : MonoBehaviour
{
    // The Adjacency List: A list of all nodes connected to this one
    public List<GraphNode> neighbors = new List<GraphNode>();

    // Gizmos to visualize the graph in the Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.5f);

        if (neighbors != null)
        {
            Gizmos.color = Color.yellow;
            foreach (GraphNode neighbor in neighbors)
            {
                if (neighbor != null)
                {
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
                }
            }
        }
    }

    // Helper to get a random connection for the Jellyfish to swim to next
    public GraphNode GetRandomNeighbor()
    {
        if (neighbors.Count == 0) return null;
        return neighbors[Random.Range(0, neighbors.Count)];
    }
}