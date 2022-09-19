using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegOrientation : MonoBehaviour
{
    public Transform Goal;
    Vector3 Dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Goal = FindObjectOfType<Biped>().BipedGoal;
        Dir = this.transform.forward;
        Quaternion dirrot = Quaternion.FromToRotation((Dir - Goal.transform.position), Vector3.up);
        Debug.DrawRay(this.transform.position, dirrot.eulerAngles, Color.red, Time.deltaTime);
        Debug.DrawRay(this.transform.position, Dir, Color.green, Time.deltaTime);

        dirrot.y = dirrot.y * -1f;
        this.transform.rotation = dirrot;
        this.transform.GetChild(0).transform.rotation = dirrot;
    }
}
