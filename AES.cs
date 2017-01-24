        #region 微信企业号解密
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Input">密文</param>
        /// <param name="EncodingAESKey"></param>
        /// <returns></returns>
        /// 
        public static string AES_decrypt(String Input, string EncodingAESKey, ref string corpid)
        {
            byte[] Key;
            Key = Convert.FromBase64String(EncodingAESKey + "=");
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            byte[] btmpMsg = AES_decrypt(Input, Iv, Key);

            int len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);


            byte[] bMsg = new byte[len];
            byte[] bCorpid = new byte[btmpMsg.Length - 20 - len];
            Array.Copy(btmpMsg, 20, bMsg, 0, len);
            Array.Copy(btmpMsg, 20 + len, bCorpid, 0, btmpMsg.Length - 20 - len);
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            corpid = Encoding.UTF8.GetString(bCorpid);


            return oriMsg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Iv"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static byte[] AES_decrypt(String Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = decode2(ms.ToArray());
            }
            return xBuff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decrypted"></param>
        /// <returns></returns>
        private static byte[] decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }

        #endregion

        #region 微信小程序解密
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="encryptData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AES_decrypt(string encryptData, string key, string iv)  
        {  
            RijndaelManaged rijalg = new RijndaelManaged();  
            //设置 cipher 格式 AES-128-CBC             
            rijalg.KeySize = 128;  
            rijalg.Padding = PaddingMode.PKCS7;  
            rijalg.Mode = CipherMode.CBC;  
            rijalg.Key = Convert.FromBase64String(key);  
            rijalg.IV = Convert.FromBase64String(iv);         
            byte[] encryptedData = Convert.FromBase64String(encryptData);  
            //解密      
            ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);
            string result = string.Empty;
            using(MemoryStream msDecrypt = new MemoryStream(encryptedData))  
            {
                using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))  
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {  
                        result = srDecrypt.ReadToEnd();
                    }
                }           
            }    
               
            return result;  
        }

        #endregion