using System;
using System.Collections.Generic;
using System.Text;
namespace MyLib
{
	/// <summary>
	/// ���l�v�Z�p�̃��C�u�����[
	/// </summary>
	public class MathLib
	{
		private MathLib()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
		/// <summary>
		/// �����_��decimals�ʂ܂Ő؂�̂ď��������Ă���܂��B
		/// </summary>
		/// <param name="number"></param>
		/// <param name="decimals"></param>
		/// <returns></returns>
		public static double NumberFormat(float number,int decimals)
		{
			double tmp = Math.Floor( (double)number * Math.Pow(10,decimals))/Math.Pow(10,decimals);
			return tmp;
		}

        ///// <summary>
        ///// �����_�ȉ���0��⊮����
        ///// </summary>
        ///// <param name="number"></param>
        ///// <param name="decimals"></param>
        ///// <returns></returns>
        //public static string NumberFormatToString(float number, int decimals)
        //{
        //    double tmp = NumberFormat(number, decimals);
        //    string tmpStr = tmp.ToString();
        //    string[] data = tmpStr.Split('.');
        //    StringBuilder strBuilder = new StringBuilder();
        //    strBuilder.Append(data[0] + "."+data[1]);
        //    for (int i = 0; i < data[1].Length - decimals; i++)
        //    {
        //        strBuilder.Append("0");
        //    }
        //    return strBuilder.ToString();
        //}

        /// <summary>
        /// �����_�ȉ���0��⊮����B0.5 ��0.5000��
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static string NumberFormatToString(float number, int decimals)
        {
            double tmp = NumberFormat(number, decimals);
            string tmpStr = tmp.ToString();
            string[] data = tmpStr.Split('.');
            StringBuilder strBuilder = new StringBuilder();
            if (data.Length > 1)
            {
                strBuilder.Append(data[0] + "." + data[1]);
                for (int i = 0; i < decimals -data[1].Length  ; i++)
                {
                    strBuilder.Append("0");
                }
            }
            else
            {
                strBuilder.Append(data[0] + ".");
                for (int i = 0; i < decimals; i++)
                {
                    strBuilder.Append("0");
                }
            }
                return strBuilder.ToString();

        }

        /// <summary>
        /// �����_�ȉ���0��⊮����B0.5 ��0.5000��
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static string NumberFormatToString(double number, int decimals)
        {
            double tmp = NumberFormat(number, decimals);
            string tmpStr = tmp.ToString();
            string[] data = tmpStr.Split('.');
            StringBuilder strBuilder = new StringBuilder();
            if (data.Length > 1)
            {
                strBuilder.Append(data[0] + "." + data[1]);
                for (int i = 0; i < decimals - data[1].Length; i++)
                {
                    strBuilder.Append("0");
                }
            }
            else
            {
                strBuilder.Append(data[0] + ".");
                for (int i = 0; i < decimals; i++)
                {
                    strBuilder.Append("0");
                }
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// �����_��decimals�ʂ܂Ő؂�̂ď��������Ă���܂��B
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double NumberFormat(double number, int decimals)
		{
			double tmp = Math.Floor( (double)number * Math.Pow(10,decimals))/Math.Pow(10,decimals);
			return tmp;
		}
		// ����������
		public static int CharToInt(char c)
		{
			if('0' <= c && c <= '9')
				return c - '0';
			else
				return -1; // �z��O�̕��������͂��ꂽ�ꍇ�A-1 ��Ԃ��B
		}

		// �����񁨐���
        /// <summary>
        /// �����񁨐����B���������̐��͈����܂���B��O������Ƃ���-1��Ԃ�
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
		public static int StringToInt(string str)
		{
			int val = 0;
			foreach(char c in str)
			{
				int i = CharToInt(c);
				if(i == -1) return -1; // �z��O�̕����񂪓��͂��ꂽ�ꍇ�A-1 ��Ԃ��B
				val = val * 10 + i;
			}
			return val;
		}

        /// <summary>
        /// �����ȊO���폜�����������Ԃ��B
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToNumberTrim(string str)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9\\.]");
            return regex.Replace(str, "");
        }

        /// <summary>
        /// �x�N�g���̋����𑪂�
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double GetDistance(double[] v1, double[] v2)
        {
            double d = 0;
            try
            {
                int length = Math.Min(v1.Length, v2.Length);
                for (int i = 0; i < length; i++)
                {
                    if (double.IsNaN(v1[i]) == false && double.IsNaN(v2[i]) == false)
                    {
                        d = d + Math.Pow(v1[i] - v2[i], 2);
                    }
                }
            }
            catch
            {
                throw;
            }
            return Math.Sqrt(d);
        }
        /// <summary>
        /// �x�N�g���̋����𑪂�
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double GetDistance(double[] v1)
        {
            double d = 0;
            try
            {
                int length = v1.Length;
                for (int i = 0; i < length; i++)
                {
                    d = d + Math.Pow(v1[i], 2);
                }
            }
            catch
            {
                throw;
            }
            return Math.Sqrt(d);
        }
        /// <summary>
        /// �x�N�g���̕��ϒl���o���B
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double[] GetAverage(List<double[]> list)
        {
          
            double[] sum = null;
            if (list.Count > 0)
            {
                sum = (double[])list[0].Clone();

                for (int i = 1; i < list.Count; i++)
                {
                    for (int k = 0; k < sum.Length; k++)
                    {
                        sum[k] = sum[k] + list[i][k];
                    }
                }

                for (int k = 0; k < sum.Length; k++)
                {
                    sum[k] = sum[k] / list.Count;
                }
            }
            return sum;
        }

        
    }
}
