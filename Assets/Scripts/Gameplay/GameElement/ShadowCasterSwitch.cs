using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCasterSwitch : Interactable
{
    public ClosableShadowCaster shadowCaster;

    public override void Interact()
    {
        shadowCaster.Close();
    }
}
