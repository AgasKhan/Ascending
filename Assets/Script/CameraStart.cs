using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraStart : MoveAndRotTrToPatrol
{
    [SerializeField]
    float ComeBackVelocity;

    CameraParent cameraParent;

    Timer myTimer;

    Camera cam;

    Action auxUpdate;

    bool saveTimeCopy;

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;

        MyStarts += MyStart;

        MyUpdates += MyUpdate;
    }

    void MyAwake()
    {
        cam = Camera.main;

        cameraParent = GetComponentInParent<CameraParent>();        

        transform.position = patrol.patrol[0].position;
        transform.rotation = patrol.patrol[0].rotation;

        saveTimeCopy = GameManager.saveTime;

        GameManager.saveTime = false;

        StartCoroutine(Wait());
    }

    void MyStart()
    {
        cameraParent.enabled = false;
        cam.gameObject.SetActive(false);
    }

    void MyUpdate()
    {
        TextCanvas.SrchMessages("Lucas").ShowText(false, "Presiona " + (" " + Controllers.jump.ToString() + " ").RichText("b").RichText("color", "green") + "para saltear");

        if (Input.GetKeyDown(Controllers.jump.principal))
        {
            StartCoroutine(CameraPan());
        }

    }

    IEnumerator Wait()
    {
        auxUpdate = MyUpdates + MyUpdate;

        MyUpdates = null;

        myTimer = TimersManager.Create(2);

        while (!myTimer.Chck)
        {
            yield return null;
        }

        TimersManager.Destroy(myTimer);
        myTimer = null;
        MyUpdates = auxUpdate;

    }

    IEnumerator CameraPan()
    {

        MyUpdates = null;

        cam.gameObject.SetActive(true);
        cameraParent.enabled = true;

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
        GameManager.saveTime = saveTimeCopy;

        Controllers.verticalMouse.enable = true;
        Controllers.horizontalMouse.enable = true;

        GameManager.instance.currentTime = 0;

        TimeController.StartAllItem();

        MainHud.ReticulaPlay("Start");
    }
}
