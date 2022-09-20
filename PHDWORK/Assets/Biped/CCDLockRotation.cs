using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDLockRotation : MonoBehaviour
{
    Vector3 StartEular;
    // Start is called before the first frame update
    void Start()
    {
        StartEular = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localEulerAngles = StartEular;
    }
}
