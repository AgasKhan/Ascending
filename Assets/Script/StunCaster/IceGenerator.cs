using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGenerator : PowerSpawner
{
    // Start is called before the first frame update

    Vector2Int Ice;

    private void Awake()
    {
        Ice = PoolObjects.SrchInCategory("Stun", "Ice");
    }

    protected override void Detect(MoveRb item)
    {
        print(Ice);

        List<bool> monos = new List<bool>();

        var ice = PoolObjects.SpawnPoolObject(Ice, item.transform.position, Quaternion.identity);

        ice.transform.parent = item.transform;

        var monosScript = item.GetComponentsInChildren<MonoBehaviour>();

        TimersManager.Create(3,
            () =>
            {                
                item.kinematic = true;

                foreach (var subitem in monosScript)
                {
                    monos.Add(subitem);

                    subitem.enabled = false;
                }
            });


        TimersManager.Create(12,
            () =>
            {
                foreach (Transform subitem in ice.transform)
                {
                    subitem.parent = null;
                }

                for (int i = 0; i < monosScript.Length; i++)
                {
                    monosScript[i].enabled = monos[i];
                }

                item.kinematic = false;

                ice.gameObject.SetActive(false);
            });
    }

}
