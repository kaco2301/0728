using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFPS : MonoBehaviour
{

    private RotateToMouseFPS rotateToMouse;//���콺 �̵����� ī�޶�ȸ��
    private WeaponPistol weapon;//��������

    private bool isMovementEnabled = true;
    private bool isShootingEnabled = true;


    private void Start()
    {

        rotateToMouse = GetComponent<RotateToMouseFPS>();
        weapon = GetComponentInChildren<WeaponPistol>();
    }

    public void LockCursor()
    {
        //���콺 Ŀ�� invisible, ��ġ����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnLockCursor()
    { 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (isMovementEnabled)
        {
            UpdateRotate();

        }
    }

    public void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Horizontal");
        float mouseY = Input.GetAxis("Vertical");

        rotateToMouse.UpdateRotate(mouseX, mouseY);


    }

    public void MUpdateRotate(Vector2 InputVector)
    {
        float inputThreshold = 0.1f;

        float mouseX = Mathf.Abs(InputVector.x) > inputThreshold ? InputVector.x : 0.0f;
        float mouseY = Mathf.Abs(InputVector.y) > inputThreshold ? InputVector.y : 0.0f;

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    public void UpdateWeaponAction()
    {
        if (isShootingEnabled && Input.GetMouseButtonDown(0))
        { 
            weapon.StartWeaponAction();
        }
    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
    }

    public void EnableShooting()
    {
        isShootingEnabled = true;
    }

    public void DisableShooting()
    {
        isShootingEnabled = false;
    }

}
