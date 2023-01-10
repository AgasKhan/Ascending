using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotRb : MoveRb
{

    [SerializeField] 
    public float desAcelerationAxis;

    Quaternion _quaternion;

    bool _rotate = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="offset"></param>
    public void RotateToDir(Vector3 dir, Vector3 offset)
    {

        float angleY;
        float angleX;

        Utilitys.DeltaAngle(dir,  Vector3.forward, out angleY, Vector3.up, transform.rotation);
        Utilitys.DeltaAngle(dir,  Vector3.up, out angleX, transform.right, transform.rotation);

        Rotate(new Vector3(angleX + offset.x - 90, angleY + offset.y, offset.z));
    }

    /// <summary>
    /// rota en Y mirando hacia la direccion que se pasa
    /// </summary>
    /// <param name="dir">vector3 que es la direccion a la que se desea observar</param>
    /// <returns>Retorna la diferencia absoluta entre el angulo y la rotacion del objeto</returns>
    public float RotateToDirY(Vector3 dir)
    {
        float angle;
        float result = Utilitys.DeltaAngleY(dir, out angle, transform.rotation);

        //print(angle);
        RotateY(angle);

        return result;
    }

    /// <summary>
    /// Rota en y un angulo
    /// </summary>
    /// <param name="Y"></param>
    public void RotateY(float Y)
    {
        Rotate(new Vector3(0, Y, 0));
    }

    /// <summary>
    /// rota en todos los angulos
    /// </summary>
    /// <param name="angles"></param>
    public void Rotate(Vector3 angles)
    {
        _quaternion = Quaternion.Euler(angles);
        _rotate = true;
    }

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
       
        MyUpdates += MyUpdate;
       
    }

    void MyAwake()
    {
        _quaternion = transform.rotation;
    }

    void MyUpdate()
    {
        if (_rotate && !isDisable && !dash)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _quaternion, Time.deltaTime * desAcelerationAxis);
            if (_quaternion.Equals(transform.rotation))
                _rotate = false;
        }
    }
}
