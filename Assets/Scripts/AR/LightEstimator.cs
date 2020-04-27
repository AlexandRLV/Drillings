using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightEstimator : MonoBehaviour
{
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private Light currentLight;

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameUpdated;
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        ARLightEstimationData lightEstimation = args.lightEstimation;
        if (lightEstimation.averageBrightness.HasValue)
            currentLight.intensity = lightEstimation.averageBrightness.Value;

        if (lightEstimation.averageColorTemperature.HasValue)
            currentLight.colorTemperature = lightEstimation.averageColorTemperature.Value;

        if (lightEstimation.colorCorrection.HasValue)
            currentLight.color = lightEstimation.colorCorrection.Value;
    }
}