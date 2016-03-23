		#region EncryptionIVECB ECB����(��׼DESECB������ƫ��)
        /// <summary>
        /// ��ȡ EncryptionIVECB ECB����(��׼DESECB������ƫ��)
        /// </summary>
        private static byte[] EncryptionIVECB
        {
            get
            {
                return new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static byte[] IV = { 0xB0, 0xA2, 0xB8, 0xA3, 0xDA, 0xCC, 0xDA, 0xCC };
        #endregion
		
		#region MergeByte Byte�ϲ�
        /// <summary>
        /// Byte�ϲ�
        /// </summary>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        /// <returns></returns>
        private static byte[] MergeByte(byte[] byte1, byte[] byte2)
        {
            try
            {
                byte[] byteArr = new byte[byte1.Length + byte2.Length];
                byte1.CopyTo(byteArr, 0);
                byte2.CopyTo(byteArr, byte1.Length);

                return byteArr;
            }
            catch (Exception ex)
            {
                log.WarnFormat("Byte�ϲ������쳣 message={0}", ex.Message);
            }

            return null;
        }
        #endregion
		
		#region CopyByte Byte����
        /// <summary>
        /// Byte����
        /// </summary>
        /// <param name="byte1"></param>
        /// <param name="beginIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static byte[] CopyByte(byte[] byte1, int beginIndex, int len)
        {
            return byte1.Skip(beginIndex).Take(len).ToArray();
        }
        #endregion
		
		/// <summary>
        /// ByteTo16
        /// </summary>
        /// <param name="byte1"></param>
        /// <returns></returns>
        private static string ByteToHex16(byte[] byte1)
        {
            if (byte1 == null)
            {
                return "";
            }
            return string.Join("", byte1.Select(d => Convert.ToString(d, 16).PadLeft(2, '0')).ToArray());
        }
		
        #region 3des2 ���ܽ���
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] Triple3DES2Encrypt(byte[] encrypt, byte[] key)
        {
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                //ָ���ܳ׳��ȣ� ������˫���������ֱ��� 64, 128, 192 Ĭ��Ϊ192λ
                des.KeySize = 128;
                //ʹ��ָ����key��IV������������
                des.Key = key;
                des.IV = IV;
                //����ģʽ��ƫ��
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                //���м���ת������
                ICryptoTransform ct = des.CreateEncryptor();
                //8�ܹؼ������ܽ����8�ֽ�����
                return ct.TransformFinalBlock(encrypt, 0, 8);
            }
            catch (Exception ex)
            {
                log.WarnFormat("3des2 ���ܳ����쳣��message={0},decrypt={1},key={2}", ex.Message, ByteToHex16(encrypt), ByteToHex16(key));
            }
            return null;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="decrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] Triple3DES2Decrypt(byte[] decrypt, byte[] key)
        {
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                //ָ���ܳ׳��ȣ�������˫���������ֱ��� 64, 128, 192 Ĭ��Ϊ192λ
                des.KeySize = 128;
                //ʹ��ָ����key��IV������������
                des.Key = key;
                des.IV = IV;
                //����ģʽ��ƫ��
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                //���м���ת������
                ICryptoTransform ct = des.CreateDecryptor();
                //8�ܹؼ������ܽ����8�ֽ�����
                return ct.TransformFinalBlock(decrypt, 0, 8);
            }
            catch (Exception ex)
            {
                log.WarnFormat("3des2 ���ܳ����쳣��message={0},decrypt={1},key={2}", ex.Message, ByteToHex16(decrypt), ByteToHex16(key));
            }
            return null;
        }
        #endregion

        #region 3des �������ܽ���
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] Triple3DESEncrypt(byte[] encrypt, byte[] key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = key;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                return des.CreateEncryptor().TransformFinalBlock(encrypt, 0, encrypt.Length);
            }
            catch (Exception ex)
            {
                log.WarnFormat("3des ���� ���ܳ����쳣��message={0},decrypt={1},key={2}", ex.Message, ByteToHex16(encrypt), ByteToHex16(key));
            }
            return null;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="decrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] Triple3DESDecrypt(byte[] decrypt, byte[] key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = key;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                return des.CreateDecryptor().TransformFinalBlock(decrypt, 0, decrypt.Length);
            }
            catch (Exception ex)
            {
                log.WarnFormat("3des ���� ���ܳ����쳣��message={0},decrypt={1},key={2}", ex.Message, ByteToHex16(decrypt), ByteToHex16(key));
            }
            return null;
        }
        #endregion
		
		#region mac
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static byte[] ByteXor(byte[] date)
        {
            //init
            byte[] bIV = EncryptionIVECB;
            byte[] bTmpBuf1 = new byte[8];
            byte[] bTmpBuf2 = new byte[8];

            int dateLen = date.Length;                              //���ݳ���
            int byteLen = dateLen % 8 == 0 ? 0 : 8 - dateLen % 8;   //��λ����

            //��λ
            if (byteLen > 0)
            {
                byte[] bytePosition = new byte[byteLen];
                Array.Copy(bIV, 0, bytePosition, 0, byteLen);
                date = MergeByte(date, bytePosition);
            }

            dateLen = date.Length;                  //�����ݳ���
            Array.Copy(date, 0, bTmpBuf1, 0, 8);    //��ʼ��byte����1

            for (int i = 1; i < dateLen / 8; i++)
            {
                Array.Copy(date, 8 * i, bTmpBuf2, 0, 8);
                for (int j = 0; j < 8; j++)
                {
                    bTmpBuf1[j] = (byte)(bTmpBuf1[j] ^ bTmpBuf2[j]);
                }
                //Array.Copy(bTmpBuf2, bTmpBuf1, 8);
            }
            return bTmpBuf1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] CreateMac(byte[] date, byte[] key)
        {
            //�����byte����
            byte[] xorReturn = ByteXor(date);

            //�����16�����ַ���
            string xorString = ByteToHex16(xorReturn).ToUpper();

            //HEXDECIMAL�� ����
            int xorStringLen = xorString.Length;
            byte[] xorStringByte1 = ASCIIToByte(xorString.Substring(0, xorStringLen / 2));
            byte[] xorStringByte2 = ASCIIToByte(xorString.Substring(xorStringLen / 2, xorStringLen / 2));

            //����
            byte[] desByte = Triple3DESEncrypt(xorStringByte1, key);

            //��һ�������byte����
            byte[] bTmpBuf1 = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bTmpBuf1[i] = (byte)(desByte[i] ^ xorStringByte2[i]);
            }
            //����
            desByte = Triple3DESEncrypt(bTmpBuf1, key);
            //HEXDECIMAL
            string macStr = ByteToHex16(desByte).ToUpper();

            string macString = macStr.Substring(0, macStr.Length / 2);

            return ASCIIToByte(macString);
        }
        #endregion
		
		        /// <summary>
        /// ASCIIת��16����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] ASCIIToByte(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }
