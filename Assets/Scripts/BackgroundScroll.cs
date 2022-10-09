using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] int distance;

    // Start is called before the first frame update
    void Start()
    {
        distance = 20;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < mainCam.gameObject.transform.position.x-distance)
        {
            MoveBG();
        }
    }

    void MoveBG()
    {
        transform.position += new Vector3(18.5f*3, 0, 0);
    }
}
