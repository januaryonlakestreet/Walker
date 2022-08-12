using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//I didn't like how much code fabrik and ccd shared and have decided to combine them with a base class.
//also means I can tidy it up.
//mostly this class contains chain set up.

public class Solver : MonoBehaviour
{
    public int ChainLength = 2;
    public Transform Target;
    public Transform Pole;
    public int Iterations = 10;
    float[] BonesLength;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }
}
