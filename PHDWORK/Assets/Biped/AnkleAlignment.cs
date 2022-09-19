using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkleAlignment : MonoBehaviour
{
    public Transform ToeBase;
    Vector3 offset;
    private void Start()
    {
        offset = this.transform.localPosition - ToeBase.transform.localPosition;
    }
    private void Update()
    {
        this.transform.localPosition = ToeBase.localPosition + offset;

    }
}
