﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClickable : MonoBehaviour, IClickable
{

    public void OnClick()
    {
        SwitchScene.Instance.DispleyMap(true);
    }


}
