using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleHelper : MonoBehaviour
{
    //makes sure the pole maintains it's location relative to the target object depending on 
    //where the goal location takes it.
    // Start is called before the first frame update

    public Transform target;
    private Vector3 offsetTarget;
    private Vector3 startlocation;
    private Vector3 pos, fw, up;
    public void Reset()
    {
        this.transform.position = startlocation;
    }
    void Start()
    {
        startlocation = this.transform.position;
        offsetTarget = this.transform.position - target.transform.position;
        pos = target.transform.InverseTransformPoint(transform.position);
        fw = target.transform.InverseTransformDirection(transform.forward);
        up = target.transform.InverseTransformDirection(transform.up);
    }

    // Update is called once per frame
    void Update()
    {
      
        var newpos = target.transform.TransformPoint(pos);
        var newfw = target.transform.TransformDirection(fw);
        var newup = target.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
      
        transform.rotation = newrot;
        transform.position = offsetTarget + newpos;
    }
}
