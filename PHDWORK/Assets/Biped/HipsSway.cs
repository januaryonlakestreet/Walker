using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipsSway : MonoBehaviour
{
    public float rotspeed, rotamount;
    public float Duration;
    public void StartRotation()
    {
    
        Biped bped = FindObjectOfType<Biped>();
        this.StopAllCoroutines();
        if(bped.LegID == 0)
        {
            StartCoroutine(rotateToR());
        }
        if(bped.LegID == 1)
        {
            StartCoroutine(rotateToL());
        }


    }

    IEnumerator rotateToL()
    {
        Vector3 NewRot = this.transform.localEulerAngles;
        NewRot.y -= rotamount;
        while(Duration < 1f)
        {
            this.transform.localEulerAngles = Vector3.Lerp(this.transform.localEulerAngles, NewRot, Time.deltaTime * rotspeed);

        Duration += Time.deltaTime * rotspeed;
        }
        Duration = 0F;
        yield return null;
    }
    IEnumerator rotateToR()
    {
        Vector3 NewRot = this.transform.localEulerAngles;
        NewRot.y += rotamount;
        while (Duration < 1f)
        {
            this.transform.localEulerAngles = Vector3.Lerp(this.transform.localEulerAngles, NewRot, Time.deltaTime * rotspeed);

            Duration += Time.deltaTime * rotspeed;
        }

        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
