using UnityEngine;

public class HeroData : MonoBehaviour
{
    public string heroName = "Archer";
    public Sprite heroPortrait;
    public float maxHP = 100f;
    public float damage = 3f;   
    public float attackPerSecond = 1f;
    public int heroAbility = 0;
    public int abilityCooldown = 0;
}