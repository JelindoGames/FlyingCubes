using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;
    [SerializeField] float controlStrength;
    [SerializeField] float maxHorizSpeed;
    [SerializeField] float vertSpeed;
    [SerializeField] float shiftSpeedMultiplier;
    [SerializeField] AudioSource windSound;
    [SerializeField] float windSpeedOnFall;

    void Update()
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition / Screen.width * cam.pixelWidth;
        Vector2 control = mousePos - screenPoint;
        Vector3 horizSpeed = new Vector3(control.x, 0, control.y) * controlStrength;
        if (horizSpeed.magnitude > maxHorizSpeed)
        {
            horizSpeed = horizSpeed.normalized * maxHorizSpeed;
        }
        float finalVertSpeed = Input.GetKey(KeyCode.LeftShift) ? vertSpeed * shiftSpeedMultiplier : vertSpeed;
        windSound.pitch = Input.GetKey(KeyCode.LeftShift) ? windSpeedOnFall : 1;
        rb.velocity = horizSpeed + Vector3.down * finalVertSpeed;
    }

    public void Kill()
    {
        rb.velocity = Vector3.zero;
        this.enabled = false;
    }
}
