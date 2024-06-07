using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDrivingCar : Seek
{
    public Path path;
    public float pathOffset = 0.0f;
    float currParam;
    public float avoidDistance = 5.0f;
    public float lookAhead = 10.0f;

    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
        currParam = 0f;
        float minPathOffset = 0f;
        float maxPathOffset = 0.2f;
        pathOffset = Random.Range(minPathOffset, maxPathOffset);
        transform.parent = null;
    }

    public override Steer GetSteer()
    {
        currParam = path.GetParam(transform.position, currParam);
        float targetParam = currParam + pathOffset;
        target.transform.position = path.GetPosition(targetParam);
        Steer steer = new Steer();
        Vector3 pos = transform.position;
        Vector3 rayVector = agent.velocity.normalized * lookAhead;
        Vector3 dir = rayVector;
        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit, lookAhead))
        {
            pos = hit.point + hit.normal * avoidDistance;
            target.transform.position = pos;
            steer = base.GetSteer();
        }
        return base.GetSteer();
    }
}
