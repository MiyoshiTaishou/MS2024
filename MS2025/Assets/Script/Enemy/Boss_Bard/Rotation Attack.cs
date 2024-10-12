using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationAttack : MonoBehaviour
{

    public int RotationSide;

    // Start is called before the first frame update
    void Start()
    {
        RotationSide = 1;
    }

    // Update is called o.nce per frame
    void Update()
    {
        //Œ»Ý‚ÌZŠp“x‚ðŽæ“¾
        float currentZAngle = transform.eulerAngles.z;

        if (currentZAngle > 180)
        {
            currentZAngle = currentZAngle - 360;
        }


        if (currentZAngle<-60)
        {
            RotationSide = 2;
        }
        else if(currentZAngle>60)
        {
            RotationSide = 1;
        }

       if (RotationSide == 1)
        {
            this.transform.Rotate(0, 0, -1);
        }
        else if (RotationSide == 2)
        {
            this.transform.Rotate(0, 0, 1);
        }
    }
}
