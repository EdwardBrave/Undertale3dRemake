using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkHandler : MonoBehaviour
{
    private const string discordLink = "https://discord.gg/HPS5fk";
    private const string youtubeLink = "https://www.youtube.com/channel/UCeXv4vbazL0GT_Hjzz8Cjeg";
    private const string gameJoltLink = "https://gamejolt.com/games/undertale3d-remake/452723";
    
    public void OpenDiscordLink()
    {
        Application.OpenURL(discordLink); 
    }
    
    public void OpenYoutubeLink()
    {
        Application.OpenURL(youtubeLink); 
    }
    
    public void OpenGameJoltLink()
    {
        Application.OpenURL(gameJoltLink); 
    }

    private void Update()
    {
        // if (Input.GetKey(KeyCode.Escape))
        // {
        //     Application.Quit();
        // }
    }
}
