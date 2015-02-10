using BrewMatic3000.Interfaces;

namespace BrewMatic3000.FakeHW
{
    public class FakeTempReader : ITempReader
    {

        private float _myValue = 20;
        public float GetValue()
        {
            return _myValue;
        }

        public FakeTempReader()
        {
            _myValue = 68;
        }

        public void SetValue(float value)
        {
            _myValue = value;
        }
    }
}
