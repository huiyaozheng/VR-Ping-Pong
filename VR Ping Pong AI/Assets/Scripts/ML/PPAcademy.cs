using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAcademy : Academy
{
    public Agent trainee;
    public override void AcademyReset()
    {
        trainee.reward = 0.0f;
    }

    public override void AcademyStep()
    {
    }

}
