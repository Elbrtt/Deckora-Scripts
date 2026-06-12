using UnityEngine;

public class BossPositioner : MonoBehaviour
{
    [Header("── Referensi ──")]
    public Transform arCamera;    

    [Header("── Posisi ──")]
    public float distanceFromCamera = 1.5f; 
    public float heightOffset = -0.3f;       

    [Header("── Smoothing ──")]
    public float moveSpeed = 5f;     

    private bool _isActive = false;

    void Update()
    {
        if (!_isActive || arCamera == null) return;


        Vector3 forward = arCamera.forward;
        forward.y = 0f; 
        forward.Normalize();

        Vector3 targetPos = arCamera.position 
                          + forward * distanceFromCamera
                          + Vector3.up * heightOffset;

        transform.position = Vector3.Lerp(
            transform.position, 
            targetPos, 
            Time.deltaTime * moveSpeed
        );

        Vector3 lookDir = arCamera.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Activate()
    {
        _isActive = true;
        gameObject.SetActive(true);
        Debug.Log("[BossPositioner] Boss aktif");
    }

    public void Deactivate()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }
}