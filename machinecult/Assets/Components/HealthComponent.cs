using Godot;
using System;

[Tool]
public partial class HealthComponent : Node3D
{
	[Export]
	public float MaxHealth
	{
		get => maxHealth;
		private set
		{
			maxHealth = value;
			if (CurrentHealth > maxHealth)
			{
				CurrentHealth = maxHealth;
			}
		}
	}

	public float CurrentHealth
	{
		get => currentHealth;
		private set
		{
			currentHealth = value;
		}
	}

	private float currentHealth;
	private float maxHealth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Damage()
	{

	}

	public void Heal()
	{

	}
}
