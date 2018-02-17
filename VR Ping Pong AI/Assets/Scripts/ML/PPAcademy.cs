using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAcademy : Academy
{
    public Agent trainee;
    public override void AcademyReset()
    {
        Debug.Log("RESET");
    }

    public override void AcademyStep()
    {
        //Debug.Log("STEP");
    }

}
