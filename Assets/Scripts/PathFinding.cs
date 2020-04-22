using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathFinding : MonoBehaviour {

	public Transform seeker, target;

	Grid grid;

	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	void Update()
	{
		if (Input.GetButtonDown("Jump")) {
			FindPath(seeker.position, target.position);
		}
	}

	void FindPath(Vector3 startPosition, Vector3 targetPosition)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();
		Node startNode = grid.GetNodeFromWorldPoint(startPosition);
		Node targetNode = grid.GetNodeFromWorldPoint(targetPosition);

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		HashSet<Node> closeSet = new HashSet<Node>();
		openSet.Add(startNode);

		while(openSet.Count > 0){
			Node currentNode = openSet.RemoveFirst();
			closeSet.Add(currentNode);

			if(currentNode == targetNode){
				sw.Stop();
				print("Path found: " + sw.ElapsedMilliseconds + " ms");
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.getNeighbours(currentNode)){
				if (!neighbour.walkable || closeSet.Contains(neighbour))
					continue;

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.Parent = currentNode;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}

		path.Reverse();

		//Test
		grid.path = path;
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(disX > disY)
			return 14 * disY + 10 * (disX - disY);
		return 14 * disX + 10 * (disY - disX);
	}
}
