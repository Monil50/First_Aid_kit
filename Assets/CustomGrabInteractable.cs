using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;
using System.Collections;

public class CustomGrabInteractable : Grabbable
{
    public Transform RightAttachTransform;
    public Transform LeftAttachTransform;
    public GameObject DropPoint;
    public bool DropWithAnimation = true;
    public float GhostEnableTime = 3f;
    public UnityEvent OnCustomGrab;
    public UnityEvent OnCustomRelease;
    public UnityEvent OnFastRelease;

    private Transform originalParent;
    private Collider dropCollider;
    private bool isRightHandGrab = false;
    private bool isLeftHandGrab = false;
    private float lerpDuration = 1f;

    // Haptic feedback properties
    public float hapticIntensity = 0.5f;
    public float hapticDuration = 0.1f;

    // Assuming Grabbers should be a List of Grabber objects
    protected List<Grabber> Grabbers = new List<Grabber>();

    private void Start()
    {
        originalParent = DropPoint.transform.parent; // Store the original parent of the object
        DropPoint.SetActive(false);
        dropCollider = DropPoint.GetComponent<Collider>();
    }

    private void Update()
    {
        // Check if the object is being held by any grabber
        if (Grabbers.Count > 0)  // If Grabbers is not empty, the object is held
        {
            var grabber = Grabbers[0];  // Use the first grabber in the list
            HandleGrab(grabber);
        }
        else
        {
            HandleRelease();
        }
    }

    // Handle the grab logic when the object is grabbed
    private void HandleGrab(Grabber grabber)
    {
        isRightHandGrab = grabber.HandSide == ControllerHand.Right;
        isLeftHandGrab = grabber.HandSide == ControllerHand.Left;

        // Trigger haptic feedback when grabbed
        TriggerHapticFeedback(isRightHandGrab);

        OnCustomGrab.Invoke();

        // Attach to hand using attach points (if defined)
        if (RightAttachTransform || LeftAttachTransform)
        {
            Transform targetAttach = isRightHandGrab ? RightAttachTransform : LeftAttachTransform;
            if (targetAttach)
            {
                StartCoroutine(LerpToAttachTransform(grabber, targetAttach));
            }
        }
    }

    // Handle the release logic when the object is released
    private void HandleRelease()
    {
        if (DropPoint.activeSelf == false)
        {
            StartCoroutine(HandleReleaseAnimation());
            OnCustomRelease.Invoke();
        }
    }

    // Handle the drop animation or instant drop logic
    private IEnumerator HandleReleaseAnimation()
    {
        if (DropWithAnimation)
        {
            float elapsedTime = 0f;

            while (elapsedTime < lerpDuration)
            {
                transform.position = Vector3.Lerp(transform.position, DropPoint.transform.position, elapsedTime / lerpDuration);
                transform.rotation = Quaternion.Lerp(transform.rotation, DropPoint.transform.rotation, elapsedTime / lerpDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            transform.position = DropPoint.transform.position;
            transform.rotation = DropPoint.transform.rotation;
        }

        DropPoint.SetActive(false);
        isRightHandGrab = false;
        isLeftHandGrab = false;
    }

    // Lerp the object to the hand attach point
    private IEnumerator LerpToAttachTransform(Grabber grabber, Transform attachTransform)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, attachTransform.localPosition, elapsedTime / lerpDuration);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, attachTransform.localRotation, elapsedTime / lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Handle trigger interaction for fast release
    private void OnTriggerEnter(Collider other)
    {
        if (other == dropCollider)
        {
            OnFastRelease.Invoke();
            DropPoint.SetActive(false);
        }
    }

    // Trigger haptic feedback
    private void TriggerHapticFeedback(bool isRightHand)
    {
        InputBridge.Instance.VibrateController(0, hapticIntensity, hapticDuration, isRightHand ? ControllerHand.Right : ControllerHand.Left);
    }
}