using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : AIBehaviour
{
    public override Steer GetSteer()
    {
        Steer steer = new Steer();
        steer.linear = target.transform.position - transform.position;
        steer.linear.Normalize();
        steer.linear = steer.linear * agent.maxAccel;
        return steer;
    }
}