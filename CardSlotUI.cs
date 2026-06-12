using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSlotUI : MonoBehaviour
{
    [Header("── UI ──")]
    public Image heroImage;
    public TextMeshProUGUI heroName;
    public Image healthFill; 
    public bool IsDead => _currentHP <= 0f; 

    [Header("── VFX Shield Setup (Direct Hierarchy) ──")]
    [SerializeField] private GameObject shieldVFXObject; 

    [Header("── VFX Heal Setup (Direct Hierarchy) ──")]
    [SerializeField] private GameObject healVFXObject;

    private ParticleSystem _shieldParticle;
    private ParticleSystem _healParticle;
    private float _maxHP = 100f;
    private float _currentHP;
    private float _shield = 0f; 
    public float MaxHP => _maxHP; 

    void Start()
    {
        _currentHP = _maxHP;
        RefreshHP();
        
    }

    public void SetData(HeroData data)
    {
        if (heroName  != null) heroName.text = data.heroName;
        if (heroImage != null && data.heroPortrait != null)
            heroImage.sprite = data.heroPortrait;

        _maxHP     = data.maxHP;
        _currentHP = _maxHP;
        
        _shield = 0f;
        UpdateShieldVisual(); 
        
        RefreshHP();
    }

    public void AddShield(float percent)
    {
        if (IsDead) return;

        _shield += _maxHP * percent;
        Debug.Log($"[CardSlot] {heroName?.text} dapat shield {_maxHP * percent} | Total shield: {_shield}");
        
        UpdateShieldVisual(); 
    }

    public void TakeDamage(float amount)
    {
        if (_currentHP <= 0) return;

        if (_shield > 0)
        {
            float absorbed = Mathf.Min(_shield, amount);
            _shield -= absorbed;
            amount  -= absorbed;
            Debug.Log($"[CardSlot] Shield absorb {absorbed} | Shield sisa: {_shield}");
        }

        UpdateShieldVisual(); 

        if (amount <= 0) return;
        _currentHP = Mathf.Max(0f, _currentHP - amount);
        RefreshHP();
    }

    public void Heal(float amount)
    {
        if (_currentHP <= 0) return;
        _currentHP = Mathf.Min(_maxHP, _currentHP + amount);
        RefreshHP();
        PlayHealVisual();
    }

    void RefreshHP()
    {
        if (healthFill != null)
            healthFill.fillAmount = _currentHP / _maxHP;
    }
    private void PlayHealVisual()
    {
        if (healVFXObject == null) return;

        if (_healParticle == null)
        {
            _healParticle = healVFXObject.GetComponent<ParticleSystem>();
        }

        if (_healParticle == null) return;

        if (!healVFXObject.activeSelf)
        {
            healVFXObject.SetActive(true);
        }
        _healParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _healParticle.Play(true);
        
        Debug.Log($"[AR VFX] Heal Triggered untuk {heroName?.text}");
    }

    private void UpdateShieldVisual()
    {
        if (shieldVFXObject == null) return;

        if (_shieldParticle == null)
        {
            _shieldParticle = shieldVFXObject.GetComponent<ParticleSystem>();
        }

        if (_shieldParticle == null) return;

        if (_shield > 0 && !IsDead)
        {
            if (!shieldVFXObject.activeSelf)
            {
                shieldVFXObject.SetActive(true);
            }

            if (!_shieldParticle.isPlaying)
            {
                _shieldParticle.Play(true);
                Debug.Log($"[AR VFX] Shield Diaktifkan & Play: {heroName?.text}");
            }
        }
        else
        {
            if (_shieldParticle.isPlaying)
            {
                _shieldParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                StartCoroutine(DisableVFXAfterSeconds(_shieldParticle.main.duration));
                Debug.Log($"[AR VFX] Shield Dihentikan (Fade Out): {heroName?.text}");
            }
            else if (shieldVFXObject.activeSelf && !_shieldParticle.IsAlive())
            {
                shieldVFXObject.SetActive(false);
            }
        }
    }

    private System.Collections.IEnumerator DisableVFXAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (_shield <= 0 || IsDead)
        {
            if (shieldVFXObject != null && !_shieldParticle.isPlaying)
            {
                shieldVFXObject.SetActive(false);
                Debug.Log($"[AR VFX] Object SetActive(false) total untuk: {heroName?.text}");
            }
        }
    }
}