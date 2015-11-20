using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSignalLib.Sources;

namespace OpenRadioLib.Modems.Digital
{
    internal enum MODEM_MODE
    {
        MOD = 0, DEMOD = 1
    }

    public class ASK
    {
        public static Signal Modulate(Signal Baseband, Signal Carrier)
        {
            if (Baseband.Samples.Length != Carrier.Samples.Length)
            {
                throw new InvalidOperationException("baseband and carrier signal must have equal number of samples");
            }
            return Baseband * Carrier;
        }

        public static Signal Modulate(Signal Baseband, float CarrierFrequency, float CarrierAmplitude = 1)
        {
            Signal Carrier = new Sinusoidal(CarrierFrequency, 0, Baseband.SamplingRate, 1, Baseband.Samples.Length);
            return Baseband * Carrier;
        }

        public static Signal DeModulate(Signal PassBand, float carrier_frequency, float threshold = 0)
        {
            Signal ccarrier = new Sinusoidal(carrier_frequency, 0, -1, 1, PassBand.Samples.Length); // generate coherent carrier
            Signal demod = ccarrier * PassBand; // multiply the carrier and the passband signal
            var s = demod.Samples.Split((int)PassBand.SamplingRate);
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
