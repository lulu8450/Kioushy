using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
	[Header("Possible Sprites")]
	public List<Sprite> spriteList = new List<Sprite>();

	void Start()
	{
		// Find the first child with a SpriteRenderer
		SpriteRenderer childRenderer = null;
		foreach (Transform child in transform)
		{
			childRenderer = child.GetComponent<SpriteRenderer>();
			if (childRenderer != null)
				break;
		}

		if (childRenderer == null)
		{
			Debug.LogWarning("No child with SpriteRenderer found!");
			return;
		}

		if (spriteList == null || spriteList.Count == 0)
		{
			Debug.LogWarning("Sprite list is empty!");
			return;
		}

		int randomIndex = Random.Range(0, spriteList.Count);
		childRenderer.sprite = spriteList[randomIndex];
	}
}
