public interface IWeapon
{
    int maxAmmo { get; set; }
    int currentAmmo { get; set; }
    void ReplenishAmmo(int amount);
    void UpdateAmmoDisplay();
}
