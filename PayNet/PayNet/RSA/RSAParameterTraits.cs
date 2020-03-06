using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PayNet.RSA
{
    /// <summary>
    /// 
    /// </summary>
    internal class RSAParameterTraits
    {
        public int size_D = -1;
        public int size_DP = -1;
        public int size_DQ = -1;
        public int size_Exp = -1;
        public int size_InvQ = -1;
        public int size_Mod = -1;
        public int size_P = -1;
        public int size_Q = -1;

        public RSAParameterTraits(int modulusLengthInBits)
        {
            int num = -1;
            double num2 = Math.Log((double)modulusLengthInBits, 2.0);
            if (num2 == ((int)num2))
            {
                num = modulusLengthInBits;
            }
            else
            {
                num = (int)(num2 + 1.0);
                num = (int)Math.Pow(2.0, (double)num);
                Debug.Assert(false);
            }
            switch (num)
            {
                case 0x200:
                    this.size_Mod = 0x40;
                    this.size_Exp = -1;
                    this.size_D = 0x40;
                    this.size_P = 0x20;
                    this.size_Q = 0x20;
                    this.size_DP = 0x20;
                    this.size_DQ = 0x20;
                    this.size_InvQ = 0x20;
                    break;

                case 0x400:
                    this.size_Mod = 0x80;
                    this.size_Exp = -1;
                    this.size_D = 0x80;
                    this.size_P = 0x40;
                    this.size_Q = 0x40;
                    this.size_DP = 0x40;
                    this.size_DQ = 0x40;
                    this.size_InvQ = 0x40;
                    break;

                case 0x800:
                    this.size_Mod = 0x100;
                    this.size_Exp = -1;
                    this.size_D = 0x100;
                    this.size_P = 0x80;
                    this.size_Q = 0x80;
                    this.size_DP = 0x80;
                    this.size_DQ = 0x80;
                    this.size_InvQ = 0x80;
                    break;

                case 0x1000:
                    this.size_Mod = 0x200;
                    this.size_Exp = -1;
                    this.size_D = 0x200;
                    this.size_P = 0x100;
                    this.size_Q = 0x100;
                    this.size_DP = 0x100;
                    this.size_DQ = 0x100;
                    this.size_InvQ = 0x100;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}