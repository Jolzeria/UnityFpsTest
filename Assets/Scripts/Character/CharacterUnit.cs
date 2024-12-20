using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnit : BeUnit
{
    protected override void Init()
    {
        base.Init();

        attribute = new CharacterAttribute();
        attribute.Init();
    }

    protected override void UnInit()
    {
        base.UnInit();

        attribute.UnInit();
    }


}
