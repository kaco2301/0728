using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    private int initialTargets = 6;

    private List<Target> targetPool;
    private List<Target> activeTargets;
    
    public void StartGame()
    {
        Target[] allTargets = FindObjectsOfType<Target>();

        // 타겟 풀과 활성 타겟 리스트 초기화
        targetPool = new List<Target>(allTargets);
        activeTargets = new List<Target>();

        // 모든 타겟을 시작하자마자 쓰러뜨리기 (OnTargetDown)
        foreach (Target target in allTargets)
        {
            target.InitializeTarget();
        }

        // 무작위로 일부 타겟 일으키기 (OnTargetUp)
        for (int i = 0; i < Mathf.Min(initialTargets, allTargets.Length); i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, targetPool.Count);
            targetPool[randomIndex].StartCoroutine("OnTargetUp");
            activeTargets.Add(targetPool[randomIndex]);
            targetPool.RemoveAt(randomIndex);
        }
    }

    public void HandleTargetDown(Target target)
    {
        activeTargets.Remove(target);
        targetPool.Add(target);

        if (activeTargets.Count < initialTargets)
        {
            int randomIndex = UnityEngine.Random.Range(0, targetPool.Count);
            targetPool[randomIndex].StartCoroutine("OnTargetUp");
            activeTargets.Add(targetPool[randomIndex]);
            targetPool.RemoveAt(randomIndex);
        }
    }
}