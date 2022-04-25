/* Makes an object follow a path of waypoints (circular)
 * Attach script to the object that should move
 * In the GUI Inspector (waypoints field) add as many waypoints as needed
 * In the GUI Hierarchy add the same number of empty objects, call them Waypoint1, 2, ...
 * Drag the Waypoints into the waypoint list.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    int currentWaypointIndex = 0;
    [SerializeField] float speed;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int prevWaypointIndex;

        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
        {
            prevWaypointIndex = currentWaypointIndex;
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
            // Change direction for default layer only
            if (gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                if (waypoints[currentWaypointIndex].transform.position.x > waypoints[prevWaypointIndex].transform.position.x)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }
}
