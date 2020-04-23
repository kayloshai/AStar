using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class will handle requests from seekers for a path by using a Queue implementation,
/// so that if there is more than one seeker trying to ask for a path from the pathfinding algorithm,
/// it will wait a short time for a path and not cause the program to stall.
/// </summary>
public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager instance;
	PathFinding pathFinding;//reference to pathfinding class;

	bool isProcessingPath;

	void Awake() {
		instance = this;
		pathFinding = GetComponent<PathFinding>();
	}
	
	public static void requestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newRequest);//How to add item to queue.
		instance.TryProcessNext();
	}

	void TryProcessNext()
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0)
		{
			currentPathRequest = pathRequestQueue.Dequeue();//How to access first item in the queue.
			isProcessingPath = true;
			pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	/// <summary>
	/// Will be called by pathfinding script once a path request has finished processing.
	/// </summary>
	public void FinishedProcessingPath(Vector3[] path, bool success) {
		currentPathRequest.callback(path, success);
		isProcessingPath = false;
		TryProcessNext();
	}

	

	//Data structure
	struct PathRequest
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
	
}
