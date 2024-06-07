using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public float angular;
    public Vector3 linear;

    public Steer()
    {
        angular = 0.0f;
        linear = new Vector3();
    }
}
