using BrewMatic3000.PID;
namespace BrewMatic3000.Interfaces
{
    public interface IPID
    {
        float GetValue(float currentValue, float preferredTemperature);
        LogValue[] GetLogValues();
    }
}
