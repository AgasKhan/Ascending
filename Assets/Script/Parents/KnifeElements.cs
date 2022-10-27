using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeElements : MonoBehaviour
{
    public float journeyTime = 1.0f;

    public Player_Character character;

    public KnifeElements Other;

    public Vector3 distance;

    [SerializeField]
    protected static List<Knifes> elements = new List<Knifes>();

    [System.Serializable]
    protected class Knifes
    {
        public Transform reference;

        public Dagger_Proyectile daggerScript;

        public Vector3 position;

        public MoveRotAndGlueRb movement;

        public Knifes(Transform r, Vector3 p, Character character)
        {
            reference = r;
            position = p;

            movement = r.GetComponent<MoveRotAndGlueRb>();

            movement.Rotate(Vector3.zero);
            movement.kinematic = true;

            daggerScript = r.GetComponent<Dagger_Proyectile>();

            daggerScript.pause = true;
            daggerScript.owner = character;

            daggerScript.transform.GetChild(0).GetComponent<Interactuable_LogicActive>().diseable = false;
        }
    }

    private void Start()
    {
        character = transform.parent.GetComponent<Player_Character>();
    }
}
