using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MoveCharacter : MonoBehaviour
{
    public GameObject Character;
    public Transform a, b;
    public float MoveSpeed;
    public bool testingRotation = false;
    // Start is called before the first frame update
    void Start()
    {
        Character.transform.position = new Vector3(a.transform.position.x, Character.transform.position.y, a.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        Character.transform.position += transform.forward * Time.deltaTime * MoveSpeed;

        if (Vector3.Distance(Character.transform.position, b.position) < 1f)
        {
            Character.GetComponent<Biped>().Reset();
            
        }
    }
}
