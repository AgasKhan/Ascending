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
    public static Pictionarys<System.Type, Ability> Abilitieslist;


    [System.Serializable]
    public abstract class Ability
    {
        /// <summary>
        /// cuando es -1, es que no esta comprada, cuando sea 0 esta desbloqueada pero no se puede usar
        /// cuando sea 1 es el primer nivel de la habilidad y si interactuaria con el juego
        /// </summary>
        int _level;

        public int level
        {
            get
            {
                return _level;
            }

            set
            {
                OnChangeLevel(value);
            }
        }

        /// <summary>
        /// si afecta al gameplay
        /// </summary>
        public bool active;

        protected virtual void OnChangeLevel(int l)
        {
            _level = l;
        }

        public void CheckOnStart()
        {
            if (active)
                OnStart();
        }

        /// <summary>
        /// funcion que ejecutara al comenzar su start
        /// </summary>
        public abstract void OnStart();

        public Ability()
        {
            this.active = false;
            Abilitieslist.Add(this.GetType(), this);
            //DebugPrint.Log(Abilitieslist.ToString());
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

        public PowerInit() : base()
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

        protected override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    maxPressedTime = 5;
                    relationXtime = 1; // 1 * 10% x segundo de carga

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

        public ChargeDagger() : base()
        {

        }
    }

    public class HitScan : Ability
    {
        public override void OnStart()
        {
            GameManager.player.atackElements.UnlockHitScan = true;
        }

        public HitScan() : base()
        {

        }
    }

    public class TimeToInteract : Ability
    {
        public float timeToInteract;
        
        public override void OnStart()
        {
            GameManager.player.timeInteractMultiply = timeToInteract;
        }

        protected override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    timeToInteract = 1.1f;

                    break;

                case 2:

                    timeToInteract = 1.25f;

                    break;

                case 3:

                    timeToInteract = 1.75f;

                    break;

                case 4:

                    timeToInteract = 2.5f;

                    break;

                default:
                    DebugPrint.Warning("Este nivel no efectua cambios en las estadisticas");
                    break;

            }
        }

        public TimeToInteract() : base()
        {

        }
    }

    public class CallAllDaggers : Ability
    {
        public override void OnStart()
        {
            GameManager.player.UnlockAtrackt = true;
        }

        public CallAllDaggers() : base()
        {

        }
    }

    public class InitialDaggers : Ability
    {
        public Pictionarys<string,int> count;

        public override void OnStart()
        {
            var aux = 0;

            Player_Character player = GameManager.player;

            foreach (var item in count)
            {
                aux += item.value;
            }

            float angle =360f / aux;

            for (int i = 1; i <= aux; i++)
            {
                var dagger = PoolObjects.SpawnPoolObject(0, "Daguita", player.transform.position + Quaternion.Euler(0, i * angle, 0) * player.transform.forward, Quaternion.identity);
                player.dagger = dagger.GetComponent<Dagger_Proyectile>();
                player.Take();
            }
        }

        public InitialDaggers() : base()
        {
            count = new Pictionarys<string, int>();
        }
    }

    public class TimeToArrive : Ability
    {
        public float timeToArrive;

        public override void OnStart()
        {
            GameManager.player.floatElements.timeToAttrackt = timeToArrive;
        }

        protected override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    timeToArrive = 1.75f;

                    break;

                case 2:

                    timeToArrive = 1.5f;

                    break;

                case 3:

                    timeToArrive = 1.25f;

                    break;

                case 4:

                    timeToArrive = 1;

                    break;

                default:
                    DebugPrint.Warning("Este nivel no efectua cambios en las estadisticas");
                    break;

            }
        }

        public TimeToArrive() : base()
        {

        }
    }

    public class HealthPoints : Ability
    {
        public float HP;

        public override void OnStart()
        {
            GameManager.player.health.maxHp = HP;
        }

        protected override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    HP = 105;

                    break;

                case 2:

                    HP = 110;

                    break;

                case 3:

                    HP = 115;

                    break;

                case 4:

                    HP = 120;

                    break;

                default:
                    DebugPrint.Warning("Este nivel no efectua cambios en las estadisticas");
                    break;

            }
        }

        public HealthPoints() : base()
        {

        }
    }

    public class Armor : Ability
    {
        public float ArmorPoints;

        public override void OnStart()
        {
            GameManager.player.health.armor = ArmorPoints;
        }

        protected override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    ArmorPoints = 1;

                    break;

                case 2:

                    ArmorPoints = 1.3f;

                    break;

                case 3:

                    ArmorPoints = 1.6f;

                    break;

                case 4:

                    ArmorPoints = 2;

                    break;

                default:
                    DebugPrint.Warning("Este nivel no efectua cambios en las estadisticas");
                    break;


            }
        }

        public Armor() : base()
        {

        }
    }

    public class Speed : Ability
    {
        public float SpeedPoints;

        public override void OnStart()
        {
            GameManager.player.maxSpeed *= SpeedPoints;
        }

        protected override void OnChangeLevel(int l)
        {
            base.OnChangeLevel(l);

            DebugPrint.Warning("nivel ingresado: " + l);
            switch (l)
            {
                case 1:

                    SpeedPoints = 1.1f;

                    break;

                case 2:

                    SpeedPoints = 1.2f;

                    break;

                case 3:

                    SpeedPoints = 1.3f;

                    break;

                case 4:

                    SpeedPoints = 1.5f;

                    break;

                default:
                    DebugPrint.Warning("Este nivel no efectua cambios en las estadisticas");
                    break;

            }
        }

        public Speed() : base()
        {

        }
    }

}






