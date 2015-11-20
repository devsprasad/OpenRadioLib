using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSignalLib.Sources;

namespace OpenRadioLib.Coders
{
    public class NRZ
    {
        public static Signal NRZ_Polar(BinaryData data, float high_volt, float low_volt)
        {
            for (int i = 0 ; i < data.Samples.Length ; i++)
            {
                data.Samples[i] = (data.Samples[i] > 0) ? high_volt : low_volt;
            }
           
            return data;
        }
    }
}
