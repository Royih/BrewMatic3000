namespace Kobush.NETMF.Hardware.LCD
{
    public interface ILiquidCrystalTransferProvider
    {
        void Send(byte data, bool mode, bool backlight);

        /// <summary>
        /// Specify if the provider works in 4-bit mode; 8-bit mode is used otherwise.
        /// </summary>
        bool FourBitMode { get; }
    }
}