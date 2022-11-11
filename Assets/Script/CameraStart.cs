using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraStart : MoveAndRotTrToPatrol
{
    [SerializeField]
    float ComeBackVelocity;

    Timer myTimer;

    Camera cam;

    Action auxUpdate;


    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;

        auxUpdate = MyUpdates + MyUpdate;

        MyUpdates = null;
    }

    void MyAwake()
    {
        cam = Camera.main;

        transform.position = patrol.patrol[0].position;
        transform.rotation = patrol.patrol[0].rotation;

        print("me estoy ejecutando");

        StartCoroutine(Wait());
    }

    void MyUpdate()
    {
        if(myTimer==null)
        {
            TextCanvas.SrchMessages("Lucas").ShowText(false, "Presiona " + " saltar ".RichText("b").RichText("color", "green") + "para saltear");

        }

        if(Input.GetButtonDown(Controllers.jump.strKey))
        {
            StartCoroutine(CameraPan());
        }

    }

    IEnumerator Wait()
    {
        myTimer = Timers.Create(2);

        while (!myTimer.Chck())
        {
            yield return null;
        }

        Timers.Destroy(myTimer);
        myTimer = null;
        MyUpdates = auxUpdate;

    }

    IEnumerator CameraPan()
    {

        MyUpdates = null;

        while (Mathf.Abs((transform.localRotation.eulerAngles - cam.transform.localRotation.eulerAngles).sqrMagnitude)> 0.1  || Mathf.Abs((transform.position - cam.transform.position).sqrMagnitude)> 0.01)
        {
            int mult = 1;

            if (Mathf.Abs((transform.localRotation.eulerAngles - cam.transform.localRotation.eulerAngles).sqrMagnitude) < 4 || Mathf.Abs((transform.position - cam.transform.position).sqrMagnitude) < 4)
                mult = 4;

            transform.position = Vector3.Lerp(transform.position , cam.transform.position, Time.deltaTime * ComeBackVelocity*mult);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, cam.transform.rotation, Time.deltaTime* mult);
            yield return null;
        }

        gameObject.SetActive(false);
        Controllers.eneable = true;
    }
}
