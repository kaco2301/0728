using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : InteractionObject
{
    [SerializeField]
    private AudioClip clipTargetUp;
    [SerializeField]
    private AudioClip clipTargetDown;
    [SerializeField]
    private TargetManager targetManager;


    private AudioSource audioSource;
    private bool isPossibleHit = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0 && isPossibleHit == true)
        {
            isPossibleHit = false;
            StartCoroutine(OnTargetDown());
        }
    }
    public void InitializeTarget()
    {
        currentHP = 0;
        StartCoroutine(OnTargetDown(initial: true));
    }

    public IEnumerator OnTargetDown(bool initial = false)
    {
        if (!initial)
            audioSource.clip = clipTargetDown;
        audioSource.Play();

        yield return StartCoroutine(OnAnimation(0, -90));

        if (!initial)
        {
            ScoreManager.AddScore(10);
            CountManager.AddTime(0.2f);
            if (targetManager != null)
            {
                targetManager.HandleTargetDown(this);
            }
            else
            {
                Debug.LogError("TargetManager is not assigned in Target.");
            }
        }
    }

    public IEnumerator OnTargetUp()
    {
        yield return new WaitForSeconds(1);

        audioSource.clip = clipTargetUp;
        audioSource.Play();

        yield return StartCoroutine(OnAnimation(-90, 0));

        isPossibleHit = true;
    }

    private IEnumerator OnAnimation(float start, float end)
    {
        float percent = 0;
        float current = 0;
        float time = 1;

        while (percent < 1)
        {
            current += Time.deltaTime * 2;
            percent = current / time;

            transform.rotation = Quaternion.Slerp(Quaternion.Euler(start, 180, 0), Quaternion.Euler(end, 180, 0), percent);

            yield return null;
        }
    }
}
