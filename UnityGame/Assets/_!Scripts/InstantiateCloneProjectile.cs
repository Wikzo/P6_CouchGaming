using UnityEngine;
using System.Collections;

public class InstantiateCloneProjectile : MonoBehaviour
{
    public GameObject ProjectileToSpawn;

    private GameObject CloneProjectile;

    private GameObject originalParents;

    public GameObject MakeProjectileClone(GameObject parent)
    {
        CloneProjectile = (GameObject)Instantiate(ProjectileToSpawn, transform.position, Quaternion.identity);
        CloneProjectile.name = parent.name;
        Destroy(CloneProjectile.GetComponent<TrailRenderer>());
        //CloneProjectile.rigidbody.isKinematic = true;

        gameObject.AddComponent<ScreenWrapping>();
        ScreenWrapping s = gameObject.GetComponent<ScreenWrapping>();

        s.Clone = CloneProjectile.transform;
        s.RootToDetectScreenEdge = gameObject;
        s.OriginalToFollow = gameObject;
        s.UseRotation = true;

        CloneProjectile.GetComponent<Projectile>().TwinProjectileToDestroy = parent;

        return CloneProjectile;
    }

    //This function is called when the MonoBehaviour will be destroyed. - https://docs.unity3d.com/Documentation/ScriptReference/MonoBehaviour.OnDestroy.html
    private void OnDestroy()
    {
        if (CloneProjectile != null)
        {
            Destroy(CloneProjectile);
        }
    }
}
