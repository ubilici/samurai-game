using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        TestButtonSequence();
    }

    private void TestButtonSequence()
    {
        Debug.Log(new ButtonSequence(3));
        Debug.Log(new ButtonSequence(5));
        Debug.Log(new ButtonSequence(7));
    }
}