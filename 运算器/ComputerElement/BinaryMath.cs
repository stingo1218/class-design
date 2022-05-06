using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ComputerElement
{
    /// <summary>
    /// 数的表示方式
    /// </summary>
    public enum Notation
    { dingDianZhengShu, dingDianXiaoShu }
    /// <summary>
    /// 是否有符号位
    /// </summary>
    public enum Sign
    { signed, unSigned }
    public static class BinaryMath
    {
        //#if(DEBUG)
        //        private static string errorMessNumNull = "数为空";
        //        private static string errorMessWei = "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1";
        //        private static string errorMessYiDongWeiShu = "移动位数至少两位";
        //#endif
        /// <summary>
        /// 十进制转换为二进制原码
        /// 返回转换状态 1运算结果正确 0输入十进制数超出指定位数所能表示范围
        /// </summary>
        /// <param name="decimalNum">待转换的数字</param>
        /// <param name="notation">定点整数，定点小数</param>
        /// <param name="digit">二进制位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>返回转换状态 1运算结果正确 0输入十进制数超出指定位数所能表示范围</returns>
        public static int DecimalToBinary(string decimalNum, Notation notation, int digit, Sign sign, out string resultNum)
        {
            #region 参数确认 检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            if (notation == Notation.dingDianZhengShu)//如果是求整数的二进制定点整数
            {
                return DecimalToBinaryInteger(decimalNum, digit, sign, out resultNum);
            }
            else//如果是求小数的二进制定点小数
            {
                return DecimalToBinaryDecimal(decimalNum, digit, sign, out resultNum);
            }
        }
        /// <summary>
        /// 十进制转换为原码二进制定点整数
        /// 返回转换状态  1运算结果正确 0输入十进制数超出指定位数所能表示范围
        /// </summary>
        /// <param name="decimalNum">待转换的数字</param>
        /// <param name="digit">二进制位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>返回转换状态  1运算结果正确 0输入十进制数超出指定位数所能表示范围</returns>
        public static int DecimalToBinaryInteger(string decimalNum, int digit, Sign sign, out string resultNum)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            int decimalNumToInt = int.Parse(decimalNum);
            /*求二进制数有效数位 若为无符号的总位数就为有效位数
              若为有符号的就是 总位数-1 */
            int youXiaoShuWei;//标记有效位数
            string fuHao;//标记符号位
            if (sign == Sign.signed)//如果是有符号的
            {
                youXiaoShuWei = digit - 1;
                if (decimalNumToInt < 0)
                {
                    decimalNumToInt = -decimalNumToInt;
                    fuHao = "1";//添加符号位1
                }
                else
                {
                    fuHao = "0";//添加符号位0
                }
            }
            else//如果是无符号的
            {
                youXiaoShuWei = digit;
                fuHao = null;//无符号数符号位空
            }
            if (CheckNoChaoChang(decimalNum, Notation.dingDianZhengShu, digit, sign))//检查十进制数是否超出所能表示范围
            {
                //进行转换的部分
                int yu;//存放余数
                int i = 0;//标记转换了的位数个数
                string strNumber = string.Empty;//存储十进制数的绝对值转换的最后结果
                while (decimalNumToInt != 0 && i < youXiaoShuWei)//当商不空 且没有超出有效数位
                {
                    yu = decimalNumToInt % 2;//求得二进制各位
                    strNumber = yu.ToString() + strNumber;//拼接在一起
                    i++;
                    decimalNumToInt = decimalNumToInt / 2;//求除余后剩余的商
                }
                //若前面转换部分没有填满所有位 则前端补零
                while (i < youXiaoShuWei)
                {
                    strNumber = "0" + strNumber;
                    i++;
                }
                resultNum = fuHao + strNumber;//符号位和绝对值部分拼接返回
                return 1;//运算结果正确
            }
            else
            {
                resultNum = null;
                return 0;//输入十进制数超出指定位数所能表示范围
            }
        }
        /// <summary>
        /// 十进制转换为原码二进制定点小数（原码）
        /// 返回转换状态 1运算结果正确 0输入十进制数超出指定位数所能表示范围
        /// </summary>
        /// <param name="decimalNum">待转换的数字</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>返回转换状态 1运算结果正确 0输入十进制数超出指定位数所能表示范围</returns>
        public static int DecimalToBinaryDecimal(string decimalNum, int digit, Sign sign, out string resultNum)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            float decimalNumToInt = float.Parse(decimalNum);
            //求二进制数有效数位 若为无符号的总位数就为有效位数
            //若为有符号的就是 总位数-1
            int youXiaoShuWei;//标记有效数位
            string fuHao;//标记符号位
            if (sign == Sign.signed)//如果是有符号的
            {
                youXiaoShuWei = digit - 1;
                if (decimalNumToInt < 0)
                {
                    decimalNumToInt = -decimalNumToInt;
                    fuHao = "1"; //添加符号位
                }
                else
                {
                    fuHao = "0"; //添加符号位
                }
            }
            else//如果是无符号的
            {
                youXiaoShuWei = digit;//有效位数是所有位数
                fuHao = null;//无符号数符号位空
            }
            if (CheckNoChaoChang(decimalNum, Notation.dingDianXiaoShu, digit, sign))//检测数据是否超出二进制所能表示的范围
            {
                int wei;//存放各位数
                int i = 0;//标记转换了的位数 以便控制转换的位数
                string strNumber = string.Empty;//存储最后的返回结果
                while (decimalNumToInt != 0 && i < youXiaoShuWei)//当乘积不空 且没有超出有效数位
                {
                    decimalNumToInt = decimalNumToInt * 2;
                    wei = (int)decimalNumToInt;//求得二进制各位
                    strNumber = strNumber + wei.ToString();
                    decimalNumToInt = decimalNumToInt - wei;//把整数去除
                    i++;
                }
                //若前面转换部分没有填满所有位 则末端补零
                while (i < youXiaoShuWei)
                {
                    strNumber = strNumber + "0";
                    i++;
                }
                resultNum = fuHao + strNumber;//符号位和绝对值部分拼接返回
                return 1;//运算结果正确
            }
            else
            {
                resultNum = null;
                return 0;//输入十进制数超出指定位数所能表示范围
            }
        }
        /// <summary>
        /// 原码二进制转换为十进制（含定点整数 定点小数）
        /// </summary>
        /// <param name="binaryNum">待转换的数字</param>
        /// <param name="notation">1定点整数，2定点小数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回转换结果</returns>
        public static string BinaryToDecimal(string binaryNum, Notation notation, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            #endregion
            if (notation == Notation.dingDianZhengShu)//如果转换的是定点整数
            {
                return BinaryIntegerToInteger(binaryNum, sign);//整数转换
            }
            else
            {
                return BinaryDecimalToDecimal(binaryNum, sign);//小数转换
            }
        }
        /// <summary>
        /// 原码二进制定点小数转换为十进制小数
        /// </summary>
        /// <param name="binaryNum">待转换的数字</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回执行结果</returns>
        public static string BinaryDecimalToDecimal(string binaryNum, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            #endregion
            int startWei;//标记从哪一位开始转换 即有效位是从第几位开始
            if (sign == Sign.signed)//有符号位
            {
                startWei = 1;//若是有符号位 在从第二个字符开始 下标为1
            }
            else
            {
                startWei = 0;//若是无符号位 在从第一个字符开始 下标为0
            }
            float number = 0;//存放中间结果
            int j = 1;//用以设置位权0-j
            for (; startWei < binaryNum.Length; startWei++)
            {
                //每一位的数与其权相乘
                //num += float.Parse(binaryNum[startWei].ToString()) * float.Parse((Math.Pow(2, 0 - j - 1)).ToString());
                number += Convert.ToSingle(binaryNum[startWei].ToString()) * Convert.ToSingle(Math.Pow(2, 0 - j));
                j++;
            }
            //如果是负数添加负号
            if (sign == Sign.signed && binaryNum[0] == '1')
            {
                number = -number;
            }
            return number.ToString();
        }
        /// <summary>
        /// 将原码二进制定点整数转换为十进制整数（原码运算）
        /// </summary>
        /// <param name="binaryNum">待转换的二进制数字</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回执行结果</returns>
        public static string BinaryIntegerToInteger(string binaryNum, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            #endregion
            int startWei;//标记从哪一位开始转换 即有效位是从第几位开始
            if (sign == Sign.signed)
            {
                startWei = 1;//若是有符号位 在从第二个字符开始
            }
            else
            {
                startWei = 0;//若是无符号位 在从第一个字符开始
            }
            int number = 0;//存放中间结果供返回
            for (; startWei < binaryNum.Length; startWei++)
            {
                //此时位权不用像二进制定点小数转换为小数时的位权因有无符号的变动
                number += Convert.ToInt32(binaryNum[startWei].ToString()) * Convert.ToInt32(Math.Pow(2, binaryNum.Length - startWei - 1));
            }
            //如果是负数添加负号
            if (sign == Sign.signed && binaryNum[0] == '1')
            {
                number = -number;
            }
            return number.ToString();
        }
        /// <summary>
        /// 计算两个二进制的和（当作无符号数直接相加 对于负数（除补码外）计算不出真实结果）
        /// 返回执行状态 1运算结果未溢出 0运算结果溢出
        /// </summary>
        /// <param name="num1">数1</param>
        /// <param name="num2">数2</param>
        /// <param name="addMode">传1做加法 2做减法</param>
        /// <param name="digit">运算位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">返回值</param>
        /// <returns>返回执行状态 1运算结果未溢出 0运算结果溢出</returns>
        public static int Add(string num1, string num2, int addMode, Sign sign, out string resultNum)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert((!string.IsNullOrEmpty(num1)) && (!string.IsNullOrEmpty(num2)), "数为空");
            Debug.Assert(addMode == 1 || addMode == 2, "运算模式取值1加法或2减法");
            //两数相加有符号至少是两位 无符号至少是1位
            Debug.Assert(sign == Sign.signed ? num1.Length >= 2 && num2.Length >= 2 : num1.Length >= 1 && num2.Length >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            int state = 1;//存放加法器运算状态 1表示运算结果未溢出，但由于有断言检测因此此处1不起作用
            /*addMode==2 减法运算 求num2的相反数的补码再相加*/
            if (addMode == 2)//减法
            {
                #region 求num2的想法数的补码 实现求减法运算
                string temp = QiuFan(num2, 0);
                string n = "1";
                for (int i = 0; i < num2.Length - 1; i++)
                {
                    n = "0" + n;
                }
                Add(temp, n, 1, sign, out num2);
                #endregion
            }
            if (sign == Sign.unSigned)//若是为无符号数添加一个默认的0符号位
            {
                num1 = "0" + num1;
                num2 = "0" + num2;
            }
            //从低位到高位逐渐相加运算（串行相加方式）
            string zhiJieXiangJiaHe = string.Empty;//存放直接运算的结果
            string jinWei = "0";//每位的和          
            string weiJia = "0";//每位的进位
            string youXiaoGaoWeiJinWei = "0";//保存最高有效位产生的进位 以便和符号位产生的进位相比较判断是否溢出
            string returnTempNum = null;//寄存要返回的中间结果
            for (int i = num1.Length - 1; i >= 0; i--)//从最低位开始相加
            {
                int n = int.Parse(num1[i].ToString()) + int.Parse(num2[i].ToString()) + int.Parse(jinWei);//两数对应位与低位进位相加
                //对于相加的结果分为以下四种情况
                if (n == 0)
                {
                    weiJia = "0";
                    jinWei = "0";
                }
                else if (n == 1)
                {
                    weiJia = "1";
                    jinWei = "0";
                }
                else if (n == 2)
                {
                    weiJia = "0";
                    jinWei = "1";
                }
                else //if (n == 3)
                {
                    weiJia = "1";
                    jinWei = "1";
                }
                zhiJieXiangJiaHe = weiJia + zhiJieXiangJiaHe;//拼接得到直接相加结果
                #region  对最高有效位和符号位处理
                if (i == 1 || i == 0)
                {
                    if (i == 1)//计算有效最高位
                    {
                        youXiaoGaoWeiJinWei = jinWei;
                    }
                    if (i == 0)//计算符号位 有符号与无符号处理不同
                    {
                        if (sign == Sign.signed)//有符号
                        {
                            returnTempNum = zhiJieXiangJiaHe;
                        }
                        else                    //无符号
                        {
                            returnTempNum = zhiJieXiangJiaHe.Substring(1);//对于无符号数从第二个数截取以除去前面加上的符号位
                        }
                        //计算运算结果有误溢出 最高有效位进位与符号位进位比较
                        if (youXiaoGaoWeiJinWei == jinWei)
                        {
                            state = 1;//运算结果未溢出
                        }
                        else
                        {
                            state = 0;//运算结果溢出
                        }
                    }
                }
                #endregion
            }
            resultNum = returnTempNum;//返回结果
            return state;//返回结果状态
        }
        /// <summary>
        /// 计算两个二进制反码的和（当作无符号数直接相加 对于负数（除补码外）计算不出真实结果）
        /// 返回执行状态 1运算结果未溢出 0溢出
        /// </summary>
        /// <param name="num1">数1</param>
        /// <param name="num2">数2</param>
        /// <param name="digit">运算位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <param name="resultNum">执行结果</param>
        /// <returns>返回执行状态 1运算结果未溢出 0溢出</returns>
        public static int AddForFanMa(string num1, string num2, Sign sign, out string resultNum)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert((!string.IsNullOrEmpty(num1)) && (!string.IsNullOrEmpty(num2)), "数为空");
            //两数相加有符号至少是两位 无符号至少是1位
            Debug.Assert(sign == Sign.signed ? num1.Length >= 2 && num2.Length >= 2 : num1.Length >= 1 && num2.Length >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            //从低位到高位逐渐相加运算（串行相加方式）
            //如果是无符号的数添加默认的符号0
            if (sign == Sign.unSigned)
            {
                num1 = "0" + num1;
                num2 = "0" + num2;
            }
            string zhiJieXiangJiaHe = string.Empty;//存放最终运算的结果
            string jinwei = "0";//每位的和
            string weijia = "0";//每位的进位
            string youXiaoGaoWeiJinWei = "0";//保存最高有效位产生的进位 以便和符号位产生的进位相比较判断是否溢出
            int state = 1;//存放加法器运算状态 1表示运算结果未溢出，但由于有断言检测因此此处1不起作用
            string returnTempNum = null; //存放中间结果
            //从最低位开始逐位相加
            for (int i = num1.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(num1[i].ToString()) + int.Parse(num2[i].ToString()) + int.Parse(jinwei);//对应各位与进位相加
                //对上面相加的结果分成以下四种结果
                if (n == 0)
                {
                    weijia = "0";
                    jinwei = "0";
                }
                else if (n == 1)
                {
                    weijia = "1";
                    jinwei = "0";
                }
                else if (n == 2)
                {
                    weijia = "0";
                    jinwei = "1";
                }
                else //if (n == 3)
                {
                    weijia = "1";
                    jinwei = "1";
                }
                zhiJieXiangJiaHe = weijia + zhiJieXiangJiaHe;//拼接得到直接相加结果
                #region  对最高有效位和符号位处理
                if (i == 1 || i == 0)
                {
                    if (i == 1)//计算有效最高位
                    {
                        youXiaoGaoWeiJinWei = jinwei;
                    }
                    if (i == 0)//计算符号位 有符号与无符号处理不同
                    {
                        if (sign == Sign.signed)//有符号
                        {
                            returnTempNum = zhiJieXiangJiaHe;
                        }
                        else                    //无符号
                        {
                            returnTempNum = zhiJieXiangJiaHe.Substring(1);
                        }
                        //计算运算结果有误溢出 最高有效位进位与符号位进位比较
                        if (youXiaoGaoWeiJinWei == jinwei)
                        {
                            state = 1;//运算结果未溢出
                        }
                        else
                        {
                            state = 0;//运算结果溢出
                        }
                    }
                }
                #endregion
            }
            if (jinwei == "0")//最高位没有产生进位
            {
                resultNum = returnTempNum;
            }
            else             //最高位产生进位 结果加1
            {
                #region 旧代码
                //string addNum = "";
                //for (int i = 0; i < num1.Length - 1; i++)
                //{
                //    addNum += "0";
                //}
                //addNum = addNum + "1";
                //Add(returnTempNum, addNum, 1, sign, out resultNum);
                #endregion
                ZiZengJian(returnTempNum, 1, sign, out resultNum);
            }
            return state;//返回状态
        }
        /// <summary>
        /// 求位反
        /// </summary>
        /// <param name="binaryNum">二进制数</param>
        /// <param name="startIndex">从哪一位开始求反从0开始</param>
        /// <returns>返回求反结果</returns>
        public static string QiuFan(string binaryNum, int startIndex)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(startIndex >= 0, "索引>=0");
            #endregion
            int i = startIndex;
            string num = string.Empty;
            for (int j = 0; j < i; j++)
            {
                num = num + binaryNum[j].ToString();
            }
            for (; i < binaryNum.Length; i++)
            {
                if (binaryNum[i] == '0')
                {
                    num = num + "1";
                }
                else
                {
                    num = num + "0";
                }
            }
            return num;
        }
        /// <summary>
        /// 求二进制反码
        /// </summary>
        /// <param name="binaryNum">待求的二进制数</param>
        /// <param name="sign">有无符号 有符号 无符号</param>
        /// <returns>返回执行结果</returns>
        public static string GetFanMa(string binaryNum, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            #endregion
            if (sign == Sign.signed)
            {
                if (binaryNum[0] == '1')
                {
                    return QiuFan(binaryNum, 1);//求反码 从第二个字符开始求反
                }
                else//若是正数直接返回
                {
                    return binaryNum;
                }
            }
            else//若是无符号数直接返回
            {
                return binaryNum;
            }
        }
        /// <summary>
        /// 求二进制补码
        /// </summary>
        /// <param name="binaryNum">待求的二进制原码</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回运算结果</returns>
        public static string GetBuMa(string binaryNum, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            #endregion
            if (sign == Sign.signed)//有符号
            {
                if (binaryNum[0] == '1')//若是负数
                {
                    string tempNum = GetFanMa(binaryNum, sign);
                    #region 旧代码
                    //string jiaYi = "1";
                    //for (int i = 0; i < binaryNum.Length - 1; i++)
                    //{
                    //    jiaYi = "0" + jiaYi;
                    //}
                    //Add(tempNum, jiaYi, 1, sign, out tempNum);//1是相加模式
                    #endregion
                    ZiZengJian(tempNum, 1, sign, out tempNum);
                    return tempNum;
                }
                else //若是正数直接返回
                {
                    return binaryNum;
                }
            }
            else//无符号数直接返回
            {
                return binaryNum;
            }
        }
        /// <summary>
        /// 求十进制数得到相应的二进制补码表示 注意所指定的二进制位数是否不够表示所给的十进制数
        /// 返回运算结果 null超出所能表示范围
        /// </summary>
        /// <param name="decimalNum">十进制数</param>
        /// <param name="notation">数据表示方式，定点整数 定点小数</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回运算结果 null超出所能表示范围</returns>
        public static string GetBuMa(string decimalNum, Notation notation, int digit, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            if (sign == Sign.signed)//如果是有符号数
            {
                if (CheckIsBuMaZuiXiaoFuShu(decimalNum, notation, digit, sign))//只有补码能表示的最小的范围
                {
                    string jieGuo = "1";
                    for (int i = 1; i < digit; i++)
                    {
                        jieGuo += "0";
                    }
                    return jieGuo;//返回只有补码能表示的最小的数
                }
                else//不是只有补码能表示的最小的范围 先转换成原码，再转换成补码
                {
                    string tempNum;//存放由DecimalToBinary返回的结果
                    int check = DecimalToBinary(decimalNum, notation, digit, sign, out tempNum);//十进制转换成二进制原码
                    if (check == 1)
                    {
                        return GetBuMa(tempNum, sign);
                    }
                    else
                    {
                        return null;//DecimalToBinary转换失败 decimalNum超出所能表示范围
                    }
                }
            }
            else//如果是无符号数
            {
                string tempNum;//存放由DecimalToBinary返回的结果
                int check = DecimalToBinary(decimalNum, notation, digit, sign, out tempNum);//计算原码即为补码
                if (check == 1)
                {
                    return tempNum;
                }
                else
                {
                    return null;//decimalNum超出所能表示范围
                }
            }
        }
        /// <summary>
        /// 检查补码二进制数是不是补码能表示的最小负数
        /// </summary>
        /// <param name="binaryNum">二进制数</param>
        /// <returns>true是最小数 false不是</returns>
        public static bool CheckBinaryIsBuMaMin(string binaryNum, Sign sign)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            #endregion
            if (sign == Sign.signed)
            {
                string checkNum = "1";
                for (int i = 0; i < binaryNum.Length - 1; i++)
                {
                    checkNum = checkNum + "0";
                }
                if (binaryNum == checkNum)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 自增自减 相等于i++ i--
        /// 返回执行状态 1运算结果未溢出 0运算结果溢出
        /// </summary>
        /// <param name="binaryNum">二进制数</param>
        /// <param name="addMode">运算模式 1加法 2减法</param>
        /// <param name="sign">是否有符号位</param>
        /// <param name="resultNum">返回结果</param>
        /// <returns>返回执行状态 1运算结果未溢出 0运算结果溢出</returns>
        public static int ZiZengJian(string binaryNum, int addMode, Sign sign, out string resultNum)
        {
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            string addNum = "1";
            for (int i = 0; i < binaryNum.Length - 1; i++)
            {
                addNum = "0" + addNum;
            }
            return Add(binaryNum, addNum, addMode, sign, out resultNum);
        }
        /// <summary>
        /// 检查decimalNum十进制参数是不是补码所能表示的最小负数
        /// true是最小数 false不是最小数
        /// </summary>
        /// <param name="decimalNum">待检查的十进制数据</param>
        /// <param name="notation">数据表示方式，定点整数 定点小数</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，为无符号</param>
        /// <returns>true是最小数 false不是最小数</returns>
        public static bool CheckIsBuMaZuiXiaoFuShu(string decimalNum, Notation notation, int digit, Sign sign)
        {
            #region 参数确认
            //检查参数有效性
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            if (notation == Notation.dingDianZhengShu)//***定点整数
            {
                if (sign == Sign.signed)//有符号
                {
                    if (int.Parse(decimalNum) == (-Math.Pow(2, digit - 1)))//与指定位数的最小补码数比较
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else//无符号不存在最小负数
                {
                    return false;
                }
            }
            else //***********************************定点小数
            {
                if (sign == Sign.signed)//有符号
                {
                    if (float.Parse(decimalNum) == -1)//定点小数补码最小能表示-1
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else//无符号不存在最小负数
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 检查输入数据是否超出给定位数表示的范围 对于原码和反码有效
        /// true不超出字长 false超出字长
        /// </summary>
        /// <param name="num">待检查的数字</param>
        /// <param name="notation">定点整数检测 定点小数检测</param>
        /// <param name="digit">给定的二进制位数</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>true不超出字长 false超出字长</returns>
        public static bool CheckNoChaoChang(string num, Notation notation, int digit, Sign sign)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(num), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            if (notation == Notation.dingDianZhengShu)//定点整数
            {
                if (sign == Sign.signed)//有符号
                {
                    //实际检查是否超出范围 digit - 1有符号数的有效位数
                    int fanWei = Convert.ToInt32((Math.Pow(2, digit - 1) - 1));//指定位数能表示的范围的绝对值
                    if (int.Parse(num) >= (-fanWei) && int.Parse(num) <= (fanWei))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else//无符号
                {
                    //实际检查是否超出范围
                    if (int.Parse(num) >= 0 && int.Parse(num) <= (Math.Pow(2, digit) - 1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else//定点小数
            {
                if (sign == Sign.signed)//有符号
                {
                    if (float.Parse(num) > -1 && float.Parse(num) < 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else//无符号
                {
                    if (float.Parse(num) >= 0 && float.Parse(num) < 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 实现强制将十进制数转换成二进制原码（转换成原码） 超出表示范围的部分则丢弃
        /// DecimalToBinary在不能转换超出二进制位数表示范围的十进制数
        /// </summary>
        /// <param name="decimalNum">强制转换的数</param>
        /// <param name="notation">定点整数 定点小数</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回运算结果</returns>
        public static string QiangZhiDecimalToBinary(string decimalNum, Notation notation, int digit, Sign sign)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            if (notation == Notation.dingDianZhengShu)//转换成定点整数
            {
                return QiangZhiZhuanHuanToInteger(decimalNum, digit, sign);
            }
            else
            {
                return QiangZhiZhuanHuanToBinaryDecimal(decimalNum, digit, sign);
            }
        }
        /// <summary>
        /// 十进制小数强制转换成二进制小数 大于二进制位数表示范围则丢弃（转换成原码）
        /// </summary>
        /// <param name="decimalNum">十进制小数</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回运算结果</returns>
        private static string QiangZhiZhuanHuanToBinaryDecimal(string decimalNum, int digit, Sign sign)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            #region 老代码
            //int youXiaoShuWei;
            ////求二进制数有效数位 若为无符号的总位数就为有效位数
            ////若为有符号的就是 总位数-1
            //if (sign == Sign.signed)
            //{
            //    youXiaoShuWei = digit - 1;
            //}
            //else
            //{
            //    youXiaoShuWei = digit;
            //}
            //bool isNegative = false;//标记是否是负数
            //float n = float.Parse(decimalNum);
            //if (n < 0)
            //{
            //    isNegative = true;
            //    n = -n;
            //}
            //int wei;//存放各位数
            //int i = 0;
            //string strNum = string.Empty;//存储最后的返回结果
            //while (n != 0 && i < youXiaoShuWei)//当乘积不空 且没有超出有效数位
            //{
            //    n = n * 2;
            //    wei = (int)n;//求得二进制各位
            //    strNum = strNum + wei.ToString();
            //    n = n - wei;//把整数去除
            //    i++;
            //}
            ////若前面转换部分没有填满所有位 则末端补零
            //while (i < youXiaoShuWei)
            //{
            //    strNum = strNum + "0";
            //    i++;
            //}
            ////添加符号位
            //if (sign == Sign.signed)
            //{
            //    if (isNegative)
            //    {
            //        strNum = "1" + strNum;
            //    }
            //    else
            //    {
            //        strNum = "0" + strNum;
            //    }
            //}
            //return strNum;//返回运算结果
            #endregion
            /*********新代码***********/
            float decimalNumToInt = float.Parse(decimalNum);
            //求二进制数有效数位 若为无符号的总位数就为有效位数
            //若为有符号的就是 总位数-1
            int youXiaoShuWei;//标记有效数位
            string fuHao;//标记符号位
            if (sign == Sign.signed)//如果是有符号的
            {
                youXiaoShuWei = digit - 1;
                if (decimalNumToInt < 0)
                {
                    decimalNumToInt = -decimalNumToInt;
                    fuHao = "1"; //添加符号位
                }
                else
                {
                    fuHao = "0"; //添加符号位
                }
            }
            else//如果是无符号的
            {
                youXiaoShuWei = digit;//有效位数是所有位数
                fuHao = null;//无符号数符号位空
            }
            int wei;//存放各位数
            int i = 0;//标记转换了的位数 以便控制转换的位数
            string strNumber = string.Empty;//存储最后的返回结果
            while (decimalNumToInt != 0 && i < youXiaoShuWei)//当乘积不空 且没有超出有效数位
            {
                decimalNumToInt = decimalNumToInt * 2;
                wei = (int)decimalNumToInt;//求得二进制各位
                strNumber = strNumber + wei.ToString();
                decimalNumToInt = decimalNumToInt - wei;//把整数去除
                i++;
            }
            //若前面转换部分没有填满所有位 则末端补零
            while (i < youXiaoShuWei)
            {
                strNumber = strNumber + "0";
                i++;
            }
            return fuHao + strNumber;//符号位和绝对值部分拼接返回
        }
        /// <summary>
        /// 十进制整数强制转成二进制整数 大于二进制位数表示范围则丢弃（原码转换）
        /// </summary>
        /// <param name="decimalNum">十进制整数</param>
        /// <param name="digit">二进制位数八位或十六位</param>
        /// <param name="sign">有无符号，有符号，无符号</param>
        /// <returns>返回运算结果</returns>
        private static string QiangZhiZhuanHuanToInteger(string decimalNum, int digit, Sign sign)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(decimalNum), "数为空");
            Debug.Assert(sign == Sign.signed ? digit >= 2 : digit >= 1, "位数参数错误\r\n有符号数位数>=2 无符号数位数>=1");
            #endregion
            #region 老代码
            //int youXiaoShuWei;
            ////求二进制数有效数位 若为无符号的总位数就为有效位数
            ////若为有符号的就是 总位数-1
            //if (sign == Sign.signed)//如果是有符号的
            //{
            //    youXiaoShuWei = digit - 1;
            //}
            //else
            //{
            //    youXiaoShuWei = digit;
            //}
            //bool isNegative = false;//判断是否是负数
            //int n = int.Parse(decimalNum);
            //if (n < 0)
            //{
            //    isNegative = true;
            //    n = -n;
            //}
            ////进行转换的部分
            //int yu;//存放余数
            //int i = 0;
            //string strNum = string.Empty;//存储最后的返回结果
            //while (n != 0 && i < youXiaoShuWei)//当商不空 且没有超出有效数位
            //{
            //    yu = n % 2;//求得二进制各位
            //    strNum = yu.ToString() + strNum;//拼接在一起
            //    i++;
            //    n = n / 2;//求除余后剩余的商
            //}
            ////若前面转换部分没有填满所有位 则前端补零
            //while (i < youXiaoShuWei)
            //{
            //    strNum = "0" + strNum;
            //    i++;
            //}
            ////添加符号位
            //if (sign == Sign.signed)
            //{
            //    if (isNegative)
            //    {
            //        strNum = "1" + strNum;
            //    }
            //    else
            //    {
            //        strNum = "0" + strNum;
            //    }
            //}
            //return strNum;//返回运算结果 
            #endregion
            int decimalNumToInt = int.Parse(decimalNum);
            /*求二进制数有效数位 若为无符号的总位数就为有效位数
              若为有符号的就是 总位数-1 */
            int youXiaoShuWei;//标记有效位数
            string fuHao;//标记符号位
            if (sign == Sign.signed)//如果是有符号的
            {
                youXiaoShuWei = digit - 1;
                if (decimalNumToInt < 0)
                {
                    decimalNumToInt = -decimalNumToInt;
                    fuHao = "1";//添加符号位1
                }
                else
                {
                    fuHao = "0";//添加符号位0
                }
            }
            else//如果是无符号的
            {
                youXiaoShuWei = digit;
                fuHao = null;//无符号数符号位空
            }
            //进行转换的部分
            int yu;//存放余数
            int i = 0;//标记转换了的位数个数
            string strNumber = string.Empty;//存储十进制数的绝对值转换的最后结果
            while (decimalNumToInt != 0 && i < youXiaoShuWei)//当商不空 且没有超出有效数位
            {
                yu = decimalNumToInt % 2;//求得二进制各位
                strNumber = yu.ToString() + strNumber;//拼接在一起
                i++;
                decimalNumToInt = decimalNumToInt / 2;//求除余后剩余的商
            }
            //若前面转换部分没有填满所有位 则前端补零
            while (i < youXiaoShuWei)
            {
                strNumber = "0" + strNumber;
                i++;
            }
            return fuHao + strNumber;//符号位和绝对值部分拼接返回
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
            #region 老代码
            //string resultNum = "";//记录最终结果
            //string tempNum = binaryNum;//记录各种中间结果
            //for (int j = 0; j < yiDongWeiShu; j++)
            //{
            //    string temp = "";
            //    for (int i = 1; i < tempNum.Length; i++)
            //    {
            //        temp = temp + tempNum[i];
            //    }
            //    resultNum = temp + "0";
            //    tempNum = resultNum;
            //}
            //return resultNum;
            #endregion
            string tempNum = binaryNum;//记录各种中间结果
            for (int j = 0; j < yiDongWeiShu; j++)
            {
                string temp = "";
                for (int i = 1; i < tempNum.Length; i++)
                {
                    temp = temp + tempNum[i];
                }
                tempNum = temp + "0";
            }
            return tempNum;
        }
        /// <summary>
        /// 实现逻辑右移
        /// </summary>
        /// <param name="binaryNum">移动的二进制数据</param>
        /// <param name="yiDongWeiShu">移动的位数</param>
        /// <returns>返回移动结果</returns>
        public static string LuoJiYouYi(string binaryNum, int yiDongWeiShu)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(yiDongWeiShu > 0, "移动位数至少两位");
            #endregion
            #region 老代码
            //string resultNum = "";
            //string tempNum = binaryNum;
            //for (int j = 0; j < yiDongWeiShu; j++)
            //{
            //    string temp = "";
            //    for (int i = 0; i < tempNum.Length - 1; i++)
            //    {
            //        temp = temp + tempNum[i];
            //    }
            //    resultNum = "0" + temp;
            //    tempNum = resultNum;
            //}
            //return resultNum;
            #endregion
            string tempNum = binaryNum;//记录各种中间结果
            for (int j = 0; j < yiDongWeiShu; j++)
            {
                string temp = "";
                for (int i = 0; i < tempNum.Length - 1; i++)
                {
                    temp = temp + tempNum[i];
                }
                tempNum = "0" + temp;
            }
            return tempNum;
        }
        /// <summary>
        /// 实现逻辑右移,并返回移出位
        /// </summary>
        /// <param name="binaryNum">移动的二进制数据</param>
        /// <param name="yiDongWeiShu">移动的位数</param>
        /// <returns>返回移动结果</returns>
        public static string LuoJiYouYi_Return(string binaryNum, int yiDongWeiShu,out char YiChuWei)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(yiDongWeiShu > 0, "移动位数至少两位");
            #endregion
            string tempNum = binaryNum;//记录各种中间结果
            YiChuWei = tempNum[7];
            for (int j = 0; j < yiDongWeiShu; j++)
            {
                string temp = "";
                for (int i = 0; i < tempNum.Length - 1; i++)
                {
                    temp = temp + tempNum[i];
                }
                tempNum = "0" + temp;
            }
            return tempNum;
        }
        /// <summary>
        /// 实现逻辑右移,并引入ACC的移出位
        /// </summary>
        /// <param name="binaryNum">移动的二进制数据</param>
        /// <param name="yiDongWeiShu">移动的位数</param>
        /// <returns>返回移动结果</returns>
        public static string LuoJiYouYi_POP(string binaryNum, int yiDongWeiShu, char YiChuWei)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(yiDongWeiShu > 0, "移动位数至少两位");
            #endregion
            string tempNum = binaryNum;//记录各种中间结果
            for (int j = 0; j < yiDongWeiShu; j++)
            {
                string temp = "";
                for (int i = 0; i < tempNum.Length - 1; i++)
                {
                    temp = temp + tempNum[i];
                }
                tempNum = YiChuWei+ temp;
            }
            return tempNum;
        }
        /// <summary>
        /// 实现算术右移
        /// </summary>
        /// <param name="binaryNum">移动的二进制数据</param>
        /// <param name="yiDongWeiShu">移动的位数</param>
        /// <returns>返回移动结果</returns>
        public static string SuanShuYouYi(string binaryNum, int yiDongWeiShu)
        {
            #region 参数确认
            Debug.Assert(!string.IsNullOrEmpty(binaryNum), "数为空");
            Debug.Assert(yiDongWeiShu > 0, "移动位数至少两位");
            #endregion
            #region 老代码
            //string resultNum = "";
            //string tempNum = binaryNum;
            //char c = binaryNum[0];
            //for (int j = 0; j < yiDongWeiShu; j++)
            //{
            //    string temp = "";
            //    for (int i = 0; i < tempNum.Length - 1; i++)
            //    {
            //        temp = temp + tempNum[i];
            //    }
            //    resultNum = c + temp;
            //    tempNum = resultNum;
            //}
            //return resultNum;
            #endregion
            string tempNum = binaryNum;//记录各种中间结果
            char c = binaryNum[0];
            for (int j = 0; j < yiDongWeiShu; j++)
            {
                string temp = "";
                for (int i = 0; i < tempNum.Length - 1; i++)
                {
                    temp = temp + tempNum[i];
                }
                tempNum = c + temp;
            }
            return tempNum;
        }
    }
}
