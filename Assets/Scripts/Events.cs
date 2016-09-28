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

public class MoveEyeLidsStartEvent : GameEvent {
	public string movePhase { get; private set; }
	public Transform moveTo { get; private set; }
	public float slideDistance { get; private set; }
	public float blinkTime { get; private set; }
	public float blinkWait { get; private set; }

	public MoveEyeLidsStartEvent(string movePhase, Transform moveTo, float slideDistance, float blinkTime, float blinkWait) {
		this.movePhase = movePhase;
		this.moveTo = moveTo;
		this.slideDistance = slideDistance;
		this.blinkTime = blinkTime;
		this.blinkWait = blinkWait;
	}
}

public class MoveEyeLidsEndEvent : GameEvent {
	public string movePhase { get; private set; }
	public Transform moveTo { get; private set; }

	public MoveEyeLidsEndEvent(string movePhase, Transform moveTo) {
		this.movePhase = movePhase;
		this.moveTo = moveTo;
	}
}

public class EyeLidsVisibleEvent : GameEvent {
	public bool isVisible { get; private set; }

	public EyeLidsVisibleEvent(bool isVisible) {
		this.isVisible = isVisible;
	}
}

