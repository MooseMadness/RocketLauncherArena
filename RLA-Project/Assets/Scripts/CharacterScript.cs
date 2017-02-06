using UnityEngine;

//базовый класс для игрового пресонажа
[RequireComponent(typeof(Rigidbody))]
public class CharacterScript : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float jumpPower;

    [SerializeField]
    private float minXAngle;
    [SerializeField]
    private float maxXAngle;

    //если между данным объектом и groundChecker есть земля,
    //то считается что персонаж стоит на земле 
    [SerializeField]
    private Transform groundChecker;
    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private Transform headTransform;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //перемещает персонажа в заданном направлении
    //с скоростью равной модулю вектора
    protected void Move(Vector3 movement)
    {
        rb.MovePosition(transform.position + movement);
    }


    //вращает пресонажа вокруг оси Y
    //вращает камеру вокруг оси X 
    protected void Rotate(float yRot, float xRot)
    {
        float clampXRot = ClampXRotation(xRot);
        headTransform.Rotate(Vector3.right, xRot);
        transform.Rotate(Vector3.up, yRot);
    }

    //осуществляет выстрел
    protected void Fire()
    {
        //реализация выстрела
    }

    //осуществляет прыжок
    protected void Jump()
    {
        if(IsGrounded())
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    //ограничивает вращение ниже minXAngle
    //и выше maxXAngle
    //возвращает ограниченную величину вращения
    private float ClampXRotation(float xRot)
    {
        float xAngle = headTransform.rotation.eulerAngles.x + xRot;

        if (xAngle > maxXAngle)
        {
            return maxXAngle - headTransform.rotation.eulerAngles.x;
        }
        else if(xAngle < minXAngle)
        {
            return -headTransform.rotation.eulerAngles.x + minXAngle;
        }
        return xRot;
    }

    //истина если пресонаж стоит на земле
    //иначе ложь
    private bool IsGrounded()
    {
        Vector3 groundVector = groundChecker.position - transform.position;
        Ray groundRay = new Ray(transform.position, groundVector);
        if (Physics.Raycast(groundRay, groundVector.magnitude, groundLayers))
            return true;
        return false;
    }
}
