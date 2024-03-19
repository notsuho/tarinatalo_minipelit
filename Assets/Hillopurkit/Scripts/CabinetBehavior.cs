using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetBehavior : MonoBehaviour
{
    private Animator cabinetAnimator;

    private void Start()
    {
        cabinetAnimator = GetComponent<Animator>();
    }

    public void CloseDoors()
    {
        cabinetAnimator.Play("CloseCabinetDoors");
    }

    public void OpenDoors()
    {
        cabinetAnimator.Play("OpenCabinetDoors");
    }
}
