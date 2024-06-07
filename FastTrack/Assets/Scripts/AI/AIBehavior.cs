using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public GameObject target;
    protected AI agent;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<AI>();
    }

    public virtual void Update()
    {
        agent.SetSteer(GetSteer());
    }

    public virtual Steer GetSteer()
    {
        return new Steer();
    }
}
