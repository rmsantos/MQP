using UnityEngine;
using System.Collections;

public class Portrait {
	Sprite sprite;
	AudioClip[] dialogue;

	public Portrait(Sprite newSprite, AudioClip[] newDialogue)
	{
		sprite = newSprite;
		dialogue = newDialogue;
	}

	public Sprite getSprite()
	{
		return sprite;
	}

	public AudioClip[] getDialogue()
	{
		return dialogue;
	}
}
