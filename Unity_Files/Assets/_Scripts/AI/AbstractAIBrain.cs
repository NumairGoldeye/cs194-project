using UnityEngine;
using System.Collections;
using System.Threading;

public abstract class AbstractAIBrain : AIBrain {

	public void PlayTurn() {
		PlayRollPhase();

		SetupTurn();
		TurnState.stateType = TurnStateType.trade; 

		PlayTradePhase();
		TurnState.stateType = TurnStateType.build; 

		PlayBuyPhase();

		TearDownTurn();
		TurnState.EndTurn();
	}

	public abstract void SetupRoad();
	
	public abstract void SetupSettlement();

	protected abstract void SetupTurn();

	protected abstract void PlayTradePhase();

	protected abstract void PlayBuyPhase();

	protected abstract void TearDownTurn();


	private void PlayRollPhase() {
		TurnState.stateType = TurnStateType.roll;
		GameManager.Instance.networkView.RPC ("makeDiceRoll", RPCMode.All);
	}
}
