using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAcademy : Academy
{
    public Catcher enemyRacket;
    public Catcher myRacket;
    public override void AcademyReset()
    {
        enemyRacket.maxRacketMovingSpeed = resetParameters["MaxRacketSpeed"];
        myRacket.maxRacketMovingSpeed = resetParameters["IncMySpeed"];
    }

    public override void AcademyStep()
    {

    }

}
