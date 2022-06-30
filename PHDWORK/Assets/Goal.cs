using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //draws a bunch of debug stuff to make seeing the goals easier.
    Vector3 CurrentLocation, StartLocation;
    public Vector3 GoalLocation;
    public List<Vector3> StepPositions = new List<Vector3>();
    private void Start()
    {
        StartLocation = this.transform.position;
    }
    public void Reset()
    {
        StepPositions.Clear();
        this.transform.position = StartLocation;
        CurrentLocation = this.transform.position;
        GoalLocation = this.transform.position;
        StartLocation = this.transform.position;
    }
    public void NewStep(List<Vector3> Positions)
    {
        StepPositions.Clear();
        StepPositions = Positions;
    }
    private void Update()
    {

        if (StepPositions.Count > 0)
        {
            this.transform.rotation = Quaternion.LookRotation((StepPositions[0] - this.transform.position).normalized, Vector3.up);
            this.transform.position += transform.forward * Time.deltaTime;
            if(Vector3.Distance(this.transform.position,StepPositions[0]) < 0.1f)
            {
                StepPositions.RemoveAt(0);
            }
        }
    }

}
