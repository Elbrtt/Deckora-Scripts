using UnityEngine;
using System.Collections;

public class Anim_Manager : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject targetObjec2;
    public Vector3 targetRotation;
    public Vector3 targetRotation2;
    public Animator anim_arch;
    public Animator anim_demon;
    public Animator anim_druid;
    public Animator anim_tyrant;
    public float minInterval = 1f;
    public float maxInterval = 3f;

    void Start()
    {
        SetRotation(targetObject, targetRotation);
        SetRotation(targetObjec2, targetRotation2);
        anim_tyrant.applyRootMotion = false;
    }

    public void SetRotation(GameObject obj, Vector3 rotation)
    {
        obj.transform.rotation = Quaternion.Euler(rotation);
    }

    public void OnSuccess()
    {
        anim_arch.SetBool("attack", true);
        anim_demon.SetBool("attack", true);
        anim_druid.SetBool("attack", true);
        StartCoroutine(RandomAttackLoop());
    }

    IEnumerator RandomAttackLoop()
    {
        while (true)
        {
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);
            if (anim_tyrant == null) yield break;

            int pick = Random.Range(0, 2);
            string stateName = pick == 0 ? "Zombie Scream" : "Zombie Attack";
            anim_tyrant.Play(stateName);
            Debug.Log("[Tyrant] Play: " + stateName);
        }
    }

    public void TyrantDie()
    {
        StopAllCoroutines();
        anim_tyrant.applyRootMotion = true;
        if (anim_tyrant != null)
            anim_tyrant.SetTrigger("Death");
        Debug.Log("[Anim_Manager] Tyrant mati!");
    }


    public void PauseAllAnimations()
    {
        SetAnimatorSpeed(0f);
        Debug.Log("[Anim_Manager] Animasi di-pause");
    }

    public void ResumeAllAnimations()
    {
        SetAnimatorSpeed(1f);
        Debug.Log("[Anim_Manager] Animasi di-resume");
    }

    void SetAnimatorSpeed(float speed)
    {
        if (anim_arch   != null) anim_arch.speed   = speed;
        if (anim_demon  != null) anim_demon.speed  = speed;
        if (anim_druid  != null) anim_druid.speed  = speed;
        if (anim_tyrant != null) anim_tyrant.speed = speed;
    }
}