using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SugarBoxSpawner : XRBaseInteractable
{
    [Header("Sugar Cube")]
    public GameObject sugarCubePrefab;

    [Header("Spawn")]
    public Transform spawnPoint;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject cube = Instantiate(
            sugarCubePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        XRGrabInteractable cubeGrab =
            cube.GetComponent<XRGrabInteractable>();

        StartCoroutine(
            TransferGrab(args, cubeGrab)
        );

        base.OnSelectEntered(args);
    }

    private IEnumerator TransferGrab(
        SelectEnterEventArgs args,
        XRGrabInteractable cubeGrab)
    {
        yield return null;

        interactionManager.SelectExit(
            args.interactorObject,
            this
        );

        interactionManager.SelectEnter(
            args.interactorObject,
            cubeGrab
        );
    }
}