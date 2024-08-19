using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColorLerp : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] float speed;
    [SerializeField] Color color1, color2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camera.backgroundColor = Color.Lerp(color1, color2, Mathf.Sin(speed * Time.time));
    }
}
