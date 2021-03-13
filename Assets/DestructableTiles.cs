using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableTiles : MonoBehaviour
{
    public Player player;
    public Tilemap DestrTM;
    public Grid DestrGrid;
    // Start is called before the first frame update
    void Start()
    {
        DestrTM = GetComponent<Tilemap>();

        //can i get handle to player?
    }

    private void OnCollisionEnter2D(Collision2D collision) //do trigger for under player
        /*
         * function to destroy all tiles under player (in player)
         * whenever collision happens (onDig)
         * rectangle under player (scan blocks and then remove)
         * 
         * 
         * 
         * max number of blocks
         * get multiple casts from player collider
         * player asks grid how big the map is
         * then decide how many collisions i want
         * 
         * ground check -> dig check
         * 
         * 
         * 
         * 
         * 
         */
    {
        player = collision.gameObject.GetComponent<Player>();
        Debug.Log("collision with tag: " + collision.gameObject.tag);
        if (player)
        {
            Debug.Log(collision.gameObject.name);
            Debug.Log("yes");
            string plyDir = player.getDirection();
            Transform[] digPoints = player.GetDigCheckPoints(plyDir);

            if (digPoints != null)
            { 

                for (int i = 0; i < digPoints.Length; i++)
                {
                    Vector3Int digHitPos = DestrTM.WorldToCell(digPoints[i].position);
                    Debug.Log("hitPos = " + digHitPos);
                    Debug.Log("WorldToCell = " + DestrTM.WorldToCell(digHitPos));

                    //Debug.Log("realHitPos = " + realHitPos);
                    //Debug.Log("Tile before = " + DestrTM.GetTile(realHitPos));
                    DestrTM.SetTile(digHitPos, null);
                    //Debug.Log("Tile after = " + DestrTM.GetTile(realHitPos));
                }
            }
            //ContactPoint2D[] contacts = contactArr(collision);



            //foreach (ContactPoint2D hit in collision.contacts) //meant to clear out ALL collisions but doesnt past 2
            ////for (int i = 0; i<contacts.Length; i++)
            //{

            //    //string playerDirection = player.getDirection();
            //    Debug.Log("collision contacts = " + collision.contacts);
            //    Debug.Log("collision contacts length = " + collision.contacts.Length);
                

            //    Vector3 hitPos = Vector3.zero;
            //    Vector3Int realHitPos = Vector3Int.zero;

            //    //ContactPoint2D testHit = contacts[i]; //testing

            //    float angle = Vector3.Angle(hit.normal, Vector3.forward);
            //    //float angle = Vector3.Angle(testHit.normal, Vector3.forward);


            //    Vector3 contactPoint = collision.contacts[0].point;
            //    Vector3 center = collision.collider.bounds.center;

               


            //    if (contactPoint.x < center.x) //hit from right
            //    {
            //        //hitPos.x = hit.point.x - 0.01f;
            //        /*
            //        if (playerDirection == "left") //if the player is moving in the left direction
            //        {
            //            hitPos.x = testHit.point.x - 0.01f;
            //        }
            //        */
            //        //hitPos.x = testHit.point.x - 0.01f;
            //        hitPos.x = hit.point.x - 0.01f;
            //        //realHitPos = DestrTM.WorldToCell(hitPos);
            //        //DestrTM.SetTile(realHitPos, null);
            //    } 
            //    if (contactPoint.x > center.x)
            //    {
            //        //hitPos.x = hit.point.x + 0.01f; //hit from left
            //        /*
            //        if (playerDirection == "right")
            //        {
            //            hitPos.x = testHit.point.x + 0.01f;
            //        }
            //        */
            //        //hitPos.x = testHit.point.x + 0.01f;
            //        hitPos.x = hit.point.x + 0.01f;
            //        //realHitPos = DestrTM.WorldToCell(hitPos);
            //        //DestrTM.SetTile(realHitPos, null);
            //    }
            //    if (contactPoint.y < center.y) //hit from top
            //    {
            //        //hitPos.y = hit.point.y - 0.01f;
            //        //hitPos.y = testHit.point.y - 0.01f;
            //        hitPos.y = hit.point.y - 0.01f;
            //        //realHitPos = DestrTM.WorldToCell(hitPos);
            //        //DestrTM.SetTile(realHitPos, null);
            //    }
            //    if (contactPoint.y > center.y)
            //    {
            //        //hitPos.y = hit.point.y + 0.01f; //hit from below
            //        //hitPos.y = testHit.point.y + 0.01f;
            //        hitPos.y = hit.point.y + 0.01f;
            //        //realHitPos = DestrTM.WorldToCell(hitPos);
            //        //DestrTM.SetTile(realHitPos, null);
            //    }





                
            //    realHitPos = DestrTM.WorldToCell(hitPos);
            //    Debug.Log("hitPos = "+hitPos);
            //    Debug.Log("WorldToCell = "+DestrTM.WorldToCell(hitPos));

            //    Debug.Log("realHitPos = " + realHitPos);
            //    Debug.Log("Tile before = " + DestrTM.GetTile(realHitPos));
            //    DestrTM.SetTile(realHitPos, null);
            //    Debug.Log("Tile after = "+DestrTM.GetTile(realHitPos));
                

            //}
            //for (int i = 0; i < collision.contacts.Length; i++)
            //{
            //    ContactPoint2D test = collision.contacts[i];
            //    Debug.Log(("blocks = ") + test.point);
            //    Debug.DrawLine(player.transform.position, test.point, Color.red, 60f);
            //}
        }
    }

    /*
    private ContactPoint2D[] contactArr(Collision2D collision)
    {
        Debug.Log(collision.contacts.Length);
        for (int i=0; i<collision.contacts.Length; i++)
        {
            ContactPoint2D test = collision.contacts[i];
            Debug.Log(("blocks = ")+test.point);
        }
        return collision.contacts;
    }
    */


}
