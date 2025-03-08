using Godot;
using System;

public partial class AnimationComponent : Node
{
	[Export]
	AnimationNodeStateMachinePlayback AnimationNodeStateMachinePlayback;

	AnimationPlayer AnimationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		AnimationPlayer.Set("parameters/playback", AnimationNodeStateMachinePlayback);
		GD.Print(AnimationNodeStateMachinePlayback);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
