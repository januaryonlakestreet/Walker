using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float SHeight;
    // Update is called once per frame
    void Update()
    {
        Vector3 goal = new Vector3(this.transform.position.x, Mathf.Sin(Mathf.PI * Time.time) * SHeight, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, goal, Time.deltaTime);
    }
}
