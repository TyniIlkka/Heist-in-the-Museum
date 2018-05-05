using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField, Tooltip("List of tracks")]
        private List<AudioClip> m_lTracks;
        [SerializeField, Tooltip("Time between tracks")]
        private float m_fDelay;
        [SerializeField, Range(0, 1), Tooltip("Playback progress")]
        private float m_fProgress;
        [SerializeField, Tooltip("Pause playback")]
        private bool m_bPause;

        private AudioSource m_asAudioSource;
        private int m_iCurrentTrack = 0;
        private float m_fOldProgress;
        private float m_fWaitStartTime;

        private float m_fDefaultVol = 0.5f;
        private float m_fMasterVol;
        private float m_fAudioVol;
        private float m_fSfxVol;

        public float MusicVol
        {
            get { return m_fAudioVol; }
            set { m_fAudioVol = value; }
        }

        public float SfxVol
        {
            get { return m_fSfxVol; }
            set { m_fSfxVol = value; }
        }

        public float MasterVol
        {
            get { return m_fMasterVol; }
            set { m_fMasterVol = value; }
        }

        public float SFXPlayVol
        {
            get { return (m_fSfxVol * m_fMasterVol); }
        }

        public float MusicPlayVol
        {
            get { return (m_fAudioVol * m_fMasterVol); }
        }

        private void Awake()
        {
            if (instance == null)
            {                
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            GameManager.instance.audioManager = this;

            Init();
        }        

        private void Init()
        {
            m_asAudioSource = GetComponent<AudioSource>();

            // TODO Load volume from save file?
            // Else use default value.
            m_fMasterVol = m_fDefaultVol;
            m_fAudioVol = m_fDefaultVol;
            m_fSfxVol = m_fDefaultVol;

            m_asAudioSource.volume = MusicPlayVol;

            if (m_fDelay < 0)
                m_fDelay = 0;
        }
        
        private void Update()
        {            
            if (m_asAudioSource.isPlaying)            
                UpdateWhenPlaying();
            
            
            else if (!m_bPause)
                UpdateWhenNotPlaying();
                        
            else if (m_fProgress == 1)            
                UpdateBetweenTracks();
            

            m_asAudioSource.volume = MusicPlayVol;            
        }

        /// <summary>
        /// Updates when the track is playing.
        /// </summary>
        private void UpdateWhenPlaying()
        {
            // The playback is paused in the editor
            if (m_bPause)
            {
                Pause();
                return;
            }

            // The playback progresses normally
            if (m_fProgress == m_fOldProgress)
            {
                m_fProgress = m_asAudioSource.time / m_lTracks[m_iCurrentTrack].length;
                m_fOldProgress = m_fProgress;
            }
            // If the playback progress has been changed in the
            // editor, the playback time is adjusted accordingly
            else
            {
                SetProgress(m_fProgress);
            }
        }

        /// <summary>
        /// Updates when the track is unpaused or over.
        /// </summary>
        private void UpdateWhenNotPlaying()
        {
            // The playback is unpaused in the editor
            if (m_fProgress < 0.99f)
            {
                Unpause();

                // If the track had ended to a
                // fade-out, it's restarted
                if (!m_asAudioSource.isPlaying)
                {
                    Play();
                }
            }
            // The track is over
            else
            {
                Finish();
            }
        }

        /// <summary>
        /// Updates between tracks.
        /// </summary>
        private void UpdateBetweenTracks()
        {
            // The next track starts if enough time has passed
            if ((Time.time - m_fWaitStartTime) >= m_fDelay)
            {
                Reset();
                NextTrack();
                Play();
            }
           
        }

        /// <summary>
        /// Sets the value of the progress bar.
        /// </summary>
        /// <param name="progress">the value of the progress bar</param>
        private void SetProgress(float progress)
        {
            m_asAudioSource.time = progress * m_lTracks[m_iCurrentTrack].length;
            m_fOldProgress = progress;
        }

        /// <summary>
        /// Starts playing the currently selected track.
        /// </summary>
        public void Play()
        {
            PlayTrack(m_iCurrentTrack);
        }

        /// <summary>
        /// Starts playing a certain track.
        /// </summary>
        /// <param name="trackNum">the track's number in the rack list</param>
        public void PlayTrack(int trackNum)
        {
            if (m_lTracks.Count > 0 && trackNum < m_lTracks.Count)
            {
                if (m_bPause)
                {
                    m_bPause = false;
                }

                m_iCurrentTrack = trackNum;
                m_asAudioSource.clip = m_lTracks[m_iCurrentTrack];
                m_asAudioSource.Play();
            }
        }

        /// <summary>
        /// Stops playback and resets the track.
        /// </summary>
        public void Stop()
        {
            m_asAudioSource.Stop();
            m_bPause = true;
            Reset();
        }

        /// <summary>
        /// Resets the track.
        /// </summary>
        private void Reset()
        {
            m_asAudioSource.time = 0;
            m_fProgress = 0;
            m_fOldProgress = 0;
        }

        /// <summary>
        /// Finishes the currently playing track.
        /// </summary>
        private void Finish()
        {
            m_fProgress = 1;
            m_bPause = true;
            m_fWaitStartTime = Time.time;
        }

        /// <summary>
        /// Pauses playback.
        /// </summary>
        public void Pause()
        {
            m_asAudioSource.Pause();
            m_bPause = true;
        }

        /// <summary>
        /// Unpauses playback.
        /// </summary>
        public void Unpause()
        {
            m_asAudioSource.UnPause();
            m_bPause = false;
        }

        /// <summary>
        /// Selects the next track in the list.
        /// </summary>
        private void NextTrack()
        {
            if (m_lTracks.Count > 0)
            {
                m_iCurrentTrack++;
                if (m_iCurrentTrack >= m_lTracks.Count)
                {
                    m_iCurrentTrack = 0;
                }
            }
        }

        /// <summary>
        /// Selects the previous track in the list.
        /// </summary>
        private void PrevTrack()
        {
            if (m_lTracks.Count > 0)
            {
                m_iCurrentTrack--;
                if (m_iCurrentTrack < 0)
                {
                    m_iCurrentTrack = m_lTracks.Count - 1;
                }
            }
        }
    }
}