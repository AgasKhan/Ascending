using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotTr : MoveTr
{
    public float rotVelocity;

    //public float minAngleRot;

    Vector3 _angle;

    public void RotationY(float angle)
    {
        Rotation(_angle.x, angle, _angle.z);
    }

    public void Rotation(Vector3 rot)
    {
        _angle = rot;
    }

    public void Rotation(float x, float y, float z)
    {
        _angle = new Vector3(x, y, z);
    }

    protected virtual void RotateOn()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_angle), Time.deltaTime * rotVelocity);
    }

    protected override void Config()
    {
        base.Config();
        MyUpdates += MyUpdate;
    }

    void MyUpdate()
    {
        RotateOn();
    }
}
