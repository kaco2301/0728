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
    private Transform           casingSpawnPoint;//탄피생성위치
    [SerializeField]
    private Transform           bulletSpawnPoint;//총알생성위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip           audioClipTakeOutWeapon;//무기장착사운드
    [SerializeField]
    private AudioClip           audioClipFire;//공격사운드

    [Header("Weapon Setting")]
    [SerializeField]
    private PistolSetting       pistolSetting;

    private float               lastAttackTime = 0;//마지막 발사시간 체크용

    private AudioSource                 audioSource; //사운드 재생
    private PlayerAnimatorController    animator;//애니메이션 재생 제어
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
        //마우스 좌클릭 단발 사격
        if (type == 0)
        {
            OnAttack();
        }
    }

    

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > pistolSetting.attackRate)
        {
            //공격 주기가 되어야 공격 가능.
            lastAttackTime = Time.time;

            //격발 애니메이션 재생
            animator.Play("Fire",-1,0);

            StartCoroutine("OnFireEffect");
            PlaySound(audioClipFire);

            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);//탄피생성

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

        //화면의 중앙지표
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        //공격 사거리안에 부딪히는 오브젝트가 있으면 광선에 부딪힌 위치가 targetpoint
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

        //첫번째 Raycast연산으로 얻어진 targetPoint 목표지점
        //총구를 시작지점으로 해서 연산
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if(Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, pistolSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * pistolSetting.attackDistance, Color.blue);
    }

}
