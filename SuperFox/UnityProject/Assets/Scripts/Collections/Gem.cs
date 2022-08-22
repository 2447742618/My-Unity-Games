using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Collections
{
    public override void Collected()
    {
        base.Collected();
        Player.gem++;
    }
}
