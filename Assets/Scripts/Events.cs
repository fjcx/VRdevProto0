using UnityEngine;
using System.Collections;
using System;

public class GameEvent
{
}

public class ModeUpdatedEvent : GameEvent {
	public string newMode { get; private set; }

	public ModeUpdatedEvent(string newMode){
		this.newMode = newMode;
	}
}


public class MoveToEvent : GameEvent {
	public Transform moveToTransform { get; private set; }

	public MoveToEvent(Transform moveToTransform){
		this.moveToTransform = moveToTransform;
	}
}

public class PlayerSelectedEvent : GameEvent {
	public string playerName { get; private set; }

	public PlayerSelectedEvent(string playerName){
		this.playerName = playerName;
	}
}

public class EnableMSicknessEffectEvent : GameEvent {
	public bool enable { get; private set; }

	public EnableMSicknessEffectEvent(bool enable){
		this.enable = enable;
	}
}

