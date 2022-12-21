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

        List<MonoBehaviour> monosScript = new List<MonoBehaviour>();

        monosScript.AddRange(item.GetComponentsInChildren<MonoBehaviour>());

        TimersManager.Create(3,
            () =>
            {                
                item.kinematic = true;

                for (int i = monosScript.Count - 1; i >= 0 ; i--)
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
            });


        TimersManager.Create(12,
            () =>
            {
                foreach (Transform subitem in ice.transform)
                {
                    subitem.parent = null;
                }

                for (int i = 0; i < monosScript.Count; i++)
                {
                    monosScript[i].enabled = monos[i];
                }

                item.kinematic = false;

                ice.gameObject.SetActive(false);
            });
    }

}
