using System;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public enum KeyResult
{
    Wrong,
    Correct,
    Complete
}

public class ButtonSequence
{
    private readonly int _length;
    private readonly KeyCode[] _sequence;
    private readonly TargetButton[] _targetButtonArray;
    private readonly Action _onSessionComplete;

    private int _sequenceIndex;

    private const float ButtonOffset = 130;

    public ButtonSequence(int length, Transform container, Action onSessionComplete)
    {
        _length = length;
        _sequence = new KeyCode[length];
        _sequenceIndex = 0;
        _onSessionComplete = onSessionComplete;

        // Create randomized sequence
        var possibleKeys = GameSettings.Instance.PossibleKeys;

        for (var i = 0; i < length; i++)
        {
            _sequence[i] = possibleKeys[Random.Range(0, possibleKeys.Length)];
        }

        // Create TargetButtons
        _targetButtonArray = new TargetButton[_length];
        var targetButtonPrefab = Resources.Load<TargetButton>("Prefabs/TargetButton");
        var middleIndex = (_length - 1) / 2;

        for (var i = 0; i < _length; i++)
        {
            var targetButton = Object.Instantiate(targetButtonPrefab, container);
            targetButton.transform.localPosition = new Vector3((i - middleIndex) * ButtonOffset, 0);
            targetButton.Initialize(_sequence[i]);
            _targetButtonArray[i] = targetButton;
        }
    }

    public KeyResult CheckKey(KeyCode key)
    {
        if (_sequence[_sequenceIndex] == key)
        {
            _targetButtonArray[_sequenceIndex].gameObject.SetActive(false);
            _sequenceIndex++;

            return _sequenceIndex == _length ? KeyResult.Complete : KeyResult.Correct;
        }

        return KeyResult.Wrong;
    }

    public void FadeInTargetButtons()
    {
        foreach (var targetButton in _targetButtonArray)
        {
            targetButton.FadeIn();
        }
    }

    public void FadeOutTargetButtons()
    {
        foreach (var targetButton in _targetButtonArray)
        {
            targetButton.FadeOut();
        }
    }

    public void Destroy(bool invokeOnComplete)
    {
        DestroyTargetButtons();
        if (invokeOnComplete) _onSessionComplete();
    }

    public override string ToString()
    {
        var log = new StringBuilder();

        for (var i = 0; i < _length; i++)
        {
            log.Append(_sequence[i]);
            if (i != _length - 1) log.Append(" - ");
        }

        return log.ToString();
    }

    private void DestroyTargetButtons()
    {
        foreach (var targetButton in _targetButtonArray)
        {
            targetButton.Destroy();
        }
    }
}