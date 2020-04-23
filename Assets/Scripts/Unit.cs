﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour {

    public Transform target;
    float speed = 5;
    Vector3[] path;
    int targetIndex;

    void Awake() {
        PathRequestManager.requestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool  pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break;//how to exit out of coroutine;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
    }
}