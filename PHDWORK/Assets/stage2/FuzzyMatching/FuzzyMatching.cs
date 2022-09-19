using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

public class FuzzyMatching : MonoBehaviour
{
    public float RefreshRate;
    [SerializeField]
    private float _RefreshRate;
    List<Transform> ImpactingActors = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        _RefreshRate = RefreshRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(_RefreshRate <= 0)
        {
            _RefreshRate = RefreshRate;
        }
        else
        {
            _RefreshRate -= Time.deltaTime;
            SnapShot();
        }
    }
    void SnapShot()
    { 
        ImpactingActors.Clear();
        List<joint> Points = new List<joint>(FindObjectsOfType<joint>());
        foreach(joint j in Points)                          
        {
            Collider[] hitColliders = Physics.OverlapSphere(j.transform.position, 0.1f);                                
            foreach(Collider c in hitColliders)
            {
                ImpactingActors.Add(c.transform);
            }
        }
        PrepareSnapshot();
    }

    public void PrepareSnapshot()
    {
        string ConstructSituationDescription = "";
        foreach(Transform t in ImpactingActors)
        {
            ConstructSituationDescription += t.transform.position.ToString();
        }
        StartCoroutine(SendSnapshot(ConstructHash(ConstructSituationDescription)));
    }
    public static string ConstructHash(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    IEnumerator SendSnapshot(string Situation)
    {
       
        string uri = "http://localhost:5000/" + Situation;
        print(uri);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    print(webRequest.downloadHandler.text);
                    break;
            }
            
        }
    }
}
