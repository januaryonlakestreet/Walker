using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ArmSwing : MonoBehaviour
{
  
    public float timepassed;
    public int direction;
    bool go = true;
    public Transform Target;
   
    public Transform ForwardObj, BackObj;
    public string Side;
   
    // Start is called before the first frame update
    public void StopSwing()
    {
        go = false;
    }
    public void startswing()
    {
      go = true;
    }
    public void switchdirections()
    {
        timepassed = 0f;
        if (direction == 0)
        {
            direction = 1;
           
            return;
        }
        if (direction == 1)
        {
         
            direction = 0;
            return;
        }
    }
    void Start()
    {
        if(Side == "Left")
        {
            direction = 0;
        }
        else
        {
            direction = 1;
        }



        if(direction == 0)
        { timepassed = 1f; }

      
       
    }

    // Update is called once per frame
    void Update()
    {
       

        if (!go) { return; }
        timepassed += 1f*Time.deltaTime;
        if (direction == 1)
        {
            this.transform.localPosition = Vector3.Lerp(BackObj.localPosition, ForwardObj.localPosition, timepassed);
        }
        if (direction == 0)
        {
            this.transform.localPosition = Vector3.Lerp(ForwardObj.localPosition, BackObj.localPosition, timepassed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
    }
}
