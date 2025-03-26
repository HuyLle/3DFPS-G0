using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons; // Mảng các vũ khí
    private int currentWeaponIndex = 0;

    void Start()
    {
        // Khởi tạo: chỉ kích hoạt vũ khí đầu tiên
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == currentWeaponIndex);
        }
    }

    void Update()
    {
        // Chuyển đổi vũ khí bằng phím số
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponIndex != 0)
        {
            SwitchWeapon(0); // Pistol
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponIndex != 1)
        {
            SwitchWeapon(1); // Rifle
        }
        //else if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeaponIndex != 2)
        //{
        //    SwitchWeapon(2); // Shotgun
        //}
    }

    void SwitchWeapon(int newIndex)
    {
        weapons[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = newIndex;
        weapons[currentWeaponIndex].SetActive(true);
    }
}
