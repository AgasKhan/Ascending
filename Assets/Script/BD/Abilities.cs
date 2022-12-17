using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Abilities
{
    /// <summary>
    /// diccionario que almacenara todas las referencias de las clases creadas sobre las abilities
    /// recordar que este diccionario al ser estatico no borrara ninguna de sus elementos
    /// por ende, se recomienda borrar toda la lista cada vez que se quiera agregar un elemento, para evitar errores logicos
    /// </summary>
    public static Pictionarys<System.Type, Ability> Abilitieslist = new Pictionarys<System.Type, Ability>();


    [System.Serializable]
    public abstract class Ability
    {
        /// <summary>
        /// cuando es -1, es que no esta comprada, cuando sea 0 esta desbloqueada pero no se puede usar
        /// cuando sea 1 es el primer nivel de la habilidad y si interactuaria con el juego
        /// </summary>
        public int level;

        /// <summary>
        /// si afecta al gameplay
        /// </summary>
        public bool active;

        public virtual void OnChangeLevel(int l)
        {
            level = l;
        }

        /// <summary>
        /// funcion que ejecutara al comenzar su start
        /// </summary>
        public abstract void OnStart();

        public Ability(int level)
        {
            OnChangeLevel(level);
            this.active = false;
            Abilitieslist.Add(this.GetType(), this);
        }
    }
    /* Se tiene que poder guardar los cambios en las habilidades
     * 
     * Deberia existir una funcion que sea virtual que defina el que pasara cuando un nivel empiece "ref: OnStart"
     * Esa funcion debe ser overrideada por los hijos, ya que cada uno debe modificar algo diferente (Armadura, vida, etc)
     * La clase debe ser guardada en un json. Esta clase = Abilities*/



    public class PowerInit<T> : Ability where T : Powers_FatherPwDbff
    {

        public override void OnStart()
        {
            GameManager.player.ReplaceFirstPower<T>();
        }

        public PowerInit(Type tipo, int level) : base(level)
        {

        }
    }

    public class ChargeDagger : Ability
    {
        public float maxPressedTime;
        public float relationXtime;

        public override void OnStart()
        {
            GameManager.player.atackElements.maxPressedTime = maxPressedTime;
            GameManager.player.atackElements.relationXtime = relationXtime;
        }

        public override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    maxPressedTime = 5;
                    relationXtime = 1;

                    break;

                case 2:

                    maxPressedTime = 3;
                    relationXtime = 2;

                    break;

                case 3:

                    maxPressedTime = 1;
                    relationXtime = 7;

                    break;

                case 4:

                    maxPressedTime = 1;
                    relationXtime = 10;

                    break;

                default:
                    DebugPrint.Warning("Este nivel no efectua cambios en las estadisticas");
                    break;

            }
        }

        public ChargeDagger(int level) : base(level)
        {

        }
    }

    public class HitScan : Ability
    {
        public override void OnStart()
        {
            GameManager.player.atackElements.UnlockHitScan = true;
        }

        public HitScan(int level) : base(level)
        {

        }
    }
}






