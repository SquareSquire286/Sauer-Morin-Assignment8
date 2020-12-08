using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    ChannelModule handles all functionalities of the DAW console. There are seven buttons and 22 sliders corresponding to 23 different audio effects.
*/

public class ChannelModule : MonoBehaviour
{
    private bool soundEnabled;

    [SerializeField] GameObject channel; // The particular channel that the DAW console controls; this should be one of the Speaker objects in the Unity scene

    [SerializeField] GameObject muteButton; // There are seven buttons used to toggle the audio effect classes on and off
    [SerializeField] GameObject chorusButton;
    [SerializeField] GameObject echoButton;
    [SerializeField] GameObject distortionButton;
    [SerializeField] GameObject reverbButton;
    [SerializeField] GameObject highPassButton;
    [SerializeField] GameObject lowPassButton;

    [SerializeField] GameObject volumeSlider; // There are 22 sliders used to alter the continuously measured values of the class-specific audio effects
    [SerializeField] GameObject dopplerSlider;
    [SerializeField] GameObject distortionSlider;
    [SerializeField] GameObject dryMixSlider;
    [SerializeField] GameObject chorusDelaySlider;
    [SerializeField] GameObject depthSlider;
    [SerializeField] GameObject rateSlider;
    [SerializeField] GameObject wetMix1Slider;
    [SerializeField] GameObject wetMix2Slider;
    [SerializeField] GameObject wetMix3Slider;
    [SerializeField] GameObject lowFrequencySlider;
    [SerializeField] GameObject lowResonanceSlider;
    [SerializeField] GameObject highFrequencySlider;
    [SerializeField] GameObject highResonanceSlider;
    [SerializeField] GameObject decayRatioSlider;
    [SerializeField] GameObject echoDelaySlider;
    [SerializeField] GameObject echoDrySlider;
    [SerializeField] GameObject echoWetSlider;
    [SerializeField] GameObject echoDensitySlider;
    [SerializeField] GameObject modalDensitySlider;
    [SerializeField] GameObject reverbDelaySlider;
    [SerializeField] GameObject reverbLevelSlider;

    private float volumeCalculation; // for each slider, a calculation must be performed on the slider's position relative to its positional range.
    private float dopplerCalculation; // This is then converted to an absolute value in accordance with the limits enforced by the Unity audio system.
    private float distortionCalculation;
    private float dryMixCalculation;
    private float chorusDelayCalculation;
    private float depthCalculation;
    private float rateCalculation;
    private float wetMix1Calculation;
    private float wetMix2Calculation;
    private float wetMix3Calculation;
    private float lowFrequencyCalculation;
    private float lowResonanceCalculation;
    private float highFrequencyCalculation;
    private float highResonanceCalculation;
    private float decayRatioCalculation;
    private float echoDelayCalculation;
    private float echoDryCalculation;
    private float echoWetCalculation;
    private float echoDensityCalculation;
    private float modalDensityCalculation;
    private float reverbDelayCalculation;
    private float reverbLevelCalculation;

    public Text volumeText; // The visible UI elements on the DAW console, which communicate the absolute magnitudes of each audio effect to the user 
    public Text dopplerText;
    public Text distortionText;
    public Text dryMixText;
    public Text chorusDelayText;
    public Text depthText;
    public Text rateText;
    public Text wetMix1Text;
    public Text wetMix2Text;
    public Text wetMix3Text;
    public Text lowFrequencyText;
    public Text lowResonanceText;
    public Text highFrequencyText;
    public Text highResonanceText;
    public Text decayRatioText;
    public Text echoDelayText;
    public Text echoDryText;
    public Text echoWetText;
    public Text echoDensityText;
    public Text modalDensityText;
    public Text reverbDelayText;
    public Text reverbLevelText;

    // Start is called before the first frame update
    void Start()
    {
        soundEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource thisChannel = channel.GetComponent<AudioSource>();
        
        // Buttons are simple - if they are depressed, then the audio effect that they control is enabled, and if they are in the OFF position, then the effect is disabled.

        if (muteButton.GetComponent<ButtonModule>().isPressed) // The mute button sets the track's volume to 0, overriding the volumeCalculation variable explained later in the code.
            soundEnabled = false;
        else soundEnabled = true;

        if (chorusButton.GetComponent<ButtonModule>().isPressed) // If the Chorus button is depressed, then a chorus filter is applied
            channel.GetComponent<AudioChorusFilter>().enabled = true;
        else channel.GetComponent<AudioChorusFilter>().enabled = false;

        if (echoButton.GetComponent<ButtonModule>().isPressed) // If the Echo button is depressed, then an echo filter is applied
            channel.GetComponent<AudioEchoFilter>().enabled = true;
        else channel.GetComponent<AudioEchoFilter>().enabled = false;

        if (distortionButton.GetComponent<ButtonModule>().isPressed) // If the Distortion button is depressed, then a distortion filter is applied
            channel.GetComponent<AudioDistortionFilter>().enabled = true;
        else channel.GetComponent<AudioDistortionFilter>().enabled = false;

        if (reverbButton.GetComponent<ButtonModule>().isPressed) // If the Reverb button is depressed, then a reverb filter is applied
            channel.GetComponent<AudioReverbFilter>().enabled = true;
        else channel.GetComponent<AudioReverbFilter>().enabled = false;

        if (lowPassButton.GetComponent<ButtonModule>().isPressed) // If the Low Pass button is depressed, then a low pass filter is applied
            channel.GetComponent<AudioLowPassFilter>().enabled = true;
        else channel.GetComponent<AudioLowPassFilter>().enabled = false;

        if (highPassButton.GetComponent<ButtonModule>().isPressed) // If the High Pass button is depressed, then a high pass filter is applied
            channel.GetComponent<AudioHighPassFilter>().enabled = true;
        else channel.GetComponent<AudioHighPassFilter>().enabled = false;

        // The calculation of the slider's absolute value is based on the ratio of the difference between the current value and the minimum value to the range - yielding a value between 0 and 1.
        // This relative value is then magnified in cases where the maximum value should not equal 1, and given an offset in cases where the minimum value should not equal 0.

        volumeCalculation = (volumeSlider.transform.position.x - volumeSlider.GetComponent<SliderModule>().GetMinX()) / (volumeSlider.GetComponent<SliderModule>().GetRange()); // volume ranges from 0 to 1, so it does not need to be altered
        dopplerCalculation = ((dopplerSlider.transform.position.x - dopplerSlider.GetComponent<SliderModule>().GetMinX()) / (dopplerSlider.GetComponent<SliderModule>().GetRange())) * 5f; // doppler level ranges from 0 to 5
        distortionCalculation = (distortionSlider.transform.position.x - distortionSlider.GetComponent<SliderModule>().GetMinX()) / (distortionSlider.GetComponent<SliderModule>().GetRange()) * 0.8f; // distortion ranges from 0 to 1, but I introduced a cap of 0.8 to protect the user's ears
        dryMixCalculation = (dryMixSlider.transform.position.x - dryMixSlider.GetComponent<SliderModule>().GetMinX()) / (dryMixSlider.GetComponent<SliderModule>().GetRange()); // dry mix ranges from 0 to 1
        chorusDelayCalculation = ((chorusDelaySlider.transform.position.x - chorusDelaySlider.GetComponent<SliderModule>().GetMinX()) / (chorusDelaySlider.GetComponent<SliderModule>().GetRange()) * 99.9f) + 0.1f; // chorus delay ranges from 0.1 to 100 ms
        depthCalculation = (depthSlider.transform.position.x - depthSlider.GetComponent<SliderModule>().GetMinX()) / (depthSlider.GetComponent<SliderModule>().GetRange()); // depth ranges from 0 to 1
        rateCalculation = (rateSlider.transform.position.x - rateSlider.GetComponent<SliderModule>().GetMinX()) / (rateSlider.GetComponent<SliderModule>().GetRange()) * 20f; // rate ranges from 0 to 20 Hz
        wetMix1Calculation = (wetMix1Slider.transform.position.x - wetMix1Slider.GetComponent<SliderModule>().GetMinX()) / (wetMix1Slider.GetComponent<SliderModule>().GetRange()); // wet mixes range from 0 to 1
        wetMix2Calculation = (wetMix2Slider.transform.position.x - wetMix2Slider.GetComponent<SliderModule>().GetMinX()) / (wetMix2Slider.GetComponent<SliderModule>().GetRange());
        wetMix3Calculation = (wetMix3Slider.transform.position.x - wetMix3Slider.GetComponent<SliderModule>().GetMinX()) / (wetMix3Slider.GetComponent<SliderModule>().GetRange());
        lowFrequencyCalculation = (lowFrequencySlider.transform.position.x - lowFrequencySlider.GetComponent<SliderModule>().GetMinX()) / (lowFrequencySlider.GetComponent<SliderModule>().GetRange()) * 22000f; // low-pass cutoff frequency ranges from 0 to 22000 Hz
        lowResonanceCalculation = ((lowResonanceSlider.transform.position.x - lowResonanceSlider.GetComponent<SliderModule>().GetMinX()) / (lowResonanceSlider.GetComponent<SliderModule>().GetRange()) * 9) + 1; // resonance ranges from 1 to 10
        highFrequencyCalculation = (highFrequencySlider.transform.position.x - highFrequencySlider.GetComponent<SliderModule>().GetMinX()) / (highFrequencySlider.GetComponent<SliderModule>().GetRange()) * 21990f + 10; // high-pass cutoff frequency ranges from 10 to 22000 Hz
        highResonanceCalculation = ((highResonanceSlider.transform.position.x - highResonanceSlider.GetComponent<SliderModule>().GetMinX()) / (highResonanceSlider.GetComponent<SliderModule>().GetRange()) * 9) + 1; // resonance scales from 1 to 10
        decayRatioCalculation = (decayRatioSlider.transform.position.x - decayRatioSlider.GetComponent<SliderModule>().GetMinX()) / (decayRatioSlider.GetComponent<SliderModule>().GetRange()); // decay ratio (decay per delay) ranges from 0 to 1
        echoDelayCalculation = (echoDelaySlider.transform.position.x - echoDelaySlider.GetComponent<SliderModule>().GetMinX()) / (echoDelaySlider.GetComponent<SliderModule>().GetRange()) * 4990 + 10; // echo delay ranges from 10 to 5000 milliseconds
        echoDryCalculation = (echoDrySlider.transform.position.x - echoDrySlider.GetComponent<SliderModule>().GetMinX()) / (echoDrySlider.GetComponent<SliderModule>().GetRange()); // dry mix ranges from 0 to 1
        echoWetCalculation = (echoWetSlider.transform.position.x - echoWetSlider.GetComponent<SliderModule>().GetMinX()) / (echoWetSlider.GetComponent<SliderModule>().GetRange()); // wet mix ranges from 0 to 1
        echoDensityCalculation = (echoDensitySlider.transform.position.x - echoDensitySlider.GetComponent<SliderModule>().GetMinX()) / (echoDensitySlider.GetComponent<SliderModule>().GetRange()) * 100f; // echo density ranges from 0 to 100
        modalDensityCalculation = (modalDensitySlider.transform.position.x - modalDensitySlider.GetComponent<SliderModule>().GetMinX()) / (modalDensitySlider.GetComponent<SliderModule>().GetRange()) * 100f; // modal density ranges from 0 to 100
        reverbDelayCalculation = (reverbDelaySlider.transform.position.x - reverbDelaySlider.GetComponent<SliderModule>().GetMinX()) / (reverbDelaySlider.GetComponent<SliderModule>().GetRange()) * 0.1f; // reverb delay ranges from 0 to 0.1
        reverbLevelCalculation = (reverbLevelSlider.transform.position.x - reverbLevelSlider.GetComponent<SliderModule>().GetMinX()) / (reverbLevelSlider.GetComponent<SliderModule>().GetRange()) * 1000; // reverb level can range from -10000 to 2000, but I limited this range to 0-1000 to protect the user's ears

        // All calculations are rounded to three decimal places for the UI display

        volumeCalculation = (float)System.Math.Round(volumeCalculation, 3);
        dopplerCalculation = (float)System.Math.Round(dopplerCalculation, 3);
        distortionCalculation = (float)System.Math.Round(distortionCalculation, 3);
        dryMixCalculation = (float)System.Math.Round(dryMixCalculation, 3);
        chorusDelayCalculation = (float)System.Math.Round(chorusDelayCalculation, 3);
        depthCalculation = (float)System.Math.Round(depthCalculation, 3);
        rateCalculation = (float)System.Math.Round(rateCalculation, 3);
        wetMix1Calculation = (float)System.Math.Round(wetMix1Calculation, 3);
        wetMix2Calculation = (float)System.Math.Round(wetMix2Calculation, 3);
        wetMix3Calculation = (float)System.Math.Round(wetMix3Calculation, 3);
        lowFrequencyCalculation = (float)System.Math.Round(lowFrequencyCalculation, 3);
        lowResonanceCalculation = (float)System.Math.Round(lowResonanceCalculation, 3);
        highFrequencyCalculation = (float)System.Math.Round(highFrequencyCalculation, 3);
        highResonanceCalculation = (float)System.Math.Round(highResonanceCalculation, 3);
        decayRatioCalculation = (float)System.Math.Round(decayRatioCalculation, 3);
        echoDelayCalculation = (float)System.Math.Round(echoDelayCalculation, 3);
        echoDryCalculation = (float)System.Math.Round(echoDryCalculation, 3);
        echoWetCalculation = (float)System.Math.Round(echoWetCalculation, 3);
        echoDensityCalculation = (float)System.Math.Round(echoDensityCalculation, 3);
        modalDensityCalculation = (float)System.Math.Round(modalDensityCalculation, 3);
        reverbDelayCalculation = (float)System.Math.Round(reverbDelayCalculation, 3);
        reverbLevelCalculation = (float)System.Math.Round(reverbLevelCalculation, 3);

        // The visible UI elements on the DAW console are updated every frame with the most recent calculation.

        volumeText.text = "Volume = " + volumeCalculation;
        dopplerText.text = "Doppler Level = " + dopplerCalculation;
        distortionText.text = "Distortion Level = " + (float)System.Math.Round(distortionCalculation * 1.25f, 3);
        dryMixText.text = "Dry Mix Level = " + dryMixCalculation;
        chorusDelayText.text = "Delay = " + chorusDelayCalculation + " ms";
        depthText.text = "Depth = " + depthCalculation;
        rateText.text = "Modulation Rate = " + rateCalculation + " Hz";
        wetMix1Text.text = "Wet Mix 1 = " + wetMix1Calculation;
        wetMix2Text.text = "Wet Mix 2 = " + wetMix2Calculation;
        wetMix3Text.text = "Wet Mix 3 = " + wetMix3Calculation;
        lowFrequencyText.text = "Cutoff Frequency = " + lowFrequencyCalculation + " Hz";
        lowResonanceText.text = "Resonance = " + lowResonanceCalculation;
        highFrequencyText.text = "Cutoff Frequency = " + highFrequencyCalculation + " Hz";
        highResonanceText.text = "Resonance = " + highResonanceCalculation;
        decayRatioText.text = "Decay Ratio = " + decayRatioCalculation;
        echoDelayText.text = "Delay = " + echoDelayCalculation + " ms";
        echoDryText.text = "Dry Mix = " + echoDryCalculation;
        echoWetText.text = "Wet Mix = " + echoWetCalculation;
        echoDensityText.text = "Echo Density = " + echoDensityCalculation;
        modalDensityText.text = "Modal Density = " + modalDensityCalculation;
        reverbDelayText.text = "Delay = " + reverbDelayCalculation + " s";
        reverbLevelText.text = "Reverb Level = " + reverbLevelCalculation;

        // The calculations are directly applied to the track's audio components.

        if (soundEnabled)
            channel.GetComponent<AudioSource>().volume = volumeCalculation;
        else channel.GetComponent<AudioSource>().volume = 0f; // the case where the mute button is pressed (the button overrides the per-frame calculation)

        thisChannel.dopplerLevel = dopplerCalculation;
        channel.GetComponent<AudioDistortionFilter>().distortionLevel = distortionCalculation;
        channel.GetComponent<AudioChorusFilter>().dryMix = dryMixCalculation;
        channel.GetComponent<AudioChorusFilter>().delay = chorusDelayCalculation;
        channel.GetComponent<AudioChorusFilter>().depth = depthCalculation;
        channel.GetComponent<AudioChorusFilter>().rate = rateCalculation;
        channel.GetComponent<AudioChorusFilter>().wetMix1 = wetMix1Calculation;
        channel.GetComponent<AudioChorusFilter>().wetMix2 = wetMix2Calculation;
        channel.GetComponent<AudioChorusFilter>().wetMix3 = wetMix3Calculation;
        channel.GetComponent<AudioLowPassFilter>().cutoffFrequency = lowFrequencyCalculation;
        channel.GetComponent<AudioLowPassFilter>().lowpassResonanceQ = lowResonanceCalculation;
        channel.GetComponent<AudioHighPassFilter>().cutoffFrequency = highFrequencyCalculation;
        channel.GetComponent<AudioHighPassFilter>().highpassResonanceQ = highResonanceCalculation;
        channel.GetComponent<AudioEchoFilter>().decayRatio = decayRatioCalculation;
        channel.GetComponent<AudioEchoFilter>().delay = echoDelayCalculation;
        channel.GetComponent<AudioEchoFilter>().dryMix = echoDryCalculation;
        channel.GetComponent<AudioEchoFilter>().wetMix = echoWetCalculation;
        channel.GetComponent<AudioReverbFilter>().diffusion = echoDensityCalculation;
        channel.GetComponent<AudioReverbFilter>().density = modalDensityCalculation;
        channel.GetComponent<AudioReverbFilter>().reverbDelay = reverbDelayCalculation;
        channel.GetComponent<AudioReverbFilter>().reverbLevel = reverbLevelCalculation;

        // For more information on Unity's audio modules, visit the Unity Manual: https://docs.unity3d.com/Manual/class-AudioSource.html (other classes can be found in the left-hand menu)
    }
}