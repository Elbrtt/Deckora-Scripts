using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScanManager : MonoBehaviour
{
    public GameObject uiScanning;
    public GameObject uiDone;
    public TextMeshProUGUI progressText;
    public Image loading;
    public RectTransform panel; 
    public Anim_Manager anim_manager;
    public TyrantController tyrant; 
    public GameManager gameManager;
    public CardSlotUI[] cardSlots;
    public AbilityManager abilityManager; 

    private int count = 0;
    public int targetTotal = 3;
    private List<TargetHandler> _detectedHandlers = new List<TargetHandler>();

    void Start()
    {
        uiDone.SetActive(false);
        loading.enabled = false;
    }

    public void OnTargetFound(TargetHandler handler)
    {
        if (_detectedHandlers.Contains(handler)) return;
        _detectedHandlers.Add(handler);
        count++;

        loading.enabled = true;
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, 500f);
        progressText.text = "Scanning... (" + count + "/" + targetTotal + ")";

        if (count >= targetTotal)
        {

            _detectedHandlers.Sort((a, b) =>
            {
                Vector3 screenA = Camera.main.WorldToScreenPoint(a.transform.position);
                Vector3 screenB = Camera.main.WorldToScreenPoint(b.transform.position);
                return screenA.x.CompareTo(screenB.x);
            });

            for (int i = 0; i < _detectedHandlers.Count; i++)
            {
                var heroData = _detectedHandlers[i].GetComponent<HeroData>();
                var heroAttack = _detectedHandlers[i].GetComponent<HeroAttack>();

                if (heroData != null && i < cardSlots.Length)
                    cardSlots[i].SetData(heroData);

                if (heroAttack != null && heroData != null)
                    heroAttack.Init(heroData, tyrant); 
                    abilityManager.InitAbility(i, heroData);
            }

            foreach (var h in _detectedHandlers)
            {
                var heroAttack = h.GetComponent<HeroAttack>();
                if (heroAttack != null) heroAttack.StartAttacking();
            }

            StartCoroutine(DelayAction());
        }
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(3f);
        LoadGameplay();
    }

    void LoadGameplay()
    {
        uiScanning.SetActive(false);
        uiDone.SetActive(true);
        anim_manager.OnSuccess();
        tyrant.gameObject.SetActive(true); 
        gameManager.StartBattle();
    }
}