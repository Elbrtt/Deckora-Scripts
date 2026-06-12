using UnityEngine;
using Vuforia;
using System.Collections;

public class VuforiaMng : MonoBehaviour
{

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        VuforiaApplication.Instance.Initialize();
    }

    void Update()
    {
        
    }
}