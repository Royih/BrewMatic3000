
using System;
using Microsoft.SPOT;

namespace BrewMatic3000.RealHW
{
    public class DS3231 : Wire
    {
        public DS3231()
            : base(0x68, 400)
        {
        }

        public DateTime GetDateTime()
        {
            try
            {
                var myBuffer = new Byte[7];
                ReadFromRegister(0x00, myBuffer);

                var second = (int)BinaryCodedDecimal2Decimal(myBuffer[0]);
                var minute = (int)BinaryCodedDecimal2Decimal(myBuffer[1]);
                var hour = (int)BinaryCodedDecimal2Decimal(myBuffer[2]);
                var day = (int)BinaryCodedDecimal2Decimal(myBuffer[4]);
                var month = (int)BinaryCodedDecimal2Decimal(myBuffer[5] & 127);
                var century = (myBuffer[5] & 128) == 128;
                var year = (int)BinaryCodedDecimal2Decimal(myBuffer[6]);
                var yearFull = century
                    ? 2000 + year
                    : 1900 + year;
                return new DateTime(yearFull, month, day, hour, minute, second);
            }
            catch (Exception ex)
            {
                Debug.Print("Error reading date from DS3231: " + ex.Message);
                return DateTime.Now;
            }
        }

        // Convert Binary Coded Decimal (BCD) to Decimal 
        private Int32 BinaryCodedDecimal2Decimal(Int32 num)
        {
            return ((num / 16 * 10) + (num % 16));
        }

        // Convert Decimal to Binary Coded Decimal (BCD) 
        private Int32 Decimal2BinaryCodedDecimal(Int32 num)
        {
            return ((num / 10 * 16) + (num % 10));
        }






        public void SetDateTime(DateTime dateTime)
        {

            var century = 128;
            var yearShort = 0;
            if (dateTime.Year >= 2000)
            {
                yearShort = dateTime.Year - 2000;
            }
            else
            {
                century = 0;
                yearShort = dateTime.Year - 1900;
            }

            WriteToRegister(0x00, 0x00);
            WriteToRegister(0x01, (byte)Decimal2BinaryCodedDecimal(dateTime.Minute));
            WriteToRegister(0x02, (byte)Decimal2BinaryCodedDecimal(dateTime.Hour));
            WriteToRegister(0x03, (byte)Decimal2BinaryCodedDecimal((int)dateTime.DayOfWeek + 1));
            WriteToRegister(0x04, (byte)Decimal2BinaryCodedDecimal(dateTime.Day));
            WriteToRegister(0x05, (byte)(dateTime.Month + century));
            WriteToRegister(0x06, (byte)Decimal2BinaryCodedDecimal(yearShort));

            //WriteToRegister(0x06, byte.Parse(dectobcd(year).ToString()));
        }


    }
}
