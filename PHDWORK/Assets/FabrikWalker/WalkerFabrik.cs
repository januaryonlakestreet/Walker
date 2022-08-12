using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WalkerFabrik : MonoBehaviour
{
    
    public float GroundHeight;
    public float StepDistance;
    public float BodyMoveDistance;
    public float DistanceToBody;
    public Vector3 StepStartPosition;
    public Vector3 StepDirection;
    public List<GameObject> Allgoals = new List<GameObject>();

    Vector3 startlocation;

    public FabrikSolver FrontL, FrontR, BackL, BackR;
   

    public int LegID = 0;
    public float FullStepMultiplir;


    Vector3 GroundLocation = Vector3.zero;
    public float LegWidth;
    Vector3[] Goals; 
    Vector3[] GoalsNew;
    Vector3 FwdPrevious;

   

    
    


    void Start()
    {
       
        StepStartPosition = this.transform.position;
        startlocation = this.transform.position;
         Allgoals = new List<GameObject>(GameObject.FindGameObjectsWithTag("Goal"));
        Allgoals[0].GetComponent<Goal>().walker = this.transform;
        Allgoals[1].GetComponent<Goal>().walker = this.transform;
        Allgoals[2].GetComponent<Goal>().walker = this.transform;
        Allgoals[3].GetComponent<Goal>().walker = this.transform;


        RaycastHit _out;
        Goals = new Vector3[4];
        GoalsNew = new Vector3[4];
        if (Physics.Raycast(this.transform.position, -transform.up, out _out, Mathf.Infinity))
        {
            GroundLocation = _out.point;

            var FrontLeftAngle = (transform.forward + -transform.right) * LegWidth;
            var FrontRightAngle = (transform.forward + transform.right) * LegWidth;

            var BackLeftAngle = (-transform.forward + -transform.right) * LegWidth;
            var BackRightAngle = (-transform.forward + transform.right) * LegWidth;

            Goals[0] = this.transform.position + (FrontLeftAngle * LegWidth);
            Goals[1] = this.transform.position + (FrontRightAngle * LegWidth);
            Goals[2] = this.transform.position + (BackLeftAngle * LegWidth);
            Goals[3] = this.transform.position + (BackRightAngle * LegWidth);

            Goals[0].y = GroundLocation.y;
            Goals[1].y = GroundLocation.y;
            Goals[2].y = GroundLocation.y;
            Goals[3].y = GroundLocation.y;

            Debug.DrawRay(GroundLocation, FrontLeftAngle, Color.blue, Time.deltaTime);
            Debug.DrawRay(GroundLocation, FrontRightAngle, Color.blue, Time.deltaTime);
            Debug.DrawRay(GroundLocation, BackLeftAngle, Color.blue, Time.deltaTime);
            Debug.DrawRay(GroundLocation, BackRightAngle, Color.blue, Time.deltaTime);

            GoalsNew[0] = Goals[0];
            GoalsNew[1] = Goals[1];
            GoalsNew[2] = Goals[2];
            GoalsNew[3] = Goals[3];

            Allgoals[0].transform.position = GoalsNew[1];
            Allgoals[1].transform.position = GoalsNew[0];
            Allgoals[2].transform.position = GoalsNew[3];
            Allgoals[3].transform.position = GoalsNew[2];

            FwdPrevious = transform.forward;


       
        }
    }
    public void WalkerReset()
    {
      
        this.transform.position = startlocation;
        StepStartPosition = this.transform.position;
    }    
    void Update()
    {         
        #region Handle rotation
        if (Vector3.Angle(this.transform.forward, FwdPrevious) > 1f)
        {
          
            if (LegID == 0 && IsPreviousStepComplete(LegID))
            {

                Goal BackLGoal = BackL.Target.gameObject.GetComponent<Goal>();
                BackLGoal.HandleRotation( CalculateStepCurve(BackLGoal.transform.position, Goals[2]), ref Goals[2], FwdPrevious);

                Goal FrontRGoal = FrontR.Target.gameObject.GetComponent<Goal>();
                FrontRGoal.HandleRotation(CalculateStepCurve(FrontRGoal.transform.position, Goals[1]),ref Goals[1], FwdPrevious);
                if (LegID + 1 == 2)
                {
                    LegID = 0;
                }
                else
                {
                    LegID++;
                }
            }
            if (LegID == 1 && IsPreviousStepComplete(LegID))
            {
                Goal BackRGoal = BackR.Target.gameObject.GetComponent<Goal>();
            BackRGoal.HandleRotation( CalculateStepCurve(BackRGoal.transform.position, Goals[3]), ref Goals[3], FwdPrevious);

            Goal FrontLGoal = FrontL.Target.gameObject.GetComponent<Goal>();
            FrontLGoal.HandleRotation( CalculateStepCurve(FrontLGoal.transform.position, Goals[0]),ref Goals[0], FwdPrevious);
                if (LegID + 1 == 2)
                {
                    LegID = 0;
                }
                else
                {
                    LegID++;
                }
            }
            FwdPrevious = this.transform.forward;
          
        }
        #endregion

        #region Handle taking a step
        if (Vector3.Distance(this.transform.position,StepStartPosition) > BodyMoveDistance)
        {
            StepDirection = (this.transform.position - StepStartPosition).normalized;
            StepStartPosition = this.transform.position;
            if (LegID == 0 && IsPreviousStepComplete(LegID))
            {         
                Goal BackLGoal = BackL.Target.gameObject.GetComponent<Goal>();
                BackLGoal.NewStep(CalculateStepCurve(BackLGoal.transform.position, Goals[2] + StepDirection * (FullStepMultiplir * StepDistance)));
           
                Goal FrontRGoal = FrontR.Target.gameObject.GetComponent<Goal>();
                FrontRGoal.NewStep(CalculateStepCurve(FrontRGoal.transform.position, Goals[1] + StepDirection * (FullStepMultiplir * StepDistance)));

                if (LegID + 1 == 2)
                {
                    LegID = 0;
                }
                else
                {
                    LegID++;
                }
            }
            if (LegID == 1 && IsPreviousStepComplete(LegID))
            {
                Goal BackRGoal = BackR.Target.gameObject.GetComponent<Goal>();
                BackRGoal.NewStep(CalculateStepCurve(BackRGoal.transform.position,Goals[3] + StepDirection * (FullStepMultiplir * StepDistance)));

                Goal FrontLGoal = FrontL.Target.gameObject.GetComponent<Goal>();
                FrontLGoal.NewStep(CalculateStepCurve(FrontLGoal.transform.position, Goals[0] + StepDirection * (FullStepMultiplir * StepDistance)));


                if (LegID + 1 == 2)
                {
                    LegID = 0;
                }
                else
                {
                    LegID++;
                }
            }        
        }
        #endregion
        UpdateGoalPosition();



    }
    bool IsPreviousStepComplete(int IdOfStepWaiting)
    {
        if(IdOfStepWaiting == 0)
        {
            FabrikSolver Solver = FrontL;
            Goal SolverGoal = Solver.Target.gameObject.GetComponent<Goal>();
            FabrikSolver Solver1 = BackR;
            Goal SolverGoal1 = Solver1.Target.gameObject.GetComponent<Goal>();

            if (SolverGoal.StepPositions.Count + SolverGoal1.StepPositions.Count == 0)
            {
                return true;
            }
            return false;
        }
        if (IdOfStepWaiting == 1)
        {
            FabrikSolver Solver = BackL;
            Goal SolverGoal = Solver.Target.gameObject.GetComponent<Goal>();
            FabrikSolver Solver1 = FrontR;
            Goal SolverGoal1 = Solver1.Target.gameObject.GetComponent<Goal>();

            if(SolverGoal.StepPositions.Count + SolverGoal1.StepPositions.Count == 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public static List<Vector3> CalculateStepCurve(Vector3 StartPosition,Vector3 GoalPosition)
    {
        List<Vector3> Positions = new List<Vector3>();
        float t = 0f;

        Vector3 CalculateThirdPoint()
        {
            Vector3 ThirdPoint = (StartPosition + GoalPosition) / 2f;
            ThirdPoint.y += 0.5f;
            return ThirdPoint;
        }
        Vector3 CurveLerp(float t, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 l1 = Vector3.Lerp(p1, p2, t);
            Vector3 l2 = Vector3.Lerp(p2, p3, t);
            return Vector3.Lerp(l1, l2, t);
        }
        while(!Positions.Contains(GoalPosition))
        {
            t += 0.5f*Time.deltaTime;
            Positions.Add(CurveLerp(t,StartPosition,CalculateThirdPoint(),GoalPosition));
            if(Positions.Contains(GoalPosition))
            {
                break;
            }
        }
        return Positions;
    }

    List<Vector3> DecimatePath(List<Vector3> path, float DecimatePercent = 0.5f)
    {
        Vector3 Start, End;
        List<Vector3> _Path = new List<Vector3>();
        List<Vector3> ResultingPath = new List<Vector3>();
        _Path = path;
        Start = path[0];
        End = path[path.Count - 1];
        int itter = Mathf.FloorToInt(DecimatePercent * path.Count);
        for (int a = 0; a < itter; a++)
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


    void UpdateGoalPosition()
    {
        RaycastHit _out;

        if (Physics.Raycast(this.transform.position, -transform.up, out _out, Mathf.Infinity))
        {
            GroundLocation = _out.point;

            var FrontLeftAngle = (transform.forward + -transform.right) * LegWidth;
            var FrontRightAngle = (transform.forward + transform.right) * LegWidth;

            var BackLeftAngle = (-transform.forward + -transform.right) * LegWidth;
            var BackRightAngle = (-transform.forward + transform.right) * LegWidth;

            Goals[0] = this.transform.position + (FrontLeftAngle * LegWidth);
            Goals[1] = this.transform.position + (FrontRightAngle * LegWidth);
            Goals[2] = this.transform.position + (BackLeftAngle * LegWidth);
            Goals[3] = this.transform.position + (BackRightAngle * LegWidth);

            Goals[0].y = GroundLocation.y;
            Goals[1].y = GroundLocation.y;
            Goals[2].y = GroundLocation.y;
            Goals[3].y = GroundLocation.y;

            Debug.DrawRay(GroundLocation, FrontLeftAngle, Color.blue, Time.deltaTime);
            Debug.DrawRay(GroundLocation, FrontRightAngle, Color.blue, Time.deltaTime);
            Debug.DrawRay(GroundLocation, BackLeftAngle, Color.blue, Time.deltaTime);
            Debug.DrawRay(GroundLocation, BackRightAngle, Color.blue, Time.deltaTime);

            GoalsNew[0] = Goals[0];
            GoalsNew[1] = Goals[1];
            GoalsNew[2] = Goals[2];
            GoalsNew[3] = Goals[3];
        }
    }
    private void OnDrawGizmos()
    {
        if (Goals != null)
        {
            if (Goals.Count() > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Goals[0], 0.1f);
                Gizmos.DrawSphere(Goals[1], 0.1f);
                Gizmos.DrawSphere(Goals[2], 0.1f);
                Gizmos.DrawSphere(Goals[3], 0.1f);
            }
        }
   
       
        
    }

}
