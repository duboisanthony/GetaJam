using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
	public void UpdateBar(float current, float max)
	{
		GetComponent<Image>().fillAmount = current / max;
	}
}
