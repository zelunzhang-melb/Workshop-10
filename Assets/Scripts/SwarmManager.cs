// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System.Collections;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    // External parameters/variables
    [SerializeField] private GameObject enemyTemplate;
    [SerializeField] private int enemyRows;
    [SerializeField] private int enemyCols;
    [SerializeField] private float enemySpacing;
    [SerializeField] private float stepSize;
    [SerializeField] private float stepTime;
    [SerializeField] private float leftBoundaryX;
    [SerializeField] private float rightBoundaryX;

    // Set to either -1 or 1 to denote the current swarm movement direction.
    private int _direction;

    private void Start()
    {
        // Initialise the swarm by instantiating enemy prefabs.
        GenerateSwarm();

        // Start moving towards the right (positive x-axis).
        this._direction = 1; 
        
        // Start swarm at the far left.
        transform.localPosition = new Vector3(this.leftBoundaryX, 0f, 0f);

        // Use a coroutine to periodically step the swarm. Coroutines are worth
        // learning about if you are unfamiliar with them. In Unity they allow
        // us to define sequences that span multiple frames in a very clean way.
        // Although it might look like it, using coroutines is *not* the same as
        // using multithreading! Read more here:
        // https://docs.unity3d.com/Manual/Coroutines.html
        StartCoroutine(StepSwarmPeriodically());
    }
    
    private IEnumerator StepSwarmPeriodically() 
    {
        // Yep, this is an infinite loop, but the gameplay isn't ever "halted"
        // since the function is invoked as a coroutine. It's also automatically 
        // stopped when the game object is destroyed.
        while (true)
        {
            yield return new WaitForSeconds(this.stepTime); // Not blocking!
            StepSwarm();
        }
    }

    // Automatically generate swarm of enemies based on the given serialized
    // attributes/parameters.
    private void GenerateSwarm()
    {
        // Create swarm of enemies in a grid formation
        for (var row = 0; row < this.enemyRows; row++)
        for (var col = 0; col < this.enemyCols; col++)
        {
            var enemyTransform = Instantiate(this.enemyTemplate).transform;
            enemyTransform.parent = transform;
            enemyTransform.localPosition =
                new Vector3(col, 0.0f, row) * this.enemySpacing;
        }
    }

    // Step the swarm across the screen, based on the current direction, or down
    // and reverse when it reaches the edge.
    private void StepSwarm()
    {
        // Compute the left and right swarm side x positions.
        var swarmWidth = (this.enemyCols - 1) * this.enemySpacing;
        var swarmMinX = transform.localPosition.x;
        var swarmMaxX = swarmMinX + swarmWidth;
        
        // Check if the swarm has reached a boundary on either side. If so swarm
        // should move down; otherwise, it should move sideways.
        if ((swarmMinX < this.leftBoundaryX && this._direction == -1) ||
            (swarmMaxX > this.rightBoundaryX && this._direction == 1))
        {
            // Move swarm down and flip direction
            transform.Translate(Vector3.back * this.stepSize);
            this._direction = -this._direction;
        }
        else
        {
            // Move swarm sideways
            transform.Translate(Vector3.right * (this._direction * this.stepSize));
        }
    }
}
