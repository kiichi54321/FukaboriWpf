using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
	/// <summary>
	/// List �̊T�v�̐����ł��B
	/// </summary>
	public class List
	{
		public List()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// ���X�g�̓��e���J���}��؂�̕�����ɂ��܂��B
		/// </summary>
		/// <param name="list"></param>
		public static string ListToString(IList list)
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
			foreach(object obj in list)
			{
				strBuilder.Append(obj.ToString());
				strBuilder.Append(",");
			}

			return strBuilder.ToString();
		}

        public static string ListToString(List<string> list)
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
            foreach (string obj in list)
            {
                strBuilder.Append(obj);
                strBuilder.Append(",");
            }
            return strBuilder.ToString();
        }

    }
}
