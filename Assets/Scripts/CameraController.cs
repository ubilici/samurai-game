using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private CanvasGroup MenuUICanvasGroup;
    [SerializeField] private Transform Island;

    private Sequence _moveSequence;
    private Tweener _rotateTweener;

    private const float MoveDuration = 0.8f;
    private const float LoopRotationDuration = 12.5f;
    private const float FadeDuration = 0.25f;
    private const float MenuViewSize = 8.8f;
    private const float GameViewSize = 2.5f;

    private static readonly Vector3 MenuViewPosition = new Vector3(-11.55f, 19.8f, -24.5f);
    private static readonly Vector3 MenuViewRotation = new Vector3(32.5f, 10.5f, 0);
    private static readonly Vector3 GameViewPosition = new Vector3(0, 4.6f, -5.375f);
    private static readonly Vector3 GameViewRotation = new Vector3(20, -0.33f, 0);

    private void Start()
    {
        RotateIsland();
    }

    public void MoveToMenuView()
    {
        _moveSequence?.Kill();
        _moveSequence = DOTween.Sequence();

        RotateIsland();
        MenuUICanvasGroup.interactable = true;

        _moveSequence.Append(MainCamera.transform.DOMove(MenuViewPosition, MoveDuration));
        _moveSequence.Join(MainCamera.transform.DORotate(MenuViewRotation, MoveDuration));
        _moveSequence.Join(MainCamera.DOOrthoSize(MenuViewSize, MoveDuration));

        _moveSequence.AppendInterval(0.2f);
        _moveSequence.Append(MenuUICanvasGroup.DOFade(1, FadeDuration));
    }

    public void MoveToGameView(Action onComplete)
    {
        _moveSequence?.Kill();
        _moveSequence = DOTween.Sequence();

        FixIslandRotation();
        MenuUICanvasGroup.interactable = false;

        _moveSequence.Append(MainCamera.transform.DOMove(GameViewPosition, MoveDuration));
        _moveSequence.Join(MainCamera.transform.DORotate(GameViewRotation, MoveDuration));
        _moveSequence.Join(MainCamera.DOOrthoSize(GameViewSize, MoveDuration));
        _moveSequence.Join(MenuUICanvasGroup.DOFade(0, FadeDuration));

        _moveSequence.onComplete = onComplete.Invoke;
    }

    private void RotateIsland()
    {
        _rotateTweener?.Kill();

        _rotateTweener = Island.DORotate(Vector3.up * 360, LoopRotationDuration, RotateMode.FastBeyond360);
        _rotateTweener.SetRelative();
        _rotateTweener.SetLoops(-1);
    }

    private void FixIslandRotation()
    {
        _rotateTweener?.Kill();

        _rotateTweener = Island.DORotate(Vector3.zero, MoveDuration);
    }
}