using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    #region Constnants
    const float Speed = 5.0f;
	const float JumpVelocity = 4.5f;
	const float CameraSensitivty = 0.003f;
	const float DodgeDuration = 0.25f;
	#endregion

	#region Variables
	bool Dodging = false;
	float DodgeSpeed = 0.0f;
    float DodgeSpeedIncrease = 2.0f;
    #endregion

    #region Nodes
    Node3D Head;
	Node3D Weapon;
	Camera3D Camera;
	Timer DodgeTimer;
	AnimationPlayer Animator;
	#endregion

	public override void _Ready()
	{
		Head = GetNode<Node3D>("Head");
		Camera = GetNode<Camera3D>("Head/Camera3D");
		Animator = GetNode<AnimationPlayer>("Head/Camera3D/Weapon/prop_sword/AnimationPlayer");
		DodgeTimer = new Timer();
		AddChild(DodgeTimer);
		DodgeTimer.Timeout += OnDodgeTimerTimeout;
		DodgeTimer.Autostart = false;
		DodgeTimer.WaitTime = DodgeDuration;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouse)
		{
            Head.RotateY(-mouse.Relative.X * CameraSensitivty);
            Camera.RotateX(-mouse.Relative.Y * CameraSensitivty);

            Vector3 cameraRotation = Camera.Rotation;
            cameraRotation.X = Mathf.Clamp(cameraRotation.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
            Camera.Rotation = cameraRotation;
        }

		if (Input.IsActionJustPressed("Esc"))
		{
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
			else
			{
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }

		if (Input.IsActionJustPressed("attack"))
		{
			Animator.Play("prop_sword_strike");
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionJustPressed("dodge") && IsOnFloor())
		{
			Dodging = true;
			DodgeSpeed = DodgeSpeedIncrease;
			DodgeTimer.Start();
		}

		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * (Speed + DodgeSpeed);
			velocity.Z = direction.Z * (Speed + DodgeSpeed);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnDodgeTimerTimeout()
	{
		Dodging = false;
		DodgeSpeed = 0.0f;
	}
}
