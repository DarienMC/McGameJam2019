using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float duration = 1;
    public float spawnSpeed = 1;
    public float additionalLength = 1;

    private LineRenderer line;
    private new CapsuleCollider collider;

    public void Set(Vector3 from, Vector3 to)
    {
        StartCoroutine(SpawnLaser(from, to));
    }

    IEnumerator SpawnLaser(Vector3 from, Vector3 to)
    {
        to += (to - from).normalized * additionalLength;

        // Spawn
        line = GetComponent<LineRenderer>();
        collider = GetComponent<CapsuleCollider>();
        float previousRatio = 0;
        Vector3 current = from;
        do
        {
            float currentRatio = previousRatio + spawnSpeed * Time.deltaTime;
            current = Vector3.Lerp(from, to, currentRatio);
            previousRatio = currentRatio;
            /*if (Physics.CheckCapsule(from, current, collider.radius / 2, LayerMask.GetMask("Player")))
            {
                RaycastHit hit;
                Physics.Raycast(from, to - from, out hit, Mathf.Infinity, LayerMask.GetMask("Player"));
                current = hit.point;
            }*/
            line.SetPosition(0, from);
            line.SetPosition(1, current);
            float length = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
            transform.rotation = Quaternion.LookRotation(to - from);
            collider.height = length + 0.5f;
            collider.center = Vector3.forward * length / 2;
            yield return new WaitForEndOfFrame();
        }
        while (previousRatio < 1);

        // Destroy
        yield return new WaitForSeconds(duration);
        do
        {
            float currentRatio = previousRatio - spawnSpeed * Time.deltaTime;
            current = Vector3.Lerp(from, to, currentRatio);
            previousRatio = currentRatio;
            /*if (Physics.CheckCapsule(from, current, collider.radius / 2, LayerMask.GetMask("Player")))
            {
                RaycastHit hit;
                Physics.Raycast(from, to - from, out hit, Vector3.Distance(from, current), LayerMask.GetMask("Player"));
                current = hit.point;
            }*/
            line.SetPosition(0, from);
            line.SetPosition(1, current);
            float length = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
            transform.rotation = Quaternion.LookRotation(to - from);
            collider.height = length + 0.5f;
            collider.center = Vector3.forward * length / 2;
            yield return new WaitForEndOfFrame();
        }
        while (previousRatio > 0);
        Destroy(gameObject);
    }
}
