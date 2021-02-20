using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform ButtonSequenceContainer;
    [SerializeField] private TextMeshProUGUI StateText;
    [SerializeField] private SamuraiController SamuraiController;
    [SerializeField] private CameraController CameraController;
    [SerializeField] private AudioController AudioController;

    private ButtonSequence _buttonSequence;
    private bool _sequenceInputsEnabled;
    private bool _sequenceTryStarted;
    private float _sequenceTryEndTime;
    private Sequence _animationSequence;
    private Sequence _textSequence;

    private const float ButtonSequenceStartInterval = 0.5f;
    private const float SessionRetryInterval = 0.65f;
    private const float StateTextFadeOutInterval = 1.25f;
    private const float StateTextFadeOutDuration = 0.25f;

    private static readonly Vector3 SamuraiSpawnPosition = new Vector3(-7, 1.395f, -2.4f);
    private static readonly Vector3 SamuraiMovePosition = new Vector3(-1.95f, 1.395f, -2.4f);
    private static readonly Vector3 SamuraiJumpPosition = new Vector3(-3.45f, 1.395f, -2.4f);

    public void Play()
    {
        CameraController.MoveToGameView(() =>
        {
            InitializeSession();
            StartSession();
        });
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (_sequenceInputsEnabled) CheckInput();
        if (_sequenceTryStarted) CheckTime();
    }

    private void InitializeSession()
    {
        SamuraiController.transform.position = SamuraiSpawnPosition;
        SamuraiController.gameObject.SetActive(true);
    }

    private void StartSession()
    {
        _animationSequence?.Kill();
        _animationSequence = DOTween.Sequence();

        _buttonSequence = new ButtonSequence(5, ButtonSequenceContainer, EndSession);

        _animationSequence.Append(SamuraiController.Run(SamuraiMovePosition));
        _animationSequence.AppendCallback(() =>
        {
            SamuraiController.DrawSword();
            _buttonSequence.FadeInTargetButtons();
        });
        _animationSequence.AppendInterval(ButtonSequenceStartInterval);
        _animationSequence.AppendCallback(() => { _sequenceInputsEnabled = true; });
    }

    private void EndSession()
    {
        _sequenceInputsEnabled = false;

        _animationSequence?.Kill();
        _animationSequence = DOTween.Sequence();

        _animationSequence.Append(SamuraiController.Jump(SamuraiJumpPosition));
        _animationSequence.AppendInterval(SessionRetryInterval);
        _animationSequence.AppendCallback(StartSession);
    }

    private void CheckTime()
    {
        var timeRemaining = _sequenceTryEndTime - Time.time;
        if (timeRemaining < 0)
        {
            DestroyCurrentSequence(false);
        }
    }

    private void CheckInput()
    {
        foreach (var possibleKey in GameSettings.Instance.PossibleKeys)
        {
            if (Input.GetKeyDown(possibleKey))
            {
                AudioController.PlayRandomSwordSwingClip();
                var result = _buttonSequence.CheckKey(possibleKey);

                switch (result)
                {
                    case KeyResult.Correct:
                        if (!_sequenceTryStarted)
                        {
                            _buttonSequence.FadeOutTargetButtons();
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
    }

    private void ShowStateText(bool isComplete)
    {
        _textSequence?.Kill();
        _textSequence = DOTween.Sequence();

        SetAlpha(StateText, 1);
        StateText.text = isComplete ? "Success!" : "Failed!";

        _textSequence.AppendInterval(StateTextFadeOutInterval);
        _textSequence.Append(StateText.DOFade(0, StateTextFadeOutDuration));
    }

    private static void SetAlpha(Graphic image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void DestroyCurrentSequence(bool isComplete)
    {
        ShowStateText(isComplete);

        _sequenceTryStarted = false;
        _sequenceTryEndTime = default;
        _buttonSequence.Destroy();
    }
}