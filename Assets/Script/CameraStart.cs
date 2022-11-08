using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStart : MoveAndRotTrToPatrol
{
    [SerializeField]
    float ComeBackVelocity;

    Timer myTimer;

    Camera cam;

    bool paneo = false;

    
    protected override void MyAwake()
    {
        cam = Camera.main;

        base.MyAwake();

        transform.position = patrol.patrol[0].position;
        transform.rotation = patrol.patrol[0].rotation;

        StartCoroutine(Wait());
    }

    protected override void MyUpdate()
    {
        if(myTimer==null && !paneo)
        {
            TextCanvas.SrchMessages("Lucas").ShowText(false, "Presiona " + " saltar ".RichText("b").RichText("color", "green") + "para saltear");
            base.MyUpdate();
        }
            


        if(Input.GetButtonDown(Controllers.jump.strKey))
        {
            paneo = true;
            StartCoroutine(CameraPan());
        }
            
        
    }

    IEnumerator Wait()
    {
        myTimer = Timers.Create(1);

        while (!myTimer.Chck())
        {
            yield return null;
        }

        Timers.Destroy(myTimer);
        myTimer = null;
    }

    IEnumerator CameraPan()
    {
        
        while (transform.localRotation != cam.transform.localRotation && transform.position!= cam.transform.position)
        {
            transform.position = Vector3.Lerp(transform.position , cam.transform.position, Time.deltaTime * ComeBackVelocity);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, cam.transform.rotation, Time.deltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
        Controllers.eneable = true;
    }
}
