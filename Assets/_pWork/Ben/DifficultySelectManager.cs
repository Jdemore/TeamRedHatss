using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Difficulty select screen with a scroll rect of checkboxes.
/// Shows completed tiers as selectable toggles, plus an "All Learned" option.
/// Sets ActiveTierSelection.ActiveFlags before loading the gameplay scene.
///
/// Setup:
/// 1. Create a ScrollRect with a vertical layout content area.
/// 2. Assign a toggle prefab (Toggle + TMP label + optional lock icon).
/// 3. This script spawns one toggle per tier + the "All" toggle at runtime.
/// </summary>
public class DifficultySelectManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform _toggleContainer;
    [SerializeField] private GameObject _togglePrefab;
    [SerializeField] private Button _startButton;
    [SerializeField] private TextMeshProUGUI _selectionSummaryText;

    [Header("Scene")]
    [SerializeField] private string _gameplayScene = "Main";

    private Toggle _allToggle;
    private Toggle[] _tierToggles;
    private bool _updatingToggles;

    private void Start()
    {
        TierFlags completed = TierProgress.CompletedFlags;
        int tierCount = KanaDatabase.TierCount;

        _tierToggles = new Toggle[tierCount];

        // Spawn "All Learned" toggle first
        _allToggle = SpawnToggle("All Learned", true);
        _allToggle.onValueChanged.AddListener(OnAllToggleChanged);

        // Spawn one toggle per tier
        for (int i = 0; i < tierCount; i++)
        {
            bool unlocked = (completed & KanaDatabase.TierIndexToFlag(i)) != 0;
            string label = KanaDatabase.Tiers[i].name;

            Toggle toggle = SpawnToggle(label, unlocked);
            _tierToggles[i] = toggle;

            if (!unlocked)
            {
                toggle.interactable = false;
                toggle.isOn = false;
            }
            else
            {
                toggle.isOn = true;
            }

            int index = i; // capture for closure
            toggle.onValueChanged.AddListener(_ => OnTierToggleChanged(index));
        }

        // Wire start button
        if (_startButton != null)
            _startButton.onClick.AddListener(StartGameplay);

        UpdateSummary();
    }

    private Toggle SpawnToggle(string label, bool interactable)
    {
        GameObject obj = Instantiate(_togglePrefab, _toggleContainer);
        Toggle toggle = obj.GetComponent<Toggle>();

        TextMeshProUGUI tmp = obj.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = label;

        toggle.interactable = interactable;
        return toggle;
    }

    private void OnAllToggleChanged(bool isOn)
    {
        if (_updatingToggles) return;
        _updatingToggles = true;

        TierFlags completed = TierProgress.CompletedFlags;
        for (int i = 0; i < _tierToggles.Length; i++)
        {
            bool unlocked = (completed & KanaDatabase.TierIndexToFlag(i)) != 0;
            if (unlocked)
                _tierToggles[i].isOn = isOn;
        }

        _updatingToggles = false;
        UpdateSummary();
    }

    private void OnTierToggleChanged(int index)
    {
        if (_updatingToggles) return;
        _updatingToggles = true;

        // Update "All" toggle to match — on if all unlocked tiers are checked
        bool allOn = true;
        TierFlags completed = TierProgress.CompletedFlags;
        for (int i = 0; i < _tierToggles.Length; i++)
        {
            bool unlocked = (completed & KanaDatabase.TierIndexToFlag(i)) != 0;
            if (unlocked && !_tierToggles[i].isOn)
            {
                allOn = false;
                break;
            }
        }
        _allToggle.isOn = allOn;

        _updatingToggles = false;
        UpdateSummary();
    }

    private TierFlags GetSelectedFlags()
    {
        TierFlags flags = TierFlags.None;
        for (int i = 0; i < _tierToggles.Length; i++)
        {
            if (_tierToggles[i].isOn)
                flags |= KanaDatabase.TierIndexToFlag(i);
        }
        return flags;
    }

    private void UpdateSummary()
    {
        if (_selectionSummaryText == null) return;

        TierFlags flags = GetSelectedFlags();
        if (flags == TierFlags.None)
        {
            _selectionSummaryText.text = "Select at least one tier";
            if (_startButton != null) _startButton.interactable = false;
            return;
        }

        if (_startButton != null) _startButton.interactable = true;

        // Build summary string
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < KanaDatabase.TierCount; i++)
        {
            if ((flags & KanaDatabase.TierIndexToFlag(i)) != 0)
            {
                if (sb.Length > 0) sb.Append(" + ");
                sb.Append(KanaDatabase.Tiers[i].name);
            }
        }
        _selectionSummaryText.text = sb.ToString();
    }

    private void StartGameplay()
    {
        TierFlags flags = GetSelectedFlags();
        if (flags == TierFlags.None) return;

        ActiveTierSelection.ActiveFlags = flags;
        ActiveTierSelection.Mode = ActiveTierSelection.PlayMode.Gameplay;
        SceneMgmt.Manager.LoadByName(_gameplayScene);
    }
}
