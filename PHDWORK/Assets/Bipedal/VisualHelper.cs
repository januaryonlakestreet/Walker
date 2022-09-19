using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// displays a sphere when selected to help visualise what is happening.
/// </summary>

public class VisualHelper : MonoBehaviour
{
    public bool Hide;
    public Color HelperColour;
    public float HelperSize;
    private void OnDrawGizmos()
    {
        if (!Hide)
        {
            HelperColour.a = 1f;
            Gizmos.color = HelperColour;

            Gizmos.DrawSphere(this.transform.position, HelperSize);
        }
       
    }
}
