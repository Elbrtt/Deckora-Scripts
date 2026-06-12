using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class ArenaAnchor: MonoBehaviour
{
    public ObserverBehaviour[] markers;
    public Transform arenaPlane;

    public float planeOffsetDown = 0f;
    public float smoothSpeed = 5f;

    [Tooltip("Offset rotasi Y")]
    public float rotationOffset = 0f;

    private bool rotationInitialized = false;
    private Quaternion lockedRotation;

    void Update()
    {
        List<Transform> trackedMarkers =
            new List<Transform>();

        foreach (ObserverBehaviour marker in markers)
        {
            if (marker != null &&
                marker.TargetStatus.Status == Status.TRACKED)
            {
                trackedMarkers.Add(marker.transform);
            }
        }

        if (trackedMarkers.Count == 0)
            return;


        Vector3 center = Vector3.zero;

        foreach (Transform t in trackedMarkers)
        {
            center += t.position;
        }

        center /= trackedMarkers.Count;

        Vector3 markerUp = trackedMarkers[0].up;

        Vector3 targetPos =
            center +
            markerUp * planeOffsetDown;

        arenaPlane.position = Vector3.Lerp(
            arenaPlane.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );


        if (!rotationInitialized)
        {
            Vector3 flatForward =
                trackedMarkers[0].forward;

            flatForward.y = 0f;

            if (flatForward == Vector3.zero)
                flatForward = Vector3.forward;

            flatForward.Normalize();

            Quaternion baseRot =
                Quaternion.LookRotation(
                    flatForward,
                    Vector3.up
                );

            lockedRotation =
                baseRot *
                Quaternion.Euler(
                    0f,
                    rotationOffset,
                    0f
                );

            rotationInitialized = true;
        }

        arenaPlane.rotation = Quaternion.Slerp(
            arenaPlane.rotation,
            lockedRotation,
            Time.deltaTime * smoothSpeed
        );
    }
}