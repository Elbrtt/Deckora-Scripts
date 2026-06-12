using UnityEngine;

public class Testing_Code : MonoBehaviour
{

    public GameObject uiScanning;
    public GameObject uiDone;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void moveUI(){
            uiScanning.SetActive(false);
            uiDone.SetActive(true);
    }
}
