using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://sites.google.com/site/auraliusproject/ccd-algorithm
//https://stackoverflow.com/questions/21373012/best-inverse-kinematics-algorithm-with-constraints-on-joint-angles 
//https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.1/manual/constraints/TwistCorrection.html do your own olly.
 
public class analytical : MonoBehaviour
{
    public Transform Goal;
    public int iterations;  
    public List<Transform> chains = new List<Transform>();
    public Transform  End;
    public Transform Pole;
    public bool UseConstraints = false;
    public bool UseTwistCorrection = true;
    public bool UsePoles = false;
    public float PoleStrengthBase;
    public bool Go = false;
    // Start is called before the first frame update
    void Start()
    {
        setupchain();
    }

    public void setupchain()
    {
        #region Chain set up
        Transform Current = transform.GetChild(0);
        while (Current.childCount != 0)
        {
            chains.Add(Current);
            Current = Current.GetChild(0);
        }
        //chains.Add(GameObject.Find("Bone014").transform);
        End = chains[chains.Count - 1];
        chains.Reverse();
        #endregion
    }
    // Update is called once per frame
    void Update()
    {
        if (Go)
        {
            Vector3 GoalPosition;
            GoalPosition = Goal.position;

            for (int a = 0; a < iterations; a++)
            {
                for (int b = 0; b < chains.Count - 1; b++)
                {
                    chains[b].transform.rotation = HandleRotation(chains[b], GoalPosition);
                    // HandleRotation(chains[b], GoalPosition);
                }
                if (UseTwistCorrection)
                {
                    for (int b = 1; b < chains.Count - 1; b++)
                    {
                        chains[b].rotation = new Quaternion(0f, chains[b].rotation.y, chains[b].rotation.z, chains[b].rotation.w);
                    }
                }
            }
        }
       

    }
    
    bool PolesCorrectlySetup()
    {
        if(UsePoles && Pole)
        {
       
            return true;
        }
       
        return false;
    }

    float PoleStrength(Transform t)
    {
        // strength of the pole on any given joint is influenced by the distance of the joint to the pole 
        // that is the closer to the pole the stronger it pulls.?
        return PoleStrengthBase / Vector3.Distance(Pole.position,t.position);
    }
    Quaternion HandleRotation(Transform bone,Vector3 goalPosition)
    {
        Vector3 Bone2Effector = End.position - bone.position;
        Vector3 Bone2Goal;

        bool ValidBone(Transform t)
        {
            if(chains.IndexOf(t) == 0 || chains.IndexOf(t) == chains.Count-1)
            {
                return false;
            }
            return true;
        }

        if (PolesCorrectlySetup() && ValidBone(bone))
        {
            Vector3 PoleDirection = Pole.position - bone.position;
            Bone2Goal = goalPosition - bone.position - (PoleDirection * PoleStrength(bone));
        }
        else
        {
            Bone2Goal = goalPosition - bone.position;
        }

       
        Quaternion fromToRotation = Quaternion.FromToRotation(Bone2Effector, Bone2Goal);
        Vector3 RotationDiff;

        if (UseConstraints)
        {
            RotationDiff = bone.eulerAngles - fromToRotation.eulerAngles;

            bool WithinConstraints(joint j)
            {
                if (RotationDiff.x < j.MinRotation.x || RotationDiff.y < j.MinRotation.y || RotationDiff.z < j.MinRotation.z
                    || RotationDiff.x > j.MaxRotation.x || RotationDiff.y > j.MaxRotation.y || RotationDiff.z > j.MaxRotation.z)
                {
                    return false;
                }
                return true;
            }
            if (!WithinConstraints(bone.gameObject.GetComponent<joint>()))
            {
                bone.rotation = bone.rotation;

            }
            else
            {
                bone.rotation = fromToRotation * bone.rotation;
            }
            return Quaternion.identity;
        }
      return fromToRotation * bone.rotation;
    }
 

  

}
