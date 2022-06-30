using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuadLerp : MonoBehaviour
{
    public GameObject goal;
    Vector3 DirectionToGoal;
    float distanceToGoal;

    public Vector3 StartPoint, ControlPoint, EndPoint;
    public List<Vector3> TestPoints = new List<Vector3>();
    public AnimationCurve Curvetest;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //  DirectionToGoal = goal.transform.position - this.transform.position;
        //  DirectionToGoal.Normalize();
        //  distanceToGoal = Vector3.Distance(this.transform.position, goal.transform.position);
        //   DirectionToGoal = this.transform.rotation * DirectionToGoal;
        //  goal.transform.position = this.transform.position + (DirectionToGoal * distanceToGoal);
        while(t < 1f)
        {
            t += Time.deltaTime;
            TestPoints.Add(CurveLerp(t, StartPoint, ControlPoint, EndPoint));
            if(t >= 1f)
            {
                GetComponent<LineRenderer>().positionCount = TestPoints.Count;
                GetComponent<LineRenderer>().SetPositions(TestPoints.ToArray());
                break;
            }
        }

    }
    float t = 0;

  
    Vector3 CurveLerp(float t, Vector3 p1,Vector3 p2,Vector3 p3)
    {
        Vector3 l1 = Vector3.Lerp(p1, p2, t);
        Vector3 l2 = Vector3.Lerp(p2, p3, t);
         return  Vector3.Lerp(l1, l2, t);
    }
}
