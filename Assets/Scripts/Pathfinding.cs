using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
	private LevelMap map;
	private Grid grid;
	private List<Node> nodeList;
	private Path path;

	
	void Start()
    {
		map = FindObjectOfType<LevelMap>();
		path = map.GetComponent<Path>();
		grid = map.GetComponent<Grid>();
		nodeList = path.NodeList;
    }

	/// <summary>
	/// FindPath: Actual pathfinding in A* using Chebyshev
	/// </summary>
	/// <param name="startPos">Object starting position</param>
	/// <param name="endPos">Object end position</param>
	/// <returns></returns>
    public List<Node> FindPath(Vector2 startPos, Vector2 endPos)
    {
		// Define start and end Nodes
		startPos = (Vector2Int)grid.WorldToCell(startPos);
		Node startNode = GetNodeFromCoord((int)startPos.x, (int)startPos.y);
		endPos = (Vector2Int)grid.WorldToCell(endPos);
		Node endNode = GetNodeFromCoord((int)endPos.x, (int)endPos.y);

		if (startNode == null)
			return new List<Node>();

        startNode.visited = true;
		List<Node> openList = new List<Node>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Set currentNode to first node in list.
            Node currentNode = openList[0];
            openList.RemoveAt(0);

            if (currentNode == endNode)
            {
				return GetFoundPath(endNode);
            }

            Node[] connectedNodes = currentNode.connectedNodes;
            int connectedNodesCount = connectedNodes.Length;

            for (int connectedNodesIndex = 0; connectedNodesIndex < connectedNodesCount; ++connectedNodesIndex)
            {
                Node connectedNode = connectedNodes[connectedNodesIndex];

				if (connectedNode == null)
					continue;
                if (connectedNode.visited)
                    continue;

                int g = currentNode.g + currentNode.connectedNodesCost[connectedNodesIndex];
				int h = DistanceHeuristic(connectedNode.x, connectedNode.y, endNode.x, endNode.y);
                int f = g + h;
                if ((f <= connectedNode.f) || !(connectedNode.visited))
                {
                    connectedNode.g = g;
                    connectedNode.h = h;
                    connectedNode.f = f;

                    if (connectedNode.visited)
                        openList.Sort();
                }
                if (!(connectedNode.visited))
                {
                    connectedNode.visited = true;
                    connectedNode.parent = currentNode;

					openList.Add(connectedNode);
					openList.Sort();
                }
            }
        }

        return GetFoundPath(null);
    }

	/// <summary>
	/// ResetNodes: Cleans the path so it can be used again
	/// </summary>
	protected void ResetNodes()
	{
		foreach (Node node in path.NodeList)
		{
			node.parent = null;
			node.visited = false;
			node.g = 0;
			node.h = 0;
			node.f = 0;
		}
	}

	/// <summary>
	/// GetFoundPath: Returns the path from the list
	/// </summary>
	/// <param name="endNode">Last node in the list.</param>
	/// <returns></returns>
	protected List<Node> GetFoundPath(Node endNode)
	{
		List<Node> foundPath = new List<Node>();
		if (endNode != null)
		{
			foundPath.Add(endNode);

			while (endNode.parent != null)
			{
				foundPath.Add(endNode.parent);
				endNode = endNode.parent;
			}

			// Reverse the path so the start node is at index 0
			foundPath.Reverse();
		}
		ResetNodes();
		return foundPath;
	}

	/// <summary>
	/// GetNodeFromCoord: Get the node with exact X and Y specifications.
	/// </summary>
	/// <param name="findX">The X value</param>
	/// <param name="findY">The Y value</param>
	/// <returns></returns>
	private Node GetNodeFromCoord(int findX, int findY)
	{
		return path.NodeList.Find(i => (i.x == findX) && (i.y == findY));
	}

	/// <summary>
	/// DistanceHeuristic: Chebyshev method of returning an H value for pathfinding.
	/// </summary>
	/// <param name="currentX">Connected node X position</param>
	/// <param name="currentY">Connected node Y position</param>
	/// <param name="targetX">End node X position</param>
	/// <param name="targetY">End node Y position</param>
	/// <returns></returns>
	private int DistanceHeuristic(int currentX, int currentY, int targetX, int targetY)
	{
		int DValue = 1;
		int DValue2 = DValue * 2;
		int dx = Mathf.Abs(currentX - targetX);
		int dy = Mathf.Abs(currentY - targetY);

		return DValue * (dx + dy) + (DValue - DValue2) * Mathf.Min(dx, dy);
	}
}