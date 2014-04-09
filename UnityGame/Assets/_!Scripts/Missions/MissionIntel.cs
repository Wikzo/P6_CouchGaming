using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MissionIntel : MissionBase
{
    // Intel base = Target
    // IntelToSteal = prop to steal

    public GameObject IntelPropToSteal;
    public float YOffset = 1.98f;
    private PickUpObject PickUpObject;

    private float counter;
    private float goalTime = 2f;

    public override void InitializeMission(GameObject player, MissionBase Template)
    {
        base.InitializeMission(player, Template);
        counter = 0;
        this.TargetPool = Template.TargetPool; // needs to know about all potential targets (bases) to see if somebody else won before me
        SetTargetBaseAndIntel(Template);
    }

    void SetTargetBaseAndIntel(MissionBase template)
    {
        if (template is MissionIntel)
        {
            MissionIntel intel = (MissionIntel)template;
            this.IntelPropToSteal = intel.IntelPropToSteal;
            PickUpObject = IntelPropToSteal.GetComponent<PickUpObject>();
            //Debug.Log("Successfully casted from MissionBase to MissionIntel");
        }
        else
            Debug.Log("ERROR - could not cast from MissionBase to MissionIntel!");
    }

    void Update()
    {
        if (!isInstanceMission)
            return;

        // stop audio if not playing
        if(GameManager.Instance.PlayingState != PlayingState.Playing && audio.isPlaying)
            audio.Stop();
    }
    
    public override bool MissionAccomplished()
    {
        // winning condition: if intel is inside a base that is MINE

        if (IntelPropToSteal == null) // e.g. another player has already solved mission (destroyed intel)
            _missionIsActive = false;
        else
            _missionIsActive = true;

        // OLD STUFF FOR WHEN INTEL COULD JUST TOUCH BASE WITHOUT PLAYER BEING THERE
        /*Bounds intelBounds = IntelPropToSteal.renderer.bounds;
        
        // if holding object right now, add an offset in collision check (YOffset + Player's height + 0.1f)
        if (PickUpObject.IsPickedUpRightNow)
        {
            intelBounds = new Bounds(IntelPropToSteal.transform.position,
                                      new Vector3(IntelPropToSteal.transform.localScale.x, IntelPropToSteal.transform.localScale.y + PickUpObject.Offset.y + Player.transform.localScale.y + 0.1f,
                                                  IntelPropToSteal.transform.localScale.z));
        }*/

        // bounds intersects looks at if objects touch each other
        if (Target.renderer.bounds.Intersects(Player.renderer.bounds)
            && PickUpObject.PlayerToFollow == Player)
        {
            counter += Time.deltaTime;

            if (!audio.isPlaying)
            {
                gameObject.audio.clip = AudioManager.Instance.IntelKeyboardPressingSound;
                gameObject.audio.loop = true;
                audio.Play();
            }


            if (counter > goalTime)
            {
                audio.Stop();

                PickUpObject.GoToBaseAndStayIdle(new Vector3(IntelPropToSteal.transform.position.x,
                                                             Target.renderer.bounds.max.y,
                                                             IntelPropToSteal.transform.position.z));
                _missionIsActive = false;
                return true;
            }
        }
        else
        {
            audio.Stop();
        }


        return false;
    }
}
