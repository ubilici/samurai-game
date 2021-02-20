using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioClip[] _swordSwingClips;

    private void Start()
    {
        _swordSwingClips = new[]
        {
            Resources.Load<AudioClip>("Sounds/SwordSwing1"),
            Resources.Load<AudioClip>("Sounds/SwordSwing2"),
            Resources.Load<AudioClip>("Sounds/SwordSwing3")
        };
    }

    public void PlayRandomSwordSwingClip()
    {
        PlayAudioClip(_swordSwingClips[Random.Range(0, _swordSwingClips.Length)]);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        var audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform, false);

        audioSource.clip = audioClip;
        audioSource.Play();

        StartCoroutine(WaitAndDestroy(audioSource.gameObject, audioClip.length));
    }

    private static IEnumerator WaitAndDestroy(Object target, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(target);
    }
}