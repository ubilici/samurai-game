using System.Text;
using UnityEngine;

public class ButtonSequence
{
    private readonly int _length;
    private readonly KeyCode[] _sequence;

    public ButtonSequence(int length)
    {
        _length = length;
        _sequence = new KeyCode[length];
        var possibleKeys = GameSettings.Instance.PossibleKeys;

        for (var i = 0; i < length; i++)
        {
            _sequence[i] = possibleKeys[Random.Range(0, possibleKeys.Length)];
        }
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
}