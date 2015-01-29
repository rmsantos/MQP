using UnityEngine;
using System.Collections;

public class Portrait {
	int job;
	Sprite sprite;
	AudioClip[] dialogue;

	public Portrait(int newJob, Sprite newSprite, AudioClip[] newDialogue)
	{
		job = newJob;
		sprite = newSprite;
		dialogue = newDialogue;
	}

	public Sprite getSprite()
	{
		return sprite;
	}

	public int getJob()
	{
		return job;
	}

	public AudioClip[] getDialogue()
	{
		return dialogue;
	}
}
