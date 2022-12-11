using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToTimerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.AddTimeController(transform);
    }
}
