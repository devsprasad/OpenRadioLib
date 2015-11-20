using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSignalLib.Sources;

namespace OpenRadioLib.Modems.Digital
{
    public class FSK
    {

        public static Signal BFSK_Modulate(BinaryData baseband, float f1, float f2)
        {
            Signal modFSK = new Signal();
            float tmp = 1 / baseband.SamplingRate; //(30 * Math.Max(f1,f2));
            float t = 0;
            BaseSignalGenerator s = new BaseSignalGenerator(    SignalType.Sine);
            modFSK.Samples = new double[baseband.Samples.Length];
            modFSK.SamplingRate = baseband.SamplingRate;
            for (int i = 0 ; i < baseband.Samples.Length ; i++)
            {
                if (baseband.Samples[i] == 1)
                {
                    s.Frequency = f1;
                }
                else
                {
                    s.Frequency = f2;
                }
                modFSK.Samples[i] = s.GetValue(t);
                t += tmp;
            }
            return modFSK;
        }

        public static Signal BFSK_DeModulate(Signal passband, float f1, float f2, float threshold = 10)
        {
            Sinusoidal c = new Sinusoidal(f1, 0, passband.SamplingRate, 1, passband.Samples.Length);
            Signal demod = passband * c;
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
