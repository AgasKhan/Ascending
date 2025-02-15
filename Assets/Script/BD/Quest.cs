
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Quests
{
    [SerializeField]
    static public List<Mission> incomplete;

    [SerializeField]
    static public List<Mission> complete;

    static public bool Update()
    {
        bool refresh = false;
        foreach (var item in incomplete)
        {
            if(item.Chck())
                refresh=true;
        }
        return refresh;
    }

    static public Mission[] SrchComplete(int level)
    {
        return Srch(level, complete);
    }

    static public Mission[] SrchIncomplete(int level)
    {
        return Srch(level, incomplete);
    }

    static Mission[] Srch(int level, List<Mission> list)
    {
        List<Mission> missions = new List<Mission>();

        foreach (var item in list)
        {
            if (item.level == level)
                missions.Add(item);
        }

        return missions.ToArray();
    }


    public static void ChargeQuests(int level)
    {
        foreach (var item in SrchIncomplete(level))
        {
            item.active = true;
        }
    }

    public static void CreateQuests()
    {
        incomplete = new List<Mission>();

        #region Level 1

        new Quests.Mission(
            1,
            "Contra  Reloj",
            "Completa  el  nivel  en  menos  de  1  minutos  para  obtener  10  puntos",
            () =>
            {
                return GameManager.CurrentTime() > 60;
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );

        int activesCount2 = 0;
        new Quests.Mission(
            1,
            "Estratega",
            "Encanta  la  daga  solo  una  vez  durante  todo  el  nivel  para  obtener  8  puntos",
            () =>
            {
                if (Controllers.active.down)
                    activesCount2++;
                return activesCount2 > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(8);
            }
        );


        new Quests.Mission(
            1,
            "Sin toxina",
            "No  utilices  el  poder  de  Toxina  para  obtener  5  puntos",
            () =>
            {
                return GameManager.player.power.Count > 0 && GameManager.player.power[0] is Toxine_Powers;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
        );

        new Quests.Mission(
            1,
            "Solo  una  daga",
            "No  atraigas  mas  de  una  daga  durante  todo  el  nivel  para  obtener  3  puntos",
            () =>
            {
                return GameManager.player.totalDaggers > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );

        new Quests.Mission(
            1,
            "Genocida",
            "Asesina  a  todos  los  enemigos  antes  de  pasarte  el  nivel  para  obtener  5  puntos",
            () =>
            {
                return GameManager.enemys.Count > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
            , false);

        #endregion

        #region Level 2

        new Quests.Mission(
            2,
            "Contra  Reloj",
            "Completa  el  nivel  en  menos  de  1  minutos  para  obtener  10  puntos",
            () =>
            {
                return GameManager.CurrentTime() > 60;
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );

        /*
        new Quests.Mission(
            2,
            "Singular",
            "Utiliza  solo  un  poder  durante  todo  el  nivel  para  obtener  10  puntos",
            () =>
            {
                return GameManager.player.lastPower != null && GameManager.player.lastPower != GameManager.player.power[0];
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );*/

        int enemyCount3 = 0;
        new Quests.Mission(
            2,
            "Asesinato  veloz",
            "Mata  a  un  enemigo  al  iniciar  el  nivel  en  menos  de  30  segundos  para  obtener  8  puntos",
            () =>
            {
                if (enemyCount3 == 0)
                    enemyCount3 = GameManager.enemys.Count;

                return (GameManager.CurrentTime() > 30) && (GameManager.enemys.Count == enemyCount3);
            },
            () =>
            {
                LobbyManager.AddPoints(8);
            }
        );



        new Quests.Mission(
            2,
            "Casi  Ileso",
            "No  pierdas  mas  de  10  puntos  de  vida  para  obtener  8  puntos",
            () =>
            {
                return GameManager.player.health.hp < (GameManager.player.health.maxHp - 10);
            },
            () =>
            {
                LobbyManager.AddPoints(8);
            }
        );

        /*
        new Quests.Mission(
            2,
            "Pies  en  la  tierra",
            "No  saltes  ni  una  sola  vez  para  obtener  3  puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );*/

        new Quests.Mission(
            2,
            "Objetivos  claros",
            "Elimina  a  todos  los  enemigos  de  Dash  para  obtener  5  puntos",
            () =>
            {
                foreach (var item in GameManager.enemys)
                {
                    if (item is DashEnemy_Enemy)
                        return true;
                }
                return false;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
        , false);


        new Quests.Mission(
            2,
            "Genocida",
            "Asesina  a  todos  los  enemigos  antes  de  pasarte  el  nivel  para  obtener  5  puntos",
            () =>
            {
                return GameManager.enemys.Count > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
            , false);

        #endregion

        #region Level 3

        new Quests.Mission(
            5,
            "Contra  Reloj",
            "Completa  el  nivel  en  menos  de  1  minutos  para  obtener  10  puntos",
            () =>
            {
                return GameManager.CurrentTime() > 60;
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );

        int dashCount = 0;
        new Quests.Mission(
            5,
            "Adicto  al  dash",
            "Utiliza  el  dash  por  lo  menos  1  vez  cada  15  segundos  para  obtener  5  puntos",
            () =>
            {
                if (Controllers.dash.down)
                    dashCount++;

                int currentTime = (int)GameManager.CurrentTime();

                return currentTime % 15 == 0 && currentTime / 15 < dashCount;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
        );

        int enemyCount = 0;
        new Quests.Mission(
            5,
            "Asesinato  veloz",
            "Mata  a  un  enemigo  al  iniciar  el  nivel  en  menos  de  30  segundos  para  obtener  8  puntos",
            () =>
            {
                if (enemyCount == 0)
                    enemyCount = GameManager.enemys.Count;

                return (GameManager.CurrentTime() > 30) && (GameManager.enemys.Count == enemyCount);
            },
            () =>
            {
                LobbyManager.AddPoints(8);
            }
        );

        new Quests.Mission(
            5,
            "Nunca  muestres  lo  que  tienes",
            "Termina  el  nivel  sin  activar  un  solo  poder  para  obtener  8  puntos",
            () =>
            {
                return Controllers.power.down;
            },
            () =>
            {
                LobbyManager.AddPoints(8);
            }
        );

        new Quests.Mission(
            5,
            "Solo  una  daga",
            "No  atraigas  mas  de  una  daga  durante  todo  el  nivel  para  obtener  3  puntos",
            () =>
            {
                return GameManager.player.totalDaggers > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );



        /*
        int chargesCount = 0;
        new Quests.Mission(
            3,
            "Ataques cargados",
            "Usa la carga al maximo antes de disparar 3 veces para obtener 3 puntos",
            () =>
            {
                if()
                
                return Controllers.jump.down;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );*/

        #endregion

        #region Level 4

        /*
        bool complete = false;
        new Quests.Mission(
            4,
            "Combo poderoso",
            "Usa el poder de Vortice y Toxina a la vez para obtener 5 puntos",
            () =>
            {
                var toxine = PoolObjects.SrchInCategory("Toxine", "toxicSmoke");
                var vortex = PoolObjects.SrchInCategory("Vortex", "Vortex");

                bool chkToxine = false, chkVortex = false;
                foreach (var item in GameManager.player.powerObjectSpawn)
                {
                    if (item == toxine)
                        chkToxine = true;
                    if (item == vortex)
                        chkVortex = true;
                }


                complete = (Controllers.aim.pressed && Controllers.attack.pressed && chkToxine && chkVortex);

                return !complete;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
        );*/

        new Quests.Mission(
            6,
            "Contra  Reloj",
            "Completa  el  nivel  en  menos  de  1  minutos  para  obtener  10  puntos",
            () =>
            {
                return GameManager.CurrentTime() > 60;
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );


        new Quests.Mission(
            6,
            "Sin  Dash",
            "No  utilices  el  Dash  durante  todo  el  nivel  para  obtener  3  puntos",
            () =>
            {
                return Controllers.dash.down;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );

        new Quests.Mission(
            6,
            "Singular",
            "Utiliza  solo  un  poder  durante  todo  el  nivel  para  obtener  10  puntos",
            () =>
            {
                return GameManager.player.lastPower != null && GameManager.player.lastPower != GameManager.player.power[0];
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );

        /*
        new Quests.Mission(
            6,
            "Coleccionista",
            "Encuentra y atrae todas las dagas del nivel para obtener 3 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );*/

        new Quests.Mission(
            6,
            "Solo  una  daga",
            "No  atraigas  mas  de  una  daga  durante  todo  el  nivel  para  obtener  3  puntos",
            () =>
            {
                return GameManager.player.totalDaggers > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(3);
            }
        );

        int activesCount = 0;
        new Quests.Mission(
            6,
            "Estratega",
            "Encanta  la  daga  solo  2  veces  durante  todo  el  nivel  para  obtener  10  puntos",
            () =>
            {
                if (Controllers.active.down)
                    activesCount++;
                return activesCount > 2;
            },
            () =>
            {
                LobbyManager.AddPoints(10);
            }
        );

        new Quests.Mission(
            6,
            "Genocida",
            "Asesina  a  todos  los  enemigos  antes  de  pasarte  el  nivel  para  obtener  5  puntos",
            () =>
            {
                return GameManager.enemys.Count > 1;
            },
            () =>
            {
                LobbyManager.AddPoints(5);
            }
            , false);

        #endregion


        CSVReader.SaveClassInPictionary("QuestsIncomplete",incomplete);
        CSVReader.SaveClassInPictionary("QuestsComplete", complete);
    }


    // Desarrollar misiones ejecuten un update y hagan check al final de los niveles

    [System.Serializable]
    public class Mission
    {
        public int level;
        public bool active;
        public DoubleString Description;
        public System.Func<bool> chck;//chequea si perdiste/condicion de derrota
        public System.Action reward;
        public bool update;

        public DoubleString Reward()
        {
            if (active)
            {
                reward();

                complete.Add(this);

                incomplete.Remove(this);

                return Description;
            }

            return new DoubleString();
        }

        public bool Chck()
        {
            bool refresh = false;

            if(active && update &&chck())
            {
                active = false;
                refresh = true;

                Debug.Log("Perdiste: "+ Description.superior);
            }

            return refresh;
        }

        public Mission(int level, string sup, string inf, Func<bool> chck, Action reward, bool update = true)
        {
            this.level = level;
            Description = new DoubleString(sup, inf);
            this.chck = chck;
            this.reward = reward;
            this.update = update;
            incomplete.Add(this);
        }
    }

    
}


