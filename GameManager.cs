using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("── Timer ──")]
    public float duration = 120f;
    public TextMeshProUGUI timerText;

    [Header("── References ──")]
    public UI_Manager ui;
    public TyrantController tyrant;
    public Anim_Manager Anim_Manager;
    public CardSlotUI[] heroSlots;

    [Header("── Tracking ──")]
    public CustomTracking[] customTrackers;
    public GameObject pausePanel;
    public float trackingGracePeriod = 3f;

    [Header("── Debug ──")]
    public TextMeshProUGUI debugText;

    public static float ElapsedTime = 0f;
    public static string BattleResult = "";
    public static int BattleRating = 0;
    public static bool IsPaused = false;

    private float _timeRemaining;
    private bool _isRunning = false;
    private bool _ended = false;
    private bool _isPaused = false;
    private bool _trackingCheckActive = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void StartBattle()
    {
        _timeRemaining = duration;
        _isRunning = true;
        _ended = false;
        _isPaused = false;
        IsPaused = false;
        _trackingCheckActive = false;

        tyrant.StartBattle();
        StartCoroutine(ActivateTrackingCheck());
        Debug.Log("[GameManager] StartBattle dipanggil!");
    }

    IEnumerator ActivateTrackingCheck()
    {
        yield return new WaitForSeconds(trackingGracePeriod);
        _trackingCheckActive = true;
        Debug.Log("[GameManager] Tracking check aktif");
    }

    void Update()
    {
        if (!_isRunning || _ended) return;

        if (debugText != null)
        {
            int visible = CountVisibleTrackers();
            debugText.text =
                $"Visible: {visible}/3\n" +
                $"Paused: {_isPaused}\n" +
                $"TrackCheck: {_trackingCheckActive}\n" +
                $"Timer: {_timeRemaining:F1}";
        }

        if (_isPaused) return;

        _timeRemaining -= Time.deltaTime;

        if (timerText != null)
        {
            float display = Mathf.Max(0f, _timeRemaining);
            int min = Mathf.FloorToInt(display / 60f);
            int sec = Mathf.FloorToInt(display % 60f);
            timerText.text = $"{min:00}:{sec:00}";
        }

        if (tyrant.IsDead)
        {
            ElapsedTime = duration - _timeRemaining;
            OnBattleEnd(win: true);
            return;
        }

        if (AllHeroesDead())
        {
            ElapsedTime = duration - _timeRemaining;
            OnBattleEnd(win: false);
            return;
        }

        if (_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            ElapsedTime = duration;
            OnBattleEnd(win: false);
        }
    }

    public void OnTrackerVisibilityChanged()
    {
        if (!_isRunning || _ended || !_trackingCheckActive) return;

        int visible = CountVisibleTrackers();
        Debug.Log($"[GameManager] Visible: {visible}/3");

        if (visible < 3 && !_isPaused)      PauseGame();
        else if (visible >= 3 && _isPaused) ResumeGame();
    }

    int CountVisibleTrackers()
    {
        int count = 0;
        foreach (var t in customTrackers)
            if (t != null && t.IsVisible) count++;
        return count;
    }

    void PauseGame()
    {
        _isPaused = true;
        IsPaused = true;
        tyrant.StopBattle();
        Anim_Manager.PauseAllAnimations();
        if (pausePanel != null) pausePanel.SetActive(true);
        Debug.Log("[GameManager] PAUSE");
    }

    void ResumeGame()
    {
        _isPaused = false;
        IsPaused = false;
        tyrant.StartBattle();
        Anim_Manager.ResumeAllAnimations();
        if (pausePanel != null) pausePanel.SetActive(false);
        Debug.Log("[GameManager] RESUME");
    }

    bool AllHeroesDead()
    {
        foreach (var slot in heroSlots)
            if (slot != null && !slot.IsDead) return false;
        return true;
    }

    void OnBattleEnd(bool win)
    {
        if (_ended) return;
        _ended = true;
        _isRunning = false;

        BattleResult = win ? "Stage Completed" : "Stage Failed";
        BattleRating = CalculateRating(win);

        tyrant.StopBattle();
        Anim_Manager.TyrantDie();

        Debug.Log($"[GameManager] {BattleResult} | Rating: {BattleRating} | Waktu: {ElapsedTime:F1}s");
        StartCoroutine(DelayLoadScene());
    }

    int CalculateRating(bool win)
    {
        if (!win) return 0;
        float timeRatio = _timeRemaining / duration;
        if (timeRatio > 0.75f) return 3;
        if (timeRatio > 0.5f)  return 2;
        return 1;
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(3f);
        ui.LoadAfterGameScene();
    }
}