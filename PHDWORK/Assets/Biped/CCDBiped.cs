using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://sites.google.com/site/auraliusproject/ccd-algorithm
//https://stackoverflow.com/questions/21373012/best-inverse-kinematics-algorithm-with-constraints-on-joint-angles 
//https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.1/manual/constraints/TwistCorrection.html do your own olly.

public class CCDBiped : MonoBehaviour
{
    public Transform Goal;
     int iterations = 1;
    public List<Transform> chains = new List<Transform>();
    public Transform End;
    public Transform Pole;
    public bool UseConstraints = false;
    
    public bool UsePoles = true;
    public float PoleStrengthBase = 0.3f;
    const int ChainLengthCutOff = 5;
    public int endcounter = 1;
    public int ChainLength;
    // Start is called before the first frame update
    void Start()
    {
      chains.Add(this.transform);
    Transform Current = transform.GetChild(0);

       while (chains.Count < ChainLengthCutOff)
        {
            chains.Add(Current);
            if(Current.childCount > 0 )
            {
                Current = Current.GetChild(0);
            }
           
        }

        End = chains[chains.Count-endcounter];
        chains.Reverse();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 GoalPosition;
        GoalPosition = Goal.position;

        for (int a = 0; a < iterations; a++)
        {
            for (int b = 1; b < chains.Count; b++)
            {         
             
                chains[b].transform.rotation = HandleRotation(chains[b], GoalPosition); 
            }

        }
    }

    bool PolesCorrectlySetup()
    {
        if (UsePoles && Pole)
        {

            return true;
        }

        return false;
    }

    float PoleStrength(Transform t)
    {
        // strength of the pole on any given joint is influenced by the distance of the joint to the pole 
        // that is the closer to the pole the stronger it pulls.?
        return Mathf.Clamp(PoleStrengthBase, 0, PoleStrengthBase / Vector3.Distance(Pole.position, t.position));
    }
    //  Quaternion HandleRotation(Transform bone,Transform boneNext = null, Vector3 goalPosition)
    Quaternion HandleRotation(Transform bone, Vector3 goalPosition)
    {
        Vector3 Bone2Effector = End.position - bone.position;
        Vector3 Bone2Goal;

        bool ValidBone(Transform t)
        {
            
            if (chains.IndexOf(t) == 1 ||  chains.IndexOf(t) == chains.Count )
            {
                
                return false;
            }
            return true;
        }

        if (PolesCorrectlySetup() &&ValidBone(bone))
        {
            Vector3 PoleDirection = bone.position - Pole.position;
            Bone2Goal = goalPosition - bone.position - (PoleDirection * PoleStrength(bone));
        }
        else
        {
            Bone2Goal = goalPosition - bone.position;
        }

      
                //calculate new fromto rotation
                Quaternion fromToRotation = Quaternion.FromToRotation(Bone2Effector, Bone2Goal);
        //uncomment for constraint code
        /*
                //calculate new applied rotation
                Quaternion NewApplied = fromToRotation * bone.rotation;
                //if angle between new applied and next is less than limit apply it 
                if(Quaternion.Angle(NewApplied,boneNext.rotation) < bone.GetComponent<joint>().MaxAngle)
                {
                    return fromToRotation * bone.rotation;
                }
                if (Quaternion.Angle(NewApplied, boneNext.rotation) > bone.GetComponent<joint>().MinAngle)
                {
                    return fromToRotation * bone.rotation;
                }
        */
        return fromToRotation * bone.rotation;
        //if not return current rotation.


    }

   


}
