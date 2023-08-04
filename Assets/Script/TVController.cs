using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public bool isTVOn = false;
    public Renderer tvRenderer; // Replace with the actual Renderer component of your TV object.
    public VideoClip[] videoClips; // An array to hold multiple videos
    public GameObject menuPanel;
    public TextMeshProUGUI OnOff_text;
    public TextMeshProUGUI CurrentMin_text;
    public TextMeshProUGUI CurrentSec_text;
    public TextMeshProUGUI TotalMin_text;
    public TextMeshProUGUI TotalSec_text;

    private int selectedVideoIndex = 0; // Index to keep track of the currently selected video
    public float skipSeconds = 5f; // Number of seconds to skip forward or backward
    private VideoPlayer videoPlayer;
    public Slider videoSlider;
    private bool isMuted = false; // Track if the video audio is muted
    public Sprite PlayIcon;
    public Sprite PauseIcon;
    public Image image;

    void Start()
    {
        // Get the Renderer component from the TV object
        tvRenderer = GetComponent<Renderer>();
        videoPlayer = GetComponent<VideoPlayer>();
        
        // Ensure the TV is initially off.
        TurnOffTV();
    }

    void Update()
    {
        if (videoPlayer.isPlaying)
        {
            SetCurrentTime();
            SetTotalTime();
        }

       UpdateVideoSlider();

    }

    public void ToggleTV()
    {
        if (isTVOn)
        {
            TurnOffTV();
        }
        else
        {
            TurnOnTV();
        }
    }

    private void TurnOnTV()
    {
        // Turn on the TV 
        isTVOn = true;
        tvRenderer.enabled = true;
        videoPlayer.Play();
        OnOff_text.text = "ON";

        // Add any other logic to handle when the TV turns on
        Debug.Log("TV is now ON!");
    }

    private void TurnOffTV()
    {
        // Turn off the TV 
        isTVOn = false;
        tvRenderer.enabled = false;
        videoPlayer.Pause();
         OnOff_text.text = "OFF";

        // Add any other logic to handle when the TV turns off
        Debug.Log("TV is now OFF!");
    }
    // This function use for Playe and Pause the video.
     public void ToggleVideoPlayback()
    {
        // Check TV is turn NO or OFF
        if(isTVOn)
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                image.sprite = PauseIcon;

                Debug.Log("Video paused.");
            }
            else
            {
                videoPlayer.Play();
                image.sprite = PlayIcon;
                Debug.Log("Video resumed.");
            }
        }
        
     }

    public void Next_Btn()
    {
        if (isTVOn)
        {
            // Increment the index and wrap around to the start if needed
            selectedVideoIndex = (selectedVideoIndex + 1) % videoClips.Length;
            SelectAndPlayVideo(videoClips[selectedVideoIndex]);
        }
        
    }

    public void Menu_Btn()
    {
        if(menuPanel.activeInHierarchy)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(true);
        }
    }

    public void SelectVideo(int selectedVideoIndex)
    {
        if (isTVOn)
        {
            videoPlayer.Stop();
            SelectAndPlayVideo(videoClips[selectedVideoIndex]);
        }
       
    }

    void SelectAndPlayVideo(VideoClip videoClip)
    {
        videoPlayer.clip = videoClip;
        SetTotalTime();
        videoPlayer.Play();
        Debug.Log("Playing selected video.");
    }

    void SetCurrentTime()
    {
        string min = Mathf.Floor((int)videoPlayer.time / 60).ToString("00");
        string sec = ((int)videoPlayer.time % 60).ToString("00");

        CurrentMin_text.text = min;
        CurrentSec_text.text = sec;
        
    }
    void SetTotalTime()
    {
        string min = Mathf.Floor ((int)videoPlayer.clip.length/60).ToString("00");
        string sec = ((int)videoPlayer.clip.length % 60).ToString("00");

        TotalMin_text.text = min;
        TotalSec_text.text = sec;

    }

    void UpdateVideoSlider()
    {
        if (videoPlayer.isPlaying)
        {
            // Update the Slider value based on the current time and total length of the video
            videoSlider.value = (float)videoPlayer.time / (float)videoPlayer.clip.length;
        }
    }

    public void OnVideoSliderValueChanged()
    {
        // Set the VideoPlayer's time based on the Slider's value to scrub through the video
        videoPlayer.time = videoSlider.value * videoPlayer.clip.length;
    }

    public void SkipForward_Btn()
    {
        videoPlayer.time += skipSeconds;
    }

    public void SkipBackward_Btn()
    {
        videoPlayer.time -= skipSeconds;
    }

     public void Mute_Btn()
    {
        isMuted = !isMuted;
        videoPlayer.SetDirectAudioMute(0, isMuted); // SetDirectAudioMute(0, true) mutes the video's audio.
    }

}


