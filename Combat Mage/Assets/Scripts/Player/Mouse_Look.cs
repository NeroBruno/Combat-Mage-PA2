using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Look : MonoBehaviour
{

    [SerializeField] private float MouseSpeed = 100f;
    [SerializeField] private float CameraClamp = 90f;

    private float xRotation;
    public Transform PlayerBody;
    public Transform barrel;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * MouseSpeed * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSpeed * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -CameraClamp, CameraClamp);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        PlayerBody.Rotate(Vector3.up * MouseX);
    }
}