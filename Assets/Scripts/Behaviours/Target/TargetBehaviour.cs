using UnityEngine;
using System.Collections.Generic;

public class TargetBehaviour : MonoBehaviour
{
    #region Fields
    protected int chance;
    protected float baseScoreWorth;
    protected float scoreWorth;
    protected float lifetimeValue;
    protected float startingLifetimeValue = 3f;
    protected float randomX;
    protected float randomY;
    protected float randomZ;
    protected Vector3 minSpawnPosition = new Vector3(-20f, 2f, 10f);
    protected Vector3 maxSpawnPosition = new Vector3(20f, 12f, 40f);
    protected HealthComponent healthComponent;
    protected MeshRenderer meshRenderer;

    float playerAccuracy;

    // List to store shot distances
    List<float> shotDistances = new List<float>();
    #endregion

    #region Methods
    #region UnityMethods
    protected void Start()
    {
        InitialiseTarget();
    }

    protected void Update()
    {
        lifetimeValue -= Time.deltaTime;
        if (lifetimeValue <= 0)
        {
            ResetTarget();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Make sure we don't collide with other targets
        if (collision.collider.gameObject.tag != "Target")
        {
            float hp = healthComponent.GetHealth();
            if (hp <= 0)
            {
                // Calculate distance from the bullet impact point to the center of the target
                float distanceToCenter = Vector3.Distance(transform.position, collision.GetContact(0).point);

                // Add the distance to the list
                shotDistances.Add(distanceToCenter);

                // Update score based on accuracy, base score worth, and distance
                float accuracyMultiplier = GameManager.Instance.accuracyValue;
                float scalingFactor = 0.5f + 0.5f * playerAccuracy;

                // You can modify this factor based on your requirements
                float distanceFactor = 1.0f;

                // Update the score
                float scoreChange = baseScoreWorth * accuracyMultiplier * scalingFactor * distanceFactor;

                // Update the score
                float score = GameManager.Instance.scoreValue + scoreChange;
                GameManager.Instance.scoreValue = score;
                GameManager.Instance.UpdateScore();

                // "Kill" the target
                meshRenderer.enabled = false;

                Invoke("ResetTarget", 0.2f);
            }
        }
    }
    #endregion

    #region NonUnityMethods
    protected virtual void InitialiseTarget()
    {
        healthComponent = GetComponent<HealthComponent>();
        meshRenderer = GetComponent<MeshRenderer>();
        transform.localScale = transform.localScale * GameManager.Instance.targetSizeMultiplier;
        playerAccuracy = GameManager.Instance.accuracyValue;
        ResetTarget();
    }

    protected virtual void ResetTarget()
    {
        lifetimeValue = startingLifetimeValue;
        playerAccuracy = GameManager.Instance.accuracyValue;
        float scalingFactor = 0.5f + 0.5f * playerAccuracy;

        chance = Random.Range(0, 100);
        if (chance <= 20)
        {
            meshRenderer.material.color = Color.red;
            baseScoreWorth = -1f;
        }
        else
        {
            meshRenderer.material.color = new Color(0, 1, 1, 1);
            baseScoreWorth = 1f;
        }

        scoreWorth = baseScoreWorth * scalingFactor;

        randomX = Random.Range(minSpawnPosition.x, maxSpawnPosition.x);
        randomY = Random.Range(minSpawnPosition.y, maxSpawnPosition.y);
        randomZ = Random.Range(minSpawnPosition.z, maxSpawnPosition.z);

        transform.position = new Vector3(randomX, randomY, randomZ);

        healthComponent.SetHealth(healthComponent.GetMaxHealth());
        meshRenderer.enabled = true;
    }
    #endregion
    #endregion
}
