using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBiped : MonoBehaviour
{
    public List<Vector3> StepPositions = new List<Vector3>();
    public float startTime;

    public float duration = 0f;
    public Vector3 startlocation;
    Vector3 stoplocation;
    public float Speed;
    bool Takingstep;
    public float stepheight;
    public void NewStep(List<Vector3> Positions,Vector3 end)
    {
       
        StepPositions.Clear();
        StepPositions = Positions;
        startTime = Time.time;
        stoplocation = end;
        StartCoroutine(AnimateStep());

    }
    public void Reset()
    {
        print("reseting " + this.name);
        this.transform.position = startlocation;
    }
    // Start is called before the first frame update
    void Start()
    {
        startlocation = this.transform.position;
    }
    public float testvla;
    IEnumerator AnimateStep()
    {
        for(int a =0; a < StepPositions.Count; a++)
        {
            float timer = 0.0f;
            while (timer < 1f)
            {

                timer += Time.deltaTime * 0.157f;
                this.transform.position = Vector3.Lerp(this.transform.position, StepPositions[0], timer);
                if(Vector3.Distance(this.transform.position, StepPositions[0]) < 0.1f)
                {
                    StepPositions.RemoveAt(0);
                }
                if (Vector3.Distance(this.transform.position, stoplocation) < 0.1f)
                {
                    timer = 1f;
                    StepPositions.Clear();
                    yield return null;
                }
              yield return new WaitForSeconds(0.001f);
            }
        }
       
        yield return null;
    }
   
  
    
}
