using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State
    {
        Roaming
    }

    State state;
    EnemyPathfinding enemyPathfinding;

    void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    void Start()
    {
        StartCoroutine(RomaingRoutine());
    }

    IEnumerator RomaingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2);
        }
    }

    Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
