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

        // Ÿ�� Ǯ�� Ȱ�� Ÿ�� ����Ʈ �ʱ�ȭ
        targetPool = new List<Target>(allTargets);
        activeTargets = new List<Target>();

        // ��� Ÿ���� �������ڸ��� �����߸��� (OnTargetDown)
        foreach (Target target in allTargets)
        {
            target.InitializeTarget();
        }

        // �������� �Ϻ� Ÿ�� ����Ű�� (OnTargetUp)
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