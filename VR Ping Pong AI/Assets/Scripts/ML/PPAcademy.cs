using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ML-agent academy.
/// </summary>
public class PPAcademy : Academy
{
    public Catcher enemyRacket;
    public Catcher myRacket;

    public override void AcademyReset()
    {
        myRacket.maxRacketMovingSpeed = resetParameters["IncMySpeed"];
    }

    public override void AcademyStep()
    {
    }
}