using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class collision : MonoBehaviour
{
    public bool detect = true;

    private PointSystem _pointSystem;
    private DictionaryManager _manager;
    private TutorialManager _tutorialManager;
    public SphereCollider sphereCol;
    public AudioManager audioManager;

    [SerializeField] private float hitCooldown = 0.7f;
    private bool canHit = true;

    private void Start()
    {
        if (sphereCol == null)
            sphereCol = GetComponent<SphereCollider>();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindReferences();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindReferences();
    }

    private void FindReferences()
    {
        _pointSystem = FindAnyObjectByType<PointSystem>();
        _manager = FindAnyObjectByType<DictionaryManager>();
        _tutorialManager = FindAnyObjectByType<TutorialManager>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (_manager == null || _pointSystem == null)
            FindReferences();

        if (_manager == null || _pointSystem == null) return;

        if (!canHit) return;

        // The collider is on Cube (child), but the tag is on box (parent).
        // Walk up one level from the collider to the box root.
        GameObject root = other.transform.parent != null
            ? other.transform.parent.gameObject
            : other.gameObject;

        Debug.Log("[Collision] Hit: " + other.gameObject.name
            + " | Parent: " + (root != null ? root.name : "null")
            + " | Tag: " + root.tag
            + " | detect: " + detect
            + " | manager: " + (_manager != null ? _manager.gameObject.name : "null")
            + " | tutorial: " + (_tutorialManager != null));

        if (detect)
        {
            if (root.CompareTag("correct"))
            {
                canHit = false;
                Debug.Log("Correct");
                _pointSystem.getPoints();
                audioManager.PlayCorrect();
                RecordStat(true);
                _manager.ShowAnswerFeedback(root, true);

                if (_tutorialManager != null)
                    _tutorialManager.OnCorrectAnswer();
                else
                    _manager.GenerateQuestion();

                StartCoroutine(DisableColliderTemporarily());
            }
            else if (root.CompareTag("incorrect"))
            {
                canHit = false;
                Debug.Log("Incorrect");
                audioManager.PlayIncorrect();
                _pointSystem.loseLife();
                RecordStat(false);
                _manager.ShowAnswerFeedback(root, false);

                if (_tutorialManager != null)
                    _tutorialManager.OnIncorrectAnswer();
                else
                    _manager.GenerateQuestion();

                StartCoroutine(DisableColliderTemporarily());
            }
        }
        // Re-enable after a short delay since the choice may be destroyed
        // before OnTriggerExit fires (destroying the collider skips the exit callback)
        detect = false;
        Invoke(nameof(ResetDetect), 0.2f);
    }

    private IEnumerator DisableColliderTemporarily()
    {
        if (sphereCol != null)
            sphereCol.enabled = false;

        yield return new WaitForSeconds(hitCooldown);

        if (sphereCol != null)
            sphereCol.enabled = true;

        canHit = true;
    }

    private void ResetDetect()
    {
        detect = true;
    }

    private void RecordStat(bool correct)
    {
        if (KanaStatsTracker.Instance != null)
        {
            KanaStatsTracker.Instance.RecordAnswer(
                _manager.CurrentTierIndex,
                _manager.CurrentKana,
                correct,
                _manager.ResponseTime
            );
        }
    }

    private void OnTriggerExit()
    {
        detect = true;
    }
}
