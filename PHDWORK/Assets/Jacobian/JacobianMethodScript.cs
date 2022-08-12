//avoiding the inverse :using the jacobian pg 183
//based off the simple example from the book 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobianMethodScript : MonoBehaviour
{
   
    public Transform[] Chain;


    public Transform Goal; // The Goal position

    public float[] AnglesX; 
    public float[] AnglesY; 
    public float[] AnglesZ; 
    public Vector4[] AnglesVector; 
    public Vector4 GoalGoalVector; 
    public Vector3 GoalGoal; 

    public Matrix4x4 JacobianX, JacobianY, JacobianZ;

    void Start()
    {
        AnglesVector = new Vector4[4];
        AnglesX = new float[Chain.Length * 3];
        AnglesY = new float[Chain.Length * 3];
        AnglesZ = new float[Chain.Length * 3];
    }
    
    void Update()
    {
        GoalGoal = Goal.position - Chain[Chain.Length - 1].position;
        GoalGoalVector = new Vector4(GoalGoal.x, GoalGoal.y, GoalGoal.z, 1.0f);

        AnglesVector[0] = CalculateJacobianTranspose(MakeJacobianX());
        AnglesVector[1] = CalculateJacobianTranspose(MakeJacobianY());
        AnglesVector[2] = CalculateJacobianTranspose(MakeJacobianZ());

        for (int i = 0; i < Chain.Length - 1; i++)
        {
            AnglesX[i] += AnglesVector[0][i];
            AnglesY[i] += AnglesVector[1][i];
            AnglesZ[i] += AnglesVector[2][i];
            Chain[i].localEulerAngles = new Vector3(AnglesX[i], AnglesY[i], AnglesZ[i]);
        }
    }
    Vector4[] CalculateJacBetter()
    {
        return new Vector4[4];
    }
    Vector4 CalculateJacobianTranspose(Matrix4x4 JacobianIn)
    {
        //angle = JacobianTranspose * 
        return JacobianIn.transpose * GoalGoalVector;
    }
    

    Matrix4x4 MakeJacobianX()
    {
       Vector3[] VectorJointX; 
       VectorJointX = new Vector3[Chain.Length];

        for (int i = 0; i < Chain.Length - 1; i++)
        {
            VectorJointX[i] = Vector3.Cross(Chain[i].right,
                (Chain[Chain.Length - 1].position - Chain[i].position));
        }
        for (int i = 0; i < Chain.Length - 1; i++)
        {
            JacobianX.SetColumn(i, new Vector4(VectorJointX[i].x, VectorJointX[i].y, VectorJointX[i].z, 0.0f));
        }
        JacobianX.SetColumn(Chain.Length - 1, new Vector4(0.0F, 0.0f, 0.0f, 1.0f));
        return JacobianX;
    }
    Matrix4x4 MakeJacobianY()
    {
        Vector3[] VectorJointY;
        VectorJointY = new Vector3[Chain.Length];

     
        for (int i = 0; i < Chain.Length - 1; i++)
        {
            VectorJointY[i] = Vector3.Cross(Chain[i].up,
                (Chain[Chain.Length - 1].position - Chain[i].position));
        }
        for (int i = 0; i < Chain.Length - 1; i++)
        {
            JacobianY.SetColumn(i, new Vector4(VectorJointY[i].x, VectorJointY[i].y, VectorJointY[i].z, 0.0f));
        }
        JacobianY.SetColumn(Chain.Length - 1, new Vector4(0.0F, 0.0f, 0.0f, 1.0f));
        return JacobianY;
    }
    Matrix4x4 MakeJacobianZ()
    {
        Vector3[] VectorJointZ;
        VectorJointZ = new Vector3[Chain.Length];

      
        for (int i = 0; i < Chain.Length - 1; i++)
        {
            VectorJointZ[i] = Vector3.Cross(Chain[i].forward,
                (Chain[Chain.Length - 1].position - Chain[i].position));
        }
        for (int i = 0; i < Chain.Length - 1; i++)
        {
            JacobianZ.SetColumn(i, new Vector4(VectorJointZ[i].x, VectorJointZ[i].y, VectorJointZ[i].z, 0.0f));
        }
        JacobianZ.SetColumn(Chain.Length - 1, new Vector4(0.0F, 0.0f, 0.0f, 1.0f));
        return JacobianZ;
    }
 

   

}
