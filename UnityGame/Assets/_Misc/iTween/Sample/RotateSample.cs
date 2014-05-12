using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class RotateSample : MonoBehaviour
{

    int note = 12;
    GlitchEffect Glitch;
    float timer;
    bool pitch;
    public ControllerPlayer Player;
    List<ControllerPlayer> ControllerPlayers; // all four players

    public AudioSource MusicAudioSource;

    public List<GameObject> FourCamerasSpots;
    public List<GameObject> FourSlots;
    public List<GameObject> PlaneSlots;
    Vector3[] OriginalScales;
    Vector3[] OriginalPositions;
    public List<RunAnimDummy> CharacterSelect;
    public List<AudioClip> SelectSounds;
    public AudioClip CancelSound;
    public AudioClip ForwardMenuSound;
    public AudioClip BackwardMenuSound;

    
    int[] currentSlots;
    bool[] readyToMoveSlot;
    RunAnimDummy[] MyCharacter;
    int selected;

	void Start()
    {
        Screen.showCursor = false;

        Glitch = gameObject.GetComponent<GlitchEffect>();
        //iTween.RotateTo(gameObject, iTween.Hash("rotation", transform.position, iTween.EaseType.easeInOutSine, "time", 1.3f));
		//iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "easeInOutBack", "loopType", iTween.LoopType.none, "delay", .4));
        
        // make controllers ready
        ControllerPlayers = new List<ControllerPlayer>();
        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            ControllerPlayer p = new ControllerPlayer(testPlayerIndex);
            ControllerPlayers.Add(p);
        }


        // make cameras ready
        currentSlots = new int[4];
        readyToMoveSlot = new bool[4];
        MyCharacter = new RunAnimDummy[4];
        OriginalScales = new Vector3[4];
        OriginalPositions = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            FourCamerasSpots[i].transform.position = FourSlots[i].transform.position;
            currentSlots[i] = i;
            OriginalScales[i] = PlaneSlots[i].transform.localScale;
            OriginalPositions[i] = PlaneSlots[i].transform.position;

            readyToMoveSlot[i] = false;
        }

        selected = 0;

	}

    void UpdateControllerStates()
    {
        foreach (ControllerPlayer p in ControllerPlayers)
        {
            p.UpdateState();
        }
    }

    void Update()
    {

        UpdateControllerStates();

        for (int i = 0; i < 4; ++i)
        {
            ControllerPlayer p = ControllerPlayers[i];

            // check if connected
            if (!p.IsControllerConnected())
            {
                PlaneSlots[i].SetActive(false);
            }
            else
                PlaneSlots[i].SetActive(true);


            if (p.ButtonPressedRightNow(ButtonsToPress.Start))
            {
                //if (counter > 2)
                Application.LoadLevel(1);
            }

            // can move when thumbstick resets
            if (p.LeftStick().x == 0)
                readyToMoveSlot[i] = true;

            // go right (character select)
            if (p.LeftStick().x > 0.7 && readyToMoveSlot[i] && MyCharacter[i] == null)
            {
                if (currentSlots[i] + 1 < 4)
                    currentSlots[i]++;
                else
                    currentSlots[i] = 0;

                FourCamerasSpots[i].transform.position = FourSlots[currentSlots[i]].transform.position;

                readyToMoveSlot[i] = false;
                audio.PlayOneShot(ForwardMenuSound);
            }
            // go left (character select)
            else if (p.LeftStick().x < -0.7 && readyToMoveSlot[i] && MyCharacter[i] == null)
            {
                if (currentSlots[i] - 1 > 0)
                    currentSlots[i]--;
                else
                    currentSlots[i] = 3;

                FourCamerasSpots[i].transform.position = FourSlots[currentSlots[i]].transform.position;

                readyToMoveSlot[i] = false;
                audio.PlayOneShot(BackwardMenuSound);


            }

            // character already selected by another player?
            if (CharacterSelect[currentSlots[i]].Selected && CharacterSelect[currentSlots[i]] != MyCharacter[i])
            {
                PlaneSlots[i].renderer.material.color = new Color(0.1f, 0.1f, 0.1f);
                PlaneSlots[i].GetComponent<CharPlaneMenuNav>().CanUseAButtonRightNow(false);
            
            }
            else if (CharacterSelect[currentSlots[i]] == MyCharacter[i]) // I selected the character
            {
                PlaneSlots[i].renderer.material.color = Color.white;
                PlaneSlots[i].GetComponent<CharPlaneMenuNav>().CanUseAButtonRightNow(false);
            }
            else // character not selected by anybody
            {
                PlaneSlots[i].renderer.material.color = new Color(0.4f, 0.4f, 0.4f);
                PlaneSlots[i].GetComponent<CharPlaneMenuNav>().CanUseAButtonRightNow(true);

            }


            // select character
            if (p.ButtonPressedRightNow(ButtonsToPress.A))
            {
                if (!CharacterSelect[currentSlots[i]].Selected)
                {
                    CharacterSelect[currentSlots[i]].Selected = true;
                    MyCharacter[i] = CharacterSelect[currentSlots[i]];
                    selected++;
                    audio.PlayOneShot(SelectSounds[selected]);

                    PlaneSlots[i].transform.position = OriginalPositions[i];
                    PlaneSlots[i].transform.localScale = OriginalScales[i];

                    PlaneSlots[i].GetComponent<CharPlaneMenuNav>().SetReadyState(true);

                    GoKitTweenExtensions.shake(PlaneSlots[i].transform, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Position);
                }
            }
            // deselect character
            else if (p.ButtonPressedRightNow(ButtonsToPress.B))
            {
                if (CharacterSelect[currentSlots[i]].Selected && CharacterSelect[currentSlots[i]] == MyCharacter[i])
                {
                    audio.PlayOneShot(CancelSound);
                    CharacterSelect[currentSlots[i]].Selected = false;
                    MyCharacter[i] = null;
                    selected--;

                    PlaneSlots[i].transform.position = OriginalPositions[i];
                    PlaneSlots[i].transform.localScale = OriginalScales[i];

                    PlaneSlots[i].GetComponent<CharPlaneMenuNav>().SetReadyState(false);

                    GoKitTweenExtensions.shake(PlaneSlots[i].transform, 0.5f, new Vector3(0.1f, 0.1f, 0.1f), GoShakeType.Scale);

                }
            }

            // rotate screen left (controller)
            if (p.ButtonPressedRightNow(ButtonsToPress.LT) || p.ButtonPressedRightNow(ButtonsToPress.LB))
                iTween.RotateBy(gameObject, iTween.Hash("y", -0.25, "easeType", "easeInOutBack", "onstart", "Pitch", "oncomplete", "GlitchNow", "loopType", iTween.LoopType.none, "delay", .4));

            // rotate screen right (controller)
            else if (p.ButtonPressedRightNow(ButtonsToPress.RT) || p.ButtonPressedRightNow(ButtonsToPress.RB))
                iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "easeInOutBack", "onstart", "Pitch", "oncomplete", "GlitchNow", "loopType", iTween.LoopType.none, "delay", .4));

            // rotate screen left (keyboard)
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                iTween.RotateBy(gameObject, iTween.Hash("y", -0.25, "easeType", "easeInOutBack", "onstart", "Pitch", "oncomplete", "GlitchNow", "loopType", iTween.LoopType.none, "delay", .4));

            // rotate screen right (keyboard)
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "easeInOutBack", "onstart", "Pitch", "oncomplete", "GlitchNow", "loopType", iTween.LoopType.none, "delay", .4));
        }
        //if (Input.GetKeyDown(KeyCode.RightArrow))
		  //  iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "easeInOutBack", "onstart", "Pitch", "oncomplete","GlitchNow", "loopType", iTween.LoopType.none, "delay", .4));
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
          //  iTween.RotateBy(gameObject, iTween.Hash("y", -0.25, "easeType", "easeInOutBack", "onstart", "Pitch", "oncomplete","GlitchNow", "loopType", iTween.LoopType.none, "delay", .4));

        // smash-styled rotation ... does notwork
        /*float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(-y, x, 0);
        transform.RotateAround(transform.position, movement, 50 * Time.deltaTime);*/

        if (pitch)
        {
            timer += Time.deltaTime * 5;
            float fracJourney = timer / 5;
            float lerp = Mathf.Lerp(1, 2f, fracJourney);
            MusicAudioSource.pitch = lerp;

            if (MusicAudioSource.pitch >= 2f)
            {
                pitch = false;
                MusicAudioSource.pitch = 1;
                timer = 0;
            }
        }

        foreach (ControllerPlayer p in ControllerPlayers)
        p.previousState = p.state;

    }

    void Pitch()
    {
        pitch = true;
    }

    void GlitchNow()
    {
        StartCoroutine(EnableGlitch());
    }

    IEnumerator EnableGlitch()
    {
        Glitch.enabled = true;

        yield return new WaitForSeconds(1);
        Glitch.enabled = false;
    }
}

