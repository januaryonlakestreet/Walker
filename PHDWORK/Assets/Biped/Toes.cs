using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toes : MonoBehaviour
{
    float ToePlane;
    // Start is called before the first frame update
    void Start()
    {
        ToePlane = this.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.localPosition.z > ToePlane)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                this.transform.localPosition.y,
                ToePlane);
        }
    }
}
