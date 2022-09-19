using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabrikBipedSolver : MonoBehaviour
{
    public int ChainLength = 2;
    public Transform Target;
    public Transform Pole;
    public int Iterations = 10;



    float[] BonesLength;
    float CompleteLength;
    public Transform[] Bones;
    Vector3[] Positions;
    Vector3[] BoneDirectionHelper;
    Quaternion[] StartRotationBone;
    Quaternion StartRotationTarget;
    Transform Root;
    

    // Start is called before the first frame update
    void Awake()
    {
        BoneDirectionHelper = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];

        Root = transform;
        for (var a = 0; a <= ChainLength; a++)
        {
            Root = Root.parent;
        }
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
        if (Vector3.Distance(targetPosition, Bones[0].position) >= CompleteLength)
        {
            Vector3 direction = (targetPosition - Positions[0]).normalized;
            for (int i = 1; i < Positions.Length; i++)
            {
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
            }
        }
        #endregion
        else
        {
            #region propagate backwards
            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                //backwards
                for (int i = Positions.Length - 1; i > 0; i--)
                {
                    if (i == Positions.Length - 1)
                    {
                        Positions[i] = targetPosition;
                    }
                    else
                    {
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i];
                    }
                }
                #endregion
                #region propagate forwards
                //forward
                for (int i = 1; i < Positions.Length; i++)
                {
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];
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
        #region Handle poles
        if (Pole)
        {
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                Plane plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(Pole.position);
                Vector3 projectedBone = plane.ClosestPointOnPlane(Positions[i]);

                float angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
            }
        }

        #endregion
        #region assign the new values to the bones
        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == Positions.Length - 1)
            {
                Quaternion EndRot = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];

                Bones[i].rotation = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];
            }
            else
            {
                Quaternion newrot = Quaternion.FromToRotation(BoneDirectionHelper[i], Positions[i + 1] - Positions[i]) * StartRotationBone[i];
                Bones[i].rotation = newrot;
            }
            for (int a = 0; a < Positions.Length; a++)
            {
                Bones[a].position = Positions[a];
            }

        }
        #endregion
    }

    private void OnDrawGizmos()
    {
      
    }
}
