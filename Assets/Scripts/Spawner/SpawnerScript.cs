using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] AtomScript player;

    List<GameObject> particles = new List<GameObject>();

    [SerializeField] GameObject[] particlePrefabs;

    [SerializeField] uint maxParticles = 100;
    [SerializeField] float minRadius = 10f;
    [SerializeField] float maxRadius = 30f;

    private void Start()
    {
        SpawnParticles();
    }



    private void Update()
    {
        RemoveParticles();
        SpawnParticles();
    }


    void RemoveParticles()
    {
        for(int i = particles.Count - 1; i > -1; i--)
        {
            //If destroyed or to far away
            if (particles[i] == null || Vector3.Distance(particles[i].transform.position, player.transform.position) > maxRadius )
            {
                Destroy(particles[i].gameObject);
                particles.RemoveAt(i);
            }
        }
    }


    void SpawnParticles()
    {
        for (int i = particles.Count; i < maxParticles; i++)
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            float distance = Random.Range(minRadius, maxRadius);

            Vector3 coordinates = player.transform.position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);

            int prefabIndex = Random.Range(0, particlePrefabs.Length);
            GameObject prefab = particlePrefabs[prefabIndex];

            GameObject newParticle = Instantiate(prefab, coordinates, Quaternion.Euler(0, 0, 0));
            particles.Add(newParticle);
        }
    }
}
