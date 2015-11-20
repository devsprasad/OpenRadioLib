using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRadioLib.Modems.Digital;
using OpenSignalLib.Sources;
using System.Diagnostics;

namespace OpenRadioLib.Tests
{
    public class ModemTests
    {
        public static void Run(float f = 10, bool[] data = null)
        {
            //
            // Data
            //
            if(data==null) data = new bool[]{ false, false, false, true };
            //
            // Baseband and carrier generation 
            //          
            BinaryData b = new BinaryData(data, 30 * f); // baseband
            //
            //mod
            //
            Signal ask_mod = ASK.Modulate(b, f, 1);
            Signal psk_mod = PSK.BPSK_Modulate(b, f);
            Signal fsk_mod = FSK.BFSK_Modulate(b, f, 2 * f);
            //
            //demod
            //
            BinaryData ask_d = (BinaryData)ASK.DeModulate(ask_mod, f);
            BinaryData psk_d = (BinaryData)PSK.BPSK_Demodulate(psk_mod, f);
            BinaryData fsk_d = (BinaryData)FSK.BFSK_DeModulate(fsk_mod, f, 2 * f);
            //
            // display
            //
            Debug.Print("ASK : "); ask_d.Display();
            Debug.Print("PSK : "); psk_d.Display();
            Debug.Print("FSK : "); fsk_d.Display();
        }
    }
}
