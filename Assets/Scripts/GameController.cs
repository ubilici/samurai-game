using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform ButtonSequenceContainer;

    private ButtonSequence _currentButtonSequence;
    private bool _sequenceTryStarted;
    private float _sequenceTryEndTime;

    private void Start()
    {
        CreateButtonSequence(3);
    }

    private void Update()
    {
        CheckInput();

        if (_sequenceTryStarted)
        {
            CheckTime();
        }
    }

    private void CreateButtonSequence(int length)
    {
        _currentButtonSequence = new ButtonSequence(length, ButtonSequenceContainer);
        Debug.Log(_currentButtonSequence);
    }

    private void CheckTime()
    {
        Debug.Log(_sequenceTryEndTime - Time.time);
    }

    private void CheckInput()
    {
        foreach (var possibleKey in GameSettings.Instance.PossibleKeys)
        {
            if (Input.GetKeyDown(possibleKey))
            {
                var result = _currentButtonSequence.CheckKey(possibleKey);
                Debug.Log(result);

                switch (result)
                {
                    case KeyResult.Correct:
                        if (!_sequenceTryStarted)
                        {
                            _sequenceTryEndTime = Time.time + GameSettings.Instance.SequenceTryTime;
                            _sequenceTryStarted = true;
                        }

                        break;
                    case KeyResult.Wrong:
                    case KeyResult.Complete:
                        _sequenceTryStarted = false;
                        _sequenceTryEndTime = default;
                        _currentButtonSequence.Destroy();
                        CreateButtonSequence(3);
                        break;
                }
            }
        }
    }
}