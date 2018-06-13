using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour
{
	public static SwitchWeapon instance;

	public Sprite pistol;
	public Sprite shotgun;
	public Sprite m4;
	public Image image;

	void Awake()
	{
		instance = this;
	}

	public void ChangeWeapon(GunType type)
	{
		if (type == GunType.Pistol)
			image.sprite = pistol;
		else if (type == GunType.Shotgun)
			image.sprite = shotgun;
		else if (type == GunType.M4)
			image.sprite = m4;
	}
}
