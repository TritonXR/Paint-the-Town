using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRaycast : RayCastObject {

    Color origColor;
    RaycastHit prevHit;
    Button uiButton;

	public override void OnRayCast(RaycastHit objHit)
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Button currButton = objHit.collider.GetComponent<Button>();
            if (currButton  != null)
            {
                currButton.onClick.Invoke();  
            }
        }
    }

    public override void OnRayCastEnter(RaycastHit objHit)
    {
            objHit.collider.GetComponent<UIClick>();
            uiButton = objHit.collider.GetComponent<Button>();
            ColorBlock cb = uiButton.colors;
            origColor = cb.normalColor;
            cb.normalColor = cb.highlightedColor;

    }

    public override void OnRayCastExit()
    {
        if (uiButton != null)
        {
            ColorBlock cb = uiButton.colors;
            cb.normalColor = origColor;
        }
    }
}
