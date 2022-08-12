using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    //click to move
    public GameObject Marker;
    private GameObject _marker;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(_marker)
                {
                    _marker.transform.position = hit.point;
                }
                else
                {
                    _marker = Instantiate(Marker, hit.point, Quaternion.identity);
                }
              
            }
        }
    }
}
