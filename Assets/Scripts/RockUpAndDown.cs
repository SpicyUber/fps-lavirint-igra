using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class RockUpAndDown : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;

    Quaternion startRotation;
    bool _front_rotation_finished = false;
    bool _back_rotation_finished = true;
    bool pause = false;
    public float rotationSpeed = 4f;
    public float moveYSpeed = 10f;
    public float moveYDistance = 2f;
    float startY;

    float timer = 0f;
    public float interval = 0.001f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startY = transform.position.y;
        startRotation = transform.rotation;
        

           

    }

    // Update is called once per frame
    void Update()
    {
        if(pause)
        {
            timer += (Time.deltaTime * 10);

            if(timer >= interval)
            {
                timer = 0f;
                pause = false;
            }
        }

        if (!_front_rotation_finished && _back_rotation_finished && !pause)
        {
            Quaternion targetRotation = Quaternion.Euler(10f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1f)
            {
                _front_rotation_finished = true;
                pause = true;
                _back_rotation_finished = false;
            }
        }

        else if (_front_rotation_finished && !_back_rotation_finished && !pause)
        {
            Quaternion targetRotation = Quaternion.Euler(-10f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1f)
            {
                _back_rotation_finished = true;
                pause = true;
                _front_rotation_finished = false;
            }

        }

   
    }

    public IEnumerator MoveY(float targetY)
    {
        float OGTargetY = targetY;
        targetY = targetY + startY;
        int sign = MathF.Sign(targetY - transform.position.y);
        while (sign * (targetY - transform.position.y) > sign * 0.01f)
        {
            transform.Translate(new Vector3(0, sign * Time.deltaTime * moveYSpeed, 0),  Space.World);
            yield return null;
        }

        yield return null;
        StartCoroutine(MoveY(-OGTargetY));

    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public void OnEnable()
    {
        StartCoroutine(MoveY(moveYDistance));
    }
}
