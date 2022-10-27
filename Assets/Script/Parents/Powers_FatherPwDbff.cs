using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase padre de los poderes
/// </summary>
abstract public class Powers_FatherPwDbff : FatherPwDbff
{
    
    static public List<Powers_FatherPwDbff> powers;

    [System.Serializable]
    public class Sprites
    {
        public Sprite[] spriteImage = new Sprite[3];
    }

    public Sprites art;

    /// <summary>
    /// Funcion que sera llamada cuando se lance la habilidad
    /// </summary>
    /// <param name="me">character duenio de la habilidad</param>
    abstract public void Activate(Character me);


    /// <summary>
    /// Funcion que se ejecuta al ganar el poder/habilidad
    /// </summary>
    /// <param name="me">character duenio de la habilidad</param>
    abstract public void On(Character me);

    /// <summary>
    /// Funcion que se ejecuta al perder el poder/habilidad
    /// </summary>
    /// <param name="me">character duenio de la habilidad</param>
    virtual public void Off(Character me)
    {
        ClearRefs(me);
    }

     void OnDestroy()
    {
        powers.Clear();
    }

    virtual protected void Awake()
    {
        StartCoroutine(PostAwake());
    }

    IEnumerator PostAwake()
    {
        while (Debuff_FatherPwDbff.instances == null || PoolObjects.instance == null)
        {
            yield return null;
        }

        if(chrAffected == null)
            chrAffected = new List<Character>();

        if (powers == null)
            powers = new List<Powers_FatherPwDbff>();

        if (!powers.Contains(this))
            powers.Add(this);
    }
}
