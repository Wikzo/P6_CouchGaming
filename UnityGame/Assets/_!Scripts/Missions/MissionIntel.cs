using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//TODO: Remove collisions when player has intel equipped
[RequireComponent(typeof(AudioSource))]
public class MissionIntel : MissionBase
{
    // Intel base = Target
    // IntelToSteal = prop to steal

    public GameObject IntelPropToSteal;
    PickUpObject pickupObjectScript;
    private PickUpObject PickUpObject;

    private float counter;
    private float goalTime = 5f;

    GameObject ring;
    bool missionIsDone;

    Vector3 ringOffset = new Vector3(0, 2, -5);

    private float TargetOffsetY = 2;

    public override void InitializeMission(GameObject player, MissionBase Template)
    {
        base.InitializeMission(player, Template);
        counter = 0;
        this.TargetPool = Template.TargetPool; // needs to know about all potential targets (bases) to see if somebody else won before me
        SetTargetBaseAndIntel(Template);

        ring = (GameObject)Instantiate(MissionManager.Instance.RingPrefabForIntelMission, this.Player.transform.position, Quaternion.identity);
        ring.renderer.enabled = false;
        ring.name = "_ring_ "+ this.Player;
        ring.transform.localEulerAngles = new Vector3(90, 180, 0);

        pickupObjectScript = IntelPropToSteal.GetComponent<PickUpObject>();
        if (pickupObjectScript == null)
            Debug.Log("ERROR - USB key doesn't have pickup object script");

        missionIsDone = false;
    }

    public override void DestroyMission()
    {
        Destroy(ring);
        base.DestroyMission();
    }

    public override void TemplateSetUp()
    {
        base.TemplateSetUp();
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

    public override void UpdateSpecificMissionStuff()
    {
        base.UpdateSpecificMissionStuff();

        if (!isInstanceMission)
            return;

        // stop audio if not playing
        if (GameManager.Instance.PlayingState != PlayingState.Playing && audio.isPlaying)
            audio.Stop();
    }

    bool RectIntersect(Vector3 aPos, Vector3 aScale, Rect b)
    {
        Rect a = new Rect(aPos.x - aScale.x / 2, aPos.y - aScale.y / 2, aScale.x, aScale.y);

        bool c1 = a.x < b.xMax;
        bool c2 = a.xMax > b.x;
        bool c3 = a.y < b.yMax;
        bool c4 = a.yMax > b.y;

        return c1 && c2 && c3 && c4;
    }

    bool CheckPlayerCollidingWithBaseTarget(Transform player, Transform target)
    {
        bool x1 = player.position.x - player.localScale.x / 2 > target.position.x - target.localScale.x*3;
        bool x2 = player.position.x + player.localScale.x / 2 < target.position.x + target.localScale.x*3;

        bool y1 = player.position.y - player.localScale.y / 2 > target.position.y - target.localScale.y*2 - TargetOffsetY;
        bool y2 = player.position.y + player.localScale.y / 2 < target.position.y + target.localScale.y*2 + TargetOffsetY;

        return x1 && x2 && y1 && y2;

    }

    void CheckOnTriggerStayViaPlayer()
    {
        if (!PickUpObject.PlayerToFollow == Player) // only if holding USB intel right now
        {
            counter = 0;
            return;
        }

        if (!CheckPlayerCollidingWithBaseTarget(this.Player.transform, this.Target.transform)) // only if colliding with target base
        {
            ring.renderer.enabled = false;
            audio.Stop();
            missionIsDone = false;

            //pickupObjectScript.PutIntelOnTerminalScreen(false, this.Target.transform.position);

            counter = 0;
            return;
        }

        //pickupObjectScript.PutIntelOnTerminalScreen(true, this.Target.transform.position);

        ring.renderer.enabled = true;

        counter += Time.deltaTime;

        ring.renderer.enabled = true;
        ring.renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, goalTime, counter));


        if (!audio.isPlaying)
        {
            gameObject.audio.clip = AudioManager.Instance.IntelKeyboardPressingSound;
            gameObject.audio.loop = true;
            audio.Play();
        }


        if (counter > goalTime)
        {
            audio.Stop();

            PickUpObject.GoToBaseAndStayIdle();
            _missionIsActive = false;
            missionIsDone = true;
        }
    }

    public override bool MissionAccomplished()
    {
        // winning condition: if intel is inside a base that is MINE

        //print("last hit: " + collisionDetect.LastTriggerStayName + "; myTarget: " + this.Target.name);


        if (IntelPropToSteal == null) // e.g. another player has already solved mission (destroyed intel)
            _missionIsActive = false;
        else
            _missionIsActive = true;

        ring.transform.position = this.Player.transform.position + ringOffset;

        CheckOnTriggerStayViaPlayer();
        //CheckOnTriggerExitViaPlayer();

        return missionIsDone;

        // OLD STUFF FOR WHEN INTEL COULD JUST TOUCH BASE WITHOUT PLAYER BEING THERE
        /*Bounds intelBounds = IntelPropToSteal.renderer.bounds;
        
        // if holding object right now, add an offset in collision check (YOffset + Player's height + 0.1f)
        if (PickUpObject.IsPickedUpRightNow)
        {
            intelBounds = new Bounds(IntelPropToSteal.transform.position,
                                      new Vector3(IntelPropToSteal.transform.localScale.x, IntelPropToSteal.transform.localScale.y + PickUpObject.Offset.y + Player.transform.localScale.y + 0.1f,
                                                  IntelPropToSteal.transform.localScale.z));
        }*/

        /*ring.transform.position = this.Player.transform.position;
        ring.renderer.enabled = true;

        Renderer playerRenderer;
        if(Player.GetComponent<Player>().PlayerBodyRenderer != null)
            playerRenderer = Player.GetComponent<Player>().PlayerBodyRenderer;
        else
            playerRenderer = Player.renderer;

        // bounds intersects looks at if objects touch each other
        if (Target.renderer.bounds.Intersects(playerRenderer.bounds)
            && PickUpObject.PlayerToFollow == Player)
        {
            counter += Time.deltaTime;

            ring.transform.position = this.Player.transform.position;
            ring.renderer.enabled = true;

            ring.renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, goalTime, counter)); 


            if (!audio.isPlaying)
            {
                gameObject.audio.clip = AudioManager.Instance.IntelKeyboardPressingSound;
                gameObject.audio.loop = true;
                audio.Play();
            }


            if (counter > goalTime)
            {
                audio.Stop();

                PickUpObject.GoToBaseAndStayIdle();
                _missionIsActive = false;

                return true;

            }
        }
        else
        {
            ring.renderer.enabled = false;
            audio.Stop();
        }


        return false;*/
    }
}
