using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform ButtonSequenceContainer;

    private ButtonSequence _currentButtonSequence;

    private void Start()
    {
        CreateButtonSequence(3);
    }

    private void Update()
    {
        CheckInput();
    }

    private void CreateButtonSequence(int length)
    {
        _currentButtonSequence = new ButtonSequence(length, ButtonSequenceContainer);
        Debug.Log(_currentButtonSequence);
    }

    private void CheckInput()
    {
        foreach (var possibleKey in GameSettings.Instance.PossibleKeys)
        {
            if (Input.GetKeyDown(possibleKey))
            {
                var result = _currentButtonSequence.CheckKey(possibleKey);
                Debug.Log(result);

                if (result == KeyResult.Wrong || result == KeyResult.Complete)
                {
                    _currentButtonSequence.Destroy();
                    CreateButtonSequence(3);
                }
            }
        }
    }
}