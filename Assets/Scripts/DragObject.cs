using UnityEngine;

public class DragObject : MonoBehaviour
{
    // Дистанция, на которой можно захватывать объекты
    public float grabDistance = 5f;

    // Сила, с которой объект притягивается к курсору
    public float attractionForce = 10f;

    // Максимальная масса объекта, которую можно поднять
    public float maxObjectMass = 10f;

    // Текущий захваченный объект
    private GameObject grabbedObject;
    private Rigidbody grabbedRigidbody;

    // Камера игрока
    private Camera playerCamera;

    void Start()
    {
        // Получаем ссылку на камеру игрока
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Проверяем нажатие левой кнопки мыши для захвата/отпускания объекта
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }

        // Если объект захвачен, перемещаем его
        if (grabbedObject != null)
        {
            MoveGrabbedObject();
        }
    }

    // Попытка захватить объект
    private void TryGrabObject()
    {
        // Создаем Raycast из камеры в направлении курсора
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            Rigidbody rb = hitObject.GetComponent<Rigidbody>();

            // Проверяем, можно ли захватить объект
            if (rb != null && rb.mass <= maxObjectMass)
            {
                grabbedObject = hitObject;
                grabbedRigidbody = rb;

                // Отключаем физику объекта для более плавного управления
                grabbedRigidbody.useGravity = false;
                grabbedRigidbody.freezeRotation = true;
            }
        }
    }

    // Перемещение захваченного объекта
    private void MoveGrabbedObject()
    {
        // Вычисляем позицию курсора в мире
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition = ray.GetPoint(grabDistance);

        // Притягиваем объект к целевой позиции
        grabbedRigidbody.linearVelocity = (targetPosition - grabbedObject.transform.position) * attractionForce;
    }

    // Отпускаем объект
    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            // Возвращаем физику объекта
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody.freezeRotation = false;

            // Сбрасываем ссылки
            grabbedObject = null;
            grabbedRigidbody = null;
        }
    }
}