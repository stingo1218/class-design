using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ComputerElement
{
    /// <summary>
    /// 二进制编码方式 原码 反码 补码
    /// </summary>
    public enum Coding
    { yuanMa, fanMa, buMa }
    public class BinaryMathBLL
    {
        //enum ResultType
        //{
        //    //运算状态 0正常 1不是数字 2数据转换有损 3不是整数 4不是小数 5不是浮点数 6输入数据超出二进制表示范围
        //    //7运算结果溢出 8不是二进制 9二进制位数错误 10参数错误
        //    succeed, shifoushuzi, zhengshu, xiaoshu, fudian, shurushuchaochu, yichu, shifouerjinzhi, other
        //}
        // 记录运算状态 0正常1输入错误
        // 0正常 1不是数字 2数据转换有损 3不是整数 4不是小数 5不是浮点数 6输入数据超出二进制表示范围
        // 7运算结果溢出 8不是二进制 9二进制位数超长 10参数错误
        //private static int _status = 0;
        //#if(DEBUG)
        //        private static string errorMessNumNull = "数为空";
        //        private static string errorMessWei = "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1";
        //        private static string errorMessYiDongWeiShu = "移动位数至少两位";
        //#endif
        /// <summary>
        /// 二进制编码转换 转换成原码、反码、补码
        /// 返回转换状态标志 0正常 2数据转换有损
        /// </summary>
        /// <param name="binaryNum">待转换的二进制数</param>
        /// <param name="oldCoding">二进制以前的编码 原码 反码 补码</param>
        /// <param name="newCoding">新编码编码 原码 反码 补码</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>返回转换状态标志 0正常 2数据转换有损</returns>
        public static int BinaryToBinary(string binaryNum, Coding oldCoding, Coding newCoding, Sign sign, out string resultNum)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum));
            #endregion
            //if (CheckIsBinaryNum(binaryNum))//检查输入数是不是二进制数
            //{
            /*首先转换成原码*/
            //int check;//标记转换状态
            string tempNum;//存放临时指向结果
            if (oldCoding == Coding.fanMa)//将反码转换成原码
            {
                tempNum = BinaryMath.GetFanMa(binaryNum, sign);//将binaryNum转换成原码
            }
            else if (oldCoding == Coding.buMa)//将补码转换成原码
            {
                #region 以前的代码
                //string checkNum = "1";
                //for (int i = 0; i < binaryNum.Length - 1; i++)
                //{
                //    checkNum = checkNum + "0";
                //}
                //if (checkNum != binaryNum)
                //{
                //    tempNum = BinaryMath.GetBuMa(binaryNum, sign);//对转换后的十进制数转换成二进制补码
                //}
                //else
                //{
                //    resultNum = null;
                //    return 2;//2数据转换有损
                //}
                #endregion
                if (!BinaryMath.CheckBinaryIsBuMaMin(binaryNum, sign))
                {
                    tempNum = BinaryMath.GetBuMa(binaryNum, sign);//对转换后的十进制数转换成二进制补码
                }
                else
                {
                    resultNum = null;
                    return 2;//2数据转换有损
                }
            }
            else //if (oldCoding == 1)//原码不变 由于断言检测 可以保证此时oldCoding==1成立
            {
                tempNum = binaryNum;
            }
            /*原码到其他编码的转换*/
            if (newCoding == Coding.yuanMa)
            {
                resultNum = tempNum;//将前面转换的原码直接返回
            }
            else if (newCoding == Coding.fanMa)
            {
                resultNum = BinaryMath.GetFanMa(tempNum, sign);//将前面转换的原码转换成反码
            }
            else
            {
                resultNum = BinaryMath.GetBuMa(tempNum, sign);//将前面转换的原码转换成补码
            }
            return 0;//0正常
            //}
            //else
            //{
            //    resultNum = null;
            //    return 8;//8不是二进制
            //}
        }
        /// <summary>
        /// 二进制转换为十进制（包含编码方式）
        /// </summary>
        /// <param name="binaryNum">待转换的数字</param>
        /// <param name="notation">定点整数，定点小数</param>
        /// <param name="coding">二进制编码方式，原码，反码，补码</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>执行结果</returns>
        public static string BinaryToDecimal(string binaryNum, Notation notation, Coding coding, int digit, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(binaryNum != null);//可以出现空字符串
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1);
            #endregion
            if (binaryNum != "")//空字符串
            {
                //if (CheckIsBinaryNum(binaryNum))//检查是不是二进制数
                //{
                //    if (CheckDigit(binaryNum, digit))
                //    {
                if (coding == Coding.yuanMa)//将binary看成原码转换成对应的十进制数
                {
                    return BinaryMath.BinaryToDecimal(binaryNum, notation, sign);//直接将原码转换成对应的十进制数
                    //return 0;//0正常
                }
                else if (coding == Coding.fanMa)//将binary反码转换成对应的十进制数
                {
                    string tempNum = BinaryMath.GetFanMa(binaryNum, sign);//求得原码
                    return BinaryMath.BinaryToDecimal(tempNum, notation, sign);//原码转换成十进制
                    //return 0;//0正常
                }
                else//补码 将binary看成补码转换成对应的十进制数
                {
                    /*此部分是判断二进制补码是不是所能表示的最小值
                     若是 直接输出最小值*/
                    #region 二进制补码是不是所能表示的最小负值 直接输出最小负值
                    //if (sign == Sign.signed)//如果是有符号数
                    //{
                    //    /*进行指定二进制补码与最小值补码值比较*/
                    //    string jieGuo = "1";
                    //    for (int i = 1; i < digit; i++)
                    //    {
                    //        jieGuo += "0";
                    //    }
                    //    if (binaryNum == jieGuo)//如果是有符号数 且等于补码所能表示的最小数
                    //    {
                    //        if (notation == Notation.dingDianZhengShu)
                    //        {
                    //            //转换成最小整数
                    //            resultNum = Convert.ToInt32((-(Math.Pow(2, digit - 1)))).ToString();
                    //        }
                    //        else
                    //        {
                    //            //转换成定点小数补码的最小值-1
                    //            resultNum = "-1";
                    //        }
                    //        return 0;//0正常
                    //    }
                    //}
                    if (BinaryMath.CheckBinaryIsBuMaMin(binaryNum, sign))
                    {
                        if (notation == Notation.dingDianZhengShu)
                        {
                            //转换成最小整数
                            return Convert.ToInt32((-(Math.Pow(2, digit - 1)))).ToString();
                        }
                        else
                        {
                            //转换成定点小数补码的最小值-1
                            return "-1";
                        }
                        //return 0;//0正常
                    }
                    #endregion 二进制补码是不是所能表示的最小值 直接输出最小值
                    #region 以前的不是很好的补码转换成原码的方法
                    /*将补码二进制数首先转换成十进制 再由这个十进制转换成补码 此时的补码正是开始的二进制数的原码*/
                    //int check = BinaryMath.BinaryToDecimal(binary, notation, sign);//先将binary看作二进制原码转换成对应的十进制数
                    //if (check == 0)
                    //{
                    //    check = BinaryMath.GetBuMa(BinaryMath._resultNum, notation, digit, sign);//将转换后的十进制数转换成二进制补码
                    //    if (check == 0)
                    //    {
                    //        return BinaryMath.BinaryToDecimal(BinaryMath._resultNum, notation, sign);//将二进制补码转换成十进制
                    //    }
                    //    else
                    //    {
                    //        return check;
                    //    }
                    //}
                    //else
                    //{
                    //    return check;
                    //}
                    #endregion
                    string tempNum = BinaryMath.GetBuMa(binaryNum, sign);
                    return BinaryMath.BinaryToDecimal(tempNum, notation, sign);//将二进制原码转换成十进制
                    //return 0;//0正常
                }
                //    }
                //    else
                //    {
                //        resultNum = null;
                //        return 9;//9二进制位数错误
                //    }
                //}
                //else
                //{
                //    resultNum = null;
                //    return 8;//8不是二进制
                //}
            }
            else
            {
                return "####";
                //return 0;//0正常
            }
        }
        /// <summary>
        /// 十进制转换为二进制（含编码方式）
        /// 返回转换状态 0正常 6输入数据超出二进制表示范围
        /// </summary>
        /// <param name="decimalNum">待转换的数字</param>
        /// <param name="notation">定点整数，定点小数</param>
        /// <param name="coding">数字编码 原码 反码 补码</param>
        /// <param name="digit">二进制位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>返回转换状态 0正常 6输入数据超出二进制表示范围</returns>
        public static int DecimalToBinary(string decimalNum, Notation notation, Coding coding, int digit, Sign sign, out string resultNum)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(decimalNum));
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1);
            #endregion
            #region 旧代码
            //if (notation == Notation.dingDianZhengShu && !CheckZhengShu(decimalNum))
            //{
            //    resultNum = null;
            //    return 3;//3不是整数
            //}
            //else if (notation == Notation.dingDianXiaoShu && !CheckXiaoShu(decimalNum))
            //{
            //    resultNum = null;
            //    return 4;//4不是小数
            //}
            //else
            //{
            #endregion
            if (coding == Coding.yuanMa)//原码
            {
                if (BinaryMath.CheckNoChaoChang(decimalNum, notation, digit, sign))
                {
                    BinaryMath.DecimalToBinary(decimalNum, notation, digit, sign, out resultNum);//直接将十进制转换成二进制原码
                    return 0;//0正常
                }
                else
                {
                    resultNum = null;
                    return 6;//6输入数据超出二进制表示范围
                }
            }
            else if (coding == Coding.fanMa)//反码
            {
                if (BinaryMath.CheckNoChaoChang(decimalNum, notation, digit, sign))
                {
                    string tempNum;
                    BinaryMath.DecimalToBinary(decimalNum, notation, digit, sign, out tempNum);//十进制转换成二进制原码
                    resultNum = BinaryMath.GetFanMa(tempNum, sign);//将转换的二进制原码转换成反码
                    return 0;//0正常
                }
                else
                {
                    resultNum = null;
                    return 6;//6输入数据超出二进制表示范围
                }
            }
            else//补码
            {
                resultNum = BinaryMath.GetBuMa(decimalNum, notation, digit, sign);
                if (resultNum != null)//输入数据超出二进制表示范围
                {
                    return 0;//0正常
                }
                else
                {
                    return 6;//6输入数据超出二进制表示范围
                }
            }
        }
        /// <summary>
        /// 检查输入数是不是二进制和位数是是指定的位数
        /// </summary>
        /// <param name="num">检查的数据</param>
        /// <param name="digit">核实位数</param>
        /// <returns>TRUE为有效，FALSE为无效</returns>
        private static bool CheckNumIsBinaryAndDigit(string num, int digit)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(num));
            Debug.Assert(digit > 0);
            #endregion
            //检查是不是二进制数 检查二进制数位数是否是指定的位数
            if (CheckIsBinaryNum(num) && CheckDigit(num, digit))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查是不是二进制数
        /// </summary>
        /// <param name="num">待检查的数据</param>
        /// <returns>TRUE为有效，FALSE为无效</returns>
        public static bool CheckIsBinaryNum(string num)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(num));
            #endregion
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] != '0' && num[i] != '1')
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查二进制数位数是否是指定的位数
        /// </summary>
        /// <param name="num">检查的数据</param>
        /// <param name="digit">核实位数</param>
        /// <returns>TRUE为有效，FALSE为无效</returns>
        public static bool CheckDigit(string num, int digit)
        {
            #region 参数确认
            //检查参数有效性
            //Debug.Assert(!string.IsNullOrEmpty(num));
            Debug.Assert(num != null);
            #endregion
            //bool check = true;
            if (num.Length != digit)
            {
                //check = false;
                return false;
            }
            else
            {
                return true;
            }
            //return check;
        }
        /// <summary>
        /// 检查数据是不是合法十进制数据
        /// </summary>
        /// <param name="num">待检测数据</param>
        /// <returns>true是合法十进制数据 false不是合法</returns>
        public static bool CheckInputIsNumber(string num)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(num));
            #endregion
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] >= '0' && num[i] <= '9' || num[i] == '.' || num[i] == '-' || num[i] == '+' || num[i] == 'E' || num[i] == 'e')
                {
                    //此处空代码
                }
                else
                {
                    return false;
                }
                //if (!char.IsNumber(num[i]))
                //{
                //    return false;
                //}
            }
            return true;
        }
        /// <summary>
        /// 数是否是整数或小数进行检查
        /// </summary>
        /// <param name="num">待检查数</param>
        /// <param name="notation">检测是否整数 检测是否小数</param>
        /// <returns>true符合检测条件 false不符合</returns>
        private static bool CheckInput(string num, Notation notation)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(num));
            #endregion
            bool check = true;
            check = CheckInputIsNumber(num);// 检查数据是不是合法十进制数据
            if (check)
            {
                if (notation == Notation.dingDianZhengShu)
                {
                    check = CheckZhengShu(num);//检查是否整数
                }
                else
                {
                    check = CheckXiaoShu(num);//检查是否小数
                }
            }
            return check;
        }
        /// <summary>
        /// 检查十进制数是不是纯小数
        /// </summary>
        /// <param name="num">待检查数</param>
        /// <returns>true是纯小数 false不是纯小数</returns>
        public static bool CheckXiaoShu(string num)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(num));
            #endregion
            bool check = CheckInputIsNumber(num);
            if (check)
            {
                float fnum = float.Parse(num);
                if (fnum >= 1 || fnum <= -1)
                {
                    //check = false;
                    return false;
                }
                else
                {
                    //check = true;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查十进制数是不是整数
        /// </summary>
        /// <param name="num">待检查数</param>
        /// <returns>true是整数false不是整数</returns>
        public static bool CheckZhengShu(string num)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(num));
            #endregion
            bool check = true;
            check = CheckInputIsNumber(num);
            if (check)
            {
                for (int i = 0; i < num.Length; i++)
                {
                    if (num[i] == '.')
                    {
                        check = false;
                    }
                }
            }
            return check;
        }
        /// <summary>
        /// 实现二进制数的位数转换（包含编码方式） 八位二进制转向十六位 十六位转向八位
        /// </summary>
        /// <param name="binaryNum">待转换的二进制数据</param>
        /// <param name="zhuanDigit">转换后的位数</param>
        /// <param name="notation">定点整数 定点小数</param>
        /// <param name="coding">编码方式 原码，反码，补码</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回转换运算结果</returns>
        public static string DigitChangeHaveCoding(string binaryNum, int zhuanDigit, Notation notation, Coding coding, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum));
            Debug.Assert(zhuanDigit > 0);
            #endregion
            string tempNum;//存放中间结果的临时变量
            switch (coding)
            {
                case Coding.yuanMa:
                    return DigitChange(binaryNum, zhuanDigit, notation, sign);//原码直接转换
                #region 旧代码
                //case Coding.fanMa:
                //    tempNum = BinaryMath.GetFanMa(binaryNum, sign);//先把转换成原码
                //    tempNum = LowToHighDigitChange(tempNum, zhuanDigit, notation, sign);//将原码进行转换
                //    if (tempNum != null)
                //    {
                //        resultNum = BinaryMath.GetFanMa(tempNum, sign);//如果原码转换成功，则转换成反码
                //        return 0;//0正常
                //    }
                //    else
                //    {
                //        resultNum = null;
                //        return 2;//2数据转换有损
                //    }
                #endregion
                default://反码 补码
                    if (binaryNum.Length > zhuanDigit)//减少位数
                    {
                        if (notation == Notation.dingDianZhengShu)//定点整数
                        {
                            return binaryNum.Substring(binaryNum.Length - zhuanDigit);
                        }
                        else//定点小数
                        {
                            return binaryNum.Substring(0, zhuanDigit);
                        }
                    }
                    else//增大位数
                    {
                        string buWei = "";//形成不足空位的二进制数位
                        for (int i = 0; i < zhuanDigit - binaryNum.Length; i++)
                        {
                            buWei += "0";
                        }
                        if (notation == Notation.dingDianZhengShu)//定点整数
                        {
                            tempNum = binaryNum + buWei;
                            return BinaryMath.SuanShuYouYi(tempNum, 8);
                        }
                        else//定点小数
                        {
                            return binaryNum + buWei;
                        }
                    }
                #region 旧代码
                //tempNum = BinaryMath.GetBuMa(binaryNum, sign);//先把转换成原码
                //tempNum = BinaryMathBLL.DigitChange(tempNum, zhuanDigit, notation, sign, yOrNJieDuan);//将原码进行转换
                //if (tempNum != null)
                //{
                //    resultNum = BinaryMath.GetBuMa(tempNum, sign);//如果原码转换成功，则转换成补码
                //    //BinaryMathBLL._status = 0;
                //    //return BinaryMathBLL._status;
                //    return 0;//0正常
                //}
                //else
                //{
                //    resultNum = null;
                //    //BinaryMathBLL._status = 2;
                //    //return BinaryMathBLL._status;
                //    return 2;//2数据转换有损
                //}
                //default://正常情况下交付版本不会返回此值
                //    resultNum = null;
                //    return -1;
                #endregion
            }
        }
        /// <summary>
        /// 实现二进制数的位数转换（原码转换） 八位二进制转向十六位 十六位转向八位
        /// 转换的位数如果不够则高位丢弃（有损转换）
        /// </summary>
        /// <param name="binaryNum">待转换的二进制数据</param>
        /// <param name="zhuanDigit">转换后的位数</param>
        /// <param name="notation">定点整数 定点小数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回转换运算结果 </returns>
        private static string DigitChange(string binaryNum, int zhuanDigit, Notation notation, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum));
            Debug.Assert(zhuanDigit > 0);
            #endregion
            //先转换成十进制 再由十进制转换成二进制
            string tempNum = BinaryMath.BinaryToDecimal(binaryNum, notation, sign);
            return BinaryMath.QiangZhiDecimalToBinary(tempNum, notation, zhuanDigit, sign);//强制转换
        }
        /// <summary>
        /// 计算两个二进制的和（当作无符号数直接相加 对于负数（除补码外）计算不出真实结果）
        /// </summary>
        /// <param name="num1">数1</param>
        /// <param name="num2">数2</param>
        /// <param name="addMode">传1做加法 2做减法</param>
        /// <param name="digit">运算位数</param>
        /// <param name="sign">是否有符号位</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>0正常 7运算溢出 8不是二进制 9二进制位数超长</returns>
        public static int Add(string num1, string num2, int addMode, int digit, Sign sign, out string resultNum)
        {
            if (CheckIsBinaryNum(num1) && CheckIsBinaryNum(num2))
            {
                if (CheckDigit(num1, digit) && CheckDigit(num2, digit))
                {
                    int check;//检查符
                    check = BinaryMath.Add(num1, num2, addMode, sign, out resultNum);
                    if (check == 1)
                    {
                        return 0;//0正常
                    }
                    else
                    {
                        return 7;//7运算溢出
                    }
                }
                else
                {
                    resultNum = null;
                    return 9;//9二进制位数错误
                }
            }
            else
            {
                resultNum = null;
                return 8;//8不是二进制
            }
        }
        /// <summary>
        /// 自增自减 相等于i++ i--
        /// 返回执行状态 0运算结果未溢出 7运算结果溢出
        /// </summary>
        /// <param name="binaryNum">二进制数</param>
        /// <param name="addMode">运算模式 1加法 2减法</param>
        /// <param name="sign">是否有符号位</param>
        /// <param name="resultNum">返回结果</param>
        /// <returns>返回执行状态 0运算结果未溢出 7运算结果溢出</returns>
        public static int ZiZengJian(string binaryNum, int addMode, Sign sign, out string resultNum)
        {
            Debug.Assert(!string.IsNullOrEmpty(binaryNum));
            Debug.Assert(addMode == 1 || addMode == 2);
            int check = BinaryMath.ZiZengJian(binaryNum, addMode, sign, out resultNum);
            if (check == 1)
            {
                return 0;//0正常
            }
            else
            {
                return 7;//7运算溢出
            }
        }
        /// <summary>
        /// 计算两个二进制反码的和（当作无符号数直接相加 对于负数（除补码外）计算不出真实结果）
        /// </summary>
        /// <param name="num1">数1</param>
        /// <param name="num2">数2</param>
        /// <param name="digit">运算位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">返回值</param>
        /// <returns>返回执行状态 0正常 7运算溢出 8不是二进制 9二进制位数错误</returns>
        public static int AddForFanMa(string num1, string num2, int digit, Sign sign, out string resultNum)
        {
            #region
            Debug.Assert(!string.IsNullOrEmpty(num1));
            Debug.Assert(!string.IsNullOrEmpty(num2));
            #endregion
            if (CheckIsBinaryNum(num1) && CheckIsBinaryNum(num2))
            {
                if (CheckDigit(num1, digit) && CheckDigit(num2, digit))
                {
                    int check = BinaryMath.AddForFanMa(num1, num2, sign, out resultNum);
                    if (check == 1)
                    {
                        return 0;//0正常
                    }
                    else
                    {
                        return 7;//7运算溢出
                    }
                }
                else
                {
                    resultNum = null;
                    return 9;//9二进制位数错误
                }
            }
            else
            {
                resultNum = null;
                return 8;//8不是二进制
            }
        }
        /// <summary>
        /// 实现左移
        /// </summary>
        /// <param name="binaryNum">移动的二进制数据</param>
        /// <param name="yiDongWeiShu">移动的位数</param>
        /// <returns>返回移动结果</returns>
        public static string ZuoYi(string binaryNum, int yiDongWeiShu)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(yiDongWeiShu > 0, "移动位数至少两位");
            #endregion
            return BinaryMath.ZuoYi(binaryNum, yiDongWeiShu);
        }
        /// <summary>
        /// 实现右移 如果是有符号的则算术右移 无符号逻辑右移
        /// </summary>
        /// <param name="binaryNum">移动的二进制数据</param>
        /// <param name="yiDongWeiShu">移动的位数</param>
        /// <param name="sign">是否有符号 有符号 无符号</param>
        /// <returns>返回移动结果</returns>
        public static string YouYi(string binaryNum, int yiDongWeiShu, Sign sign)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(yiDongWeiShu > 0, "移动位数至少两位");
            #endregion
            if (sign == Sign.signed)
            {
                return BinaryMath.SuanShuYouYi(binaryNum, yiDongWeiShu);
            }
            else
            {
                return BinaryMath.LuoJiYouYi(binaryNum, yiDongWeiShu);
            }
        }
        /// <summary>
        /// 浮点数转换成十进制数
        /// </summary>
        /// <param name="binaryNum">二进制的浮点数</param>
        /// <param name="fuDianWeiShu">浮点数的总位数</param>
        /// <param name="jieMaWeiShu">阶码位数</param>
        /// <returns>执行结果</returns>
        public static string FuDianToShiJinZhi(string binaryNum, int fuDianWeiShu, int jieMaWeiShu)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum));
            #endregion
            /*
            bool checkAllZero = true;
            for (int i = 0; i < binaryNum.Length; i++)
            {
                if (binaryNum[i] != '0')
                {
                    checkAllZero = false;
                    break;
                }
            }
            if (checkAllZero)
            {
                return "正0";//全为0
            }
            string zeroJieMa = "";//全0阶码
            string AllOneJieMa = "";//全1阶码
            string zeroWeiShu = "";//全0尾数
            for (int i = 0; i < jieMaWeiShu; i++)
            {
                zeroJieMa += "0";
                AllOneJieMa += "1";
            }
            for (int i = 0; i < binaryNum.Length - jieMaWeiShu -1; i++)
            {
                zeroWeiShu += "0";
            }
            if(binaryNum.Substring(0,jieMaWeiShu) == zeroJieMa && binaryNum.Substring(jieMaWeiShu+1) == zeroWeiShu && binaryNum[jieMaWeiShu]=='1')
            {
                return "负0";
            }
            if (binaryNum.Substring(0, jieMaWeiShu) == AllOneJieMa &&  binaryNum.Substring(jieMaWeiShu+1) == zeroWeiShu)
            {
                if(binaryNum[jieMaWeiShu] == '1')
                {
                    return "负无穷大";
                }
                else
                {
                    return "正无穷大";
                }
            }
            if (binaryNum.Substring(0, jieMaWeiShu) == AllOneJieMa && binaryNum.Substring(jieMaWeiShu + 1) != zeroWeiShu)
            {
                return "NAN";
            }*/
            string jieMa = binaryNum.Substring(0, jieMaWeiShu);//阶码部分
            string weiShu = binaryNum.Substring(jieMaWeiShu);//尾数部分
            string tempNum;//存放临时结果
            tempNum = BinaryToDecimal(weiShu, Notation.dingDianXiaoShu, Coding.buMa, fuDianWeiShu - jieMaWeiShu, Sign.signed);
            //
            float num = float.Parse(tempNum);//此时tempNum中的数据已不再需要
            tempNum = BinaryToDecimal(jieMa, Notation.dingDianZhengShu, Coding.buMa, jieMaWeiShu, Sign.signed);
            int jieMaNum = int.Parse(tempNum);
            num = (float)(num * Math.Pow(2, jieMaNum));//计算数值
            return num.ToString();
            //return 0;//0正常
        }
        /// <summary>
        /// 十进制转换成二进制浮点表示
        /// 执行状态 0正常 6输入数据超出二进制表示范围
        /// </summary>
        /// <param name="floatNum">二进制数</param>
        /// <param name="fuDianWeiShu">浮点数的总位数</param>
        /// <param name="jieMaWeiShu">阶码位数</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>执行状态 0正常 6输入数据超出二进制表示范围</returns>
        public static int FuDianToBinaryNum(string floatNum, int fuDianWeiShu, int jieMaWeiShu, out string resultNum)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(floatNum));
            #endregion
            //if (CheckInputIsNumber(floatNum))//检测是否是浮点数
            //{
            float num = float.Parse(floatNum);
            int i = 0;//记录移动的次数 也就是阶码
            if (num >= 1 || num <= -1)//绝对值大于等于1的转换成规格化小数
            {
                while (num >= 1 || num <= -1)
                {
                    num /= 2;
                    i++;
                }
            }
            else if ((num < 0.5 && num > -0.5) && num != 0)//将绝对值小于0.5的转换成规格化形式
            {
                while (num < 0.5 && num > -0.5)
                {
                    num *= 2;
                    i++;
                }
                i = -i;//阶码取相反数
            }
            string weiShu;//尾数部分
            DecimalToBinary(num.ToString(), Notation.dingDianXiaoShu, Coding.buMa, fuDianWeiShu - jieMaWeiShu, Sign.signed, out weiShu);//位数转换低位精度会丢失
            string jieMa;//阶码部分
            int check = DecimalToBinary(i.ToString(), Notation.dingDianZhengShu, Coding.buMa, jieMaWeiShu, Sign.signed, out jieMa);
            if (check == 0)
            {
                resultNum = jieMa + weiShu;
                return 0;//0正常
            }
            else//阶码过大 超出二进制位数表示范围
            {
                resultNum = null;
                return 6;//6输入数据超出二进制表示范围
            }
            //}
            //else
            //{
            //    resultNum = null;
            //    return 5;//5不是浮点数
            //}
        }
        /// <summary>
        /// 浮点二进制相加
        /// </summary>
        /// <param name="floatBinaryOne">浮点数1</param>
        /// <param name="floatBinaryTwo">浮点数2</param>
        /// <param name="jieMaWeiShu">阶码位数</param>
        /// <param name="resultNum">返回结果</param>
        /// <returns>执行状态0正常 7运算结果溢出</returns>
        public static int FloatAdd(string floatBinaryOne, string floatBinaryTwo, int jieMaWeiShu, out string resultNum)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(floatBinaryOne));
            Debug.Assert(!string.IsNullOrEmpty(floatBinaryTwo));
            #endregion
            #region 两个全零浮点数相加
            bool numNotAllZero = false;
            for (int i = 0; i < floatBinaryOne.Length; i++)
            {
                if (floatBinaryOne[i] != '0')
                {
                    numNotAllZero = true;
                }
            }
            for (int i = 0; i < floatBinaryTwo.Length; i++)
            {
                if (floatBinaryTwo[i] != '0')
                {
                    numNotAllZero = true;
                }
            }
            if (!numNotAllZero)
            {
                resultNum = floatBinaryOne;
                return 0;
            }
            #endregion
            string floatBinaryOnejieMa = floatBinaryOne.Substring(0, jieMaWeiShu);//数1阶码
            string floatBinaryOneweiShu = floatBinaryOne.Substring(jieMaWeiShu);//数1尾数
            string floatBinaryTwojieMa = floatBinaryTwo.Substring(0, jieMaWeiShu);//数2阶码
            string floatBinaryTwoweiShu = floatBinaryTwo.Substring(jieMaWeiShu);//数2尾数
            string temp;//存放下面四行二进制阶码计算出的十进制表示
            temp = BinaryToDecimal(floatBinaryOnejieMa, Notation.dingDianZhengShu, Coding.buMa, jieMaWeiShu, Sign.signed);//求阶码1的十进制表示
            int numOneJieMa = int.Parse(temp);
            temp = BinaryToDecimal(floatBinaryTwojieMa, Notation.dingDianZhengShu, Coding.buMa, jieMaWeiShu, Sign.signed);//求阶码2的十进制表示
            int numTwoJieMa = int.Parse(temp);
            int JieMaCha = numOneJieMa - numTwoJieMa;//求阶码差
            string resultJieMa;//最终求和结果的阶码
            #region /*以下三个条件完成对阶*/
            if (JieMaCha == 0)//阶差为0 不用对阶 任选一个数的阶码
            {
                resultJieMa = floatBinaryTwojieMa;
            }
            else if (JieMaCha < 0)//第一个数的阶码小于第二个阶码 第一个数向右移动阶差个位数
            {
                char gaoWeiOfDiuQi;//存放因右移导致的被丢弃的最高位 以便进行舍入处理
                if (JieMaCha >= -(floatBinaryOne.Length - jieMaWeiShu))//阶差在有效尾数数位内
                {
                    gaoWeiOfDiuQi = floatBinaryOneweiShu[floatBinaryOne.Length - jieMaWeiShu + JieMaCha];//记录被丢掉位最高位
                }
                else//超过尾数位数范围 默认为‘0’字符
                {
                    gaoWeiOfDiuQi = '0';
                }
                floatBinaryOneweiShu = YouYi(floatBinaryOneweiShu, -JieMaCha, Sign.signed);//算术右移
                resultJieMa = floatBinaryTwojieMa;//阶码采用较大的一个 第二个数的阶码
                if (gaoWeiOfDiuQi == '1')//如果被丢掉位最高位是1 进行舍入处理即加1
                {
                    //Add(floatBinaryOneweiShu, "00000000001", 1, 11, Sign.signed, out floatBinaryOneweiShu);
                    ZiZengJian(floatBinaryOneweiShu, 1, Sign.signed, out floatBinaryOneweiShu);//自增
                }
            }
            else//第一个数的阶码大于第二个阶码 第二个数向右移动阶差个位数
            {
                char gaoWeiOfDiuQi;//存放因右移导致的被丢弃的最高位 以便进行舍入处理
                if (JieMaCha <= (floatBinaryOne.Length - jieMaWeiShu))//阶差在有效尾数数位内
                {
                    gaoWeiOfDiuQi = floatBinaryOneweiShu[floatBinaryOne.Length - jieMaWeiShu - JieMaCha];//记录被丢掉位最高位
                }
                else//超过尾数位数范围 默认为‘0’字符
                {
                    gaoWeiOfDiuQi = '0';
                }
                floatBinaryTwoweiShu = YouYi(floatBinaryTwoweiShu, JieMaCha, Sign.signed);//算术右移
                resultJieMa = floatBinaryOnejieMa;//阶码采用较大的一个 第一个数的阶码
                if (gaoWeiOfDiuQi == '1')//如果被丢掉位最高位是1 进行舍入处理即加1
                {
                    //Add(floatBinaryOneweiShu, "00000000001", 1, 11, Sign.signed, out floatBinaryOneweiShu);//有错误 计算了第一个数
                    ZiZengJian(floatBinaryTwoweiShu, 1, Sign.signed, out floatBinaryTwoweiShu);
                }
            }
            #endregion
            /*求和部分*/
            string weiShuHe;
            int check = Add(floatBinaryOneweiShu, floatBinaryTwoweiShu, 1, floatBinaryOne.Length - jieMaWeiShu, Sign.signed, out weiShuHe);//尾数求和
            if (check == 0)//求和结果未溢出
            {
                resultNum = GuiGeHua(resultJieMa + weiShuHe, jieMaWeiShu);//返回经过规格化处理的结果 向左规格化
                return 0;
            }
            else//溢出
            {
                /*向右规格化 正溢出与负溢出处理不同*/
                if (weiShuHe[0] == '1')//正溢出
                {
                    char gaoWeiOfDiuQi = weiShuHe[weiShuHe.Length - 1];//记录被丢掉位最高位
                    //resultNum = YouYi(weiShuHe, 1, Sign.unSigned);//逻辑右移
                    string resultWeiShuHe = YouYi(weiShuHe, 1, Sign.unSigned);//逻辑右移
                    //check = Add(resultJieMa, "00001", 1, 5, Sign.signed, out tempNum);
                    check = ZiZengJian(resultJieMa, 1, Sign.signed, out resultJieMa);
                    if (gaoWeiOfDiuQi == '1')//如果被丢掉位最高位是1 进行舍入处理即加1
                    {
                        //Add(resultNum, "00000000001", 1, 11, Sign.signed, out resultNum);
                        ZiZengJian(resultWeiShuHe, 1, Sign.signed, out resultWeiShuHe);
                    }
                    resultNum = resultJieMa + resultWeiShuHe;
                    if (check == 0)
                    {
                        return 0;//0正常
                    }
                    else
                    {
                        return 7;//7运算结果溢出
                    }
                }
                else//负溢出
                {
                    char gaoWeiOfDiuQi = weiShuHe[weiShuHe.Length - 1];//记录被丢掉位最高位
                    //resultNum = YouYi(weiShuHe, 1, Sign.unSigned);//逻辑右移
                    string resultWeiShuHe = YouYi(weiShuHe, 1, Sign.unSigned);//逻辑右移
                    resultWeiShuHe = "1" + resultWeiShuHe.Substring(1);//最高位为1，表示负值
                    //check = Add(resultJieMa, "00001", 1, 5, Sign.signed, out tempNum);
                    check = ZiZengJian(resultJieMa, 1, Sign.signed, out resultJieMa);
                    if (gaoWeiOfDiuQi == '1')//如果被丢掉位最高位是1 进行舍入处理即加1
                    {
                        //Add(resultNum, "00000000001", 1, 11, Sign.signed, out resultNum);
                        ZiZengJian(resultWeiShuHe, 1, Sign.signed, out resultWeiShuHe);
                    }
                    resultNum = resultJieMa + resultWeiShuHe;
                    if (check == 0)
                    {
                        return 0;//0正常
                    }
                    else
                    {
                        return 7;//7运算结果溢出
                    }
                }
            }
        }
        /// <summary>
        /// 浮点数规格化（适用补码表示） 向左规格化 尾数左移一位，阶码-1
        /// 返回规格化后的二进制浮点数 null规格化时失败
        /// </summary>
        /// <param name="floatNum">浮点数</param>
        /// <param name="jieMaWeiShu">阶码位数</param>
        /// <returns>返回规格化后的二进制浮点数</returns>
        private static string GuiGeHua(string floatNum, int jieMaWeiShu)
        {
            Debug.Assert(!string.IsNullOrEmpty(floatNum));
            string jieMa = floatNum.Substring(0, jieMaWeiShu);//阶码
            string weiShu = floatNum.Substring(jieMaWeiShu);//尾数;
            if (weiShu[0] != weiShu[1])//判断是否规格化
            {
                return floatNum;//返回规格化的数
            }
            else//不是规格化
            {
                int i = 0;//记录移动的次数
                while (weiShu[0] == weiShu[1] && i < floatNum.Length - jieMaWeiShu)
                {
                    weiShu = ZuoYi(weiShu, 1);
                    i++;
                }
                string yiDongWeiShu;//存放经i转换后的二进制
                string returnJieMa;//阶码变化后的值
                if (DecimalToBinary(i.ToString(), Notation.dingDianZhengShu, Coding.buMa, jieMaWeiShu, Sign.signed, out yiDongWeiShu) == 0)
                {
                    Add(jieMa, yiDongWeiShu, 2, jieMaWeiShu, Sign.signed, out returnJieMa);//做减法运算 求得规格化后的阶码
                    return returnJieMa + weiShu;//返回规格化后的结果
                }
                else
                {
                    return null;
                }
            }
        }
    }
}








