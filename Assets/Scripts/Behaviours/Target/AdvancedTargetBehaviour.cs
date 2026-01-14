using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTargetBehaviour : MovingTargetBehaviour
{
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
    }

    protected override void MoveTarget()
    {
        t += Time.deltaTime;

        // Calculate the current speed using an exponential function
        float currentSpeed = Mathf.Clamp(advancedBaseSpeed + Mathf.Exp(t * speedAmplitude) * speedAmplitude, minSpeed, maxSpeed);

        // Use an easing function for smoother acceleration and deceleration
        float easedT = base.EaseInOutQuad(t);

        Vector3 newPosition = Vector3.Lerp(transform.position, waypointPosition, easedT * currentSpeed);

        // Update the position
        transform.position = newPosition;

        if (t >= 1.0f)
        {
            SetWaypoint();
            t = 0f;
            movingToB = !movingToB; // Change direction
        }
    }
}
