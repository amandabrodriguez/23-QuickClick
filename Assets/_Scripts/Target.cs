using UnityEngine;

public class Target : MonoBehaviour
{

    private Rigidbody _rigidbody;

    private float minForce = 12f;
    private float maxForce = 16f;
    private float torqueForce = 10f;
    private float xRange = 4f;
    private float ySpawnPos = -6f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(RandomForce(), ForceMode.Impulse);
        _rigidbody.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Genera una fuerza aleatoria hacia arriba dentro de un rango especificado.
    /// </summary>
    /// <returns>Fuerza aleatoria para arriba</returns>
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minForce, maxForce);
    }

    /// <summary>
    /// Genera un torque aleatorio dentro de un rango especificado.
    /// </summary>
    /// <returns>Valor aleatorio entre -torqueForce y torqueForce</returns>
    private float RandomTorque()
    {
        return Random.Range(-torqueForce, torqueForce);
    }

    /// <summary>
    /// Genera una posición de spawn aleatoria dentro de un rango especificado en el eje X.
    /// </summary>
    /// <returns>Posición aleatoria en 3D, con la coordenada z =0 </returns>
    private Vector3 RandomSpawnPosition()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
