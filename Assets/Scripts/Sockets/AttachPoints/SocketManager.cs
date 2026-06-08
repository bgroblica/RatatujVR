using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketManager : MonoBehaviour
{
  //  public XRSocketInteractor socket;
  //
  //  private XRGrabInteractable hoveredInteractable;
  //
  //  private void OnEnable()
  //  {
  //      socket.hoverEntered.AddListener(OnHoverEntered);
  //      socket.hoverExited.AddListener(OnHoverExited);
  //      socket.selectEntered.AddListener(OnSelectEntered);
  //      socket.selectExited.AddListener(OnSelectExited);
  //  }
  //
  //  private void OnDisable()
  //  {
  //      socket.hoverEntered.RemoveListener(OnHoverEntered);
  //      socket.hoverExited.RemoveListener(OnHoverExited);
  //      socket.selectEntered.RemoveListener(OnSelectEntered);
  //      socket.selectExited.RemoveListener(OnSelectExited);
  //  }
  //
  //  // ✅ HOVER = safe time to change attach behavior
  //  private void OnHoverEntered(HoverEnterEventArgs args)
  //  {
  //      hoveredInteractable =
  //          args.interactableObject.transform.GetComponent<XRGrabInteractable>();
  //
  //      if (hoveredInteractable == null)
  //          return;
  //
  //      var points =
  //          hoveredInteractable.GetComponent<SocketAttachPoints>();
  //
  //      if (points == null)
  //          return;
  //
  //      socket.attachTransform = points.socketAttach;
  //  }
  //
  //  // ✅ RESET when no longer hovering
  //  private void OnHoverExited(HoverExitEventArgs args)
  //  {
  //      hoveredInteractable = null;
  //
  //      socket.attachTransform = null;
  //  }
  //
  //  // Optional safety: ensure correct pose is locked in
  //  private void OnSelectEntered(SelectEnterEventArgs args)
  //  {
  //      var grab =
  //          args.interactableObject.transform.GetComponent<XRGrabInteractable>();
  //
  //      var points =
  //          grab?.GetComponent<SocketAttachPoints>();
  //
  //      if (grab == null || points == null)
  //          return;
  //
  //      // Ensure correct socket pose is still active
  //      grab.attachTransform = points.socketAttach;
  //  }
  //
  //  // IMPORTANT: restore hand attach AFTER full deselect
  //  private void OnSelectExited(SelectExitEventArgs args)
  //  {
  //      var grab =
  //          args.interactableObject.transform.GetComponent<XRGrabInteractable>();
  //
  //      var points =
  //          grab?.GetComponent<SocketAttachPoints>();
  //
  //      if (grab == null || points == null)
  //          return;
  //
  //      // Wait 1 frame so XR fully releases socket ownership
  //      StartCoroutine(RestoreAfterRelease(grab, points));
  //  }
  //
  //  private System.Collections.IEnumerator RestoreAfterRelease(
  //      XRGrabInteractable grab,
  //      SocketAttachPoints points)
  //  {
  //      yield return null; // critical XR sync step
  //
  //      grab.attachTransform = points.handAttach1;
  //      grab.secondaryAttachTransform = points.handAttach2;
  //  }
}