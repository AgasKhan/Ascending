using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEffect : MonoBehaviour
{
    public LineRenderer line;

    public void SetLine(Vector3 p1, Vector3 p2)
    {
        line.SetPosition(0, p1);
        line.SetPosition(1, p2);
    }

    void Update()
    {
        line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), line.GetPosition(0), Time.deltaTime));
        if ((line.GetPosition(0) - line.GetPosition(1)).sqrMagnitude < 0.5f)
            this.enabled = false;
    }
}
