# AI Agents com Unity ML-Agents

Este projeto utiliza o [Unity ML-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents) para treinar agentes inteligentes em ambientes 3D.

## Pré-requisitos

- Unity (versão recomendada: veja [releases do ML-Agents](https://github.com/Unity-Technologies/ml-agents/releases))
- Python 3.6–3.10
- [mlagents](https://pypi.org/project/mlagents/)
- [torch](https://pytorch.org/)
- `protobuf<=3.20.3`

## Instalação

### 1. Instale as dependências Python

```sh
python -m venv .venv
.venv\Scripts\activate
pip install --upgrade pip
pip install mlagents torch "protobuf<=3.20.3"
```

### 2. Configure o Unity

- Importe o pacote ML-Agents no seu projeto Unity.
- No GameObject do agente, adicione:
  - **Behavior Parameters**
  - **Decision Requester**
  - Seu script de agente (ex: `MoveOnAgent`)
- Configure as observações e ações conforme necessário.

### 3. Treinamento

No terminal, execute:

```sh
mlagents-learn config.yaml --run-id=MeuTreino
```

Config YAML

- behaviors Define os comportamentos treináveis. O nome (ex: My Behavior) deve bater com o que está no Unity.
- trainer_type Algoritmo usado (ppo, sac, bc). O mais comum é ppo.
- hyperparameters Hiperparâmetros de aprendizado (batch size, taxa de aprendizado, etc).
- network_settings Estrutura da rede neural: quantas camadas e neurônios por camada.
- reward_signals Tipo(s) de sinal de recompensa. O padrão é extrinsic.
- max_steps Quantidade total de passos de simulação antes de parar o treinamento.
- summary_freq Frequência com que os dados de treino são salvos no TensorBoard.

Siga as instruções do console para iniciar o treinamento.

## Referências

- [Documentação oficial ML-Agents](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md)
- [Exemplos de uso](https://github.com/Unity-Technologies/ml-agents/tree/develop/docs)

---

> **Dica:**  
> Certifique-se de que a versão do Python e dos pacotes é compatível com a versão do ML-Agents usada no Unity.
