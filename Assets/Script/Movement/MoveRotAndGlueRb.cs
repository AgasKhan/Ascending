using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotAndGlueRb : MoveAndRotRb
{
    /// <summary>
    /// referencia al objeto glue que funciona como intermediario
    /// </summary>
    public GameObject glue;

    /// <summary>
    /// Funcion que pega el objeto al padre seleccionado
    /// </summary>
    /// <param name="t">padre al q se pegara el objeto</param>
    public void AddGlue(Transform t)
    {
        transform.SetParent(null);
        glue.transform.SetParent(null);
        glue.transform.localScale = Vector3.one;
        transform.localScale = Vector3.one;
        glue.transform.SetPositionAndRotation(transform.position, t.rotation);
        glue.transform.SetParent(t.transform);
        transform.SetParent(glue.transform);
    }

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
    }

    void MyAwake()
    {

        glue = new GameObject("Glue " + name);

        GameManager.AddTimeController(glue.transform);
    }
}
