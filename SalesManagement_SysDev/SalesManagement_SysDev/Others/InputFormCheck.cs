using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SalesManagement_SysDev
{
    class InputFormCheck
    {
        public bool CheckIdFormat(int iCheck)
        {
            //長さ6文字であるかを判定 : false = out
            bool flg = true;
            if(!(iCheck <= NumericRange.ID))
            {
                flg = false;
            }

            return flg;
        }
        public bool CheckTelFaxFormat(string sCheck)
        {
            //長さ13文字以下で数字のみであるかを判定
            bool flg = true;
            if(sCheck.Length <= 13)
            {
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(sCheck))
                    flg = false;
            }
            else
                flg = false;            
            
            return flg;
        }
        public bool CheckPostCodeFormat(string sCheck)
        {
            //長さ7文字で半角数字のみであるかを判定
            bool flg = true;
            if(sCheck.Length == 7)
            {
                Regex regex = new Regex("^[a-zA-Z0-9]+$");
                if (!regex.IsMatch(sCheck))
                    flg = false;
            }            
            else
                flg = false;

            return flg;
        }
        public bool CheckPasswordFormat(string sCheck)
        {
            //長さ10文字以下で半角英数字であるかを判定
            bool flg = true;
            if(sCheck.Length <= 10)
            {
                Regex regex = new Regex("^[a-zA-Z0-9]+$");
                if (!regex.IsMatch(sCheck))
                    flg = false;
            }            
            else
                flg = false;

            return flg;
        }
    }
}
