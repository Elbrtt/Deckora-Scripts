using UnityEngine;
using Vuforia;

public class CustomTracking : MonoBehaviour
{
    public GameObject object3D;
    public GameManager gameManager; 

    private float extendedTimer = 0f;
    private bool isExtendedTracking = false;

    public bool IsVisible { get; private set; } = false;

    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnStatusChanged;

        if (object3D != null)
        {
            var anim = object3D.GetComponentInChildren<Animator>();
            if (anim != null)
                anim.keepAnimatorStateOnDisable = true;
        }
    }

    void Update()
    {
        if (isExtendedTracking)
        {
            extendedTimer += Time.deltaTime;
            if (extendedTimer >= 0.5f)
            {
                if (IsVisible)
                {
                    IsVisible = false;
                    object3D.SetActive(false);
                    gameManager?.OnTrackerVisibilityChanged(); 
                }
            }
        }
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED)
        {
            object3D.SetActive(true);
            isExtendedTracking = false;
            extendedTimer = 0f;

            if (!IsVisible)
            {
                IsVisible = true;
                gameManager?.OnTrackerVisibilityChanged(); 
            }
        }
        else if (status.Status == Status.EXTENDED_TRACKED)
        {
            isExtendedTracking = true;
            extendedTimer = 0f;
        }
        else
        {
            isExtendedTracking = false;
            extendedTimer = 0f;

            if (IsVisible)
            {
                IsVisible = false;
                object3D.SetActive(false);
                gameManager?.OnTrackerVisibilityChanged();
            }
        }
    }
}