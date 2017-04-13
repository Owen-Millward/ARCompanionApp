using HoloToolkit.Unity.InputModule;
using UnityEngine;



/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
/// 
    public class Interactible : MonoBehaviour, IInputClickHandler
    {
        //Audio
        private AudioSource audioSource;    //Source for audio

        [Tooltip("Audio clips for this object")]
        public AudioClip[] AudioClips;      //Array of audioclips for this object

        //Button
        public enum ButtonType { MIC, SHOCK, PURGE, SHIELD, FUSE, DOOR };   //Different type of buttons

        [Tooltip("Which object is this?")]
        public ButtonType Button;

        //GameObjects
        private PowerUpController pwr;
        private Communicator comm;
        [Tooltip("FuseBox(Handle)")]
        public FuseBox fuseBox;
        [Tooltip("PowerUpManager(Managers)")]
        public PowerupManager PowerMan;
        [Tooltip("FuseBox(Door)")]
        public Door door;

        //Gameplay
        private int charge; //current charge of powerupmanager

        /// <summary>
        /// Initialisation:
        /// 
        /// Get the charge from the powermanager (0)
        /// Add a box collider if none is present 
        /// Get the communicator and powerupcontroller
        /// Enable audio haptic feedback
        /// </summary>
        void Start()
        {
            charge = (int)PowerMan.getCharge();

            // Add a BoxCollider if the interactible does not contain one.
            Collider collider = GetComponentInChildren<Collider>();

            if (collider == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }

            // Get Communicator for starting voice recording
            comm = GameObject.Find("Communicator").GetComponent<Communicator>();

            // Get power up controller for setting spawn flags
            pwr = GameObject.Find("PowerUpAzure").GetComponent<PowerUpController>();

            EnableAudioHapticFeedback();
        }

        /// <summary>
        /// Enables the audio haptic feedback.
        /// </summary>
        private void EnableAudioHapticFeedback()
        {
            //get audio source
            audioSource = GetComponent<AudioSource>();

            //add an audio source is none exists
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            //audio source settings
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }

        /// <summary>
        /// Handles if the fusebox blows while recording 
        /// Acts as if the recording button had been pushed to end recording. 
        /// </summary>
        void Update()
        {
            if (fuseBox.getState() == FuseBox.FuseState.broken)
            {
                if (comm.recording)
                {
                    audioSource.clip = AudioClips[1];
                    audioSource.Play();
                    comm.RecordStop();
                }
            }
        }

        /// <summary>
        /// When the object has been air tapped
        /// </summary>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            //Get the current charge
            charge = (int)PowerMan.getCharge();

            // Which button has been pressed?
            switch (Button)
            {
                //MICROPHONE BUTTON
                case ButtonType.MIC:

                    //IF THE FUSE IS BROKEN - play not working sound
                    if (fuseBox.getState() == FuseBox.FuseState.broken)
                    {
                        audioSource.clip = AudioClips[2];
                        audioSource.Play();
                    }
                    //IF THE FUSE IS WORKING
                    else
                    {
                        //IF RECORDING - play stop recording sound and stop recording
                        if (comm.recording)
                        {
                            audioSource.clip = AudioClips[1];
                            audioSource.Play();
                            comm.RecordStop();
                        }
                        //IF NOT RECORDING - play start recording sound and start recording
                        else
                        {
                            audioSource.clip = AudioClips[0];
                            audioSource.Play();
                            comm.Record();
                        }
                    }

                    break;

                //SHOCKWAVE BUTTON 
                case ButtonType.SHOCK:

                    //IF FUSE IS BROKEN - play not working sound
                    if (fuseBox.getState() == FuseBox.FuseState.broken)
                    {
                        audioSource.clip = AudioClips[1];
                        audioSource.Play();
                    }

                    //IF FUSE IS WORKING
                    else if (fuseBox.getState() == FuseBox.FuseState.working)
                    {
                        //POWERUP IS OFF COOLDOWN & UNLOCKED & TERMINAL IS IDLE - send power up flat is VR play working sound
                        if (PowerMan.powerUps[0].OnCoolDown() == false && PowerMan.powerUps[0].isLocked() == false && PowerMan.getTerminalState() == PowerupManager.TerminalState.Idle)
                        {
                            pwr.ShockButton();
                            audioSource.clip = AudioClips[0];
                            audioSource.Play();
                        }
                        //POWER UP IS ON COOLDOWN OR LOCKED OR TERMINAL IS NOT IDLE
                        else
                        {
                            //POWER UP UNLOCKING
                            if (PowerMan.powerUps[0].isLocked() && PowerMan.getTerminalState() == PowerupManager.TerminalState.Idle && PowerMan.powerUps[0].getChargeRequired() <= PowerMan.getCharge())
                            {
                                audioSource.clip = AudioClips[0];
                            }
                            //BUTTON NOT WORKNG
                            else
                            {
                                audioSource.clip = AudioClips[1];
                            }

                            audioSource.Play();
                        }

                        PowerMan.usePowerUp(0);
                    }
                    break;

                case ButtonType.PURGE:

                    //IF FUSE IS BROKEN - play not working sound
                    if (fuseBox.getState() == FuseBox.FuseState.broken)
                    {
                        audioSource.clip = AudioClips[1];
                        audioSource.Play();
                    }

                    //IF FUSE IS WORKING
                    else if (fuseBox.getState() == FuseBox.FuseState.working)
                    {
                        //POWERUP IS OFF COOLDOWN & UNLOCKED & TERMINAL IS IDLE - send power up flat is VR play working sound
                        if (PowerMan.powerUps[2].OnCoolDown() == false && PowerMan.powerUps[2].isLocked() == false && PowerMan.getTerminalState() == PowerupManager.TerminalState.Idle)
                        {
                            pwr.PurgeButton();
                            audioSource.clip = AudioClips[0];
                            audioSource.Play();
                        }
                        //POWER UP IS ON COOLDOWN OR LOCKED OR TERMINAL IS NOT IDLE
                        else
                        {
                            //POWER UP UNLOCKING
                            if (PowerMan.powerUps[2].isLocked() && PowerMan.getTerminalState() == PowerupManager.TerminalState.Idle && PowerMan.powerUps[2].getChargeRequired() <= PowerMan.getCharge())
                            {
                                audioSource.clip = AudioClips[0];
                            }
                            //BUTTON NOT WORKNG
                            else
                            {
                                audioSource.clip = AudioClips[1];
                            }

                            audioSource.Play();
                        }

                        PowerMan.usePowerUp(2);
                    }
                    break;

                case ButtonType.SHIELD:


                    //IF FUSE IS BROKEN - play not working sound
                    if (fuseBox.getState() == FuseBox.FuseState.broken)
                    {
                        audioSource.clip = AudioClips[1];
                        audioSource.Play();
                    }

                    //IF FUSE IS WORKING
                    else if (fuseBox.getState() == FuseBox.FuseState.working)
                    {
                        //POWERUP IS OFF COOLDOWN & UNLOCKED & TERMINAL IS IDLE - send power up flat is VR play working sound
                        if (PowerMan.powerUps[1].OnCoolDown() == false && PowerMan.powerUps[1].isLocked() == false && PowerMan.getTerminalState() == PowerupManager.TerminalState.Idle)
                        {
                            pwr.SheildButton();
                            audioSource.clip = AudioClips[0];
                            audioSource.Play();
                        }
                        //POWER UP IS ON COOLDOWN OR LOCKED OR TERMINAL IS NOT IDLE
                        else
                        {
                            //POWER UP UNLOCKING
                            if (PowerMan.powerUps[1].isLocked() && PowerMan.getTerminalState() == PowerupManager.TerminalState.Idle && PowerMan.powerUps[1].getChargeRequired() <= PowerMan.getCharge())
                            {
                                audioSource.clip = AudioClips[0];
                            }
                            //BUTTON NOT WORKNG
                            else
                            {
                                audioSource.clip = AudioClips[1];
                            }

                            audioSource.Play();
                        }

                        PowerMan.usePowerUp(1);
                    }
                    break;

                //DOOR - switch between open and closed
                case ButtonType.DOOR:

                    door.useDoor();
                    break;
            }

            //FUSE - if fuse is broken when interacted with fix the fuse and play fixed sound 
            if (fuseBox.getState() == FuseBox.FuseState.broken)
            {
                if (Button == ButtonType.FUSE)
                {
                    fuseBox.fuseFix();
                    audioSource.Play();
                }
            }

          
        }


    }