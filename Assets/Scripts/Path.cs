using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : IComparable<Node>
{
	// Node details
	public int index;
	public int x;
	public int y;
	public Vector2 pos;
	public TileBase tileData;

	// Node relationships
	public Node parent;
	public Node[] connectedNodes = new Node[8];
	public int[] connectedNodesCost = new int[8];
	public bool visited = false;

	// A* values
	public int f;
	public int g;
	public int h;
	// public bool onOpenList = false;

	public int CompareTo(Node node)
	{
		if (f < node.f)
		{
			return -1;
		}
		else if (f > node.f)
		{
			return 1;
		}
		else if (g < node.g)
		{
			return -1;
		}
		else if (g > node.g)
		{
			return 1;
		}

		return 0;
	}
}

public class Path : MonoBehaviour
{
	private LevelMap map;
	private Grid grid;
	private List<Node> nodeList;

	public List<Node> NodeList
    {
		get { return nodeList; }
    }

	private enum Direction { up, down, left, right, UL, UR, DL, DR }

	/// <summary>
	/// Initializing all the nodes using the map formed in LevelMap
	/// </summary>
	void Start()
    {
		map = GetComponent<LevelMap>();
		grid = map.GetComponent<Grid>();
		nodeList = new List<Node>();
		int index = 0;

		// Create Node List
		for (int y = map.Bounds.yMin; y < map.Bounds.yMax; ++y)
		{
			for (int x = map.Bounds.xMin; x < map.Bounds.xMax; ++x)
			{
				// Node creation
				Node newNode = new Node();
				newNode.index = index;
				newNode.x = x;
				newNode.y = y;
				newNode.tileData = map.MapTiles[index];
				// Save cell position
				newNode.pos = grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
				map.NumColRow.AddOrUpdate(y, 1, (id, count) => count + 1);

				nodeList.Add(newNode);

				index++;
			}
		}

		// Connect Nodes
		FillConnectedNodes();
	}

	/// <summary>
	/// FillConnectedNodes: Sets all the nodes connected around each of the nodes.
	/// </summary>
	private void FillConnectedNodes()
	{
		foreach (Node node in nodeList)
		{
			int arrayPos = 0;
			// Up
			Node up = GetConnectedNode(node, Direction.up);
			if (up != null)
			{
				if (up.tileData != null)
				{
					node.connectedNodes[arrayPos] = up;
					node.connectedNodesCost[arrayPos] = 1;
					arrayPos++;
				}
			}
			// Down
			Node down = GetConnectedNode(node, Direction.down);
			if (down != null)
			{
				if (down.tileData != null)
				{
					node.connectedNodes[arrayPos] = down;
					node.connectedNodesCost[arrayPos] = 1;
					arrayPos++;
				}
			}
			// Left
			Node left = GetConnectedNode(node, Direction.left);
			if (left != null)
			{
				if (left.tileData != null)
				{
					node.connectedNodes[arrayPos] = left;
					node.connectedNodesCost[arrayPos] = 1;
					arrayPos++;
				}
			}
			// Right
			Node right = GetConnectedNode(node, Direction.right);
			if (right != null)
			{
				if (right.tileData != null)
				{
					node.connectedNodes[arrayPos] = right;
					node.connectedNodesCost[arrayPos] = 1;
					arrayPos++;
				}
			}
			// Up-Left
			Node uL = GetConnectedNode(node, Direction.UL);
			if (uL != null)
			{
				if (uL.tileData != null)
				{
					node.connectedNodes[arrayPos] = uL;
					node.connectedNodesCost[arrayPos] = 2;
					arrayPos++;
				}
			}
			// Up Right
			Node uR = GetConnectedNode(node, Direction.UR);
			if (uR != null)
			{
				if (uR.tileData != null)
				{
					node.connectedNodes[arrayPos] = uR;
					node.connectedNodesCost[arrayPos] = 2;
					arrayPos++;
				}
			}
			// Down Left
			Node dL = GetConnectedNode(node, Direction.DL);
			if (dL != null)
			{
				if (dL.tileData != null)
				{
					node.connectedNodes[arrayPos] = dL;
					node.connectedNodesCost[arrayPos] = 2;
					arrayPos++;
				}
			}
			// Down Right
			Node dR = GetConnectedNode(node, Direction.DR);
			if (dR != null)
			{
				if (dR.tileData != null)
				{
					node.connectedNodes[arrayPos] = dR;
					node.connectedNodesCost[arrayPos] = 2;
				}
			}
		}
	}

	/// <summary>
	/// GetConnectedNode: Used to decide which nodes are actually connected.
	/// It is also where we decide which tiles are blocked.
	/// </summary>
	/// <param name="node">The center node.</param>
	/// <param name="dir">Which direction to check for connection.</param>
	/// <returns></returns>
	private Node GetConnectedNode(Node node, Direction dir)
	{
		Node connectedNode = null;
		int connectedNodeIndex;

		if (node.tileData != null)
		{
			switch (dir)
			{
				case Direction.up:
					if (map.NumColRow.ContainsKey(node.y + 1))
					{
						connectedNodeIndex = node.index + ((map.NumColRow[node.y] + map.NumColRow[node.y + 1]) / 2);
						connectedNode = (connectedNodeIndex < nodeList.Count) ? nodeList[connectedNodeIndex] : null;
					}
					break;
				case Direction.down:
					if (map.NumColRow.ContainsKey(node.y - 1))
					{
						connectedNodeIndex = node.index - ((map.NumColRow[node.y] + map.NumColRow[node.y - 1]) / 2);
						connectedNode = (connectedNodeIndex >= 0) ? nodeList[connectedNodeIndex] : null;
					}
					break;
				case Direction.left:
					connectedNodeIndex = node.index - 1;
					if (connectedNodeIndex >= 0)
					{
						if (nodeList[connectedNodeIndex].x == (node.x - 1))
							connectedNode = nodeList[connectedNodeIndex];
					}
					break;
				case Direction.right:
					connectedNodeIndex = node.index + 1;
					if (connectedNodeIndex < nodeList.Count)
					{
						if (nodeList[connectedNodeIndex].x == (node.x + 1))
							connectedNode = nodeList[connectedNodeIndex];
					}
					break;
				case Direction.UL:
					if (map.NumColRow.ContainsKey(node.y + 1))
					{
						connectedNodeIndex = node.index + ((map.NumColRow[node.y] + map.NumColRow[node.y + 1]) / 2) - 1;
						if (connectedNodeIndex >= 0 && connectedNodeIndex < nodeList.Count)
						{
							if (nodeList[connectedNodeIndex].x == (node.x - 1))
								connectedNode = nodeList[connectedNodeIndex];
						}
					}
					break;
				case Direction.UR:
					if (map.NumColRow.ContainsKey(node.y + 1))
					{
						connectedNodeIndex = node.index + ((map.NumColRow[node.y] + map.NumColRow[node.y + 1]) / 2) + 1;
						if (connectedNodeIndex >= 0 && connectedNodeIndex < nodeList.Count)
						{
							if (nodeList[connectedNodeIndex].x == (node.x + 1))
								connectedNode = nodeList[connectedNodeIndex];
						}
					}
					break;
				case Direction.DL:
					if (map.NumColRow.ContainsKey(node.y - 1))
					{
						connectedNodeIndex = node.index - ((map.NumColRow[node.y] + map.NumColRow[node.y - 1]) / 2) - 1;
						if (connectedNodeIndex >= 0 && connectedNodeIndex < nodeList.Count)
						{
							if (nodeList[connectedNodeIndex].x == (node.x - 1))
								connectedNode = nodeList[connectedNodeIndex];
						}
					}
					break;
				case Direction.DR:
					if (map.NumColRow.ContainsKey(node.y - 1))
					{
						connectedNodeIndex = node.index - ((map.NumColRow[node.y] + map.NumColRow[node.y - 1]) / 2) + 1;
						if (connectedNodeIndex >= 0 && connectedNodeIndex < nodeList.Count)
						{
							if (nodeList[connectedNodeIndex].x == (node.x + 1))
								connectedNode = nodeList[connectedNodeIndex];
						}
					}
					break;
			}
			if (connectedNode != null)
				if (connectedNode.tileData != null)
					return (!map.blockedTiles.Contains(connectedNode.tileData)) ? connectedNode : null;
		}
		return null;
	}
}
