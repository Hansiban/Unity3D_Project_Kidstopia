using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class LenderCameraController : MonoBehaviour
{
    // camera
    public Transform target = null; // player
    private float axisY;

    private float rotSensitive = 3f; // camera rotate sensitive
    private float disZ = 2.5f;
    private float disY = 1.2f;
    private float smoothTime = 0.2f;

    private Vector3 firstRoatition;
    private Vector3 targetRotation;
    private Vector3 currentVel;

    private void OnEnable()
    {
        // ī�޶� �÷��̾��� �������� �ʱ�ȭ
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
        transform.LookAt(target);
    }
    private void Update()
    {
        RotationCamera();
    }

    private void RotationCamera()
    {
        float x = Input.GetAxis("Mouse X");
        if (!target.GetComponent<PlayerControll>().createPanel.activeSelf && !targetRotation.Equals(firstRoatition))
        {
            transform.LookAt(target); // ī�޶� �÷��̾��� �������� �ʱ�ȭ
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase.Equals(TouchPhase.Began))
                    {
                        if (touch.phase.Equals(TouchPhase.Moved))
                        {
                            x = Input.touches[i].deltaPosition.x;
                        }
                    }
                }
                Rotate(x);
            }
        }
        else
        { // window
            if (Input.GetMouseButton(0))
            {
                Rotate(x);
            }
        }
    }

    private void Rotate(float x)
    {
        axisY = axisY + x * rotSensitive; // �¿� ������ ����
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(targetRotation.x, axisY), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation; // ī�޶� ȸ��
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
    }
}
