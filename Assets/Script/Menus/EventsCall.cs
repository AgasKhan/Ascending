using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Events;
using TMPro;

public class EventsCall : MonoBehaviour
{

    MenuManager menu;

    public void Event(GameObject g)
    {
        print("configurando: " + g.name);

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        if (g.TryGetComponent(out Button b))
        {
            print("configurado boton");
            b.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            b.onClick.RemoveAllListeners();

            //UnityEventTools.RemovePersistentListener(b.onClick, 0);
            b.onClick.AddListener(() => { 
                menu.eventListVoid[g.name](g);
            });
            menu.eventListVoid[g.name](g);
            return;
        }
        else if (g.TryGetComponent(out Slider s))
        {
            print("configurado slider");
            s.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            s.onValueChanged.RemoveAllListeners();

            //UnityEventTools.RemovePersistentListener(s.onValueChanged, 0);
            s.onValueChanged.AddListener((float f) => { menu.eventListFloat[g.name](g,f); });
            menu.eventListFloat[g.name](g,s.value);
            return;
        }

        DebugPrint.Warning("no contiene uno de los componentes esperados: " + g.name);
    }
    void Start()
    {
        menu = MenuManager.instance;
    }
}
