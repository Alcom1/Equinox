using UnityEngine;
using System.Collections;

public class ScriptEngi_Manuver : ScriptEngi_Default
{
    public float bounceCut;
    public float bounceRaise;
    public float minBounceMultiplier;
    private float maxBounceMultiplier;

    public override void subStart()
    {
        maxBounceMultiplier = bounceMultiplier;
    }

    public override void subMoveManipulate()
    {
        bounceMultiplier += bounceRaise * Time.deltaTime;

        if (bounceMultiplier > maxBounceMultiplier)
            bounceMultiplier = maxBounceMultiplier;
    }

    public override void subDisorient()
    {
        bounceMultiplier *= bounceCut;

        if (bounceMultiplier < minBounceMultiplier)
            bounceMultiplier = minBounceMultiplier;
    }
}
