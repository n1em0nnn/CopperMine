using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float mouseSense = 1;
    [SerializeField] Transform Body;
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 10f;
    [SerializeField] float gravity = 50f;
    [SerializeField] float jumpf = 30f;
    [SerializeField] float minXRotation = -70f;
    [SerializeField] float maxXRotation = 70f;

    private float currentXRotation = 0f;
    private Vector3 direction;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Вращение тела от камеры
        Quaternion cameraRotation = transform.rotation;
        float yRotation = cameraRotation.eulerAngles.y;
        Quaternion newRotation = Quaternion.Euler(0, yRotation, 0);
        Body.transform.rotation = newRotation;

        // Движение тела
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            direction = new Vector3(moveHorizontal, 0, moveVertical);
            direction = Body.transform.TransformDirection(direction) * speed;

            // Проверка нажатия клавиши для прыжка
            if (Input.GetButtonDown("Jump")) // "Jump" обычно соответствует пробелу
            {
                direction.y = jumpf; // Устанавливаем вертикальную скорость для прыжка
            }
        }
        else
        {
            // Если не на земле, применяем гравитацию
            direction.y -= gravity * Time.deltaTime;
        }

        controller.Move(direction * Time.deltaTime);

        // Перемещение камеры за телом
        transform.position = new Vector3(Body.position.x, Body.position.y + 1, Body.position.z);

        // Поворот камеры
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        // Обновление текущего угла поворота по оси X с ограничениями
        currentXRotation -= rotateY;
        currentXRotation = Mathf.Clamp(currentXRotation, minXRotation, maxXRotation);

        // Применение ограниченного угла поворота к камере
        Vector3 rotPlayer = transform.rotation.eulerAngles;
        rotPlayer.x = currentXRotation;
        rotPlayer.z = 0;
        rotPlayer.y += rotateX;
        transform.rotation = Quaternion.Euler(rotPlayer);
    }
}
