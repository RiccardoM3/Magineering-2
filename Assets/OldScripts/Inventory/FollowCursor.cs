using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    public int offset_x = 0;
    public int offset_y = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Input.mousePosition + new Vector3(offset_x, offset_y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(offset_x, offset_y, 0);
    }
}
