using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Walker : MonoBehaviour
{
    
    public float GroundHeight;
    public float StepDistance;
    public float BodyMoveDistance;
    public float DistanceToBody;
    public Vector3 StepStartPosition;
    public Vector3 StepDirection;
     List<GameObject> Allgoals = new List<GameObject>();

    Vector3 startlocation;

    public Solver FrontL, FrontR, BackL, BackR;
  
    public int LegID = 0;
    public float FullStepMultiplir;


    public Vector3 GroundLocation = Vector3.zero;
    public float LegWidth;
    public Vector3[] Goals;
    public Vector3[] GoalsNew;
    public Vector3 FwdPrevious;
    void Start()
    {
        StepStartPosition = this.transform.position;
        startlocation = this.transform.position; 
        Allgoals = new List<GameObject>(GameObject.FindGameObjectsWithTag("Goal"));
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
      
        if (Vector3.Distance(this.transform.position,StepStartPosition) > BodyMoveDistance || Vector3.Angle(transform.forward, FwdPrevious) > 15f)
        {
            StepDirection = (this.transform.position - StepStartPosition).normalized;
            StepStartPosition = this.transform.position;
            if (LegID == 0 && IsPreviousStepComplete(LegID))
            {         
                Goal BackLGoal = BackL.Target.gameObject.GetComponent<Goal>();
                BackLGoal.NewStep(CalculateStepCurve(BackLGoal.transform.position, Goals[2] + StepDirection * (FullStepMultiplir * StepDistance)));

                Solver Solver1 = FrontR;
                Goal SolverGoal1 = Solver1.Target.gameObject.GetComponent<Goal>();
                Solver1.Target.gameObject.GetComponent<Goal>().NewStep(CalculateStepCurve(Solver1.Target.gameObject.GetComponent<Goal>().transform.position, Goals[1] + StepDirection * (FullStepMultiplir * StepDistance)));


            }
            if (LegID == 1 && IsPreviousStepComplete(LegID))
            {
                Solver Solver = BackR;
                Goal SolverGoal = Solver.Target.gameObject.GetComponent<Goal>();
                Solver.Target.gameObject.GetComponent<Goal>().NewStep(CalculateStepCurve(Solver.Target.gameObject.GetComponent<Goal>().transform.position,
                   Goals[3] + StepDirection * (FullStepMultiplir * StepDistance)));
                //Solver.Target.gameObject.GetComponent<Goal>().NewStep(CalculateStepCurve(Solver.Target.gameObject.GetComponent<Goal>().transform.position,
                //SolverGoal.transform.position + StepDirection * (FullStepMultiplir * StepDistance)));
            


                Solver Solver1 = FrontL;
                Goal SolverGoal1 = Solver1.Target.gameObject.GetComponent<Goal>();
                Solver1.Target.gameObject.GetComponent<Goal>().NewStep(CalculateStepCurve(Solver1.Target.gameObject.GetComponent<Goal>().transform.position,
                Goals[0] + StepDirection * (FullStepMultiplir * StepDistance)));
              //  Solver1.Target.gameObject.GetComponent<Goal>().NewStep(CalculateStepCurve(Solver1.Target.gameObject.GetComponent<Goal>().transform.position,
               // SolverGoal1.transform.position + StepDirection * (FullStepMultiplir * StepDistance)));
            }
          if(LegID +1 == 2)
            {
                LegID = 0;
            }
            else
            {
                LegID++;
            } 
          if(Vector3.Angle(transform.forward, FwdPrevious) > 15f)
            {
                FwdPrevious = this.transform.forward;
            }
        }
        UpdateGoalPosition();



    }
    bool IsPreviousStepComplete(int IdOfStepWaiting)
    {
        if(IdOfStepWaiting == 0)
        {
            Solver Solver = FrontL;
            Goal SolverGoal = Solver.Target.gameObject.GetComponent<Goal>();
            Solver Solver1 = BackR;
            Goal SolverGoal1 = Solver1.Target.gameObject.GetComponent<Goal>();

            if (SolverGoal.StepPositions.Count + SolverGoal1.StepPositions.Count == 0)
            {
                return true;
            }
            return false;
        }
        if (IdOfStepWaiting == 1)
        {
            Solver Solver = BackL;
            Goal SolverGoal = Solver.Target.gameObject.GetComponent<Goal>();
            Solver Solver1 = FrontR;
            Goal SolverGoal1 = Solver1.Target.gameObject.GetComponent<Goal>();

            if(SolverGoal.StepPositions.Count + SolverGoal1.StepPositions.Count == 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    List<Vector3> CalculateStepCurve(Vector3 StartPosition,Vector3 GoalPosition)
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
