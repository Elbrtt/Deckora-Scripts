using UnityEngine;
using System.Collections;
using Vuforia;

public class HeroAttack : MonoBehaviour
{
    [Header("── Stats (diisi dari HeroData) ──")]
    public float damage = 3f;
    public float attackPerSecond = 1f;

    private TyrantController _tyrant;
    private bool _isActive = false;

    private float _damageMultiplier = 1f;

    public void SetDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
        Debug.Log($"[HeroAttack] Damage multiplier: {_damageMultiplier}x");
    }

    public void Init(HeroData data, TyrantController tyrant)
    {
        damage          = data.damage;
        attackPerSecond = data.attackPerSecond;
        _tyrant         = tyrant;
    }

    public void StartAttacking()
    {
        _isActive = true;
        StartCoroutine(AttackLoop());
    }

    public void StopAttacking()
    {
        _isActive = false;
        StopAllCoroutines();
    }

    IEnumerator AttackLoop()
    {
        while (_isActive)
        {
            yield return new WaitForSeconds(1f / attackPerSecond);
            if (_tyrant != null && !_tyrant.IsDead && !GameManager.IsPaused)
                _tyrant.TakeDamage(damage * _damageMultiplier); 
                Debug.Log($"dmg{damage} + dmp{_damageMultiplier}");
        }
    }
}