using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPistol : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField]
    private GameObject          FireEffect;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform           casingSpawnPoint;//ź�ǻ�����ġ
    [SerializeField]
    private Transform           bulletSpawnPoint;//�Ѿ˻�����ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip           audioClipTakeOutWeapon;//������������
    [SerializeField]
    private AudioClip           audioClipFire;//���ݻ���

    [Header("Weapon Setting")]
    [SerializeField]
    private PistolSetting       pistolSetting;

    private float               lastAttackTime = 0;//������ �߻�ð� üũ��

    private AudioSource                 audioSource; //���� ���
    private PlayerAnimatorController    animator;//�ִϸ��̼� ��� ����
    private CasingMemoryPool            casingMemoryPool;
    private ImpactMemoryPool            impactMemoryPool;
    private Camera                      mainCamera;

    //==================================================================================================

    private void Awake()
    {
        audioSource =           GetComponent<AudioSource>();
        animator    =           GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool =      GetComponent<CasingMemoryPool>();
        impactMemoryPool =      GetComponent<ImpactMemoryPool>();
        mainCamera =            Camera.main;
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
        FireEffect.SetActive(false);
    }
    

    public void StartWeaponAction(int type =0)
    {
        //���콺 ��Ŭ�� �ܹ� ���
        if (type == 0)
        {
            OnAttack();
        }
    }

    

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > pistolSetting.attackRate)
        {
            //���� �ֱⰡ �Ǿ�� ���� ����.
            lastAttackTime = Time.time;

            //�ݹ� �ִϸ��̼� ���
            animator.Play("Fire",-1,0);

            StartCoroutine("OnFireEffect");
            PlaySound(audioClipFire);

            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);//ź�ǻ���

            TwoStepRaycast();
        }
    }

    private IEnumerator OnFireEffect()
    {
        FireEffect.SetActive(true);

        yield return new WaitForSeconds(pistolSetting.attackRate * 0.3f);

        FireEffect.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        //ȭ���� �߾���ǥ
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        //���� ��Ÿ��ȿ� �ε����� ������Ʈ�� ������ ������ �ε��� ��ġ�� targetpoint
        if(Physics.Raycast(ray,out hit, pistolSetting.attackDistance))
        {
            targetPoint = hit.point;
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(pistolSetting.damage);
            }
        }
        else
        {
            targetPoint = ray.origin + ray.direction * pistolSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * pistolSetting.attackDistance, Color.red);

        //ù��° Raycast�������� ����� targetPoint ��ǥ����
        //�ѱ��� ������������ �ؼ� ����
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if(Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, pistolSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * pistolSetting.attackDistance, Color.blue);
    }

}
