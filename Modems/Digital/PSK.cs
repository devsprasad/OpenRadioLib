using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSignalLib.Sources;

namespace OpenRadioLib.Modems.Digital
{

    public static class Helpers
    {
        /// <summary>
        /// Splits an array into several smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to split.</param>
        /// <param name="size">The size of the smaller arrays.</param>
        /// <returns>An array containing smaller arrays.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0 ; i < (float)array.Length / size ; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }
    public class PSK
    {

        //public static Signal BPSK_Modulate(BinaryData baseband, Signal Carrier1, Signal Carrier2)
        //{
        //    Signal modFSK = new Signal();
        //    for (int i = 0 ; i < baseband.Data.Length ; i++)
        //    {
        //        modFSK = (baseband.Data[i] == 1) ? modFSK.Extend(Carrier1) : modFSK.Extend(Carrier2);               
        //    }
        //    modFSK.SamplingRate = Carrier1.SamplingRate;
        //    return modFSK;
        //}


        public static Signal BPSK_Modulate(BinaryData data, float frequency)
        {
            Signal baseband = OpenRadioLib.Coders.NRZ.NRZ_Polar(data, 1, -1);
            Signal carrier = new OpenSignalLib.Sources.Sinusoidal(frequency, (float)Math.PI, (float)baseband.SamplingRate, 1,
                baseband.Samples.Length);
            Signal mod = baseband * carrier;
            return mod;
        }



        public static Signal BPSK_Demodulate(Signal passband, float CarrierFrequency, float threshold = 0)
        {
            Signal ccarrier = new Sinusoidal(CarrierFrequency, (float)Math.PI, -1, 1, passband.Samples.Length); // generate coherent carrier
            Signal demod = ccarrier * passband; // multiply the carrier and the passband signal
            var s = demod.Samples.Split((int)passband.SamplingRate);
            bool[] bits = new bool[s.Count()];
            int j = 0;
            foreach (var item in s)
            {
                var sm = item.Sum();
                bits[j] = sm > threshold;
                j++;
            }
            return new BinaryData(bits, demod.Samples.Length);
        }

        
    }
}
