using System.Collections.Generic;
using UnityEngine;

public class MatchLighter : GameItemScript
{
    List<Match> spawnMatches = new List<Match>();

    public override void OnCreate()
    {
    }

    public override void OnActivate()
    {
        Transform player = LevelManager.Instance.player.transform;
        GameObject matchObj = GameObject.Instantiate(data.relativePrefab , player.position, Quaternion.identity, LevelManager.Instance.levelMap.transform);
        
        //投掷
		AudioManager.Instance.PlaySfxWithCD("点火柴", 0.5f);
        matchObj.GetComponent<Rigidbody2D>().AddForce((player.localScale.x < 0 ? -player.right : player.right) * 4f + player.up * 2f, ForceMode2D.Impulse);
        matchObj.GetComponent<Rigidbody2D>().AddForceAtPosition(player.transform.right * 0.05f, Vector2.up, ForceMode2D.Impulse);
        spawnMatches.Add(matchObj.GetComponent<Match>());
        if (spawnMatches.Count > 3)
        {
            spawnMatches[spawnMatches.Count - 3].PutOut();
        }

        //销毁火柴
        if (spawnMatches.Count > 10)
        {
            GameObject.Destroy(spawnMatches[0].gameObject);
            spawnMatches.RemoveAt(0);
        }

        for (int i = spawnMatches.Count - 1; i >= 0; i--)
        {
            if (spawnMatches[i].destroyTime <= 0f)
            {
                GameObject.Destroy(spawnMatches[i].gameObject);
                spawnMatches.RemoveAt(i);
            }
        }
    }

    public override void OnHold()
    {

    }
}