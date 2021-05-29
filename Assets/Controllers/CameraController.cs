using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Defines the camera movement variables.
    [SerializeField] Transform MainCharacter;
    [SerializeField] float CameraSensitivity;
    [SerializeField] float CameraRotationX;
    [SerializeField] float CameraRotationY;
    public Canvas CanvasInventory;
    public Canvas CanvasTutorial;

    void Start()
    {
        
    }

    void Update()
    {

        //Gets the movement of the mouse on screen.
        float MouseX = Input.GetAxis("Mouse X") * CameraSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * CameraSensitivity * Time.deltaTime;

        //Updates the rotation values.
        CameraRotationX += MouseX * Convert.ToInt32(!CanvasInventory.gameObject.activeSelf && !CanvasTutorial.gameObject.activeSelf);
        CameraRotationY -= MouseY * Convert.ToInt32(!CanvasInventory.gameObject.activeSelf && !CanvasTutorial.gameObject.activeSelf);
        CameraRotationY = Mathf.Clamp(CameraRotationY, -90, 90);

        //Rotates the camera around the mouse axis.
        transform.localRotation = Quaternion.Euler(CameraRotationY, 0, 0);

        //Rotates the character with the camera.
        MainCharacter.localRotation = Quaternion.Euler(0, CameraRotationX, 0);
        
    }

}
