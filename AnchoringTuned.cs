using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class AnchoringTuned : MonoBehaviour
{
    public ObserverBehaviour[] markers;
    public Transform arenaPlane;
    public float planeOffsetDown = 0.0f;
    public float smoothSpeed = 5f;

    [Tooltip("Offset rotasi Y arena dalam derajat, sesuaikan di Inspector")]
    public float rotationOffset = 0f;

    void Update()
    {
        List<Transform> trackedMarkers = new List<Transform>();

        foreach (ObserverBehaviour marker in markers)
        {
            if (marker != null && marker.TargetStatus.Status == Status.TRACKED)
                trackedMarkers.Add(marker.transform);
        }

        if (trackedMarkers.Count == 0) return;

    
        Vector3 center = Vector3.zero;
        foreach (Transform t in trackedMarkers)
            center += t.position;
        center /= trackedMarkers.Count;

        Vector3 markerUp = trackedMarkers[0].up;
        Vector3 targetPos = center + markerUp * planeOffsetDown;

        Vector3 markerRight = trackedMarkers[0].right;
        markerRight.y = 0f; 
        if (markerRight == Vector3.zero) markerRight = Vector3.forward;
        markerRight.Normalize();

  
        Quaternion baseRot = Quaternion.LookRotation(markerRight, Vector3.up);
        Quaternion targetRot = baseRot * Quaternion.Euler(0f, rotationOffset, 0f);

    
        arenaPlane.position = Vector3.Lerp(
            arenaPlane.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );

        arenaPlane.rotation = Quaternion.Slerp(
            arenaPlane.rotation,
            targetRot,
            Time.deltaTime * smoothSpeed
        );
    }
}