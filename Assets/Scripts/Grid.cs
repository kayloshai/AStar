using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	//public Transform player; //Test Player Position
	public bool onlyDisplayPathGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;
	void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		CreatGrid();
	}

	public int MaxSize {
		get
		{
			return gridSizeX * gridSizeY;
		}
	}

	void CreatGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;//vector3.forward will give z axis in 3D space
		for(int x = 0; x < gridSizeX; x++){
			for (int y = 0; y < gridSizeY; y++){
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
				grid[x, y] = new Node(walkable, worldPoint, x, y);//Pass to constructor created.
			}
		}
	}

	public List<Node> getNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();
		//loop to search in a 3x3 block
		for(int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >=0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

	public Node GetNodeFromWorldPoint(Vector3 worldPosition)
	{
		//far left % of 0, middle % 0.5, far right %1
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.x/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	//Testing reverse path
	public List<Node> path;

	//Unity buildt in
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (onlyDisplayPathGizmos)
		{
			if (path != null)
			{
				foreach (Node n in path)
				{
					Gizmos.color = Color.black;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
				}
			}
		}
		else {
			if (grid != null)
			{
				//Node playerNode = GetNodeFromWorldPoint(player.position); //Test Player Position
				foreach (Node n in grid)
				{
					Gizmos.color = (n.walkable) ? Color.white : Color.red; // ? Then, : else
																		   //if(playerNode == n)
																		   //Gizmos.color = Color.cyan; //Test Player Position
																		   //Test reverse path													   
					if (path != null)
						if (path.Contains(n))
							Gizmos.color = Color.black;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
				}
			}
		}
	}
}
