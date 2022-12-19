using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    // Duda: Crear todas las misiones a la vez? (Start o Awake del menu)
    void ChargeQuests(int level)
    {
        foreach (var item in Quests.SrchIncomplete(level))
        {
            item.active = true;
        }
    }

    void CreateQuests()
    {
        #region Level 1

        new Quests.Mission(
            1,
            "Contra Reloj",
            "Completa el nivel en menos de 2 minutos para obtener 10 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(10);
            }
        );


        new Quests.Mission(
            1,
            "Sin toxina",
            "No utilices el poder de Toxina para obtener 5 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(5);
            }
        );

        new Quests.Mission(
            1,
            "Solo una daga",
            "No atraigas mas de una daga durante todo el nivel para obtener 3 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(3);
            }
        );

        new Quests.Mission(
            1,
            "Genocida",
            "Asesina a todos los enemigos antes de pasarte el nivel para obtener 5 puntos",
            () =>
            {
                return GameManager.enemys.Count > 1;
            },
            () =>
            {
                AddPoints(5);
            }
        );

        #endregion

        #region Level 2
        new Quests.Mission(
            2,
            "Singular",
            "Utiliza solo un poder durante todo el nivel para obtener 10 puntos",
            () =>
            {
                return GameManager.player.power.Count > 1;
            },
            () =>
            {
                AddPoints(10);
            }
        );

        new Quests.Mission(
            2,
            "Casi Ileso",
            "No pierdas mas de 10 puntos de vida para obtener 8 puntos",
            () =>
            {
                return GameManager.player.health.hp < (GameManager.player.health.maxHp - 10);
            },
            () =>
            {
                AddPoints(8);
            }
        );

        new Quests.Mission(
            2,
            "Pies en la tierra",
            "No saltes ni una sola vez para obtener 3 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(3);
            }
        );

        new Quests.Mission(
            2,
            "Objetivos claros",
            "Elimina a todos los enemigos de Dash para obtener 5 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(5);
            }
        );



        #endregion

        #region Level 3

        new Quests.Mission(
            3,
            "Adicto al dash",
            "Utiliza el dash al menos 10 veces para obtener 5 puntos",
            () =>
            {
                return Controllers.dash.down;
            },
            () =>
            {
                AddPoints(5);
            }
        );

        new Quests.Mission(
            3,
            "Asesinato veloz",
            "Mata a un enemigo al iniciar el nivel en menos de 30 segundos para obtener 8 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(8);
            }
        );

        new Quests.Mission(
            3,
            "Sin poderes",
            "Termina el nivel sin activar un solo poder para obtener 8 puntos",
            () =>
            {
                return Controllers.power.down;
            },
            () =>
            {
                AddPoints(8);
            }
        );


        new Quests.Mission(
            3,
            "Ataques cargados",
            "Usa la carga al maximo antes de disparar 3 veces para obtener 3 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(3);
            }
        );

        #endregion

        #region Level 4

        new Quests.Mission(
            4,
            "Combo poderoso",
            "Usa el poder de Vortice y Toxina a la vez para obtener 5 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(5);
            }
        );

        new Quests.Mission(
            4,
            "Sin Dash",
            "No utilices el Dash durante todo el nivel para obtener 3 puntos",
            () =>
            {
                return Controllers.dash.down;
            },
            () =>
            {
                AddPoints(3);
            }
        );

        new Quests.Mission(
            4,
            "Coleccionista",
            "Encuentra y atrae todas las dagas del nivel para obtener 3 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(3);
            }
        );

        new Quests.Mission(
            4,
            "Estratega",
            "Encanta la daga solo 2 veces durante todo el nivel para obtener 10 puntos",
            () =>
            {
                return Controllers.jump.down;
            },
            () =>
            {
                AddPoints(10);
            }
        );
        #endregion


        //ejemplo de como activar todas las misiones de un nivel

        //Quests.SrchIncomplete(1)
        foreach (var item in Quests.SrchIncomplete(1))
        {
            item.active = true;
        }

    }


    public void AddPoints(int p)
    {
        var aux= CSVReader.LoadFromPictionary<int>("PlayerPoints");
        CSVReader.SaveInPictionary<int>("PlayerPoints", aux + p);
    }

    #region lobby buttons

    public GameObject[] skillButtons;

    void Awake()
    {
        MenuManager.instance.eventListVoid.AddRange(new Pictionarys<string, System.Action<GameObject>>()
        {
            {"rLevel", RestartLevel},
            {"nLevel", NextLevel},
            {"sLevel", SelectLevelLobby},
            {"bMenu", BackMenu}

        });

    }

    void BackMenu(GameObject g)
    {
        MenuManager.instance.refSceneChanger.Load("MainMenu");
    }
    void RestartLevel(GameObject g)
    {
        int aux = CSVReader.LoadFromPictionary<int>("CurrentLevel");
        MenuManager.instance.refSceneChanger.Load("Level_" + aux.ToString());

        ChargeQuests(aux);
    }
    void NextLevel(GameObject g)
    {
        // int aux = CSVReader.LoadFromPictionary<int>("CurrentLevel");
        // aux +1

        int aux = CSVReader.LoadFromPictionary<int>("LastUnlockedLevel");
        MenuManager.instance.refSceneChanger.Load("Level_" + aux.ToString());

        ChargeQuests(aux);
    }
    void SelectLevelLobby(GameObject g)
    {
        MenuManager.instance.ChangeMenu(1);
    }
    #endregion



}
