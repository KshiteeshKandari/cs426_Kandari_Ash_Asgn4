using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 cameraDir;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        cameraDir = Camera.main.transform.forward;
        cameraDir.y = 90;
      
        transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
