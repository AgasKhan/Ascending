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

        protected float[,] values;

        protected float[] value
        {
            get
            {
                float[] arr = new float[values.GetLength(1)];

                for (int i = 0; i < values.GetLength(1); i++)
                {
                    arr[i] = values[_level-1 , i];
                }

                return arr;
            }
        }


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
            DebugPrint.Log("nivel ingresado: " + l);
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
        }
    }
    /* Se tiene que poder guardar los cambios en las habilidades
     * 
     * Deberia existir una funcion que sea virtual que defina el que pasara cuando un nivel empiece "ref: OnStart"
     * Esa funcion debe ser overrideada por los hijos, ya que cada uno debe modificar algo diferente (Armadura, vida, etc)
     * La clase debe ser guardada en un json. Esta clase = Abilities*/



    public class PowerInit<T> : Ability where T : Powers_FatherPwDbff, new()
    {
        public override void OnStart()
        {
            GameManager.player.ReplaceFirstPower<T>();
        }
    
    }

    public class ChargeDagger : Ability
    {
        public override void OnStart()
        {
            //primero es el maxPressed, segundo es el relationTime
            values = new float[,]
                {
                    {5,1},
                    {3,2},
                    {1,7},
                    {1,10}
                };

            GameManager.player.attackElements.maxPressedTime = value[0];
            GameManager.player.attackElements.relationXtime = value[1];
        }

    }

    public class HitScan : Ability
    {
        public override void OnStart()
        {
            GameManager.player.attackElements.UnlockHitScan = true;
        }
    }

    public class TimeToInteract : Ability
    {
        public override void OnStart()
        {
            values = new float[,]
                {
                    {1.1f},
                    {1.25f},
                    {1.75f},
                    {2.5f},
                };

            GameManager.player.timeInteractMultiply = value[0];
        }
    }

    public class CallAllDaggers : Ability
    {
        public override void OnStart()
        {
            GameManager.player.UnlockAtrackt = true;
        }
    }

     public abstract class InitialDaggersFather : Ability
    {
        protected void SpawnDaggers(int aux)
        {
            Player_Character player = GameManager.player;
            
            float angle = 360f / aux;

            for (int i = 1; i <= aux; i++)
            {
                player.dagger = PoolObjects.SpawnPoolObject<Dagger_Proyectile>(0, "Daguita", player.transform.position + Quaternion.Euler(0, i * angle, 0) * player.transform.forward, Quaternion.identity);
                player.Take();
            }
        }
    }

    public class InitialDaggersI : InitialDaggersFather
    {
        public override void OnStart()
        {
            SpawnDaggers(1);
            GameManager.player.damage *= 0.9f;
        }
    }

    public class InitialDaggersII : InitialDaggersFather
    {
        public override void OnStart()
        {
            SpawnDaggers(1);
            GameManager.player.cameraScript.coneOfVision *= 0.50f;
        }
    }

    public class InitialDaggersIII : InitialDaggersFather
    {
        public override void OnStart()
        {
            SpawnDaggers(2);
        }
    }

    public class TimeToArrive : Ability
    {
        public override void OnStart()
        {
            values = new float[,]
                {
                    {1.75f},
                    {1.5f},
                    {1.25f},
                    {1},
                };

            GameManager.player.floatElements.timeToAttrackt = value[0];
        }

    }

    public class HealthPoints : Ability
    {
        public override void OnStart()
        {
            values = new float[,]
                {
                    {105},
                    {110},
                    {115},
                    {120},
                };

            GameManager.player.health.maxHp = value[0];
        }

    }

    public class Armor : Ability
    {
        public float ArmorPoints;

        public override void OnStart()
        {
            values = new float[,]
                {
                    {1},
                    {1.3f},
                    {1.6f},
                    {2},
                };

            GameManager.player.health.armor = value[0];
        }

        

    }

    public class Speed : Ability
    {
        public override void OnStart()
        {
            values = new float[,]
               {
                    {1.1f},
                    {1.2f},
                    {1.3f},
                    {1.5f},
               };

            GameManager.player.maxSpeed *= value[0];
        }
    }

}






