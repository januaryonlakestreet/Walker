using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegOrientation : MonoBehaviour
{
    public Transform Goal;
    Vector3 Dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Goal = FindObjectOfType<Biped>().BipedGoal;
        Dir = this.transform.position;
        Vector3 DirectionToGoal = Goal.transform.position - Dir;

        Debug.DrawRay(this.transform.position, DirectionToGoal, Color.red, Time.deltaTime);
        this.transform.rotation = Quaternion.LookRotation(DirectionToGoal, -Vector3.up);
    }
}
