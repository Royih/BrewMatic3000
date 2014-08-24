namespace BrewMatic3000.PID
{
    public interface IPID
    {
        float GetValue(float currentValue, float preferredTemperature);
    }
}
