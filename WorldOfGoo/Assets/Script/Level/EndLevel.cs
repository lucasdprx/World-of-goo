using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private int score;
    public static int _lastSkull = -99;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("win");
        if (collision.gameObject.layer == 7)
        {
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            SetNewNodeOnMonsters(List_Monsters.Instance._listMonsters);
            _lastSkull = collision.GetComponent<MoveMonsters>()._indexSkull;
            print(_lastSkull);
        }
        // next level
    }

    private void SetNewNodeOnMonsters(List<MoveMonsters> monsters)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].enabled)
            {
                monsters[i]._endGame = true;
                score++;
            }
        }
    }

}
