using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool isPlaying = false;

    [Range(0f, 1f)] //functions as a clamp in the inspector. Yay sliders
    public float volume = 0.7f;
    [Range(0f, 1f)]
    public float pitch = 1f;

    //randomness in volume makes game more interesting
    public bool addVariation = true;
    [Range(0f, .25f)]
    public float volumeVarience = 0.05f;
    [Range(0f, .25f)]
    public float pitchVarience = 0.05f;

    private AudioSource source;

    public bool loop = false;

    public void SetSource(AudioSource _source){
        source = _source;
        source.clip = clip; 
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume;
        source.pitch = pitch;
        //Do I even need this???

        if (addVariation)
        {
            source.volume *= (1 + Random.Range(-volumeVarience, volumeVarience));
            source.pitch *= (1 + Random.Range(-pitchVarience, pitchVarience));
        }
       
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField]
    private Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this) Destroy(gameObject);//if the new one is not the same, destroy the original
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this); //keeps audioManager thru scene changes
        }
        //not sure why this is
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name); //YOU CAN RENAME STUFF LIKE THIS??!?!?
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        PlaySound("Intro");
        //ADJUST THIS
    }


    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                sounds[i].isPlaying = true;
                return;
            }
        }

        Debug.Log("ERROR AudioManager: Sound not found in sounds list:" + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        { 
            if (sounds[i].name == _name)
            {
                try
                {
                    sounds[i].Stop();
                    sounds[i].isPlaying = false;
                    Debug.Log("Song - " + _name + " stopped.");
                    return;
                }
                catch { Debug.Log("AudioManager: Sound not found in sounds list:" + _name); }
            }
        }
    }
    public void StopAll()
    {
        //TODO: Option to pause the music instead of just stopping.
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].isPlaying) sounds[i].Stop();
        }
    }
}
