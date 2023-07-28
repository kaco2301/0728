using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouseFPS : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5;//x
    [SerializeField]
    private float rotCamYAxisSpeed = 3;//y

    private float limitMaxX = 30; // x축 최대범위
    private float limitMinX = -50; // x축 최소범위

    private float limitMaxY = 50; // x축 최대범위
    private float limitMinY = -50; // x축 최소범위
    private float eulerAngleX;
    private float eulerAngleY;

    private Vector2 currentInput;
    private float smoothTime = 0.1f;
    private Vector2 velocity;


    public void UpdateRotate(float mouseX, float mouseY)
    {
        Vector2 targetInput = new Vector2(mouseX, mouseY);
        currentInput = Vector2.SmoothDamp(currentInput, targetInput, ref velocity, smoothTime);

        float inputMagnitude = currentInput.magnitude;
        float adjustedRotCamXAxisSpeed = rotCamXAxisSpeed * inputMagnitude;
        float adjustedRotCamYAxisSpeed = rotCamYAxisSpeed * inputMagnitude;

        eulerAngleY += currentInput.x * adjustedRotCamYAxisSpeed;
        eulerAngleX -= currentInput.y * adjustedRotCamXAxisSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        eulerAngleY = ClampAngle(eulerAngleY, limitMinY, limitMaxY);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) { angle += 360; }
        if (angle > 360) { angle -= 360; }

        return Mathf.Clamp(angle, min, max);

    }
}
