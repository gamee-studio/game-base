using System;
using System.Collections.Generic;
using Pancake;
using UnityEngine;

public class GoMove : MonoBehaviour
{
    public GameObject movingObject;
    public List<Transform> points; // List of points the object will move through
    public float speed = 1.0f; // The speed at which the object will move between points
    public bool moveOnAwake = true; // Flag to indicate whether the object should start moving on awake
    public bool loop = true; // Flag to indicate whether the object should loop through the points
    private bool _reverse; // Flag to indicate whether the object should move in reverse
    private int _currentPoint; // The current point the object is moving towards
    private bool _isMoving = true; // Flag to indicate whether the object is currently moving

    void Start()
    {
        movingObject.transform.position = points[0].position;
        if(!moveOnAwake)
        {
            _isMoving = false;
        }
    }

    void Update()
    {
        if (_isMoving)
        {
            if (_currentPoint < points.Count)
            {
                // Move the object towards the next point
                movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, points[_currentPoint].position, speed * Time.deltaTime);
                if (movingObject.transform.position == points[_currentPoint].position)
                {
                    // When the object reaches the point, move on to the next one
                    if (!_reverse)
                    {
                        _currentPoint++;
                    }
                    else
                    {
                        _currentPoint--;
                    }

                    if (_currentPoint == points.Count && loop)
                    {
                        _currentPoint = 0;
                    }
                    if (_currentPoint < 0 && loop)
                    {
                        _currentPoint = points.Count - 1;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!points.IsNullOrEmpty())
        {
            try
            {
                foreach (Transform item in points)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(item.transform.position, 0.2f);
                }
            }
            catch (Exception e)
            {
                points = new List<Transform>();
                throw;
            }
        }
    }
    
    public void StopMoving()
    {
        _isMoving = false;
    }
    
    public void ResumeMoving()
    {
        _isMoving = true;
    }
    
    [Button]
    public void ReverseMoving()
    {
        _reverse = !_reverse;
        if (_currentPoint == 0)
        {
            _currentPoint = points.Count - 1;
        }
        else if (_currentPoint == points.Count - 1)
        {
            _currentPoint = 0;
        }
    }
}