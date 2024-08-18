using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;
    [SerializeField] float controlStrength;
    [SerializeField] float maxHorizSpeed;
    [SerializeField] float vertSpeed;

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
        rb.velocity = horizSpeed + Vector3.down * vertSpeed;
    }
}
