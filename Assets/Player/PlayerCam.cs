using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : NetworkBehaviour
{
    public float sensitivity = 10f;
    public Transform target;
    public float distance = 10f;
    public Vector2 pitchMinMax = new Vector2(0, 85);

    float yaw;
    float pitch;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        gameObject.SetActive(true); //player camera is active
        var player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

    }


    void LateUpdate()
    {
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);

        //pitch, yaw, roll
        transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * distance;
    }
}
