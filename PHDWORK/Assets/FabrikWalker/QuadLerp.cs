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
                List<Vector3> pointsDecimated = DecimatePath(TestPoints);
               
                GetComponent<LineRenderer>().positionCount = pointsDecimated.Count;
                GetComponent<LineRenderer>().SetPositions(pointsDecimated.ToArray());
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


    List<Vector3> DecimatePath(List<Vector3> path,float DecimatePercent = 0.5f)
   {
        Vector3 Start, End;
        List<Vector3> _Path = new List<Vector3>();
        List<Vector3> ResultingPath = new List<Vector3>();
        _Path = path;
        Start = path[0];
        End = path[path.Count-1];
        int itter = Mathf.FloorToInt(DecimatePercent * path.Count);
        for (int a= 0; a < itter; a++)
        {
            ResultingPath.Clear();
            for (int b = 1; b < _Path.Count; b++)
            {
                ResultingPath.Add((_Path[b] + _Path[b - 1]) / 2f);
            }
            _Path.Clear();
            _Path.AddRange(ResultingPath);
        
        }
        List<Vector3> _ResultingPath = new List<Vector3>();
        _ResultingPath.Add(Start);
        _ResultingPath.AddRange(ResultingPath);
        _ResultingPath.Add(End);
        return _ResultingPath;
    }
}
