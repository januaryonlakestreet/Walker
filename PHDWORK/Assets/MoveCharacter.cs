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
        Vector3 Direction = (Character.transform.position - b.transform.position).normalized;
        Character.transform.rotation = Quaternion.Lerp(Character.transform.rotation,Quaternion.LookRotation(Direction, Vector3.up),Time.deltaTime);
        if(!testingRotation)
        {
            Character.transform.position += transform.forward * Time.deltaTime * MoveSpeed;
        }
       
        if(Vector3.Distance(Character.transform.position,b.position) < 1f)
        {
            List<GameObject> G = GameObject.FindGameObjectsWithTag("Goal").ToList();

            foreach (GameObject g in G)
            {
                g.GetComponent<Goal>().Reset();
            }
            Character.GetComponent<Walker>().WalkerReset();
            
        }
    }
}
