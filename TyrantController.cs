using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TyrantController : MonoBehaviour
{
    [Header("── Health ──")]
    public float maxHP = 1000f;
    public Image healthFill;

    [Header("── Attack ke Hero ──")]
    public float damage = 3f;
    public float attackInterval = 2f;
    public CardSlotUI[] heroSlots;

    [Header("── Damage Flash ──")]
    public Renderer[] tyrantRenderers;

    private Color[][] originalColors;

    private float _currentHP;
    private bool _isActive = false;

    public bool IsDead => _currentHP <= 0f;

    void Start()
    {
        _currentHP = maxHP;
        RefreshHP();

        originalColors = new Color[tyrantRenderers.Length][];

        for (int i = 0; i < tyrantRenderers.Length; i++)
        {
            Material[] mats = tyrantRenderers[i].materials;
            originalColors[i] = new Color[mats.Length];

            for (int j = 0; j < mats.Length; j++)
            {
                originalColors[i][j] = mats[j].color;
            }
        }
    }

    public void StartBattle()
    {
        _isActive = true;
        StartCoroutine(AttackLoop());
    }

    public void StopBattle()
    {
        _isActive = false;
        StopAllCoroutines();
    }

    public void TakeDamage(float amount)
    {
        if (_currentHP <= 0f) return;

        _currentHP = Mathf.Max(0f, _currentHP - amount);

        RefreshHP();

        StartCoroutine(DamageFlash());

        Debug.Log($"[Tyrant] HP: {_currentHP}/{maxHP}");
    }

    void RefreshHP()
    {
        if (healthFill != null)
            healthFill.fillAmount = _currentHP / maxHP;
    }

    IEnumerator DamageFlash()
    {
        foreach (var rend in tyrantRenderers)
        {
            foreach (var mat in rend.materials)
            {
                mat.color = Color.red;
            }
        }

        yield return new WaitForSeconds(0.15f);


        for (int i = 0; i < tyrantRenderers.Length; i++)
        {
            Material[] mats = tyrantRenderers[i].materials;

            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = originalColors[i][j];
            }
        }
    }

    IEnumerator AttackLoop()
    {
        while (_isActive)
        {
            yield return new WaitForSeconds(attackInterval);

            foreach (var slot in heroSlots)
            {
                if (slot != null)
                    slot.TakeDamage(damage);
            }
        }
    }
}