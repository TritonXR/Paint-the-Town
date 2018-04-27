using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRaycast : RayCastObject {

    Color origColor;
    RaycastHit prevHit;

	public override void OnRayCast(RaycastHit objHit)
    {

    }

    public override void OnRayCastEnter(RaycastHit objHit)
    {
            objHit.collider.GetComponent<UIClick>();
            Button uiButton = objHit.collider.GetComponent<Button>();
            ColorBlock cb = uiButton.colors;
            origColor = cb.normalColor;
            cb.normalColor = cb.highlightedColor;

    }

    public override void OnRayCastExit(RaycastHit objHit)
    {
        objHit.collider.GetComponent<UIClick>();
        Button uiButton = objHit.collider.GetComponent<Button>();
        ColorBlock cb = uiButton.colors;
        cb.normalColor = origColor;
    }
}
