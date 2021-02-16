using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform ButtonSequenceContainer;

    private void Start()
    {
        TestButtonSequence(5);
    }

    private void TestButtonSequence(int length)
    {
        var buttonSequence = new ButtonSequence(length);
        Debug.Log(buttonSequence.ToString());

        var targetButtonArray = buttonSequence.GetTargetButtons();

        foreach (var targetButton in targetButtonArray)
        {
            targetButton.transform.SetParent(ButtonSequenceContainer, false);
        }
    }
}