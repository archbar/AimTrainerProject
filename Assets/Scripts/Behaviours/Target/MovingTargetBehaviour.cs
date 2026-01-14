using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargetBehaviour : TargetBehaviour
{
    #region Fields
    [Header("Movement Settings")]
    [SerializeField] protected float standardBaseSpeed = 0.01f;
    [SerializeField] protected float advancedBaseSpeed = 0.01f;
    [SerializeField] protected float minSpeed = 0.001f;
    [SerializeField] protected float maxSpeed = 0.002f;
    [SerializeField] protected float speedAmplitude = 0.01f;

    protected float endDistanceFromPoint;
    protected float t = 0f;
    protected bool leftMoving;
    protected Vector3 pointA;
    protected Vector3 pointB;
    protected Vector3 waypointPosition;
    protected bool movingToB = true; // Indicates whether the target is moving towards point B
    #endregion

    #region Methods
    #region UnityMethods
    protected new void Start()
    {
        base.Start();
        SetWaypoint();
    }

    protected new void Update()
    {
        base.Update();
        MoveTarget();
    }
    #endregion

    #region NonUnityMethods
    protected override void InitialiseTarget()
    {
        base.InitialiseTarget();
        if (chance > 50) leftMoving = true;
    }

    protected override void ResetTarget()
    {
        base.ResetTarget();

        float spawnRange = 5f;

        // Set pointA more to the left for standard movement
        pointA = new Vector3(Random.Range(-spawnRange, spawnRange), randomY, randomZ);
        // Reduce the distance for the standard movement
        pointB = new Vector3(Random.Range(-spawnRange, spawnRange), randomY, randomZ);

        movingToB = true; // Start by moving towards point B
    }


    protected virtual void MoveTarget()
    {
        t += Time.deltaTime;

        float distance = Vector3.Distance(pointA, pointB);
        float totalDuration = distance / standardBaseSpeed;

        // Calculate the position based on constant speed
        Vector3 newPosition;

        if (movingToB)
            newPosition = Vector3.Lerp(pointA, pointB, t / totalDuration);
        else
            newPosition = Vector3.Lerp(pointB, pointA, t / totalDuration);

        // Update the position
        transform.position = newPosition;

        if (t >= totalDuration)
        {
            SetWaypoint();
            t = 0f;
            movingToB = !movingToB; // Change direction
        }
    }

    protected virtual void SetWaypoint()
    {
        t = 0.0f;
        float randomFactor = Random.Range(0.3f, 0.7f);

        // Choose the destination based on the current direction
        waypointPosition = movingToB ? Vector3.Lerp(pointA, pointB, randomFactor) : Vector3.Lerp(pointB, pointA, randomFactor);
    }

    // Easing function for smooth acceleration and deceleration
    protected float EaseInOutQuad(float t)
    {
        return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }
    #endregion
    #endregion
}