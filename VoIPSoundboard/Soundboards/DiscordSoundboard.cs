using System;
using Discord;
using Discord.Net;
using NAudio.Wave;
using System.Media;
using Discord.Audio;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using NAudio.CoreAudioApi;
using System.Windows.Forms;
using System.Collections.Generic;
using DiscordUser = Discord.User;
using HiT.VoIPSoundboard.Properties;
namespace HiT.VoIPSoundboard.Soundboards
{
    public class DiscordSoundboard : ISoundboard
    {
        bool ready;
        bool playing;
        string email;
        bool hasLogin;
        WaveIn waveIn;
        string password;
        Thread playThread;
        MMDevice microphone;
        string microphoneID;
        float mutedPeakLevel;
        DiscordUser activeMe;
        DiscordClient client;
        int newMicroponeIndex;
        ulong followingUserID;
        bool notifyMutedTalking;
        IAudioClient voiceClient;
        List<DiscordUser> meClones;
        SoundPlayer talkingMutedSound;
        DiscordUser activeFollowingUser;
        Stopwatch lastNotificationStopwatch;
        List<DiscordUser> followingUserClones;
        public DiscordSoundboard()
        {
            this.waveIn = new WaveIn();
            this.client = new DiscordClient();
            this.client.Ready += client_Ready;
            this.client.AddService<AudioService>();
            this.meClones = new List<DiscordUser>();
            this.followingUserClones = new List<DiscordUser>();
            this.lastNotificationStopwatch = Stopwatch.StartNew();
            this.talkingMutedSound = new SoundPlayer(Resources.MutedNotification);
        }
        public DiscordSoundboard(RegistryKey appRegistryKey) : this()
        {
            string microphoneID;
            object registryValue = appRegistryKey.GetValue("DiscordEmail");
            if (registryValue != null)
            {
                string email = registryValue.ToString();
                registryValue = appRegistryKey.GetValue("DiscordPassword");
                if (registryValue != null)
                {
                    this.email = email;
                    this.password = registryValue.ToString();
                    SetLogin(email, password);
                }
            }
            registryValue = appRegistryKey.GetValue("DiscordFollowing");
            if (registryValue != null)
            {
                this.followingUserID = Convert.ToUInt64(registryValue);
            }
            registryValue = appRegistryKey.GetValue("DiscordMicrophoneID");
            microphoneID = registryValue != null ? registryValue.ToString() : String.Empty;
            if (microphoneID != String.Empty)
            {
                int index;
                this.microphone = MicrophoneHelper.GetMicrophoneFromID(microphoneID, out index);
                microphoneID = this.microphone.ID;
                this.waveIn.DeviceNumber = index;
                this.waveIn.StartRecording();
            }
            registryValue = appRegistryKey.GetValue("DiscordPeakLevel");
            if (registryValue != null)
            {
                this.mutedPeakLevel = (int)registryValue / 100F;
            }
            registryValue = appRegistryKey.GetValue("DiscordMutedTalking");
            if (registryValue != null)
            {
                this.notifyMutedTalking = (int)registryValue == 1;
            }
        }
        private void client_Ready(object sender, EventArgs e)
        {
            int currMatchingServerIndex = 0;
            int activeServerIndex = -1;
            DiscordUser currUser;
            ready = false;
            meClones.Clear();
            followingUserClones.Clear();
            //MessageBox.Show("Client connected!");
            foreach (var currServer in client.Servers)
            {
                currUser = currServer.GetUser(followingUserID);
                if (currUser != null)
                {
                    followingUserClones.Add(currUser);
                    if (currUser.VoiceChannel != null)
                    {
                        activeServerIndex = currMatchingServerIndex;
                    }
                    meClones.Add(currServer.GetUser(client.CurrentUser.Id));
                    currMatchingServerIndex++;
                }
            }
            if (activeServerIndex != -1)
            {
                activeFollowingUser = followingUserClones[activeServerIndex];
                JoinVoiceChannel(activeFollowingUser.VoiceChannel); //We know for a fact the voice channel is not null
                activeMe = meClones[activeServerIndex];
            }
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
        public bool NotifyMutedTalking
        {
            get
            {
                return notifyMutedTalking;
            }
            set
            {
                notifyMutedTalking = value;
            }
        }
        public float MutedPeakLevel
        {
            get
            {
                return mutedPeakLevel;
            }
            set
            {
                mutedPeakLevel = value;
            }
        }
        public MMDevice Microphone
        {
            get
            {
                return microphone;
            }
        }
        public string MicrophoneID
        {
            get
            {
                return microphoneID;
            }
        }
        public void SetMicrophoneFromIndex(int index)
        {
            newMicroponeIndex = index;
        }
        public async void SoundboardService()
        {
            if (newMicroponeIndex >= 0)
            {
                waveIn.StopRecording();
                waveIn.DeviceNumber = newMicroponeIndex;
                microphone = MicrophoneHelper.GetMicrophone(newMicroponeIndex);
                microphoneID = microphone.ID;
                waveIn.StartRecording();
                newMicroponeIndex = -1;
            }
            if (client.State == ConnectionState.Connected && ready)
            {
                if (activeFollowingUser != null && activeFollowingUser.VoiceChannel != null) 
                {
                    if (activeMe.VoiceChannel != activeFollowingUser.VoiceChannel) //Am I in the same voice channel as the activeFollowingUser?
                    {
                        //No, let's join it!
                        JoinVoiceChannel(activeFollowingUser.VoiceChannel);
                    }
                    else if (notifyMutedTalking && microphone != null && activeFollowingUser.IsSelfMuted && microphone.AudioMeterInformation.MasterPeakValue >= mutedPeakLevel &&
                             lastNotificationStopwatch.ElapsedMilliseconds >= 5000) //Yes I am. Do I notify if the user is talking while muted?
                    {
                        lastNotificationStopwatch.Restart();
                        talkingMutedSound.Play();
                    }
                }
                else
                {
                    DiscordUser currFollowingUser;
                    for (int currUserCloneIndex = 0; currUserCloneIndex < followingUserClones.Count; currUserCloneIndex++) //Find the activeFollowingUser
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
                    //MessageBox.Show("Connecting with email: " + email + " | password: " + password);
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
        public void PlayFile(string filePath, WaveStream waveStream)
        {
            var stream = new WaveFormatConversionStream(new WaveFormat(48000, 16, client.GetService<AudioService>().Config.Channels), waveStream);
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
                        //Incomplete Frame
                        for (int currByte = byteCount; currByte < blockSize; currByte++)
                        {
                            buffer[currByte] = 0;
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
        public async void JoinVoiceChannel(Channel voiceChannel)
        {
            voiceClient = await client.GetService<AudioService>().Join(voiceChannel);
        }
        public bool IsReady()
        {
            return client.State == ConnectionState.Connected && voiceClient != null && voiceClient.State == ConnectionState.Connected;
        }
        public void SaveSettings(RegistryKey appRegistryKey)
        {
            appRegistryKey.SetValue("DiscordEmail", email, RegistryValueKind.String);
            appRegistryKey.SetValue("DiscordPassword", password, RegistryValueKind.String);
            appRegistryKey.SetValue("DiscordFollowing", followingUserID, RegistryValueKind.QWord);
            appRegistryKey.SetValue("DiscordPeakLevel", (int)Math.Round(mutedPeakLevel * 100), RegistryValueKind.DWord);
            appRegistryKey.SetValue("DiscordMutedTalking", notifyMutedTalking ? 1 : 0, RegistryValueKind.DWord);
            //appRegistryKey.SetValue("DiscordMicrophoneID", microphone != null ? microphoneID : String.Empty, RegistryValueKind.String);
        }
    }
}
