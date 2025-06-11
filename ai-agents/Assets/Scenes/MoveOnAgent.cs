using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveOnAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform startPoint;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Usa os valores de ação contínua (ML-Agents controla isso durante o treinamento)
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 move = new Vector3(moveX, 0, moveZ);
        transform.position += move * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Coleta input manual do teclado para testes
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        // Usa o novo sistema de input apenas aqui
        float moveX = 0f;
        float moveZ = 0f;

        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed) moveX = 1f;
            if (Keyboard.current.wKey.isPressed) moveZ = 1f;
            if (Keyboard.current.sKey.isPressed) moveZ = -1f;
        }

        actions[0] = moveX;
        actions[1] = moveZ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("goal"))
        {
            SetReward(+1f);
            EndEpisode();
        }

        if (other.CompareTag("wall"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}