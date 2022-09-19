using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HipPosition : MonoBehaviour
{

    // Start is called before the first frame update

   

   
    public Vector3 HipGroundPosition, DesiredHipPosition;
    public Transform lfoot, rfoot;
    public float tspeed;
    private void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            HipGroundPosition = hit.point;
           
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        DesiredHipPosition = new Vector3(
           (lfoot.position.x + rfoot.position.x) / 2,
           this.transform.position.y,
            (lfoot.position.z + rfoot.position.z) / 2
           );

        this.gameObject.transform.position = DesiredHipPosition;



    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(DesiredHipPosition, 0.1f);
    }
    public Vector3 GetHipGroundPosition()
    {
        return HipGroundPosition;
    }
}
