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

public class PlayBlinkEffectEvent : GameEvent {
	public bool enable { get; private set; }
	public Transform moveTo { get; private set; }
	public float closeTimeSpreader { get; private set; }
	public float openTimeSpreader { get; private set; }
	public float blinkWait { get; private set; }

	public PlayBlinkEffectEvent(bool enable, Transform moveTo, float closeTimeSpreader, float openTimeSpreader, float blinkWait) {
		this.enable = enable;
		this.moveTo = moveTo;
		this.closeTimeSpreader = closeTimeSpreader;
		this.openTimeSpreader = openTimeSpreader;
		this.blinkWait = blinkWait;
	}
}

public class TeleportPlayerEvent : GameEvent {
	public Transform moveTo { get; private set; }

	public TeleportPlayerEvent(Transform moveTo) {
		this.moveTo = moveTo;
	}
}

public class LaunchPTrailEvent : GameEvent
{
    public Transform targetBodyTransform { get; private set; }

    public LaunchPTrailEvent(Transform targetBodyTransform)
    {
        this.targetBodyTransform = targetBodyTransform;
    }
}

public class SwitchBodiesEvent : GameEvent {
	public Transform newBodyTransform { get; private set; }

	public SwitchBodiesEvent(Transform newBodyTransform) {
		this.newBodyTransform = newBodyTransform;
	}
}


public class SlideStandingStonesEvent : GameEvent
{
    public String slideStonesDirection { get; private set; }

    public SlideStandingStonesEvent(String slideStonesDirection)
    {
        this.slideStonesDirection = slideStonesDirection;
    }
}

