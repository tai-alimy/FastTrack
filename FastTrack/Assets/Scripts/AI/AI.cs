using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float MaxSpeed;
    public float maxAccel;
    public float rotation;
    public Vector3 velocity;
    protected Steer steer;
    private Vector3 oldPos;

    private void Start()
    {
        velocity = Vector3.zero;
        steer = new Steer();
    }

    public void SetSteer(Steer steer)
    {
        this.steer = steer;
    }

    public virtual void FixedUpdate()
    {
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        transform.Translate(displacement, Space.World);
        RotateCarToFaceForwrard();
    }

    public virtual void LateUpdate()
    {
        velocity += steer.linear * Time.deltaTime;
        rotation += steer.angular * Time.deltaTime;
        if (velocity.magnitude > MaxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * MaxSpeed;
        }

        if (steer.angular == 0.0f)
        {
            rotation = 0.0f;
        }

        if (steer.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }

        steer = new Steer();
        oldPos = transform.position;
    }

    void RotateCarToFaceForwrard()
    {
        Vector3 dir = (transform.position - oldPos) * Time.fixedDeltaTime;
        Quaternion lookAtRotation = Quaternion.LookRotation(dir);
        Quaternion lookAtRotationOnlyY = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                                            lookAtRotation.eulerAngles.y,
                                                            transform.rotation.eulerAngles.z);
        transform.rotation = lookAtRotationOnlyY;

    }



}
