using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
    [Header("── Referensi ──")]
    public TyrantController tyrant;
    public CardSlotUI[] heroSlots;

    [Header("── Cooldown Panels ──")]
    public Image cdPanel1;
    public Image cdPanel2;
    public Image cdPanel3;

    private HeroData[] _heroData = new HeroData[3];                                                         
    private bool[] _onCooldown = new bool[3];
    public Material glowMaterial;
    public Renderer[] archerModel;

    public void InitAbility(int slotIndex, HeroData data)
    {

        if (slotIndex >= 3) return;
        _heroData[slotIndex]   = data;
        _onCooldown[slotIndex] = false;
        GetPanel(slotIndex).fillAmount = 0f;

        Debug.Log($"[Ability] Slot {slotIndex+1} = {data.heroName} | Ability: {data.heroAbility} | CD: {data.abilityCooldown}s");
    }

    public void TriggerAbility0() => TriggerAbility(0);
    public void TriggerAbility1() => TriggerAbility(1);
    public void TriggerAbility2() => TriggerAbility(2);
    public void TriggerAbility(int slotIndex)
    {
        if (GameManager.IsPaused) return;
        if (_onCooldown[slotIndex]) return;
        if (_heroData[slotIndex] == null) return;

        UseAbility(slotIndex, _heroData[slotIndex]);
        StartCoroutine(RunCooldown(slotIndex, _heroData[slotIndex].abilityCooldown));
    }

    void UseAbility(int index, HeroData data)
    {
        switch (data.heroAbility)
        {
            case 1: 
                var heroAttack = heroSlots[index].GetComponent<HeroAttack>();
                if (heroAttack != null){
                    StartCoroutine(EnhanceAttack(heroAttack, 3.5f, 5f));
                } 
                Debug.Log($"[Ability] {data.heroName}: Enhanced Attack 1.5x!");
                break;

            case 2: 
                foreach (var slot in heroSlots)
                    if (slot != null) slot.AddShield(0.1f); 
                Debug.Log($"[Ability] {data.heroName}: Shield 10% ke semua hero!");
                break;

            case 3:
                foreach (var slot in heroSlots)
                    if (slot != null) slot.Heal(slot.MaxHP * 0.15f); 
                Debug.Log($"[Ability] {data.heroName}: Heal 15% ke semua hero!");
                break;

            default:
                Debug.LogWarning($"[Ability] {data.heroName}: heroAbility={data.heroAbility} tidak dikenal");
                break;
        }
    }


    IEnumerator EnhanceAttack(HeroAttack heroAttack, float multiplier, float duration)
    {
        Debug.Log("Enhance Attack Active");

        if (archerModel == null || archerModel.Length == 0)
        {
            Debug.LogError("archerModel BELUM diassign!");
            yield break;
        }

        heroAttack.SetDamageMultiplier(multiplier);

        Color emissionColor = new Color32(236, 176, 14, 255);
        Color finalGlow = emissionColor * 1f;

        for (int i = 0; i < archerModel.Length; i++)
        {
            if (archerModel[i] == null)
            {
                Debug.LogError("Renderer NULL di index: " + i);
                continue;
            }

            Debug.Log("Renderer ketemu: " + archerModel[i].name);

            Material mat = archerModel[i].material;

            if (mat == null)
            {
                Debug.LogError("Material NULL di renderer: " + archerModel[i].name);
                continue;
            }

            Debug.Log("Material ketemu: " + mat.name);

            mat.EnableKeyword("_EMISSION");

            mat.SetColor("_EmissionColor", finalGlow);

            Debug.Log("Emission Applied ke: " + archerModel[i].name);
        }

        float timer = 0f;

        while (timer < duration)
        {
            if (!GameManager.IsPaused)
                timer += Time.deltaTime;

            yield return null;
        }

        heroAttack.SetDamageMultiplier(1f);

        for (int i = 0; i < archerModel.Length; i++)
        {
            if (archerModel[i] == null) continue;

            Material mat = archerModel[i].material;

            if (mat == null) continue;

            mat.SetColor("_EmissionColor", Color.black);

            Debug.Log("Emission Reset: " + archerModel[i].name);
        }
    }
    IEnumerator RunCooldown(int index, float cd)
    {
        _onCooldown[index] = true;
        Image panel = GetPanel(index);

        float timer = 0f;
        panel.fillAmount = 1f;

        while (timer < cd)
        {
            if (!GameManager.IsPaused)
                timer += Time.deltaTime;
            panel.fillAmount = timer / cd;
            yield return null;
        }

        panel.fillAmount   = 0f;
        _onCooldown[index] = false;
    }


    Image GetPanel(int index) => index switch
    {
        0 => cdPanel1,
        1 => cdPanel2,
        _ => cdPanel3
    };
}