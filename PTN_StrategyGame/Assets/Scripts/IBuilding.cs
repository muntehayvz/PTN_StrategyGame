public interface IBuilding
{
    int MaxHealthPoints { get; }
    void DisplayInfo();
    void GetDamage(int damage);
    float GetHealthPercent();
    int GetHealth();
}