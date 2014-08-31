using System;

namespace BrewMatic3000.PID
{
    public interface IPID
    {
        float Value { get; }

        void Start(float preferredTemperature);

        void Stop();

        float GetPreferredTemperature { get; }

        bool Started();
        
    }
}
