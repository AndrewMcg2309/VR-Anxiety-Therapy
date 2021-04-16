using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Raycast : MonoBehaviour
{
    // eplosion pieces
    public float cubeSize = 0.2f;
    public int cubesInRow = 3;
    float cubesDistance;
    Vector3 cubesPivot;

    // explosion forces
    public float explosionForce = 50f;
    public float explosionRadius = 5f;
    public float upwardForce = 0.4f;

    // audio
    AudioSource audioSource;

    // color grab 
    Color color;

    // score
    int score;
    public Text scoreText;
    

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast (ray, out hitInfo, 100))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            if(hitInfo.collider.tag == "Cube")
            {
                // color
                color = hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.color;

                // destroy object
                Destroy(hitInfo.transform.gameObject);
                
                // audio
                audioSource.Play(0);
                
                // explode
                Explode(hitInfo.transform.position, color);

                // add point
                AddScore();
            }
        }      
    }

    void AddScore()
    {
        score++;
        scoreText.text = "" + score;
    }

    void Explode(Vector3 pos, Color objectColor)
    {
        // loop 3 times for small cubes generation
        for (int x = 0; x < cubesInRow; x++) 
        {
            for (int y = 0; y < cubesInRow; y++) 
            {
                for (int z = 0; z < cubesInRow; z++) 
                {
                    createPiece(pos.x + (x * cubeSize), pos.y + (y * cubeSize), pos.z + (z * cubeSize), objectColor);
                }
            }
        }

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(pos, explosionRadius);

        // explosion
        foreach (Collider hit in colliders) 
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null) 
            {
                // add forces to cubes
                rb.AddExplosionForce(explosionForce, pos, explosionRadius, upwardForce);
            }
        }
    }

    // Use this for initialization
    void Start() 
    {
        //initialize score
        score = 0;

        audioSource = GetComponent<AudioSource>();


        // calculate pivot distance
         // use this value to create pivot vector)
         
        cubesDistance = cubeSize * cubesInRow / 2;
       
        cubesPivot = new Vector3(cubesDistance, cubesDistance, cubesDistance);
    }

    void createPiece(float x, float y, float z, Color objectColor) 
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.layer = 8;

        // assign colour
        piece.GetComponent<Renderer>().material.color = objectColor;

        // set piece position and size
        piece.transform.position = new Vector3(x, y, z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        // add rigidbody 
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        //destoy
        Destroy(piece, 1);
    }
}