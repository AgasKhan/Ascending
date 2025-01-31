using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Powers : Powers_FatherPwDbff
{
    [SerializeField]
    Vector2Int Ice;
    public override void OnEnterState(Character me)
    {
        Ice = SchPowerObject("Ice");

        me.AddDebuffToAplicate<Stun_Debuff>();

        if(me.CompareTag("Player"))
            on_Update = MyUpdatePlayer;
    }

    public override void OnExitState(Character me)
    {
        me.RemoveDebuffToAplicate<Stun_Debuff>();
    }

    public override void ButtonEvent(Character me, float timePressed)
    {
        me.ActionOnDamage += IceGemerator;
    }

    void IceGemerator(Collider item)
    {
        if (!item.gameObject.TryGetComponent(out MoveRb moveRb))
        {
            return;            
        }

        List<bool> monos = new List<bool>();

        var ice = (GameObject)PoolObjects.SpawnPoolObject(Ice, item.transform.position, Quaternion.identity);

        List<Behaviour> monosScript = new List<Behaviour>();

        monosScript.AddRange(item.GetComponentsInChildren<Behaviour>());

        for (int i = monosScript.Count - 1; i >= 0; i--)
        {
            if (!monosScript[i].CompareTag("Dagger"))
            {
                monos.Add(monosScript[i]);

                monosScript[i].enabled = false;
            }
            else
            {
                monosScript.RemoveAt(i);
            }

        }

        TimersManager.Create(2,
            () =>
            {
                moveRb.kinematic = true;
            });


        TimersManager.Create(12,
            () =>
            {
                foreach (Transform subitem in ice.transform)
                {
                    subitem.SetParent(null);
                }

                for (int i = 0; i < monosScript.Count; i++)
                {
                    monosScript[i].enabled = monos[i];
                }

                moveRb.kinematic = false;

                ice.gameObject.SetActive(false);
            });

        ice.transform.parent = item.transform;
    }

    void MyUpdatePlayer(Character me)
    {

        if (me.scoped != null && me.scoped.gameObject.CompareTags("rb"))
        {
            MainHud.ReticulaPlay("Power");
        }
        else
        {
            MainHud.ReticulaPlay("Default");
        }
    }
}
