using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class Solver : MonoBehaviour
    {
    
    public int ChainLength = 2;
    public Transform Target;
    public Transform Pole;
    int Iterations = 10;
    float[] BonesLength; 
    float CompleteLength;
    Transform[] Bones;
    Vector3[] Positions;
    Vector3[] BoneDirectionHelper;
    Quaternion[] StartRotationBone;
    Quaternion StartRotationTarget;
    Transform Root;


    // Start is called before the first frame update
    void Awake()
    {
        #region initalises everything   
        //BoneDirectionHelper is a directional vector to the next bone in the chain.
        //StartBoneRotation is the rotation of the transform in the chain.
        //Bones the actual bone transform
        //The positions everything in the chain
        //bone length is the length between two bones in the chain.
        //plus one values are there to incorporate in the target, such as when the boneDirectionHelp gets the direction to the target from the last bone in the chain.
        BoneDirectionHelper = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];
        #endregion
        #region Find the root bone by traversing up the chain of game objects
        //could probably have done this by putting this script on the root and traversing down the chain
        Root = transform;
        for (var a = 0; a <= ChainLength; a++)
        {
            Root = Root.parent;
        }
        #endregion

        StartRotationTarget = Target.rotation;

        Transform current = transform;
        CompleteLength = 0;
        for (var a = Bones.Length - 1; a >= 0; a--)
        {
            Bones[a] = current;
            StartRotationBone[a] = current.rotation;

            if (a == Bones.Length - 1)
            {               
                BoneDirectionHelper[a] = Target.position - current.position;
            }
            else
            {
                BoneDirectionHelper[a] = Bones[a + 1].position - current.position;
                BonesLength[a] = BoneDirectionHelper[a].magnitude;
                CompleteLength += BonesLength[a];
            }
            current = current.parent;
        }
        #region Get the closest goal object
        List<GameObject> AllTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Goal"));
        float maxdist = Mathf.Infinity;
        foreach (GameObject g in AllTargets)
        {
            if (Vector3.Distance(this.transform.position, g.transform.position) < maxdist)
            {
                maxdist = Vector3.Distance(this.transform.position, g.transform.position);
                Target = g.transform;
            }
        }
        #endregion

    }
    // Update is called once per frame
    void LateUpdate()
    {
        //get position
        for (int i = 0; i < Bones.Length; i++)
        {
            Positions[i] = Bones[i].position;
        }
        #region can we actually reach the target?
        Vector3 targetPosition = Target.position;
        if(Vector3.Distance(targetPosition, Bones[0].position) >= CompleteLength)
        {
            Vector3 direction = (targetPosition - Positions[0]).normalized;
            for (int a = 1; a < Positions.Length; a++)
            {
                Positions[a] = Positions[a - 1] + direction * BonesLength[a - 1];
            }    
        }
        #endregion
        else
        {
                #region propagate backwards
            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                //backwards
                for (int a = Positions.Length - 1; a > 0; a--)
                {
                    if (a == Positions.Length - 1)
                    {
                        Positions[a] = targetPosition;
                    }
                    else
                    {
                        Positions[a] = Positions[a + 1] + (Positions[a] - Positions[a + 1]).normalized * BonesLength[a];
                    }
                }
                #endregion
                #region propagate forwards
                //forward
                for (int a = 1; a < Positions.Length; a++)
                {
                    Positions[a] = Positions[a - 1] + (Positions[a] - Positions[a - 1]).normalized * BonesLength[a - 1];
                }
                #endregion
                #region termination condition (are we there yet?)
                //termination 
                if (Vector3.Distance(Positions[Positions.Length - 1], targetPosition) < float.Epsilon)
                {
                    break;
                }
                #endregion

            }
        }
        #region Handle bending from poles.
        for (int a = 1; a < Positions.Length - 1; a++)
        {
            Plane plane = new Plane(Positions[a + 1] - Positions[a - 1], Positions[a - 1]);
            Vector3 projectedPole = plane.ClosestPointOnPlane(Pole.position);
            Vector3 projectedBone = plane.ClosestPointOnPlane(Positions[a]);
            
            float angle = Vector3.SignedAngle(projectedBone - Positions[a - 1], projectedPole - Positions[a - 1], plane.normal);
            Positions[a] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[a] - Positions[a - 1]) + Positions[a - 1];
        }
        #endregion
        #region assign the new values to the bones
        for (int a = 0; a < Positions.Length; a++)
        {
            if (a == Positions.Length - 1)
            {
                Bones[a].rotation = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[a];
            }
            else
            {
                Bones[a].rotation = Quaternion.FromToRotation(BoneDirectionHelper[a], Positions[a + 1] - Positions[a]) * StartRotationBone[a];
            }
            for(int b = 0; b < Positions.Length; b++)
            {

                Bones[b].position = Positions[b];
            }
        }
        #endregion
    }
}

