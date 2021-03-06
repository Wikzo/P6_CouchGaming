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

    public int[] playersChosenID;
    int currentPlayersChosenSlotID; // between 0 and 3

    public GameObject RememberControllers;

    public GameObject StartGameText;

    
    int[] currentSlots;
    bool[] readyToMoveSlot;
    RunAnimDummy[] MyCharacter;
    int selected;

	void Start()
    {
        Screen.showCursor = false;
        playersChosenID = new int[4];
        currentPlayersChosenSlotID = 0;

        selected = 0;

        StartGameText.SetActive(false);
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


            if (selected >= 2)
            {
                StartGameText.SetActive(true);

                if (p.ButtonPressedRightNow(ButtonsToPress.Start))
                    StartGameAndFindControllers();
            }
            else
                StartGameText.SetActive(false);


            // can move when thumbstick resets
            if (p.LeftStick().x == 0)
                readyToMoveSlot[i] = true;

            // go right (character select)
            if (p.LeftStick().x > 0.7 && readyToMoveSlot[i] && MyCharacter[i] == null)
            {
                if (currentSlots[i] + 1 <= 3)
                    currentSlots[i]++;
                else
                    currentSlots[i] = 0;

                FourCamerasSpots[i].transform.position = FourSlots[currentSlots[i]].transform.position;

                readyToMoveSlot[i] = false;

                Transform rightArrow = PlaneSlots[i].GetComponent<CharPlaneMenuNav>().ReadyObjects[2].transform;
                rightArrow.localScale = new Vector3(0.2981013f, 0.1465021f, 0.1377661f);
                GoKitTweenExtensions.shake(rightArrow, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Scale);

                audio.PlayOneShot(ForwardMenuSound);
            }
            // go left (character select)
            else if (p.LeftStick().x < -0.7 && readyToMoveSlot[i] && MyCharacter[i] == null)
            {
                if (currentSlots[i] - 1 >= 0)
                    currentSlots[i]--;
                else
                    currentSlots[i] = 3;

                FourCamerasSpots[i].transform.position = FourSlots[currentSlots[i]].transform.position;

                readyToMoveSlot[i] = false;

                Transform leftArrow = PlaneSlots[i].GetComponent<CharPlaneMenuNav>().ReadyObjects[1].transform;
                leftArrow.localScale = new Vector3(0.2981013f, 0.1465021f, 0.1377661f);
                GoKitTweenExtensions.shake(leftArrow, 0.5f, new Vector3(0.2f, 0.2f, 0.2f), GoShakeType.Scale);

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

                    CharacterSelect[currentSlots[i]].PlayerChosenSlot = (int)p.Index;

                    audio.PlayOneShot(SelectSounds[selected]);
                    selected++;



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

                    CharacterSelect[currentSlots[i]].PlayerChosenSlot = -10;


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

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

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

    void StartGameAndFindControllers()
    {
        FindRightControllers.Instance.PlayerSlotsToRemember = new int[4];

        for (int i = 0; i < 4; i++)
        {
            FindRightControllers.Instance.PlayerSlotsToRemember[i] = -10;
        }

        foreach (RunAnimDummy r in CharacterSelect)
        {
            if (r.PlayerChosenSlot != -10)
            {
                FindRightControllers.Instance.PlayerSlotsToRemember[r.MyID] = r.PlayerChosenSlot;
            }
        }

        Application.LoadLevel(1);
    }
}

