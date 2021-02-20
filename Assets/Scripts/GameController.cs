using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform BackgroundIsland;
    [SerializeField] private Transform ButtonSequenceContainer;
    [SerializeField] private TextMeshProUGUI StateText;
    [SerializeField] private SamuraiController SamuraiController;

    private ButtonSequence _currentButtonSequence;
    private bool _sequenceTryStarted;
    private float _sequenceTryEndTime;

    private float BackgroundRotateSpeed = 0.025f;

    private void Start()
    {
        CreateButtonSequence(5);
    }

    private void Update()
    {
        // RotateBackground();
        CheckInput();

        if (_sequenceTryStarted)
        {
            CheckTime();
        }
    }

    private void CreateButtonSequence(int length)
    {
        _currentButtonSequence = new ButtonSequence(length, ButtonSequenceContainer);
    }

    private void CheckTime()
    {
        var timeRemaining = _sequenceTryEndTime - Time.time;
        if (timeRemaining < 0)
        {
            DestroyCurrentSequence(false);
        }
        else
        {
            StateText.text = $"{timeRemaining:F2}";
        }
    }

    private void CheckInput()
    {
        foreach (var possibleKey in GameSettings.Instance.PossibleKeys)
        {
            if (Input.GetKeyDown(possibleKey))
            {
                var result = _currentButtonSequence.CheckKey(possibleKey);

                switch (result)
                {
                    case KeyResult.Correct:
                        if (!_sequenceTryStarted)
                        {
                            _currentButtonSequence.FadeOutTargetButtons();
                            _sequenceTryEndTime = Time.time + GameSettings.Instance.SequenceTryTime;
                            _sequenceTryStarted = true;
                        }

                        break;

                    case KeyResult.Wrong:
                        DestroyCurrentSequence(false);
                        break;

                    case KeyResult.Complete:
                        DestroyCurrentSequence(true);
                        break;
                }
            }
        }

        // Samurai controller test code
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            if (Input.GetMouseButtonDown(0)) SamuraiController.Run(hit.point);
            else if (Input.GetMouseButtonDown(1)) SamuraiController.Jump(hit.point);
        }
    }

    private void RotateBackground()
    {
        BackgroundIsland.Rotate(Vector3.up, BackgroundRotateSpeed);
    }

    private void DestroyCurrentSequence(bool isComplete)
    {
        StateText.text = isComplete ? "Success!" : "Failed!";

        _sequenceTryStarted = false;
        _sequenceTryEndTime = default;
        _currentButtonSequence.Destroy();

        // Create another sequence for testing
        CreateButtonSequence(5);
    }
}