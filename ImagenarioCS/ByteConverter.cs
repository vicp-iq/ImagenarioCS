using System;

namespace ImagenarioCS
{
    public class ByteConverter
    {
        /** 
          *灏�32浣嶇殑int鍊兼斁鍒�4瀛楄妭鐨勯噷 
          * @param num 
          * @return 
          */
        public static byte[] int2byteArray(int num, bool reverse)
        {
            byte[] result = new byte[4];
            var unsignedInt = (uint) num;
            if (reverse)
            {
                result[3] = (byte) (unsignedInt >> 24); //鍙栨渶楂�8浣嶆斁鍒�0涓嬫爣  
                result[2] = (byte) (unsignedInt >> 16); //鍙栨�￠珮8涓烘斁鍒�1涓嬫爣  
                result[1] = (byte) (unsignedInt >> 8); //鍙栨�′綆8浣嶆斁鍒�2涓嬫爣  
                result[0] = (byte) unsignedInt; //鍙栨渶浣�8浣嶆斁鍒�3涓嬫爣  
            }
            else
            {
                result[0] = (byte) (unsignedInt >> 24); //鍙栨渶楂�8浣嶆斁鍒�0涓嬫爣  
                result[1] = (byte) (unsignedInt >> 16); //鍙栨�￠珮8涓烘斁鍒�1涓嬫爣  
                result[2] = (byte) (unsignedInt >> 8); //鍙栨�′綆8浣嶆斁鍒�2涓嬫爣  
                result[3] = (byte) unsignedInt; //鍙栨渶浣�8浣嶆斁鍒�3涓嬫爣   
            }

            return result;
        }

        /** 
          * 灏�4瀛楄妭鐨刡yte鏁扮粍杞�鎴愪竴涓猧nt鍊� 
          * @param b 
          * @return 
          */
        public static int byteArray2int(byte[] b, bool reverse)
        {
            byte[] a = new byte[4];
            int i = a.Length - 1, j;
            if (reverse)
            {
                for (j = 0; i >= 0; --i, ++j)
                {
                    //浠巄鐨勫熬閮�(鍗砳nt鍊肩殑浣庝綅)寮€濮媍opy鏁版嵁  
                    if (j < 4)
                        a[i] = b[j];
                    else
                        a[i] = 0; //濡傛灉b.length涓嶈冻4,鍒欏皢楂樹綅琛�0
                }
            }
            else
            {
                for (j = b.Length - 1; i >= 0; i--, j--)
                {
                    //浠巄鐨勫熬閮�(鍗砳nt鍊肩殑浣庝綅)寮€濮媍opy鏁版嵁  
                    if (j >= 0)
                        a[i] = b[j];
                    else
                        a[i] = 0; //濡傛灉b.length涓嶈冻4,鍒欏皢楂樹綅琛�0
                }
            }

            int v0 = (a[0] & 0xff) << 24; //&0xff灏哹yte鍊兼棤宸�寮傝浆鎴恑nt,閬垮厤Java鑷�鍔ㄧ被鍨嬫彁鍗囧悗,浼氫繚鐣欓珮浣嶇殑绗﹀彿浣�  
            int v1 = (a[1] & 0xff) << 16;
            int v2 = (a[2] & 0xff) << 8;
            int v3 = (a[3] & 0xff);
            return v0 | v1 | v2 | v3;
        }

        /** 
          * 杞�鎹�short涓篵yte 
          * 
          * @param b 
          * @param s 闇€瑕佽浆鎹㈢殑short 
          * @param index 
          */
        public static void putShort(byte[] b, short s, int index)
        {
            b[index + 1] = (byte) (s >> 8);
            b[index + 0] = (byte) (s >> 0);
        }

        /** 
          * 閫氳繃byte鏁扮粍鍙栧埌short 
          * 
          * @param b 
          * @param index 绗�鍑犱綅寮€濮嬪彇 
          * @return 
          */
        public static short getShort(byte[] b, int index)
        {
            return (short) (((b[index + 1] << 8) | b[index + 0] & 0xff));
        }

        /** 
          * 瀛楃�﹀埌瀛楄妭杞�鎹� 
          * 
          * @param ch 
          * @return 
          */
        public static void putChar(byte[] bb, char ch, int index)
        {
            int temp = (int) ch;
            // byte[] b = new byte[2];  
            for (int i = 0; i < 2; i++)
            {
                // 灏嗘渶楂樹綅淇濆瓨鍦ㄦ渶浣庝綅  
                bb[index + i] = Convert.ToByte(temp & 0xff);
                temp = temp >> 8; // 鍚戝彸绉�8浣�  
            }
        }

        /** 
          * 瀛楄妭鍒板瓧绗﹁浆鎹� 
          * 
          * @param b 
          * @return 
          */
        public static char getChar(byte[] b, int index)
        {
            int s = 0;
            if (b[index + 1] > 0)
                s += b[index + 1];
            else
                s += 256 + b[index + 0];
            s *= 256;
            if (b[index + 0] > 0)
                s += b[index + 1];
            else
                s += 256 + b[index + 0];
            char ch = (char) s;
            return ch;
        }

        /** 
          * float杞�鎹�byte 
          * 
          * @param bb 
          * @param x 
          * @param index 
          */
        public static void putFloat(byte[] bb, float x, int index)
        {
            // byte[] b = new byte[4];  
            int l = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
            for (int i = 0; i < 4; i++)
            {
                bb[index + i] = Convert.ToByte(l);
                l = l >> 8;
            }
        }

        /** 
          * 閫氳繃byte鏁扮粍鍙栧緱float 
          * 
          * @param bb 
          * @param index 
          * @return 
          */
        public static float getFloat(byte[] b, int index)
        {
            //int l;
            //l = b[index + 0];
            //l &= 0xff;
            //l |= ((long)b[index + 1] << 8);
            //l &= 0xffff;
            //l |= ((long)b[index + 2] << 16);
            //l &= 0xffffff;
            //l |= ((long)b[index + 3] << 24);
            //return Float.intBitsToFloat(l);
            throw new NotSupportedException();
        }

        /** 
          * double杞�鎹�byte 
          * 
          * @param bb 
          * @param x 
          * @param index 
          */
        public static void putDouble(byte[] bb, double x, int index)
        {
            // byte[] b = new byte[8];  
            //long l = Double.doubleToLongBits(x);
            //for (int i = 0; i < 4; i++)
            //{
            //    bb[index + i] = new Long(l).byteValue();
            //    l = l >> 8;
            //}
            throw new NotSupportedException();
        }

        /** 
          * 閫氳繃byte鏁扮粍鍙栧緱float 
          * 
          * @param bb 
          * @param index 
          * @return 
          */
        public static double getDouble(byte[] b, int index)
        {
            //long l;
            //l = b[0];
            //l &= 0xff;
            //l |= ((long)b[1] << 8);
            //l &= 0xffff;
            //l |= ((long)b[2] << 16);
            //l &= 0xffffff;
            //l |= ((long)b[3] << 24);
            //l &= 0xffffffffl;
            //l |= ((long)b[4] << 32);
            //l &= 0xffffffffffl;
            //l |= ((long)b[5] << 40);
            //l &= 0xffffffffffffl;
            //l |= ((long)b[6] << 48);
            //l &= 0xffffffffffffffl;
            //l |= ((long)b[7] << 56);
            //return Double.longBitsToDouble(l);
            throw new NotSupportedException();
        }
    }
}