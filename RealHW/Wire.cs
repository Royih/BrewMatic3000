using System;
using Toolbox.NETMF.Hardware;

namespace BrewMatic3000.RealHW
{
    /// <summary>
    /// I2C device abstraction based on the I2CPlug class by Jeroen Swart.
    /// Wire class for easy implementation of new I2C drivers.
    /// v0.1 by Lars Toft Jacobsen, ITU, IxDLab
    /// CC-BY-SA
    /// </summary>
    public class Wire
    {

        private const int DefaultClockRate = 400;

        private MultiI2C i2cDevice;

        public Wire(byte address, int clockRateKhz)
        {
            this.i2cDevice = new MultiI2C(address, clockRateKhz);
        }

        public Wire(byte address)
            : this(address, DefaultClockRate)
        {
        }

        /// <summary>
        /// Write to I2C device
        /// </summary>
        /// <param name="writeBuffer">buffer to write</param>
        private void Write(byte[] writeBuffer)
        {
            lock (i2cDevice)
            {
                i2cDevice.Write(writeBuffer);
            }
        }

        /// <summary>
        /// Read from I2C device
        /// </summary>
        /// <param name="readBuffer">read buffer</param>
        private void Read(byte[] readBuffer)
        {
            lock (i2cDevice)
            {
                i2cDevice.Read(readBuffer);
            }
        }

        /// <summary>
        /// Write to register
        /// </summary>
        /// <param name="register">Register</param>
        /// <param name="value">Value to be written</param>
        protected void WriteToRegister(byte register, byte value)
        {
            this.Write(new byte[] { register, value });
        }

        /// <summary>
        /// Burst write to multiple registers
        /// </summary>
        /// <param name="register">Start register</param>
        /// <param name="values">Values to be written</param>
        protected void WriteToRegister(byte register, byte[] values)
        {
            // create a single buffer, so register and values can be send in a single transaction
            byte[] writeBuffer = new byte[values.Length + 1];
            writeBuffer[0] = register;
            Array.Copy(values, 0, writeBuffer, 1, values.Length);

            this.Write(writeBuffer);
        }

        /// <summary>
        /// Read one or burst read multiple values from register
        /// </summary>
        /// <param name="register">Register</param>
        /// <param name="readBuffer">Read buffer</param>
        protected void ReadFromRegister(byte register, byte[] readBuffer)
        {
            this.Write(new byte[] { register });
            this.Read(readBuffer);
        }

    }
}

