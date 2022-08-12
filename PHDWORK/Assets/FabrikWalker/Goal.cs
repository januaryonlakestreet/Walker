using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Goal : MonoBehaviour
{
    //draws a bunch of debug stuff to make seeing the goals easier.
    public Vector3 CurrentLocation, StartLocation;
    public Vector3 GoalLocation;
    public List<Vector3> StepPositions = new List<Vector3>();
    bool Takingstep = false;
    public float smoothTime = 9F;
   

    float duration = 0.05f;

    float startTime;
   
   
    Vector3 _emergStopPoint;
    Vector3 fwdAtRotationStart;

    public Transform walker;
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

    public void HandleRotation(List<Vector3> path,ref Vector3 emergStopPoint,Vector3 fwd)
    {
        StepPositions = path;
        _emergStopPoint = emergStopPoint;
        fwdAtRotationStart = fwd;
    
    }

    public void DoEmergencyStop()
    {
        StepPositions.Clear();
        // StepPositions.AddRange(WalkerFabrik.CalculateStepCurve(this.transform.position, _emergStopPoint));
        StepPositions.Add(_emergStopPoint);

    }


    public void NewStep(List<Vector3> Positions)
    {
        if (Takingstep) { return; }
        Takingstep = true;
        StepPositions = Positions;     
        startTime = Time.time;


    }
  
    private void LateUpdate()
    {
       


        if (StepPositions.Count > 0)
        {   
          if(Vector3.Angle(walker.transform.forward, fwdAtRotationStart) > 15f)
            {
              
                DoEmergencyStop();
            }
                



            float t = (Time.time - startTime) / duration;
            this.transform.position = new Vector3(Mathf.SmoothStep(this.transform.position.x, StepPositions[0].x, t),
                Mathf.SmoothStep(this.transform.position.y, StepPositions[0].y, t),
                Mathf.SmoothStep(this.transform.position.z, StepPositions[0].z, t));

            if (Vector3.Distance(this.transform.position,StepPositions[0]) < 0.1f)
            {
                StepPositions.RemoveAt(0);
            }

        }
        else
        {
            Takingstep = false;
           
        }
     
    }

}
