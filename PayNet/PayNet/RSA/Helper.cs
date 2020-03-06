using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace PayNet.RSA
{
    public class Helpers
    {
        private static Dictionary<PEMtypes, string> PEMs;

        static Helpers()
        {
            Dictionary<PEMtypes, string> dictionary1 = new Dictionary<PEMtypes, string>();
            dictionary1.Add(PEMtypes.PEM_X509_OLD, "X509 CERTIFICATE");
            dictionary1.Add(PEMtypes.PEM_X509, "CERTIFICATE");
            dictionary1.Add(PEMtypes.PEM_X509_PAIR, "CERTIFICATE PAIR");
            dictionary1.Add(PEMtypes.PEM_X509_TRUSTED, "TRUSTED CERTIFICATE");
            dictionary1.Add(PEMtypes.PEM_X509_REQ_OLD, "NEW CERTIFICATE REQUEST");
            dictionary1.Add(PEMtypes.PEM_X509_REQ, "CERTIFICATE REQUEST");
            dictionary1.Add(PEMtypes.PEM_X509_CRL, "X509 CRL");
            dictionary1.Add(PEMtypes.PEM_EVP_PKEY, "ANY PRIVATE KEY");
            dictionary1.Add(PEMtypes.PEM_PUBLIC, "PUBLIC KEY");
            dictionary1.Add(PEMtypes.PEM_RSA, "RSA PRIVATE KEY");
            dictionary1.Add(PEMtypes.PEM_RSA_PUBLIC, "RSA PUBLIC KEY");
            dictionary1.Add(PEMtypes.PEM_DSA, "DSA PRIVATE KEY");
            dictionary1.Add(PEMtypes.PEM_DSA_PUBLIC, "DSA PUBLIC KEY");
            dictionary1.Add(PEMtypes.PEM_PKCS7, "PKCS7");
            dictionary1.Add(PEMtypes.PEM_PKCS7_SIGNED, "PKCS #7 SIGNED DATA");
            dictionary1.Add(PEMtypes.PEM_PKCS8, "ENCRYPTED PRIVATE KEY");
            dictionary1.Add(PEMtypes.PEM_PKCS8INF, "PRIVATE KEY");
            dictionary1.Add(PEMtypes.PEM_DHPARAMS, "DH PARAMETERS");
            dictionary1.Add(PEMtypes.PEM_SSL_SESSION, "SSL SESSION PARAMETERS");
            dictionary1.Add(PEMtypes.PEM_DSAPARAMS, "DSA PARAMETERS");
            dictionary1.Add(PEMtypes.PEM_ECDSA_PUBLIC, "ECDSA PUBLIC KEY");
            dictionary1.Add(PEMtypes.PEM_ECPARAMETERS, "EC PARAMETERS");
            dictionary1.Add(PEMtypes.PEM_ECPRIVATEKEY, "EC PRIVATE KEY");
            dictionary1.Add(PEMtypes.PEM_CMS, "CMS");
            dictionary1.Add(PEMtypes.PEM_SSH2_PUBLIC, "SSH2 PUBLIC KEY");
            dictionary1.Add(PEMtypes.unknown, "UNKNOWN");
            PEMs = dictionary1;
        }

        public static byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            int length = inputBytes.Length;
            if ((alignSize != -1) && (length < alignSize))
            {
                byte[] buffer = new byte[alignSize];
                for (int i = 0; i < length; i++)
                {
                    buffer[i + (alignSize - length)] = inputBytes[i];
                }
                return buffer;
            }
            return inputBytes;
        }

        public static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            int index = 0;
            foreach (byte num3 in a)
            {
                if (num3 != b[index])
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        public static int DecodeIntegerSize(BinaryReader rd)
        {
            int num2;
            if (rd.ReadByte() != 2)
            {
                return 0;
            }
            byte num = rd.ReadByte();
            if (num == 0x81)
            {
                num2 = rd.ReadByte();
            }
            else if (num == 130)
            {
                byte num4 = rd.ReadByte();
                byte num5 = rd.ReadByte();
                byte[] buffer1 = new byte[] { num5, num4 };
                num2 = BitConverter.ToUInt16(buffer1, 0);
            }
            else
            {
                num2 = num;
            }
            while (rd.ReadByte() == 0)
            {
                num2--;
            }
            rd.BaseStream.Seek(-1L, SeekOrigin.Current);
            return num2;
        }

        public static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, [Optional, DefaultParameterValue(true)] bool forceUnsigned)
        {
            stream.Write((byte)2);
            int index = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] > 0)
                {
                    break;
                }
                index++;
            }
            if ((value.Length - index) == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && (value[index] > 0x7f))
                {
                    EncodeLength(stream, (value.Length - index) + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - index);
                }
                for (int j = index; j < value.Length; j++)
                {
                    stream.Write(value[j]);
                }
            }
        }

        public static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            }
            if (length < 0x80)
            {
                stream.Write((byte)length);
            }
            else
            {
                int num = length;
                int num2 = 0;
                while (num > 0)
                {
                    num = num >> 8;
                    num2++;
                }
                stream.Write((byte)(num2 | 0x80));
                for (int i = num2 - 1; i >= 0; i--)
                {
                    stream.Write((byte)((length >> (8 * i)) & 0xff));
                }
            }
        }

        public static byte[] GetBytesFromPEM(string pemString)
        {
            Dictionary<string, string> dictionary;
            PEMtypes type = getPEMType(pemString);
            if (type == PEMtypes.unknown)
            {
                return null;
            }
            return GetBytesFromPEM(pemString, type, out dictionary);
        }

        public static byte[] GetBytesFromPEM(string pemString, out Dictionary<string, string> extras)
        {
            PEMtypes type = getPEMType(pemString);
            return GetBytesFromPEM(pemString, type, out extras);
        }

        public static byte[] GetBytesFromPEM(string pemString, PEMtypes type)
        {
            Dictionary<string, string> dictionary;
            return GetBytesFromPEM(pemString, type, out dictionary);
        }

        public static byte[] GetBytesFromPEM(string pemString, PEMtypes type, out Dictionary<string, string> extras)
        {
            extras = new Dictionary<string, string>();
            string str3 = "";
            string str = PEMheader(type);
            string str2 = PEMfooter(type);
            char[] separator = new char[] { '\n' };
            foreach (string str4 in pemString.Replace("\r", "").Split(separator))
            {
                if (str4.Contains(":"))
                {
                    extras.Add(str4.Substring(0, str4.IndexOf(":") - 1), str4.Substring(str4.IndexOf(":") + 1));
                }
                else if (str4 != "")
                {
                    str3 = str3 + str4 + "\n";
                }
            }
            int startIndex = str3.IndexOf(str) + str.Length;
            int length = str3.IndexOf(str2, startIndex) - startIndex;
            return Convert.FromBase64String(str3.Substring(startIndex, length));
        }

        public static PEMtypes getPEMType(string pemString)
        {
            foreach (PEMtypes mtypes in Enum.GetValues(typeof(PEMtypes)))
            {
                if (pemString.Contains(PEMheader(mtypes)) && pemString.Contains(PEMfooter(mtypes)))
                {
                    return mtypes;
                }
            }
            return PEMtypes.unknown;
        }

        public static string PEMfooter(PEMtypes p)
        {
            if (p == PEMtypes.PEM_SSH2_PUBLIC)
            {
                return ("---- END " + PEMs[p] + " ----");
            }
            return ("-----END " + PEMs[p] + "-----");
        }

        public static string PEMheader(PEMtypes p)
        {
            if (p == PEMtypes.PEM_SSH2_PUBLIC)
            {
                return ("---- BEGIN " + PEMs[p] + " ----");
            }
            return ("-----BEGIN " + PEMs[p] + "-----");
        }
    }
}