using System;
using Discord;
using Discord.Net;
using NAudio.Wave;
using Discord.Audio;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using DiscordUser = Discord.User;
namespace HiT.VoIPSoundboard.Soundboards
{
    public class DiscordSoundboard : ISoundboard
    {
        bool ready;
        bool playing;
        string email;
        bool hasLogin;
        string password;
        Thread playThread;
        DiscordUser activeMe;
        DiscordClient client;
        ulong followingUserID;
        IAudioClient voiceClient;
        List<DiscordUser> meClones;
        DiscordUser activeFollowingUser;
        List<DiscordUser> followingUserClones;
        public DiscordSoundboard()
        {
            this.meClones = new List<User>();
            this.client = new DiscordClient();
            this.client.Ready += client_Ready;
            this.client.AddService<AudioService>();
            this.followingUserClones = new List<DiscordUser>();
        }
        public DiscordSoundboard(string email, string password, ulong followingUserID) : this()
        {
            this.followingUserID = followingUserID;
            SetLogin(email, password);
        }
        private void client_Ready(object sender, EventArgs e)
        {
            int currMatchingServerIndex = 0;
            bool foundFollowingUserClone;
            int activeServerIndex = -1;
            bool foundMyClone;
            ready = false;
            meClones.Clear();
            followingUserClones.Clear();
            foreach (var currServer in client.Servers)
            {
                foundMyClone = false;
                foundFollowingUserClone = false;
                foreach (var currUser in currServer.Users)
                {
                    if (currUser.Id == followingUserID)
                    {
                        followingUserClones.Add(currUser);
                        if (currUser.VoiceChannel != null)
                        {
                            activeServerIndex = currMatchingServerIndex;
                        }
                        foundFollowingUserClone = true;
                        currMatchingServerIndex++;
                        if (foundMyClone)
                        {
                            break;
                        }
                    }
                    else if (currUser.Id == client.CurrentUser.Id)
                    {
                        foundMyClone = true;
                        meClones.Add(currUser);
                        if (foundFollowingUserClone)
                        {
                            break;
                        }
                    }
                }
                if (!foundFollowingUserClone) //This can happen... Say you left the server the bot is on but the bot doesn't leave it
                {
                    meClones.RemoveAt(meClones.Count - 1);
                }
            }
            if (activeServerIndex != -1)
            {
                activeFollowingUser = followingUserClones[activeServerIndex];
                JoinVoiceChannel(activeFollowingUser.VoiceChannel); //We know for a fact the voice channel is not null
                activeMe = meClones[activeServerIndex];
            }
            //await client.CurrentUser.Edit(avatar = );
            client.SetGame("Tilting Simulator");
            ready = true;
        }
        public DiscordClient Client
        {
            get
            {
                return client;
            }
        }
        public DiscordUser ActiveFollowingUser
        {
            get
            {
                return activeFollowingUser;
            }
            set
            {
                followingUserID = value.Id;
                activeFollowingUser = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
        }
        public bool HasLogin
        {
            get
            {
                return hasLogin;
            }
        }
        public async void SoundboardService()
        {
            if (client.State == ConnectionState.Connected)
            {
                if (activeFollowingUser != null && activeFollowingUser.VoiceChannel != null) 
                {
                    if (ready && activeMe.VoiceChannel != activeFollowingUser.VoiceChannel) //Am I in the same voice channel as the activeFollowingUser?
                    {
                        JoinVoiceChannel(activeFollowingUser.VoiceChannel);
                    }
                }
                else if (ready)
                {
                    DiscordUser currFollowingUser;
                    for (int currUserCloneIndex = 0; currUserCloneIndex < followingUserClones.Count; currUserCloneIndex++)
                    {
                        currFollowingUser = followingUserClones[currUserCloneIndex];
                        if (currFollowingUser.VoiceChannel != null)
                        {
                            activeFollowingUser = currFollowingUser;
                            activeMe = meClones[currUserCloneIndex];
                            break;
                        }
                    }
                }
            }
            else if (hasLogin && client.State == ConnectionState.Disconnected)
            {
                try
                {
                    await client.Connect(email, password);
                }
                catch (HttpException)
                {
                    MessageBox.Show("ERROR: Wrong login credentials", "ERROR: Invalid login.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //password = String.Empty;
                    //email = String.Empty;
                    hasLogin = false;
                }
                catch (Exception ex)
                {
                    Utils.ShowUnhandledException(ex, "DiscordSoundboard->SoundboardService");
                }
            }
        }
        public void PlayFile(string filePath, WaveFileReader waveFileReader)
        {
            var stream = new WaveFormatConversionStream(new WaveFormat(48000, 16, client.GetService<AudioService>().Config.Channels), waveFileReader);
            playThread = new Thread(() => FilePlayingService(stream));
            playThread.Start();
        }
        private void FilePlayingService(WaveFormatConversionStream stream)
        {
            int blockSize = stream.WaveFormat.AverageBytesPerSecond / 50;
            byte[] buffer = new byte[blockSize];
            int byteCount;
            playing = true;
            while (playing)
            {
                byteCount = stream.Read(buffer, 0, blockSize);
                if (byteCount > 0)
                {
                    if (byteCount < blockSize)
                    {
                        // Incomplete Frame
                        for (int i = byteCount; i < blockSize; i++)
                        {
                            buffer[i] = 0;
                        }
                    }
                    voiceClient.Send(buffer, 0, blockSize);
                }
                else
                {
                    playing = false;
                }
            }
            stream.Dispose();
        }
        public void StopPlaying()
        {
            playing = false;
        }
        public void StartRecording()
        {
            //Save the path in a variable and save received bytes in file
        }
        public void StopRecording()
        {
            //Close the file
        }
        public void SetLogin(string email, string password)
        {
            this.password = password;
            this.email = email;
            hasLogin = true;
        }
        public void SaveSettings(RegistryKey appRegistryKey)
        {
            appRegistryKey.SetValue("DiscordEmail", email, RegistryValueKind.String);
            appRegistryKey.SetValue("DiscordPassword", password, RegistryValueKind.String);
            appRegistryKey.SetValue("DiscordFollowing", followingUserID, RegistryValueKind.QWord);
        }
        public async void JoinVoiceChannel(Channel voiceChannel)
        {
            voiceClient = await client.GetService<AudioService>().Join(voiceChannel);
        }
        public bool IsReady()
        {
            return client.State == ConnectionState.Connected && voiceClient != null && voiceClient.State == ConnectionState.Connected;
        }
    }
}
