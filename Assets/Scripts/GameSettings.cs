using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/Create Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    public static GameSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameSettings>("GameSettings");
            }

            return _instance;
        }
    }

    private static GameSettings _instance;

    public KeyCode[] PossibleKeys;
    public float SequenceTryTime;
}