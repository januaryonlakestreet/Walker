using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Biped : MonoBehaviour
{
    public Transform LeftLeg, RightLeg;
    public CCDBiped BipedLeftLegGoal, BipedRightLegGoal, BipedLeftArm, BipedRightArm;
    public Vector3 StepStartPosition;
    
 
    public float StepDistance; 
    Vector3 StartLocation;
    public Transform GoalHolder;
    public Transform BipedGoal;
    public int LegID = 0;
    public int stepcounter = 0;
    Vector3 DirToGoal;
    public int goalid = 0;
    public GameObject HipReference;
    public float StepDistanceNew;
    public float FootDistance;
    Vector3 dir;
    Vector3 LabelPos;
    GameObject Label;
    public float x, y;


    public void Reset()
    {
        this.transform.localPosition = StartLocation;
      
        BipedLeftLegGoal.Goal.GetComponent<GoalBiped>().Reset();
        BipedRightLegGoal.Goal.GetComponent<GoalBiped>().Reset();
        BipedLeftLegGoal.Pole.GetComponent<PoleHelper>().Reset();
        BipedRightLegGoal.Pole.GetComponent<PoleHelper>().Reset();
    }
    // Start is called before the first frame update
    void Start()
    {
        x = 80f;
        y = 30f;
        StepStartPosition = this.transform.localPosition;
        StartLocation = this.transform.localPosition;
        List<ArmSwing> arms = FindObjectsOfType<ArmSwing>().ToList();
        foreach (ArmSwing a in arms)
        {
            a.startswing();
        }
        BipedGoal = GoalHolder.GetChild(goalid).transform;
    }
   
    float _stepdistNew(Transform t)
    {
        if (stepcounter == 0)
        {
            return StepDistanceNew / 2;
        }
        return StepDistanceNew;
    }

    float _stepdist(Transform t)
    {
        if(stepcounter == 0)
        {
            return StepDistance / 2;
        }
        return StepDistance;
    }
    Vector3 LeftGoal;

    public Vector3 NewGoal;
    
   
   
    private void Update()
    {
        LabelPos = Camera.main.WorldToScreenPoint(HipReference.transform.position);
        LabelPos = new Vector3(LabelPos.x + x, LabelPos.y + y, LabelPos.z);
        Label = GameObject.Find("ccdLabel");
        Label.GetComponent<RectTransform>().position = LabelPos;


        DirToGoal = BipedGoal.transform.position - HipReference.transform.position;
        List<ArmSwing> arms = FindObjectsOfType<ArmSwing>().ToList();
        
        if (Vector3.Angle(this.transform.forward, DirToGoal) > 2f)
        {

            Quaternion newdir = Quaternion.LookRotation(DirToGoal,Vector3.up);
            Vector3 newDirEular = new Vector3(this.transform.eulerAngles.x, newdir.eulerAngles.y, this.transform.eulerAngles.z);
            HipReference.transform.eulerAngles = newDirEular;


        }
        
        if (!AtDestination() && LegsClearToMove())
        {
            foreach (ArmSwing a in arms)
            {
                a.startswing();
            }

            GoalBiped StepGoalObject = null;
            switch (LegID)
            {
                case 0:
                    StepGoalObject = BipedLeftLegGoal.Goal.GetComponent<GoalBiped>();
              
                break;
                case 1:
                    StepGoalObject = BipedRightLegGoal.Goal.GetComponent<GoalBiped>();
              
                break;
            }
            Vector3 StepGoal =ConstructGoalPosition(StepGoalObject.transform);
            NewGoal = ConstructGoalPositionNew(StepGoalObject.transform);
            StepGoalObject.NewStep(CalculateStepCurve(StepGoalObject.transform.position, NewGoal), NewGoal);
            Debug.DrawRay(HipReference.transform.localPosition, DirToGoal * _stepdistNew(StepGoalObject.transform), Color.red, Time.deltaTime);

            foreach (ArmSwing a in arms)
            {
                a.switchdirections();
            }
            if (LegID + 1 == 2)
            {
                LegID = 0;
            }
            else
            {
                LegID = 1;
            }
            stepcounter++;
        }
        if (AtDestination())
        {        
            updateBipedGoal();         
            foreach (ArmSwing a in arms)
            {
                a.StopSwing();
            }
        }
        
    }
    Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
    bool LegsClearToMove()
    {
        if(BipedLeftLegGoal.Goal.GetComponent<GoalBiped>().StepPositions.Count < 3 &&
            BipedRightLegGoal.Goal.GetComponent<GoalBiped>().StepPositions.Count < 3)
        {
            return true;
        }
        return false;
    }
   
    void updateBipedGoal()
    {
       // stepcounter = 0;
        if(goalid +1 < GoalHolder.childCount-1)
        {
            goalid +=1;
            BipedGoal = GoalHolder.GetChild(goalid).transform;
            return;
        }
        else
        {
            goalid = 0;
            BipedGoal = GoalHolder.GetChild(goalid).transform;
            return;
        }
       
    }
    bool AtDestination()
    {
        
        if (Vector3.Distance(BipedGoal.transform.position,HipReference.transform.position) < StepDistance )
        {        
            return true;
        }
        return false;
    }
   

    Vector3 ConstructGoalPosition(Transform t)
    {

      
        LeftGoal = t.transform.position + (transform.forward * _stepdist(t));
     
        LeftGoal.y = 0f;
        return LeftGoal;
    }
    Vector3 ConstructGoalPositionNew(Transform t)
    {
        Vector3 start =HipReference.transform.position;
        dir =HipReference.transform.forward;


        NewGoal = start + (dir * _stepdistNew(t));
        switch (LegID)
        {
            case 0:
                NewGoal.x -= FootDistance;

                break;
            case 1:
                NewGoal.x += FootDistance;

                break;
        }
        NewGoal.y =HipReference.GetComponent<HipPosition>().GetHipGroundPosition().y;

        return NewGoal;
    }




    public static List<Vector3> CalculateStepCurve(Vector3 StartPosition, Vector3 GoalPosition,bool curvedown=false)
    {
        List<Vector3> Positions = new List<Vector3>();
        float t = 0f;

        Vector3 CalculateThirdPoint()
        {
            float Height = 0.55f;
            Vector3 ThirdPoint = (StartPosition + GoalPosition) / 2f;
            if (!curvedown)
            {
                ThirdPoint.y += Height;
            }
            else
            {
                ThirdPoint.y -= Height;
            }
        
            return ThirdPoint;
        }
        Vector3 CurveLerp(float t, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 l1 = Vector3.Lerp(p1, p2, t);
            Vector3 l2 = Vector3.Lerp(p2, p3, t);
            return Vector3.Lerp(l1, l2, t);
        }
        while (!Positions.Contains(GoalPosition))
        {
            t += 0.8f * Time.deltaTime;
            Positions.Add(CurveLerp(t, StartPosition, CalculateThirdPoint(), GoalPosition));
            if (Positions.Contains(GoalPosition) || Positions.Count > 1000)
            {
                break;
            }
        }

        return Positions;
    }


    private void OnDrawGizmos()
    {
        
    }
}
