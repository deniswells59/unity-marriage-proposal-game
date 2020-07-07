using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParallax : MonoBehaviour
{
    public Player player;
    public float parallaxUnit = 0.25f;

    float originalX;

    private void Start() {
      this.originalX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
     float parallaxXAxis = this.originalX + (float)(-player.transform.position.x * parallaxUnit);
     transform.position = new Vector2(parallaxXAxis, transform.position.y);
    }
}
