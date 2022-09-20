using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootOrientation : MonoBehaviour
{
    public Transform Goal;
    Vector3 Dir;
    Vector3 startEular;
    public Vector3 DesiredEular;
    public float yvalue = 0f;
     
    // Start is called before the first frame update
    void Start()
    {
        startEular = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Goal = FindObjectOfType<Biped>().BipedGoal;
        Dir = this.transform.position;
        Vector3 DirectionToGoal = Goal.transform.position - Dir;

        Debug.DrawRay(this.transform.position, DirectionToGoal, Color.red, Time.deltaTime);

    
        DesiredEular = Quaternion.LookRotation(DirectionToGoal, Vector3.up).eulerAngles;
        yvalue = DesiredEular.y;
        this.transform.localEulerAngles = startEular;
     // this.transform.GetChild(0).transform.eulerAngles = startEular;
    }
 
}
