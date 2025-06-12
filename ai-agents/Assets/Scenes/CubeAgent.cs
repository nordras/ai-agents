using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

namespace Scenes
{
    public class CubeAgent : Agent
    {
        private Rigidbody agentRb;
        [SerializeField] private Transform targetBall;
        [SerializeField] private float speed = 5f;

        private void Awake()
        {
            agentRb = GetComponent<Rigidbody>();
            if (agentRb == null)
                Debug.LogError("Rigidbody não encontrado no agente!");
            else
                Debug.Log("Rigidbody encontrado com sucesso.");

            if (targetBall == null)
                targetBall = GameObject.FindWithTag("Ball")?.transform;
            if (targetBall == null)
                Debug.LogWarning("targetBall não atribuído nem encontrado por tag!");
            else
                Debug.Log("targetBall atribuído: " + targetBall.name);
        }

        public override void OnEpisodeBegin()
        {
            if (agentRb == null)
                agentRb = GetComponent<Rigidbody>();
            agentRb.linearVelocity = Vector3.zero;
            agentRb.angularVelocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0.5f, 0);

            if (targetBall != null)
            {
                targetBall.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
                Debug.Log($"Nova posição da bola: {targetBall.localPosition}");
            }
            else
            {
                Debug.LogWarning("targetBall está nulo no início do episódio!");
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
            if (targetBall != null)
                sensor.AddObservation(targetBall.localPosition);
            else
                sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(agentRb != null ? agentRb.linearVelocity : Vector3.zero);
            Debug.Log($"Observações coletadas: posAgente={transform.localPosition}, posBola={(targetBall != null ? targetBall.localPosition : Vector3.zero)}, velAgente={(agentRb != null ? agentRb.linearVelocity : Vector3.zero)}");
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            if (actions.ContinuousActions.Length < 2)
            {
                Debug.LogError($"Esperado pelo menos 2 ações contínuas, mas recebido: {actions.ContinuousActions.Length}");
                return;
            }
            float moveX = actions.ContinuousActions[0];
            float moveZ = actions.ContinuousActions[1];
            Debug.Log($"Ação recebida: moveX={moveX}, moveZ={moveZ}");

            if (agentRb != null)
            {
                Vector3 force = new Vector3(moveX, 0, moveZ) * speed;
                agentRb.AddForce(force);
                Debug.Log($"Força aplicada: {force}");
            }
            else
            {
                Debug.LogWarning("agentRb está nulo ao tentar aplicar força!");
            }

            if (targetBall != null && Vector3.Distance(transform.localPosition, targetBall.localPosition) < 1.0f)
            {
                SetReward(1.0f);
                Debug.Log("Recompensa: 1.0f (alcançou a bola)");
                EndEpisode();
            }
            else if (Mathf.Abs(transform.localPosition.x) > 5 || Mathf.Abs(transform.localPosition.z) > 5)
            {
                SetReward(-1.0f);
                Debug.Log("Penalidade: -1.0f (saiu dos limites)");
                EndEpisode();
            }
            else
            {
                AddReward(-0.001f);
                Debug.Log("Penalidade por passo: -0.001f");
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var cont = actionsOut.ContinuousActions;
            float moveX = 0f;
            float moveZ = 0f;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                    moveX = -1f;
                else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                    moveX = 1f;

                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                    moveZ = 1f;
                else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                    moveZ = -1f;
            }
            cont[0] = moveX;
            cont[1] = moveZ;
            Debug.Log($"Heuristic: moveX={moveX}, moveZ={moveZ}");
        }
    }
}