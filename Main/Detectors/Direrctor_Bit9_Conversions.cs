using Fido_Main.Fido_Support.Objects.Fido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fido_Main.Main.Detectors
{
    public static class Direrctor_Utils
    {
        public static void AntiVirusToBit9( FidoReturnValues lFidoReturnValues )
        {
            var lBit9ReturnValues = new Bit9ReturnValues();
            var sFileInfo = lFidoReturnValues.Antivirus.FilePath.Split('\\');
            if ((sFileInfo != null) && (sFileInfo.Length != 0))
            {
                Console.WriteLine(@"Antivirus detector found! Cross-referencing with Bit9.");
                lBit9ReturnValues.FileName = sFileInfo[sFileInfo.Length - 1];
                lFidoReturnValues.Antivirus.FileName = lBit9ReturnValues.FileName;
                for (var i = 0; i < sFileInfo.Length - 1; i++)
                {
                    if (i == sFileInfo.Length - 2)
                    {
                        lBit9ReturnValues.FilePath += sFileInfo[i];
                    }
                    else
                    {
                        if (!sFileInfo[i].Contains("'"))
                        {
                            lBit9ReturnValues.FilePath += sFileInfo[i] + "\\";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                lBit9ReturnValues.HostName = lFidoReturnValues.Hostname;
                var lBit9Info = Detect_Bit9.GetFileInfo(null, lBit9ReturnValues);
            }
        }

        //todo: is this still necessary? should we handle this in the bit9 module?
        public static FidoReturnValues FireEyeHashToBit9( FidoReturnValues lFidoReturnValues )
        {
            //Check FireEye returns and  go to Bit9 to see if the hash exists, where and
            //if it was executed, then go to VT and pass hash info on there too
            var lVirusTotalReturnValues = new VirusTotalReturnValues();
            List<string> sBit9FileInfo = Detect_Bit9.GetFileInfo(lFidoReturnValues.FireEye.MD5Hash, null);
            if (sBit9FileInfo.Count == 0) return lFidoReturnValues;
            if (lFidoReturnValues.Bit9 == null)
            {
                lFidoReturnValues.Bit9 = new Bit9ReturnValues { Bit9Hashes = sBit9FileInfo.ToArray() };
            }
            else
            {
                lFidoReturnValues.Bit9.Bit9Hashes = sBit9FileInfo.ToArray();
            }
            return lFidoReturnValues;
        }
    }
}
