using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    void obstacleHit(GameObject obs, float speed);
    void poleHit(GameObject obs);
    void humanHit(GameObject obs);
    void gameOver(GameObject obs);
    void finish(GameObject obs);
}
