using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotion : MonoBehaviour
{
    public const float bodyheight = 0.017f;
    public float Speed;
    // Update is called once per frame
    void Update()
    {

        if(GameObject.Find("Marker(Clone)"))
        {
            if(Vector3.Distance(GameObject.Find("Marker(Clone)").transform.position,this.transform.position) < 1f)
            {
                return;
            }
            Vector3 Point = new Vector3(GameObject.Find("Marker(Clone)").transform.position.x, bodyheight, GameObject.Find("Marker(Clone)").transform.position.z);
            Quaternion RotToMarker = Quaternion.LookRotation(Point - this.transform.position, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotToMarker, Time.deltaTime);
            this.transform.position += transform.forward * Speed * Time.deltaTime;
           
           
        }
    }
}
