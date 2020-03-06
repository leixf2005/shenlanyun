using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web;

namespace PayNet.RSA
{
    /// <summary>
    /// 
    /// </summary>
    public class Crypto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider DecodeRsaPrivateKey(string privateKey)
        {
            Dictionary<string, string> extras = new Dictionary<string, string>();
            byte[] bytesFromPEM = Helpers.GetBytesFromPEM(privateKey, out extras);
            return DecodeRsaPrivateKey(bytesFromPEM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="privateKeyBytes"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privateKeyBytes)
        {
            RSACryptoServiceProvider provider2;
            MemoryStream input = new MemoryStream(privateKeyBytes);
            BinaryReader rd = new BinaryReader(input);
            try
            {
                switch (rd.ReadUInt16())
                {
                    case 0x8130:
                        rd.ReadByte();
                        break;

                    case 0x8230:
                        rd.ReadInt16();
                        break;

                    default:
                        return null;
                }
                if (rd.ReadUInt16() != 0x102)
                {
                    return null;
                }
                if (rd.ReadByte() > 0)
                {
                    return null;
                }

                CspParameters parameters = new CspParameters();
                parameters.Flags = CspProviderFlags.NoFlags;
                parameters.KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant();
                parameters.ProviderType = ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;

                RSAParameters parameters2 = new RSAParameters();
                parameters2.Modulus = rd.ReadBytes(Helpers.DecodeIntegerSize(rd));

                RSAParameterTraits traits = new RSAParameterTraits(parameters2.Modulus.Length * 8);
                parameters2.Modulus = Helpers.AlignBytes(parameters2.Modulus, traits.size_Mod);
                parameters2.Exponent = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_Exp);
                parameters2.D = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_D);
                parameters2.P = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_P);
                parameters2.Q = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_Q);
                parameters2.DP = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_DP);
                parameters2.DQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_DQ);
                parameters2.InverseQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_InvQ);

                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(parameters);
                provider.ImportParameters(parameters2);
                provider2 = provider;
            }
            catch (Exception ex)
            {
                provider2 = null;
            }
            finally
            {
                rd.Close();
            }
            return provider2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider DecodeX509PublicKey(string publicKey)
        {
            byte[] bytes = Helpers.GetBytesFromPEM(publicKey);
            return DecodePublicKey(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKeyBytes"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider DecodePublicKey(byte[] publicKeyBytes)
        {
            RSACryptoServiceProvider provider2;
            MemoryStream input = new MemoryStream(publicKeyBytes);
            BinaryReader rd = new BinaryReader(input);
            byte[] b = new byte[] { 0x30, 13, 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 1, 1, 5, 0 };
            byte[] buffer2 = new byte[15];
            try
            {
                switch (rd.ReadUInt16())
                {
                    case 0x8130:
                        rd.ReadByte();
                        break;

                    case 0x8230:
                        rd.ReadInt16();
                        break;

                    default:
                        Debug.Assert(false);
                        return null;
                }
                if (Helpers.CompareBytearrays(rd.ReadBytes(15), b))
                {
                    switch (rd.ReadUInt16())
                    {
                        case 0x8103:
                            rd.ReadByte();
                            goto Label_00CF;

                        case 0x8203:
                            rd.ReadInt16();
                            goto Label_00CF;
                    }
                }
                return null;
            Label_00CF:
                if (rd.ReadByte() > 0)
                {
                    Debug.Assert(false);
                    return null;
                }
                ushort num2 = rd.ReadUInt16();
                if (num2 == 0x8130)
                {
                    rd.ReadByte();
                }
                else if (num2 == 0x8230)
                {
                    rd.ReadInt16();
                }
                else
                {
                    return null;
                }
                CspParameters parameters = new CspParameters();
                parameters.Flags = CspProviderFlags.NoFlags;
                parameters.KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant();
                parameters.ProviderType = ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(parameters);
                RSAParameters parameters2 = new RSAParameters();
                parameters2.Modulus = rd.ReadBytes(Helpers.DecodeIntegerSize(rd));
                RSAParameterTraits traits = new RSAParameterTraits(parameters2.Modulus.Length * 8);
                parameters2.Modulus = Helpers.AlignBytes(parameters2.Modulus, traits.size_Mod);
                parameters2.Exponent = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_Exp);
                provider.ImportParameters(parameters2);
                provider2 = provider;
            }
            catch (Exception)
            {
                Debug.Assert(false);
                provider2 = null;
            }
            finally
            {
                rd.Close();
            }
            return provider2;
        }

    }
}