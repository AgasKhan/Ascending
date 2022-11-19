using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEffect : MonoBehaviour
{
    
    public LineRenderer line;

    public float off;
    public float velocity;
    public float time;

    public void SetLine(Vector3 p1, Vector3 p2)
    {
        line.enabled = true;
        this.enabled = true;
        line.SetPosition(0, p1);
        line.SetPosition(1, p2);

    }

    void Update()
    {
        time += Time.deltaTime * velocity;

        line.SetPosition(1, Vector3.MoveTowards(line.GetPosition(1), line.GetPosition(0), time));
        if ((line.GetPosition(0) - line.GetPosition(1)).sqrMagnitude < off * off)
        {
            line.enabled = false;
            this.enabled = false;
            //Vector3.Lerp(line.GetPosition(1), line.GetPosition(0), time)
        }
    }
}
