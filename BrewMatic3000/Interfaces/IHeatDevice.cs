namespace BrewMatic3000.Interfaces
{
    public interface IHeatDevice
    {
        void SetValue(float percentage);

        float GetCurrentValue();

        float GetCurrentWatts();

        void Start();

    }
}
