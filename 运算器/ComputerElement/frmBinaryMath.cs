namespace ComputerElement
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using System.IO;
    using System.Text;
    /// <summary>
    /// Defines the <see cref="frmBinaryMath" />.
    /// </summary>
    public partial class frmBinaryMath : Form
    {
        /// <summary>
        /// Defines the RD_String.
        /// </summary>
        public static string RD_TEXT;
        /// <summary>
        /// Defines the RD_int.
        /// </summary>
        public static int RD_int;
        public static string RS;
        public static string RD;
        public static string Speed_string;
        public static int Speed_int;
        /// <summary>
        /// Defines the RD_Hex.
        /// </summary>
        public static string RD_Hex;
        /// <summary>
        /// Defines the RS.
        /// </summary>
        public static string RS_TEXT;
        /// <summary>
        /// Defines the RS_int.
        /// </summary>
        public static int RS_int;
        /// <summary>
        /// Defines the RS_Hex.
        /// </summary>
        public static string RS_Hex;
        /// <summary>
        /// Defines the digit.
        /// </summary>
        internal int digit = 16;//二进制位数八位或十六位
        /// <summary>
        /// Defines the numNotation.
        /// </summary>
        internal Notation numNotation = Notation.dingDianZhengShu;//数据表示方式，定点整数 定点小数 （默认定点整数）
        /// <summary>
        /// Defines the numCoding.
        /// </summary>
        internal Coding numCoding = Coding.buMa;//二进制编码方式，原码，反码，补码 （默认补码）
        /// <summary>
        /// Defines the sign.
        /// </summary>
        internal Sign sign = Sign.signed;//有无符号，有符号，无符号 （默认有符号）
        /// <summary>
        /// Defines the focused.
        /// </summary>
        internal int focused = 3;//默认操作累加寄存器
        /// <summary>
        /// Defines the Num_True1.
        /// </summary>
        internal string Num_True1;
        /// <summary>
        /// Defines the Num_True2.
        /// </summary>
        internal string Num_True2;
        /// <summary>
        /// Initializes a new instance of the <see cref="frmBinaryMath"/> class.
        /// </summary>
        public frmBinaryMath()
        {
            InitializeComponent();
            if (rdoFixed8Bit.Checked = true)
            {
                Mult.Enabled = true;
            }
            else
            {
                Mult.Enabled = false;
            }
            this.cbFuDianJieMaWeiShu.Text = "5";//浮点数默认阶码位数为5
        }
        /// <summary>
        /// The frmBinaryMath_Load.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void frmBinaryMath_Load(object sender, EventArgs e)
        {
            InitializationForForm();// 控件初始化
        }
        /// <summary>
        /// The SetDigit.
        /// </summary>
        private void SetDigit()
        {
            if (this.digit == 8)
            {
                this.txtNumOne.MaxLength = 8;
                this.txtNumTwo.MaxLength = 8;
                this.YunSuanJIeGuo_Bin.MaxLength = 8;
            }
            else
            {
                this.txtNumOne.MaxLength = 16;
                this.txtNumTwo.MaxLength = 16;
                this.YunSuanJIeGuo_Bin.MaxLength = 16;
            }
        }
        /// <summary>
        /// The InitializationForForm.
        /// </summary>
        private void InitializationForForm()
        {
            this.txtNumOne.Text = "0000000000000000";
            this.txtNumTwo.Text = "0000000000000000";
            this.YunSuanJIeGuo_Bin.Text = "00000000";
            this.txtFloatBinaryNumOne.Text = "0000000000000000";
            this.txtFloatBinaryNumTwo.Text = "0000000000000000";
            Azx.Text = "0";
            Bzx.Text = "0";
            for (int i = 2; i >= 14; i++)//添加2~14用于浮点数的阶码位数
            {
                this.cbFuDianJieMaWeiShu.Items.Add(i.ToString());
            }
            //设置定点数的位数信息
            if (this.rdoFixed8Bit.Checked)
            {
                this.digit = 8;
            }
            else
            {
                this.digit = 16;
            }
            SetDigit();//设置寄存器位数
            //设置编码形式
            if (this.rdoDingDianZhengShu.Checked)//是定点整数
            {
                this.numNotation = Notation.dingDianZhengShu;
            }
            else//是定点小数
            {
                this.numNotation = Notation.dingDianXiaoShu;
            }
        }
        /// <summary>
        /// The CheckUserInputBinary.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckUserInputBinary()
        {
            bool check = true;
            //首先检查二进制数的位数 不满足返回 满足再检查是不是二进制数
            if (!BinaryMathBLL.CheckDigit(txtNumOne.Text, digit))
            {
                MessageBox.Show("二进制位数1不是" + digit + "位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                check = false;
            }
            if (!BinaryMathBLL.CheckDigit(txtNumTwo.Text, digit))
            {
                MessageBox.Show("二进制位数2不是" + digit + "位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                check = false;
            }
            if (!check) { return false; }
            if (!BinaryMathBLL.CheckIsBinaryNum(txtNumOne.Text))
            {
                MessageBox.Show("数1不是二进制数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                check = false;
            }
            if (!BinaryMathBLL.CheckIsBinaryNum(txtNumTwo.Text))
            {
                MessageBox.Show("数2不是二进制数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                check = false;
            }
            return check;
        }
        /// <summary>
        /// The CheckUserInputDecimal.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckUserInputDecimal()
        {
            bool check = true;
            if (numNotation == Notation.dingDianZhengShu && !BinaryMathBLL.CheckZhengShu(Azx.Text))
            {
                MessageBox.Show("数1不是整数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                check = false;
            }
            else if (numNotation == Notation.dingDianXiaoShu && !BinaryMathBLL.CheckXiaoShu(Azx.Text))
            {
                MessageBox.Show("数1不是小数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                check = false;
            }
            if (numNotation == Notation.dingDianZhengShu && !BinaryMathBLL.CheckZhengShu(Bzx.Text))
            {
                MessageBox.Show("数2不是整数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                check = false;
            }
            else if (numNotation == Notation.dingDianXiaoShu && !BinaryMathBLL.CheckXiaoShu(Bzx.Text))
            {
                MessageBox.Show("数2不是小数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                check = false;
            }
            return check;
        }
        /// <summary>
        /// The CheckTxtNumOne.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckTxtNumOne()
        {
            bool checkInputNum = true;
            if (!BinaryMathBLL.CheckDigit(txtNumOne.Text, digit))
            {
                Azx.Text = "####";
                lblFixedNumOneError.Visible = true;
                checkInputNum = false;
            }
            else
            {
                lblFixedNumOneError.Visible = false;
            }
            if (!checkInputNum) { return false; }
            if (!BinaryMathBLL.CheckIsBinaryNum(txtNumOne.Text))
            {
                MessageBox.Show("不是二进制数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkInputNum = false;
            }
            return checkInputNum;
        }
        /// <summary>
        /// The CheckTxtNumTwo.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckTxtNumTwo()
        {
            bool checkInputNum = true;
            if (!BinaryMathBLL.CheckDigit(txtNumTwo.Text, digit))
            {
                Bzx.Text = "####";
                lblFixedNumTwoError.Visible = true;
                checkInputNum = false;
            }
            else
            {
                lblFixedNumTwoError.Visible = false;
            }
            if (!checkInputNum) { return false; }
            if (!BinaryMathBLL.CheckIsBinaryNum(txtNumTwo.Text))
            {
                MessageBox.Show("不是二进制数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkInputNum = false;
            }
            return checkInputNum;
        }
        /// <summary>
        /// The ZhunaHuan_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void ZhunaHuan_Click(object sender, EventArgs e)
        {
            if (!CheckUserInputDecimal())//检查用户输入数据的有效性
            {
                return;
            }
            for (int i = 1; i <= 2; i++)//数1还是数2
            {
                string num;//存放执行结果
                int check;//存放运算状态
                if (i == 1)
                {
                    check = BinaryMathBLL.DecimalToBinary(this.Azx.Text, numNotation, numCoding, digit, sign, out num);//十进制转换二进制
                }
                else
                {
                    check = BinaryMathBLL.DecimalToBinary(this.Bzx.Text, numNotation, numCoding, digit, sign, out num);//十进制转换二进制
                }
                if (check == 0)
                {
                    if (i == 1)
                    {
                        this.txtNumOne.Text = num;
                        Num_True1 = num;//存放num
                    }
                    else
                    {
                        this.txtNumTwo.Text = num;
                        Num_True2 = num;//存放num
                    }
                }
                else if (check == 6)
                {
                    MessageBox.Show("数" + i + "超出二进制表示范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
        /// <summary>
        /// The btnFixedXingShiZiZeng_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnFixedXingShiZiZeng_Click(object sender, EventArgs e)
        {
            if (!CheckTxtNumOne())
            {
                return;
            }
            string addResult;
            int check = BinaryMathBLL.ZiZengJian(this.txtNumOne.Text, 1, sign, out addResult);
            this.txtNumOne.Text = addResult;
            if (check == 7)
            {
                MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// The rdoFixed8Bit_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rdoFixed8Bit_Click(object sender, EventArgs e)
        {
            //int check = BinaryMathBLL.DigitChangeHaveCoding(this.txtNumOne.Text, digit, numNotation, numCoding, sign, 0, out num);
            //if (check == 0)
            //{
            //    this.txtNumOne.Text = num;
            //}
            //else if (check == 2)
            //{
            //    this.radioButton2.Checked = true;
            //    this.digit = 16;
            //    CheckDigit();
            //    MessageBox.Show("数据转换有损");
            //}
            if (!CheckUserInputBinary())
            {
                this.digit = 8;
                SetDigit();
                return;
            }
            this.digit = 8;
            SetDigit();
            if (txtNumOne.Text.Length == 16 && txtNumTwo.Text.Length == 16 && YunSuanJIeGuo_Bin.Text.Length == 16)
            {
                //if (numNotation == Notation.dingDianZhengShu)//定点整数
                //{
                //    num = this.txtNumOne.Text.Substring(8);
                //    this.txtNumOne.Text = num;
                //    num = this.txtNumTwo.Text.Substring(8);
                //    this.txtNumTwo.Text = num;
                //    num = this.txtNumResult.Text.Substring(8);
                //    this.txtNumResult.Text = num;
                //}
                //else//定点小数
                //{
                //    num = this.txtNumOne.Text.Substring(0, 8);
                //    this.txtNumOne.Text = num;
                //    num = this.txtNumTwo.Text.Substring(0, 8);
                //    this.txtNumTwo.Text = num;
                //    num = this.txtNumResult.Text.Substring(0, 8);
                //    this.txtNumResult.Text = num;
                //}
                this.txtNumOne.Text = BinaryMathBLL.DigitChangeHaveCoding(this.txtNumOne.Text, digit, numNotation, numCoding, sign);
                this.txtNumTwo.Text = BinaryMathBLL.DigitChangeHaveCoding(this.txtNumTwo.Text, digit, numNotation, numCoding, sign);
                this.YunSuanJIeGuo_Bin.Text = BinaryMathBLL.DigitChangeHaveCoding(this.YunSuanJIeGuo_Bin.Text, digit, numNotation, numCoding, sign);
            }
        }
        /// <summary>
        /// The rdoFixed16Bit_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rdoFixed16Bit_Click(object sender, EventArgs e)
        {
            if (!CheckUserInputBinary())
            {
                this.digit = 16;
                SetDigit();
                return;
            }
            this.digit = 16;
            SetDigit();
            if (txtNumOne.Text.Length == 8 && txtNumTwo.Text.Length == 8 && YunSuanJIeGuo_Bin.Text.Length == 8)
            {
                this.txtNumOne.Text = BinaryMathBLL.DigitChangeHaveCoding(this.txtNumOne.Text, digit, numNotation, numCoding, sign);
                this.txtNumTwo.Text = BinaryMathBLL.DigitChangeHaveCoding(this.txtNumTwo.Text, digit, numNotation, numCoding, sign);
                this.YunSuanJIeGuo_Bin.Text = BinaryMathBLL.DigitChangeHaveCoding(this.YunSuanJIeGuo_Bin.Text, digit, numNotation, numCoding, sign);
            }
        }
        /// <summary>
        /// The txtNumOne_TextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtNumOne_TextChanged(object sender, EventArgs e)
        {
            if (!CheckTxtNumOne())
            {
                return;
            }
            this.Azx.Text = BinaryMathBLL.BinaryToDecimal(this.txtNumOne.Text, numNotation, numCoding, digit, sign);
            //this.txtNumOneValue.Text = num;
            this.lblFixedNumOneError.Visible = false;
        }
        /// <summary>
        /// The txtNumTwo_TextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtNumTwo_TextChanged(object sender, EventArgs e)
        {
            if (!CheckTxtNumTwo())
            {
                return;
            }
            this.Bzx.Text = BinaryMathBLL.BinaryToDecimal(this.txtNumTwo.Text, numNotation, numCoding, digit, sign);
            //this.txtNumTwoValue.Text = num;
            this.lblFixedNumTwoError.Visible = false;
        }
        /// <summary>
        /// The txtNumResult_TextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtNumResult_TextChanged(object sender, EventArgs e)
        {
            this.YunSuanJieGuo_Int.Text = BinaryMathBLL.BinaryToDecimal(this.YunSuanJIeGuo_Bin.Text, numNotation, numCoding, digit, sign);
        }
        /// <summary>
        /// The rdoDingDianZhengShu_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rdoDingDianZhengShu_Click(object sender, EventArgs e)
        {
            this.numNotation = Notation.dingDianZhengShu;
        }
        /// <summary>
        /// The rdoDingDianXiaoShu_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rdoDingDianXiaoShu_Click(object sender, EventArgs e)
        {
            this.numNotation = Notation.dingDianXiaoShu;
        }
        /// <summary>
        /// The chkIsFuHaoWei_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void chkIsFuHaoWei_Click(object sender, EventArgs e)
        {
            if (!CheckUserInputBinary())
            {
                return;
            }
            Sign oldSign = this.sign;
            if (this.chkIsFuHaoWei.Checked)
            {
                this.sign = Sign.signed;
            }
            else
            {
                this.sign = Sign.unSigned;
            }
            for (int i = 0; i > 3; i++)
            {
                //string num;
                if (i == 0)
                {
                    this.Azx.Text = BinaryMathBLL.BinaryToDecimal(this.txtNumOne.Text, numNotation, numCoding, digit, sign);
                }
                else if (i == 1)
                {
                    this.Bzx.Text = BinaryMathBLL.BinaryToDecimal(this.txtNumTwo.Text, numNotation, numCoding, digit, sign);
                }
                else
                {
                    this.YunSuanJieGuo_Int.Text = BinaryMathBLL.BinaryToDecimal(this.YunSuanJIeGuo_Bin.Text, numNotation, numCoding, digit, sign);
                }
                //if (i == 0)
                //{
                //    this.txtNumOneValue.Text = num;
                //}
                //else if (i == 1)
                //{
                //    this.txtNumTwoValue.Text = num;
                //}
                //else
                //{
                //    this.txtNumResultValue.Text = num;
                //}
            }
        }
        /// <summary>
        /// The ChangeBianMa.
        /// </summary>
        /// <param name="coding">The coding<see cref="Coding"/>.</param>
        private void ChangeBianMa(Coding coding)
        {
            if (!CheckUserInputBinary())
            {
                return;
            }
            //bool result = false;
            Coding oldCoding = this.numCoding;
            Coding newCoding = coding;
            this.numCoding = coding;
            for (int i = 0; i > 3; i++)
            {
                string num;
                int check;
                if (i == 0)
                {
                    check = BinaryMathBLL.BinaryToBinary(this.txtNumOne.Text, oldCoding, newCoding, sign, out num);
                }
                else if (i == 1)
                {
                    check = BinaryMathBLL.BinaryToBinary(this.txtNumTwo.Text, oldCoding, newCoding, sign, out num);
                }
                else
                {
                    check = BinaryMathBLL.BinaryToBinary(this.YunSuanJIeGuo_Bin.Text, oldCoding, newCoding, sign, out num);
                }
                if (check == 0)
                {
                    if (i == 0)
                    {
                        this.txtNumOne.Text = num;
                    }
                    else if (i == 1)
                    {
                        this.txtNumTwo.Text = num;
                    }
                    else
                    {
                        this.YunSuanJIeGuo_Bin.Text = num;
                    }
                    //result = true;
                }
                else //check == 2
                {
                    MessageBox.Show("数据转换有损", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //rboBuMa.Checked = true;
                    //result = false;
                    break;
                }
            }
        }
        /// <summary>
        /// The rboYuanMa_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rboYuanMa_Click(object sender, EventArgs e)
        {
            ChangeBianMa(Coding.yuanMa);
            //this.numCoding = Coding.yuanMa;
            this.AddBT.Enabled = false;
            this.SubBT.Enabled = false;
        }
        /// <summary>
        /// The rboFanMa_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rboFanMa_Click(object sender, EventArgs e)
        {
            //string num;
            //int check = BinaryMathBLL.BinaryToBinary(this.txtNumOne.Text, this.numCoding, 2, sign, out num);
            //if (check == 0)
            //{
            //    this.txtNumOne.Text = num;
            //}
            //else if (check == 2)
            //{
            //    MessageBox.Show("数据转换有损");
            //}
            //else if (check == 8)
            //{
            //    CheckDigit();
            //    MessageBox.Show("不是二进制");
            //}
            ChangeBianMa(Coding.fanMa);
            //this.numCoding = Coding.fanMa;
            this.AddBT.Enabled = true;
            this.SubBT.Enabled = false;
        }
        /// <summary>
        /// The rboBuMa_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void rboBuMa_Click(object sender, EventArgs e)
        {
            //string num;
            //int check = BinaryMathBLL.BinaryToBinary(this.txtNumOne.Text, this.numCoding, 3, sign, out num);
            //if (check == 0 || check == 2)
            //{
            //    this.txtNumOne.Text = num;
            //}
            //else if (check == 8)
            //{
            //    CheckDigit();
            //    MessageBox.Show("不是二进制");
            //}
            ChangeBianMa(Coding.buMa);
            //this.numCoding = Coding.buMa;
            this.AddBT.Enabled = true;
            this.SubBT.Enabled = true;
        }
        /// <summary>
        /// The JiaFa_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void JiaFa_Click(object sender, EventArgs e)
        {
            Debug.Assert(numCoding != Coding.yuanMa);//?
            if (!CheckUserInputBinary())//检查用户输入的二进制数有效性
            {
                return;
            }
            string addResult;
            string addNum;
            if (chkLeiJiaQi.Checked)//如果开启累加
            {
                addNum = this.YunSuanJIeGuo_Bin.Text;
            }
            else
            {
                addNum = this.txtNumOne.Text;
            }
            if (numCoding == Coding.buMa)//补码运算
            {
                int check = BinaryMathBLL.Add(addNum, this.txtNumTwo.Text, 1, digit, sign, out addResult);
                //string addResult = BinaryMath.GetNumStr();
                if (check == 0)
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;//打印结果
                    //this.txtNumResult.Text = addResult;
                }
                else if (check == 7)//运算溢出
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;
                    MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PSW_V.Text = "1";
                    PSW_C.Text = "1";
                }
            }
            else//反码运算
            {
                int check = BinaryMathBLL.AddForFanMa(addNum, this.txtNumTwo.Text, digit, sign, out addResult);
                if (check == 0)
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;//打印结果
                }
                else
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;
                    MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            ShowBox.Text = "";
            ShowBox.Text += "\r\n";
            ShowBox.Text += Num_True1 + "----->A";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += Num_True2 + "----->B";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += "--------";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += addResult + "----->结果";
            ShowBox.Text += "\r\n";
        }
        /// <summary>
        /// The btnZuoYi_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnZuoYi_Click(object sender, EventArgs e)
        {
            if (!CheckUserInputBinary())//检查用户输入的有效性
            {
                return;
            }
            if (chkLeiJiaQi.Checked)
            {
                YunSuanJIeGuo_Bin.Text = BinaryMathBLL.ZuoYi(YunSuanJIeGuo_Bin.Text, 1);
            }
            else
            {
                if (this.focused == 1)
                {
                    txtNumOne.Text = BinaryMathBLL.ZuoYi(txtNumOne.Text, 1);
                }
                else if (this.focused == 2)
                {
                    txtNumTwo.Text = BinaryMathBLL.ZuoYi(txtNumTwo.Text, 1);
                }
                else
                {
                    YunSuanJIeGuo_Bin.Text = BinaryMathBLL.ZuoYi(YunSuanJIeGuo_Bin.Text, 1);
                }
            }
        }
        /// <summary>
        /// The btnLuoJiYouYi_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnLuoJiYouYi_Click(object sender, EventArgs e)
        {
            if (!CheckUserInputBinary())//检查用户输入的有效性
            {
                return;
            }
            if (chkLeiJiaQi.Checked)
            {
                YunSuanJIeGuo_Bin.Text = BinaryMathBLL.YouYi(YunSuanJIeGuo_Bin.Text, 1, Sign.unSigned);
            }
            else
            {
                if (this.focused == 1)
                {
                    txtNumOne.Text = BinaryMathBLL.YouYi(txtNumOne.Text, 1, Sign.unSigned);
                }
                else if (this.focused == 2)
                {
                    txtNumTwo.Text = BinaryMathBLL.YouYi(txtNumTwo.Text, 1, Sign.unSigned);
                }
                else
                {
                    YunSuanJIeGuo_Bin.Text = BinaryMathBLL.YouYi(YunSuanJIeGuo_Bin.Text, 1, Sign.unSigned);
                }
            }
        }
        /// <summary>
        /// The btnSuanShuYouYi_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnSuanShuYouYi_Click(object sender, EventArgs e)
        {
            if (!CheckUserInputBinary())//检查用户输入的有效性
            {
                return;
            }
            if (chkLeiJiaQi.Checked)
            {
                YunSuanJIeGuo_Bin.Text = BinaryMathBLL.YouYi(YunSuanJIeGuo_Bin.Text, 1, Sign.signed);
            }
            else
            {
                if (this.focused == 1)
                {
                    txtNumOne.Text = BinaryMathBLL.YouYi(txtNumOne.Text, 1, Sign.signed);
                }
                else if (this.focused == 2)
                {
                    txtNumTwo.Text = BinaryMathBLL.YouYi(txtNumTwo.Text, 1, Sign.signed);
                }
                else
                {
                    YunSuanJIeGuo_Bin.Text = BinaryMathBLL.YouYi(YunSuanJIeGuo_Bin.Text, 1, Sign.signed);
                }
            }
        }
        /// <summary>
        /// The txtNumOne_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtNumOne_Click(object sender, EventArgs e)
        {
            this.focused = 1;
        }
        /// <summary>
        /// The txtNumTwo_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtNumTwo_Click(object sender, EventArgs e)
        {
            this.focused = 2;
        }
        /// <summary>
        /// The txtNumResult_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtNumResult_Click(object sender, EventArgs e)
        {
            this.focused = 3;
        }
        /// <summary>
        /// The btnFixedLeiJiaQiClear_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnFixedLeiJiaQiClear_Click(object sender, EventArgs e)
        {
            string ling = "";
            for (int i = 0; i > digit; i++)
            {
                ling += "0";
            }
            this.YunSuanJIeGuo_Bin.Text = ling;
        }
        /// <summary>
        /// The SubBT_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void SubBT_Click(object sender, EventArgs e)
        {
            Debug.Assert(numCoding == Coding.buMa);
            if (!CheckUserInputBinary())//检查用户输入的二进制数有效性
            {
                return;
            }
            string addResult;
            string addNum;
            if (chkLeiJiaQi.Checked)
            {
                addNum = this.YunSuanJIeGuo_Bin.Text;
            }
            else
            {
                addNum = this.txtNumOne.Text;
            }
            int check = BinaryMathBLL.Add(addNum, this.txtNumTwo.Text, 2, digit, sign, out addResult);
            //string addResult = BinaryMath.GetNumStr();
            if (check == 0)
            {
                this.YunSuanJIeGuo_Bin.Text = addResult;
                //this.txtNumResult.Text = addResult;
            }
            else if (check == 7)//运算溢出
            {
                this.YunSuanJIeGuo_Bin.Text = addResult;
                MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PSW_V.Text = "1";
                PSW_C.Text = "1";
            }
            ShowBox.Text = "";
            ShowBox.Text += "\r\n";
            ShowBox.Text += Num_True1 + "----->A";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += Num_True2 + "----->B";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += "--------";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += addResult + "----->结果";
            ShowBox.Text += "\r\n";
        }
        /// <summary>
        /// The txtFloatBinaryNumOne_TextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtFloatBinaryNumOne_TextChanged(object sender, EventArgs e)
        {
            bool checkFloatNum = true;
            if (!BinaryMathBLL.CheckIsBinaryNum(txtFloatBinaryNumOne.Text))
            {
                MessageBox.Show("数1不是浮点数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkFloatNum = false;
            }
            if (!BinaryMathBLL.CheckDigit(txtFloatBinaryNumOne.Text, 16))
            {
                MessageBox.Show("数1位数错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkFloatNum = false;
            }
            if (!checkFloatNum)
            {
                return;
            }
            string tempNum = BinaryMathBLL.FuDianToShiJinZhi(this.txtFloatBinaryNumOne.Text, digit, int.Parse(cbFuDianJieMaWeiShu.Text));
            this.txtFloatNumOneValue.Text = tempNum;
            this.lblFloatNumOneError.Visible = false;
        }
        /// <summary>
        /// The txtFloatBinaryNumTwo_TextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void txtFloatBinaryNumTwo_TextChanged(object sender, EventArgs e)
        {
            bool checkFloatNum = true;
            if (!BinaryMathBLL.CheckIsBinaryNum(txtFloatBinaryNumTwo.Text))
            {
                MessageBox.Show("数2不是浮点数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkFloatNum = false;
            }
            if (!BinaryMathBLL.CheckDigit(txtFloatBinaryNumTwo.Text, 16))
            {
                MessageBox.Show("数2位数错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkFloatNum = false;
            }
            if (!checkFloatNum)
            {
                return;
            }
            string tempNum = BinaryMathBLL.FuDianToShiJinZhi(this.txtFloatBinaryNumTwo.Text, digit, int.Parse(cbFuDianJieMaWeiShu.Text));
            this.txtFloatNumTwoValue.Text = tempNum;
            this.lblFloatNumTwoError.Visible = false;
        }
        /// <summary>
        /// The btnFloatFuZhi_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnFloatFuZhi_Click(object sender, EventArgs e)
        {
            bool checkFloatNum = true;
            if (!BinaryMathBLL.CheckInputIsNumber(txtFloatNumOneValue.Text))
            {
                MessageBox.Show("数1不是浮点数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkFloatNum = false;
            }
            if (!BinaryMathBLL.CheckInputIsNumber(txtFloatNumTwoValue.Text))
            {
                MessageBox.Show("数2不是浮点数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkFloatNum = false;
            }
            if (!checkFloatNum)
            {
                return;
            }
            for (int i = 0; i > 2; i++)
            {
                string tempNum;
                int check;
                if (i == 0)
                {
                    check = BinaryMathBLL.FuDianToBinaryNum(txtFloatNumOneValue.Text, digit, int.Parse(cbFuDianJieMaWeiShu.Text), out tempNum);
                }
                else
                {
                    check = BinaryMathBLL.FuDianToBinaryNum(txtFloatNumTwoValue.Text, digit, int.Parse(cbFuDianJieMaWeiShu.Text), out tempNum);
                }
                if (check == 0)
                {
                    if (i == 0)
                    {
                        this.txtFloatBinaryNumOne.Text = tempNum;
                    }
                    else
                    {
                        this.txtFloatBinaryNumTwo.Text = tempNum;
                    }
                }
                else if (check == 6)
                {
                    MessageBox.Show("输入数据超出二进制表示范围", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// The btnFloatAdder_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void btnFloatAdder_Click(object sender, EventArgs e)
        {
            bool checkFloatNum = true;
            if (!BinaryMathBLL.CheckIsBinaryNum(txtFloatBinaryNumOne.Text))
            {
                MessageBox.Show("数1不是浮点数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkFloatNum = false;
            }
            if (!BinaryMathBLL.CheckIsBinaryNum(txtFloatBinaryNumTwo.Text))
            {
                MessageBox.Show("数2不是浮点数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkFloatNum = false;
            }
            if (!checkFloatNum)
            {
                return;
            }
            string tempNum;
            int check = BinaryMathBLL.FloatAdd(this.txtFloatBinaryNumOne.Text, this.txtFloatBinaryNumTwo.Text, int.Parse(cbFuDianJieMaWeiShu.Text), out tempNum);
            this.txtFloatBinaryNumTwo.Text = tempNum;
            if (check == 7)
            {
                MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// The CLA_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void CLA_Click(object sender, EventArgs e)
        {
            //输出结果
            Debug.Assert(numCoding != Coding.yuanMa);//?
            if (!CheckUserInputBinary())//检查用户输入的二进制数有效性
            {
                return;
            }
            string addResult;
            string addNum;
            if (chkLeiJiaQi.Checked)//如果开启累加
            {
                addNum = this.YunSuanJIeGuo_Bin.Text;
            }
            else
            {
                addNum = this.txtNumOne.Text;
            }
            if (numCoding == Coding.buMa)//补码运算
            {
                int check = BinaryMathBLL.Add(addNum, this.txtNumTwo.Text, 1, digit, sign, out addResult);
                //string addResult = BinaryMath.GetNumStr();
                if (check == 0)
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;
                    //this.txtNumResult.Text = addResult;
                }
                else if (check == 7)//运算溢出
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;
                    MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else//反码运算
            {
                int check = BinaryMathBLL.AddForFanMa(addNum, this.txtNumTwo.Text, digit, sign, out addResult);
                if (check == 0)
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;
                }
                else
                {
                    this.YunSuanJIeGuo_Bin.Text = addResult;
                    MessageBox.Show("运算结果溢出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            //输出过程
            //超前进位
            Char[] a = Num_True1.ToCharArray();
            Char[] b = Num_True2.ToCharArray();
            int[] A = new int[8];
            int[] B = new int[8];
            for (int i = 0; i < a.Length; i++)
            {
                A[i] = Convert.ToInt32(a[i].ToString());
            }
            for (int i = 0; i <b.Length; i++)
            {
                B[i] = Convert.ToInt32(b[i].ToString());
            }
            int[] C = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] G = new int[8];
            int[] P = new int[8];
            int[] S = new int[8];
            for (int i = 7; i >= 0; i--)
            {
                G[i] = A[i] & B[i];
                P[i] = A[i] | B[i];
                C[i] = G[i] | (P[i] & C[i]);
                S[i] = A[i] ^ B[i] ^ C[i + 1];
            }
            ShowBox.Text = string.Empty;//清空内容
            ShowBox.Text += "Ai:值A的第i位  Bi:值B的第i位  Gi:生成信号  Pi:传播信号  Ci:进位  Si:结果的第i位 "; Pause();
            ShowBox.Text += "\r\n\r\n\r\n\r\n";
            for (int i = 0; i <8; i++)
            {
                ShowBox.Text += "A" + (7 - i) + "=" + A[i] + '\t';
            }
            ShowBox.Text += "\r\n"; Pause();
            for (int i = 0; i < 8; i++)
            {
                ShowBox.Text += "B" + (7 - i) + "=" + B[i] + '\t';
            }
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += "____________________________________________________________";
            ShowBox.Text += "\r\n"; Pause();
            for (int i = 0; i < 8; i++)
            {
                ShowBox.Text += "G" + (7 - i) + "=" + G[i] + '\t';
            }
            ShowBox.Text += "\r\n"; Pause();
            for (int i = 0; i < 8; i++)
            {
                ShowBox.Text += "P" + (7 - i) + "=" + P[i] + '\t';
            }
            ShowBox.Text += "\r\n";
            ShowBox.Text += "____________________________________________________________";
            ShowBox.Text += "\r\n"; Pause();
            for (int i = 0; i < 8; i++)
            {
                ShowBox.Text += "C" + (8 - i) + "=" + C[i] + '\t';
            }
            ShowBox.Text += "C0=0";
            ShowBox.Text += "\r\n"; Pause();
            for (int i = 0; i < 8; i++)
            {
                ShowBox.Text += "S" + (7 - i) + "=" + S[i] + '\t';
            }
            ShowBox.Text += "\r\n";
            ShowBox.Text += "____________________________________________________________";
        }
        /// <summary>
        /// The Mult_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Mult_Click(object sender, EventArgs e)
        {
            rdoFixed8Bit.Checked = true;
            ZhunaHuan.PerformClick();
            rboYuanMa.Checked = true;
            string addResult;
            char YiChuWei = '0';
            ShowX.Text = txtNumOne.Text;
            ShowMQ.Text = txtNumTwo.Text;
            ShowACC.Text = "00000000";//置零
            Char[] MQString = ShowMQ.Text.ToCharArray();//字符串数组存放MQ中的文本
            int[] MQNum = new int[8];
            for (int i = 0; i > MQString.Length; i++)//字符串转数组
            {
                MQNum[i] = Convert.ToInt32(MQString[i].ToString());
            }
            Pause();
            for (int i = 0; i < 7; i++)
            {
                MQString = ShowMQ.Text.ToCharArray();//字符串转数组//更新MQ数组
                for (int j = 0; j < MQString.Length; j++)
                {
                    MQNum[j] = Convert.ToInt32(MQString[j].ToString());
                }
                if (MQNum[7] == 1)//当前参与乘法位为1
                {
                    //算术
                    BinaryMathBLL.Add(ShowACC.Text, ShowX.Text, 1, digit, sign, out addResult);
                    ShowACC.Text = addResult;
                    //右移
                    ShowACC.Text = BinaryMath.LuoJiYouYi_Return(ShowACC.Text, 1, out YiChuWei);//ACC右移并返回移出位
                    ShowMQ.Text = BinaryMath.LuoJiYouYi_POP(ShowMQ.Text, 1, YiChuWei);//引入ACC移出位后MQ右移
                    //暂停
                    Pause();
                }
                else//为0
                {
                    //算术
                    //右移
                    ShowACC.Text = BinaryMath.LuoJiYouYi_Return(ShowACC.Text, 1, out YiChuWei);//ACC右移并返回移出位
                    ShowMQ.Text = BinaryMath.LuoJiYouYi_POP(ShowMQ.Text, 1, YiChuWei);//引入ACC移出位后MQ右移
                    //暂停
                    Pause();
                }
            }
            ShowBox.Text = "";
            MessageBox.Show("运算结束!");
            ShowBox.Text += "运算结束!\r\n";
            ShowBox.Text += "运算结果为ACC第一位到MQ倒数第二位的数字串:\r\n";
            ShowBox.Text += ShowACC.Text + ShowMQ.Text.Substring(0, 7);
            YunSuanJIeGuo_Bin.Text = ShowACC.Text + ShowMQ.Text.Substring(0, 7);
            int NumA = int.Parse(Azx.Text);
            int NumB = int.Parse(Bzx.Text);
            YunSuanJieGuo_Int.Text = (NumA * NumB).ToString();
        }
        /// <summary>
        /// The Yv_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Yv_Click(object sender, EventArgs e)
        {
            rboYuanMa.Checked = true;
            byte a = Convert.ToByte(Num_True1, 2);
            byte b = Convert.ToByte(Num_True2, 2);
            byte c = (byte)(a & b);
            string num;
            BinaryMathBLL.DecimalToBinary(c.ToString(), numNotation, numCoding, digit, sign, out num);//十进制转换二进制
            ShowBox.Text = "";
            ShowBox.Text += Num_True1 + "----->A";
            ShowBox.Text += "\r\n";
            Pause();
            ShowBox.Text += Num_True2 + "----->B";
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += "________";
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += num + "----->结果";
            ShowBox.Text += "\r\n";
            ShowBox.Text += "\r\n";
            ShowBox.Text += "结果:\r\n";
            ShowBox.Text += "整形：" + (c.ToString()) + "\r\n" + "原码：";
            YunSuanJieGuo_Int.Text = (c.ToString());//运算结果（整形）
            ShowBox.Text += num;
            YunSuanJIeGuo_Bin.Text = num;//运算结果（二进制）
        }
        /// <summary>
        /// The Pause.
        /// </summary>
        int times = 30;
        public void Pause()
        {
            int Pause_Interval = times;
            //int times = 30;
            while (Pause_Interval > 0)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                Pause_Interval--;
            }
        }
        /// <summary>
        /// The Huo_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Huo_Click(object sender, EventArgs e)
        {
            rboYuanMa.Checked = true;
            byte a = Convert.ToByte(Num_True1, 2);
            byte b = Convert.ToByte(Num_True2, 2);
            byte c = (byte)(a | b);
            string num;
            BinaryMathBLL.DecimalToBinary(c.ToString(), numNotation, numCoding, digit, sign, out num);//十进制转换二进制
            ShowBox.Text = "";
            ShowBox.Text += Num_True1 + "----->A";
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += Num_True2 + "----->B";
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += "________";
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += num + "----->结果";
            ShowBox.Text += "\r\n";
            ShowBox.Text += "\r\n";
            ShowBox.Text += "结果:\r\n";
            ShowBox.Text += "整形：" + (c.ToString()) + "\r\n" + "原码：";
            YunSuanJieGuo_Int.Text = (c.ToString());//运算结果（整形）
            ShowBox.Text += num;
            YunSuanJIeGuo_Bin.Text = num;//运算结果（二进制）
        }
        /// <summary>
        /// The Fei_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void Fei_Click(object sender, EventArgs e)
        {
            rboYuanMa.Checked = true;
            ShowBox.Text = "";
            byte a = Convert.ToByte(Num_True1, 2);
            byte b = Convert.ToByte(Num_True2, 2);
            byte Fa = (byte)(~a);
            byte Fb = (byte)(~b);
            string sfa;
            string sfb;
            ShowBox.Text = "";
            ShowBox.Text += "按位取反中...";
            ShowBox.Text += "\r\n";
            Pause();
            sfa = Convert.ToString(Fa, 2);
            ShowBox.Text += "A: " + Num_True1 + "---->" + " ~A:" + sfa;
            ShowBox.Text += "   ~A整形：" + (Fa.ToString()) + "\r\n";
            ShowBox.Text += "\r\n";
            sfb = Convert.ToString(Fb, 2);
            ShowBox.Text += "B: " + Num_True2 + "---->" + " ~B:" + sfb;
            ShowBox.Text += "   ~B整形：" + (Fb.ToString()) + "\r\n";
            ShowBox.Text += "结束";
        }
        /// <summary>
        /// The QiuFan_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void QiuFan_Click(object sender, EventArgs e)
        {
            ShowBox.Text = "";
            rboFanMa.Checked = true;
            ShowBox.Text += "求反中..."; Pause();
            ShowBox.Text += "\r\n";
            ShowBox.Text += "A反码:" + txtNumOne.Text;
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += "B反码:" + txtNumTwo.Text;
            ShowBox.Text += "\r\n";
            ShowBox.Text += "结束";
        }
        /// <summary>
        /// The button2_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            ShowBox.Text = "";
            rboBuMa.Checked = true;
            ShowBox.Text += "求补中..."; Pause();
            ShowBox.Text += "\r\n";
            ShowBox.Text += "A补码:" + txtNumOne.Text;
            ShowBox.Text += "\r\n"; Pause();
            ShowBox.Text += "B补码:" + txtNumTwo.Text;
            ShowBox.Text += "\r\n";
            ShowBox.Text += "结束";
        }
        /// <summary>
        /// The BuMaJia_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void BuMaJia_Click(object sender, EventArgs e)
        {
            rboBuMa.Checked = true;
            string resultNum;
            BinaryMathBLL.Add(txtNumOne.Text, this.txtNumTwo.Text, 1, digit, sign, out resultNum);
            ShowBox.Text = "";
            ShowBox.Text += txtNumOne.Text + "----->A补" + "\r\n"; Pause();
            ShowBox.Text += txtNumTwo.Text + "----->B补" + "\r\n";
            ShowBox.Text += "--------\r\n"; Pause();
            ShowBox.Text += resultNum + "----->结果";
        }
        private void TongBu()
        {
            PCBOX.Text = PCBOX1.Text;
            MARBOX.Text = MARBOX1.Text;
            BUSBOX.Text = BUSBOX1.Text;
            MDRBOX.Text = MDRBOX1.Text;
            SRBOX.Text = SRBOX1.Text;
            IMARBOX.Text = IMARBOX1.Text;
            DRBOX.Text = DRBOX1.Text;
            IMDRBOX.Text = IMDRBOX1.Text;
        }
        private void QuanBai()
        {
            for(int k=0;k<15;k++)
            {
                KongZhiCunChu.Items[k].BackColor = Color.FromArgb(255, 255, 255);//恢复白色
            }
        }
        private void QuanHui()
        {
            BUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            LABOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            LTBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            IMDRBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            IMARBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            SRBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            DRBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            MDRBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            IRBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
            MARBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
        }
        /// <summary>
        /// The DanBuZhiXing_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void ZiLvZhiXing_Click(object sender, EventArgs e)
        {
            BinaryBox.Text = "";
            rdoFixed16Bit.Checked = true;
            rboBuMa.Checked = true;
            chkIsFuHaoWei.Checked = true;
            //读取多行
            string[] AsmArry = new string[AsmBox.Lines.Length];
            //输出指令
            for (int i = 0; i < AsmBox.Lines.Length; i++)
            {
                AsmArry[i] = AsmBox.Lines[i];
                AsmArry[i] = AsmArry[i].ToUpper();
                //识别指令
                string[] SplitStr = AsmArry[i].Split(new char[3] { ' ', ',', '，' });//分割内容
                switch (SplitStr[0])//SplitStr[0]助记符  SplitStr[1]RD/X SplitStr[2]RS/N/Z/K/X
                {
                    case "ADD":
                        BinaryBox.Text += "0001 ";
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 ";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0100 ";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 1000 ";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 1100 ";
                                break;
                            case "R4":
                                BinaryBox.Text += "0001 0000 ";
                                break;
                            case "R5":
                                BinaryBox.Text += "0001 0100 ";
                                break;
                            case "R6":
                                BinaryBox.Text += "0001 1000 ";
                                break;
                            case "R7":
                                BinaryBox.Text += "0001 1100 ";
                                break;
                        }
                        switch (SplitStr[2])//判断源寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000";
                                break;
                            case "R1":
                                BinaryBox.Text += "0001";
                                break;
                            case "R2":
                                BinaryBox.Text += "0010";
                                break;
                            case "R3":
                                BinaryBox.Text += "0011";
                                break;
                            case "R4":
                                BinaryBox.Text += "0100";
                                break;
                            case "R5":
                                BinaryBox.Text += "0101";
                                break;
                            case "R6":
                                BinaryBox.Text += "0110";
                                break;
                            case "R7":
                                BinaryBox.Text += "0111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "SUB"://减指令
                        BinaryBox.Text += "0010 ";
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 ";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0100 ";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 1000 ";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 1100 ";
                                break;
                            case "R4":
                                BinaryBox.Text += "0001 0000 ";
                                break;
                            case "R5":
                                BinaryBox.Text += "0001 0100 ";
                                break;
                            case "R6":
                                BinaryBox.Text += "0001 1000 ";
                                break;
                            case "R7":
                                BinaryBox.Text += "0001 1100 ";
                                break;
                        }
                        switch (SplitStr[2])//判断源寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000";
                                break;
                            case "R1":
                                BinaryBox.Text += "0001";
                                break;
                            case "R2":
                                BinaryBox.Text += "0010";
                                break;
                            case "R3":
                                BinaryBox.Text += "0011";
                                break;
                            case "R4":
                                BinaryBox.Text += "0100";
                                break;
                            case "R5":
                                BinaryBox.Text += "0101";
                                break;
                            case "R6":
                                BinaryBox.Text += "0110";
                                break;
                            case "R7":
                                BinaryBox.Text += "0111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "MUL":
                        BinaryBox.Text += "0011 ";
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 ";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0100 ";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 1000 ";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 1100 ";
                                break;
                            case "R4":
                                BinaryBox.Text += "0001 0000 ";
                                break;
                            case "R5":
                                BinaryBox.Text += "0001 0100 ";
                                break;
                            case "R6":
                                BinaryBox.Text += "0001 1000 ";
                                break;
                            case "R7":
                                BinaryBox.Text += "0001 1100 ";
                                break;
                        }
                        switch (SplitStr[2])//判断源寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000";
                                break;
                            case "R1":
                                BinaryBox.Text += "0001";
                                break;
                            case "R2":
                                BinaryBox.Text += "0010";
                                break;
                            case "R3":
                                BinaryBox.Text += "0011";
                                break;
                            case "R4":
                                BinaryBox.Text += "0100";
                                break;
                            case "R5":
                                BinaryBox.Text += "0101";
                                break;
                            case "R6":
                                BinaryBox.Text += "0110";
                                break;
                            case "R7":
                                BinaryBox.Text += "0111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "JMP":
                        BinaryBox.Text += "0111 0000 0000 0";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "000";
                                break;
                            case "R1":
                                BinaryBox.Text += "001";
                                break;
                            case "R2":
                                BinaryBox.Text += "010";
                                break;
                            case "R3":
                                BinaryBox.Text += "011";
                                break;
                            case "R4":
                                BinaryBox.Text += "100";
                                break;
                            case "R5":
                                BinaryBox.Text += "101";
                                break;
                            case "R6":
                                BinaryBox.Text += "110";
                                break;
                            case "R7":
                                BinaryBox.Text += "111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "JC":
                        BinaryBox.Text += "1000 ";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 ";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0100 ";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 1000 ";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 1100 ";
                                break;
                            case "R4":
                                BinaryBox.Text += "0001 0000 ";
                                break;
                            case "R5":
                                BinaryBox.Text += "0001 0100 ";
                                break;
                            case "R6":
                                BinaryBox.Text += "0001 1000 ";
                                break;
                            case "R7":
                                BinaryBox.Text += "0001 1100 ";
                                break;
                        }
                        switch (SplitStr[2])//N/Z   N:0001 Z:0010
                        {
                            case "N":
                                BinaryBox.Text += "0001";
                                break;
                            case "Z":
                                BinaryBox.Text += "0010";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "MOV":
                        BinaryBox.Text += "1001 ";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 ";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0100 ";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 1000 ";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 1100 ";
                                break;
                            case "R4":
                                BinaryBox.Text += "0001 0000 ";
                                break;
                            case "R5":
                                BinaryBox.Text += "0001 0100 ";
                                break;
                            case "R6":
                                BinaryBox.Text += "0001 1000 ";
                                break;
                            case "R7":
                                BinaryBox.Text += "0001 1100 ";
                                break;
                        }
                        switch (SplitStr[2])//RS源寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000";
                                break;
                            case "R1":
                                BinaryBox.Text += "0001";
                                break;
                            case "R2":
                                BinaryBox.Text += "0010";
                                break;
                            case "R3":
                                BinaryBox.Text += "0011";
                                break;
                            case "R4":
                                BinaryBox.Text += "0100";
                                break;
                            case "R5":
                                BinaryBox.Text += "0101";
                                break;
                            case "R6":
                                BinaryBox.Text += "0110";
                                break;
                            case "R7":
                                BinaryBox.Text += "0111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "LDI":
                        BinaryBox.Text += "1010 ";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "0000 ";
                                break;
                            case "R1":
                                BinaryBox.Text += "0001 ";
                                break;
                            case "R2":
                                BinaryBox.Text += "0010 ";
                                break;
                            case "R3":
                                BinaryBox.Text += "0011 ";
                                break;
                            case "R4":
                                BinaryBox.Text += "0100 ";
                                break;
                            case "R5":
                                BinaryBox.Text += "0101 ";
                                break;
                            case "R6":
                                BinaryBox.Text += "0110 ";
                                break;
                            case "R7":
                                BinaryBox.Text += "0111 ";
                                break;
                        }
                        //立即数转int
                        int K_int;
                        K_int = System.Int32.Parse(SplitStr[2], System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                        //int转二进制
                        string K_bin = Convert.ToString(K_int, 2).PadLeft(8, '0');//8位，不足补0
                        //字符串插入' '
                        K_bin = K_bin.Insert(4, " ");
                        BinaryBox.Text += K_bin;
                        BinaryBox.Text += "\r\n";
                        break;
                    case "LD":
                        BinaryBox.Text += "1011 ";
                        switch (SplitStr[1])//目的寄存器   R7内容存入RD
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 0000";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0000 0001";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 0000 0010";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 0000 0011";
                                break;
                            case "R4":
                                BinaryBox.Text += "0000 0000 0100";
                                break;
                            case "R5":
                                BinaryBox.Text += "0000 0000 0101";
                                break;
                            case "R6":
                                BinaryBox.Text += "0000 0000 0110";
                                break;
                            case "R7":
                                BinaryBox.Text += "0000 0000 0111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "ST":
                        BinaryBox.Text += "1100 ";
                        switch (SplitStr[2])//源寄存器   RS存R7
                        {
                            case "R0":
                                BinaryBox.Text += "0000 0000 0000";
                                break;
                            case "R1":
                                BinaryBox.Text += "0000 0100 0000";
                                break;
                            case "R2":
                                BinaryBox.Text += "0000 1000 0000";
                                break;
                            case "R3":
                                BinaryBox.Text += "0000 1100 0000";
                                break;
                            case "R4":
                                BinaryBox.Text += "0001 0000 0000";
                                break;
                            case "R5":
                                BinaryBox.Text += "0001 0100 0000";
                                break;
                            case "R6":
                                BinaryBox.Text += "0001 1000 0000";
                                break;
                            case "R7":
                                BinaryBox.Text += "0001 1100 0000";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "INC":
                        BinaryBox.Text += "0100 0000 0000 0";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "000";
                                break;
                            case "R1":
                                BinaryBox.Text += "001";
                                break;
                            case "R2":
                                BinaryBox.Text += "010";
                                break;
                            case "R3":
                                BinaryBox.Text += "011";
                                break;
                            case "R4":
                                BinaryBox.Text += "100";
                                break;
                            case "R5":
                                BinaryBox.Text += "101";
                                break;
                            case "R6":
                                BinaryBox.Text += "110";
                                break;
                            case "R7":
                                BinaryBox.Text += "111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "DEC":
                        BinaryBox.Text += "0101 0000 0000 0";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "000";
                                break;
                            case "R1":
                                BinaryBox.Text += "001";
                                break;
                            case "R2":
                                BinaryBox.Text += "010";
                                break;
                            case "R3":
                                BinaryBox.Text += "011";
                                break;
                            case "R4":
                                BinaryBox.Text += "100";
                                break;
                            case "R5":
                                BinaryBox.Text += "101";
                                break;
                            case "R6":
                                BinaryBox.Text += "110";
                                break;
                            case "R7":
                                BinaryBox.Text += "111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "NEG":
                        BinaryBox.Text += "0110 0000 0000 0";
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                BinaryBox.Text += "000";
                                break;
                            case "R1":
                                BinaryBox.Text += "001";
                                break;
                            case "R2":
                                BinaryBox.Text += "010";
                                break;
                            case "R3":
                                BinaryBox.Text += "011";
                                break;
                            case "R4":
                                BinaryBox.Text += "100";
                                break;
                            case "R5":
                                BinaryBox.Text += "101";
                                break;
                            case "R6":
                                BinaryBox.Text += "110";
                                break;
                            case "R7":
                                BinaryBox.Text += "111";
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "NOP":
                        BinaryBox.Text += "0000 0000 0000 0000";
                        BinaryBox.Text += "\r\n";
                        break;
                }
            }
            //IR填入第一条指令
            string IR_Bin = BinaryBox.Lines[0];
            IR_Bin=IR_Bin.Replace(" ", "");
            int IR_int = Convert.ToInt32(IR_Bin, 2);
            string IR_Hex = IR_int.ToString("X4");
            IRBOX1.Text = IR_Hex;
            //控存打印微指令
            {
                string[] KongCunWeiZhiLin = new string[]
             {
                "000:0001 011 01 000 0000 01 1 0 1 000 001",
                "001:0011 001 00 000 0000 00 0 1 0 000 002",
                "002:0010 010 00 000 0000 00 0 0 0 000 125",
                "125:0010 000 00 010 0000 00 0 0 0 000 202",
                "202:0101 000 00 001 0000 00 0 0 0 000 270",
                "270:0010 011 00 000 0000 00 0 0 0 100 273",
                "273:0011 101 00 000 0000 00 0 0 0 000 000",
                "274:0011 101 00 000 0001 00 0 0 0 000 000",
                "275:0011 101 00 000 0011 00 0 0 0 000 000",
                "276:0101 001 00 000 0000 00 0 0 0 000 000",
                "277:0110 101 00 000 0000 00 0 0 0 000 000",
                "278:0110 000 10 000 0000 10 0 0 0 000 000",
                "279:0000 000 00 000 0011 00 0 0 0 000 000",
                "280:0000 000 00 000 0100 00 0 0 0 000 000",
                "281:0000 000 00 000 0101 00 0 0 0 000 000"
            };
                for (int j = 0; j < 15; j++)   //添加
                {
                    ListViewItem Lv_KongZhiCunChu = new ListViewItem();
                    Lv_KongZhiCunChu.Text = KongCunWeiZhiLin[j];
                    this.KongZhiCunChu.Items.Add(Lv_KongZhiCunChu);
                }
                //指令存储单元打印二进制指令
                for (int i = 0; i < AsmBox.Lines.Length; i++)
                {
                    AsmArry[i] = BinaryBox.Lines[i];//存二进制指令
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = (4096 + i).ToString("X4");
                    lvi.SubItems.Add(AsmArry[i]);
                    this.ZhiLinCunChuList.Items.Add(lvi);
                }
                //主存单元打印初始值
                for (int i = 0; i < AsmBox.Lines.Length; i++)
                {
                    AsmArry[i] = BinaryBox.Lines[i];//存二进制指令
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = (32 + i).ToString("X4");
                    lvi.SubItems.Add("00");
                    this.ZhuCunBox.Items.Add(lvi);
                }
            }
            //解析指令
            for (int i = 0; i < AsmBox.Lines.Length; i++)
            {
                AsmArry[i] = AsmBox.Lines[i];
                AsmArry[i] = AsmArry[i].ToUpper();
                //识别指令
                string[] SplitStr = AsmArry[i].Split(new char[3] { ' ', ',', '，' });//分割内容
                switch (SplitStr[0])//SplitStr[0]助记符  SplitStr[1]RD/X SplitStr[2]RS/N/Z/K/X
                {
                    case "ADD":
                        //判断目的寄存器
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                RD_TEXT = R0BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R1":
                                RD_TEXT = R1BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R2":
                                RD_TEXT = R2BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R3":
                                RD_TEXT = R3BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R4":
                                RD_TEXT = R4BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R5":
                                RD_TEXT = R5BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R6":
                                RD_TEXT = R6BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R7":
                                RD_TEXT = R7BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                        }
                        //判断源寄存器
                        switch (SplitStr[2])//判断源寄存器
                        {
                            case "R0":
                                RS_TEXT = R0BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R1":
                                RS_TEXT = R1BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R2":
                                RS_TEXT = R2BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R3":
                                RS_TEXT = R3BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R4":
                                RS_TEXT = R4BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R5":
                                RS_TEXT = R5BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R6":
                                RS_TEXT = R6BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R7":
                                RS_TEXT = R7BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        //运算部分
                        ZhunaHuan_Click(sender, e);//调用转换事件
                        JiaFa_Click(sender, e);//调用加法按钮
                        string RD_String_dec = YunSuanJieGuo_Int.Text;//此时RD为十进制字符串
                        RD_int = Convert.ToInt32(RD_String_dec);
                        if (RD_int == 0)
                        {
                            PSW_Z.Text = "1";
                        }
                        else if (RD_int < 0)
                        {
                            PSW_N.Text = "1";
                        }
                        //运算结束
                        //打印微命令序列 + 流程图可视化
                        //TF
                        string[] TFXvlie = new string[]
                        {   
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    QuanHui();
                                    BUSBOX1.BackColor= Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int=PC_int+2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor= Color.SkyBlue;
                                    IRBOX1.BackColor= Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i+1];
                                    if(IR_Bin!="")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ST
                        string[] STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化
                            BUSBOX1.Text = RS_TEXT;
                            SRBOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();//恢复白色 
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                        }
                        Pause();
                        //DT
                        string[] DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化
                            BUSBOX1.Text = RD_TEXT;
                            LABOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                        }
                        Pause();
                        //ET
                        string[] ETXvlie = new string[] { "SR->BUS,ADD,ALU->LT", "LT->BUS,BUS->Rd" };
                        for (int j = 0; j < 2; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视
                                    QuanHui();
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    BUSBOX1.Text = SRBOX1.Text;
                                    LTBOX1.Text = RD_Hex;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[5].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    QuanHui();
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = LTBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[6].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        QuanHui();
                        switch (SplitStr[1])//判断目的寄存器  存入数据
                        {
                            case "R0":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = Convert.ToString(RD_int, 16);
                                R0BOX.Text = RD_Hex;
                                break;
                            case "R1":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R1BOX.Text = RD_Hex;
                                break;
                            case "R2":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R2BOX.Text = RD_Hex;
                                break;
                            case "R3":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R3BOX.Text = RD_Hex;
                                break;
                            case "R4":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R4BOX.Text = RD_Hex;
                                break;
                            case "R5":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R5BOX.Text = RD_Hex;
                                break;
                            case "R6":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R6BOX.Text = RD_Hex;
                                break;
                            case "R7":
                                //RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R7BOX.Text = RD_Hex;
                                break;
                        }
                        break;
                    case "SUB"://减指令
                        //RD
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                RD_TEXT = R0BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R1":
                                RD_TEXT = R1BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R2":
                                RD_TEXT = R2BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R3":
                                RD_TEXT = R3BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R4":
                                RD_TEXT = R4BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R5":
                                RD_TEXT = R5BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R6":
                                RD_TEXT = R6BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R7":
                                RD_TEXT = R7BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                        }
                        //RS
                        switch (SplitStr[2])//判断源寄存器
                        {
                            case "R0":
                                RS_TEXT = R0BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R1":
                                RS_TEXT = R1BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R2":
                                RS_TEXT = R2BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R3":
                                RS_TEXT = R3BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R4":
                                RS_TEXT = R4BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R5":
                                RS_TEXT = R5BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R6":
                                RS_TEXT = R6BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R7":
                                RS_TEXT = R7BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        //运算部分
                        ZhunaHuan_Click(sender, e);//调用转换事件
                        SubBT_Click(sender, e);//调用减法按钮
                        RD_TEXT = YunSuanJieGuo_Int.Text;//此时RD为十进制字符串
                        //打印微命令序列 + 流程图可视化
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IMARBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int=PC_int+2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ST
                        STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化
                            BUSBOX1.Text = RS_TEXT;
                            SRBOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                        }
                        Pause();
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            QuanHui();
                            //寄存器可视化
                            BUSBOX1.Text = RD_TEXT;
                            LABOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                        }
                        Pause();
                        //ET
                        ETXvlie = new string[] { "SR->BUS,SUB,ALU->LT", "LT->BUS,BUS->Rd" };
                        for (int j = 0; j < 2; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    //寄存器可视化  注意同步
                                    BUSBOX1.Text = SRBOX1.Text;
                                    LTBOX1.Text = RD_Hex;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[5].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    LTBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = LTBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[6].BackColor = Color.SkyBlue;
                                    break;
                            }
                        }
                        Pause();
                        QuanHui();
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R0BOX.Text = RD_Hex;
                                break;
                            case "R1":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R1BOX.Text = RD_Hex;
                                break;
                            case "R2":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R2BOX.Text = RD_Hex;
                                break;
                            case "R3":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R3BOX.Text = RD_Hex;
                                break;
                            case "R4":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R4BOX.Text = RD_Hex;
                                break;
                            case "R5":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R5BOX.Text = RD_Hex;
                                break;
                            case "R6":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R6BOX.Text = RD_Hex;
                                break;
                            case "R7":
                                RD_int = Convert.ToInt32(RD_TEXT);
                                RD_Hex = RD_int.ToString("X4");
                                R7BOX.Text = RD_Hex;
                                break;
                        }
                        if (RD_int == 0)
                        {
                            PSW_Z.Text = "1";
                        }
                        else if (RD_int < 0)
                        {
                            PSW_N.Text = "1";
                        }
                        break;
                    case "MUL":
                        BinaryBox.Text += "\r\n";
                        //RD
                        switch (SplitStr[1])//判断目的寄存器
                        {
                            case "R0":
                                RD_TEXT = R0BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R1":
                                RD_TEXT = R1BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R2":
                                RD_TEXT = R2BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R3":
                                RD_TEXT = R3BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R4":
                                RD_TEXT = R4BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R5":
                                RD_TEXT = R5BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R6":
                                RD_TEXT = R6BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R7":
                                RD_TEXT = R7BOX.Text;
                                RD_int = System.Int32.Parse(RD_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Azx.Text = RD_int.ToString();//十进制转字符串，填入运算器
                                break;
                        }
                        //RS
                        switch (SplitStr[2])//判断源寄存器
                        {
                            case "R0":
                                RS_TEXT = R0BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R1":
                                RS_TEXT = R1BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R2":
                                RS_TEXT = R2BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R3":
                                RS_TEXT = R3BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R4":
                                RS_TEXT = R4BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R5":
                                RS_TEXT = R5BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R6":
                                RS_TEXT = R6BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                            case "R7":
                                RS_TEXT = R7BOX.Text;
                                RS_int = System.Int32.Parse(RS_TEXT, System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                                Bzx.Text = RS_int.ToString();//十进制转字符串，填入运算器
                                break;
                        }
                        //运算部分
                        RD_int = RD_int * RS_int;
                        if (RD_int > 65535)//判断溢出
                        {
                            PSW_C.Text = "1";
                        }
                        else if (RD_int == 0)//判断0标志位
                        {
                            PSW_Z.Text = "1";
                        }
                        //打印微命令序列 + 流程图可视化
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IMARBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int=PC_int+2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    LABOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    LTBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                            KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                        }
                        //ST
                        STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化
                            BUSBOX1.Text = RS_TEXT;
                            SRBOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                        }
                        Pause();
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化
                            BUSBOX1.Text = RD_TEXT;
                            LABOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                        }
                        Pause();
                        //ET
                        ETXvlie = new string[] { "SR->BUS,MUL,ALU->LT", "LT->BUS,BUS->Rd" };
                        for (int j = 0; j < 2; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    //寄存器可视化  注意同步
                                    BUSBOX1.Text = SRBOX1.Text;
                                    LTBOX1.Text = RD_Hex;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[5].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    LTBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = LTBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[6].BackColor = Color.SkyBlue;
                                    break;
                            }
                        }
                        Pause();
                        QuanHui();
                        switch (SplitStr[1])//结果转16进制，判断目的寄存器,结果存回目的寄存器
                        {
                            case "R0":
                                RD_Hex = Convert.ToString(RD_int, 16);
                                R0BOX.Text = RD_Hex;
                                break;
                            case "R1":
                                RD_Hex = RD_int.ToString("X4");
                                R1BOX.Text = RD_Hex;
                                break;
                            case "R2":
                                RD_Hex = RD_int.ToString("X4");
                                R2BOX.Text = RD_Hex;
                                break;
                            case "R3":
                                RD_Hex = RD_int.ToString("X4");
                                R3BOX.Text = RD_Hex;
                                break;
                            case "R4":
                                RD_Hex = RD_int.ToString("X4");
                                R4BOX.Text = RD_Hex;
                                break;
                            case "R5":
                                RD_Hex = RD_int.ToString("X4");
                                R5BOX.Text = RD_Hex;
                                break;
                            case "R6":
                                RD_Hex = RD_int.ToString("X4");
                                R6BOX.Text = RD_Hex;
                                break;
                            case "R7":
                                RD_Hex = RD_int.ToString("X4");
                                R7BOX.Text = RD_Hex;
                                break;
                        }
                        break;
                    case "JMP":
                        BinaryBox.Text += "\r\n";
                        switch (SplitStr[1])//判断目的寄存器,将目的寄存器值存入PC
                        {
                            case "R0":
                                PCBOX1.Text = R0BOX.Text;
                                break;
                            case "R1":
                                PCBOX1.Text = R1BOX.Text;
                                break;
                            case "R2":
                                PCBOX1.Text = R2BOX.Text;
                                break;
                            case "R3":
                                PCBOX1.Text = R3BOX.Text;
                                break;
                            case "R4":
                                PCBOX1.Text = R4BOX.Text;
                                break;
                            case "R5":
                                PCBOX1.Text = R5BOX.Text;
                                break;
                            case "R6":
                                PCBOX1.Text = R6BOX.Text;
                                break;
                            case "R7":
                                PCBOX1.Text = R7BOX.Text;
                                break;
                        }
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IMARBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    LABOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    LTBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ET
                        ETXvlie = new string[] { "Rd->BUS,BUS->PC" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            BUSBOX1.BackColor = Color.SkyBlue;
                            PCBOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化  注意同步
                            BUSBOX1.Text = PCBOX1.Text;
                            PCBOX.Text = PCBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[9].BackColor = Color.SkyBlue;
                        }
                        break;
                    case "JC":
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IMARBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    LABOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    LTBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ET
                        ETXvlie = new string[] { "Rd->BUS,BUS->PC" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            BUSBOX1.BackColor = Color.SkyBlue;
                            PCBOX1.BackColor = Color.SkyBlue;
                            //寄存器可视化  注意同步
                            BUSBOX1.Text = PCBOX1.Text;
                            PCBOX.Text = PCBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[9].BackColor = Color.SkyBlue;
                        }
                        switch (SplitStr[2])//N/Z   N:0001 Z:0010
                        {
                            case "N":
                                if (PSW_N.Text == "1")
                                {
                                    switch (SplitStr[1])//判断目的寄存器,将目的寄存器值存入PC
                                    {
                                        case "R0":
                                            PCBOX.Text = R0BOX.Text;
                                            break;
                                        case "R1":
                                            PCBOX.Text = R1BOX.Text;
                                            break;
                                        case "R2":
                                            PCBOX.Text = R2BOX.Text;
                                            break;
                                        case "R3":
                                            PCBOX.Text = R3BOX.Text;
                                            break;
                                        case "R4":
                                            PCBOX.Text = R4BOX.Text;
                                            break;
                                        case "R5":
                                            PCBOX.Text = R5BOX.Text;
                                            break;
                                        case "R6":
                                            PCBOX.Text = R6BOX.Text;
                                            break;
                                        case "R7":
                                            PCBOX.Text = R7BOX.Text;
                                            break;
                                    }
                                }
                                break;
                            case "Z":
                                if (PSW_Z.Text == "1")
                                {
                                    switch (SplitStr[1])//判断目的寄存器,将目的寄存器值存入PC
                                    {
                                        case "R0":
                                            PCBOX.Text = R0BOX.Text;
                                            break;
                                        case "R1":
                                            PCBOX.Text = R1BOX.Text;
                                            break;
                                        case "R2":
                                            PCBOX.Text = R2BOX.Text;
                                            break;
                                        case "R3":
                                            PCBOX.Text = R3BOX.Text;
                                            break;
                                        case "R4":
                                            PCBOX.Text = R4BOX.Text;
                                            break;
                                        case "R5":
                                            PCBOX.Text = R5BOX.Text;
                                            break;
                                        case "R6":
                                            PCBOX.Text = R6BOX.Text;
                                            break;
                                        case "R7":
                                            PCBOX.Text = R7BOX.Text;
                                            break;
                                    }
                                }
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "MOV":
                        switch (SplitStr[2])//RS源寄存器
                        {
                            case "R0":
                                RS = R0BOX.Text;
                                switch (SplitStr[1])//判断RD寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R0BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R0BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R0BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R0BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R0BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R0BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R0BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R0BOX.Text;
                                        break;
                                }
                                break;
                            case "R1":
                                RS = R1BOX.Text;
                                switch (SplitStr[1])//判断RS寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R1BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R1BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R1BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R1BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R1BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R1BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R1BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R1BOX.Text;
                                        break;
                                }
                                break;
                            case "R2":
                                RS = R2BOX.Text;
                                switch (SplitStr[1])//判断RS寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R2BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R2BOX.Text;
                                        R1BOX.Text = R2BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R2BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R2BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R2BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R2BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R2BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R2BOX.Text;
                                        break;
                                }
                                break;
                            case "R3":
                                RS = R3BOX.Text;
                                switch (SplitStr[1])//判断RD寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R3BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R3BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R3BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R3BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R3BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R3BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R3BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R3BOX.Text;
                                        break;
                                }
                                break;
                            case "R4":
                                RS = R4BOX.Text;
                                switch (SplitStr[1])//判断RD寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R4BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R4BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R4BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R4BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R4BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R4BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R4BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R4BOX.Text;
                                        break;
                                }
                                break;
                            case "R5":
                                RS = R5BOX.Text;
                                switch (SplitStr[1])//判断RD寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R5BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R5BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R5BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R5BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R5BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R5BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R5BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R0BOX.Text;
                                        break;
                                }
                                break;
                            case "R6":
                                RS = R6BOX.Text;
                                switch (SplitStr[1])//判断RD寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R6BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R6BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R6BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R6BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R6BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R6BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R6BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R6BOX.Text;
                                        break;
                                }
                                break;
                            case "R7":
                                RS = R7BOX.Text;
                                switch (SplitStr[1])//判断RD寄存器,将RS->RD
                                {
                                    case "R0":
                                        RD = R0BOX.Text;
                                        R0BOX.Text = R7BOX.Text;
                                        break;
                                    case "R1":
                                        RD = R1BOX.Text;
                                        R1BOX.Text = R7BOX.Text;
                                        break;
                                    case "R2":
                                        RD = R2BOX.Text;
                                        R2BOX.Text = R7BOX.Text;
                                        break;
                                    case "R3":
                                        RD = R3BOX.Text;
                                        R3BOX.Text = R7BOX.Text;
                                        break;
                                    case "R4":
                                        RD = R4BOX.Text;
                                        R4BOX.Text = R7BOX.Text;
                                        break;
                                    case "R5":
                                        RD = R5BOX.Text;
                                        R5BOX.Text = R7BOX.Text;
                                        break;
                                    case "R6":
                                        RD = R6BOX.Text;
                                        R6BOX.Text = R7BOX.Text;
                                        break;
                                    case "R7":
                                        RD = R7BOX.Text;
                                        R7BOX.Text = R7BOX.Text;
                                        break;
                                }
                                break;
                        }
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int=PC_int+2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ST
                        STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RS;
                            SRBOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                        }
                        //ET
                        ETXvlie = new string[] { "SR->BUS,BUS->Rd" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = SRBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[10].BackColor = Color.SkyBlue;
                        }
                        BinaryBox.Text += "\r\n";
                        break;
                    case "LDI":
                        //立即数转int
                        RS = SplitStr[2];
                        int K_int;
                        K_int = System.Int32.Parse(SplitStr[2], System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                        //int转二进制
                        string K_bin = Convert.ToString(K_int, 2).PadLeft(8, '0');//8位，不足补0
                        //字符串插入' '
                        K_bin = K_bin.Insert(4, " ");
                        //立即数存入寄存器
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                RD = R0BOX.Text;
                                R0BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R1":
                                RD = R1BOX.Text;
                                R1BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R2":
                                RD = R2BOX.Text;
                                R2BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R3":
                                RD = R3BOX.Text;
                                R3BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R4":
                                RD = R4BOX.Text;
                                R4BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R5":
                                RD = R5BOX.Text;
                                R5BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R6":
                                RD = R6BOX.Text;
                                R6BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                            case "R7":
                                RD = R7BOX.Text;
                                R7BOX.Text = K_int.ToString("X4");//8位存入目的寄存器
                                break;
                        }
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ST
                        STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RS;
                            SRBOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                        }
                        //ET
                        ETXvlie = new string[] { "SR->BUS,BUS->Rd" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = SRBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[10].BackColor = Color.SkyBlue;
                        }
                        break;
                    case "LD":
                        string MemoryAdress;
                        ListViewItem lvi = new ListViewItem();
                        ListViewItem foundItem = this.ZhuCunBox.FindItemWithText(R7BOX.Text, false, 0);    //参数1：要查找的文本；参数2：是否子项也要查找；参数3：开始查找位置
                        RS = R7BOX.Text;
                        switch (SplitStr[1])//目的寄存器   R7所指内存内热存入RD
                        {
                            case "R0":
                                RD = R0BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R0BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) > 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R0BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                        //DialogResult re = MessageBox.Show("请选择中止，重试，忽略？", "操作选择", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
                                        //switch (re)
                                        //{
                                        //    case (DialogResult)MessageBoxButtons.AbortRetryIgnore:
                                        //        break;
                                        //}
                                    }
                                }
                                break;
                            case "R1":
                                RD = R1BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R1BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) > 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R1BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                            case "R2":
                                RD = R2BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R2BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) > 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R2BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                            case "R3":
                                RD = R3BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R3BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) > 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R3BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                            case "R4":
                                RD = R4BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R4BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) > 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R4BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                            case "R5":
                                RD = R5BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R5BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber)> 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R5BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                            case "R6":
                                RD = R6BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R6BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber)> 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R6BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                            case "R7":
                                RD = R7BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    R7BOX.Text = foundItem.Text;
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    MemoryAdress = R7BOX.Text;
                                    lvi = new ListViewItem();
                                    if (System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) > 0020)//如果地址小于0020
                                    {
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add("00");//赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                        R7BOX.Text = "0000";
                                    }
                                    else
                                    {
                                        //MessageBox.Show("主存地址小于0020!");
                                    }
                                }
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ST
                        STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RS;
                            SRBOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                            Pause();
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                            Pause();
                        }
                        //ET
                        ETXvlie = new string[] { "SR->BUS,BUS->Rd" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = SRBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[10].BackColor = Color.SkyBlue;
                            Pause();
                        }
                        break;
                    case "ST":
                        foundItem = this.ZhuCunBox.FindItemWithText(R7BOX.Text, false, 0);    //参数1：要查找的文本；参数2：是否子项也要查找；参数3：开始查找位置
                        RD = R7BOX.Text;
                        switch (SplitStr[2])//源寄存器   RS存R7指向内存
                        {
                            case "R0":
                                RS = R0BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R0BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R0BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R0BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R1":
                                RS = R1BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R1BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R1BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R1BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R2":
                                RS = R2BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R2BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R2BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R2BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R3":
                                RS = R3BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R3BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R3BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R3BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R4":
                                RS = R4BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R4BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R4BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R4BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R5":
                                RS = R5BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R5BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R5BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R5BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R6":
                                RS = R6BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R6BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R6BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R6BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                            case "R7":
                                RS = R7BOX.Text;
                                if (foundItem != null)
                                {
                                    this.ZhuCunBox.TopItem = foundItem;  //定位到该项
                                    foundItem.ForeColor = Color.BlueViolet;
                                    if (Convert.ToInt32(R7BOX.Text) > 255)
                                    {
                                        MessageBox.Show("数据超出主存位数范围!");
                                    }
                                    else
                                    {
                                        foundItem.Text = R7BOX.Text;
                                    }
                                }
                                else if (foundItem == null)//如果没找到
                                {
                                    lvi = new ListViewItem();
                                    //if (Convert.ToInt32(R7BOX.Text) < 0020)//如果地址小于0020
                                    //{
                                    //    MessageBox.Show("主存地址小于0020!");
                                    //}
                                    //else
                                    {
                                        MemoryAdress = R7BOX.Text;
                                        lvi.Text = MemoryAdress;
                                        lvi.SubItems.Add(R7BOX.Text.Substring(2));//16位转8位赋初值
                                        this.ZhuCunBox.Items.Add(lvi);
                                    }
                                }
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //ST
                        STXvlie = new string[] { "Rs->BUS,BUS->SR" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = STXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            SRBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RS;
                            SRBOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[3].BackColor = Color.SkyBlue;
                            Pause();
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                            Pause();
                        }
                        //ET
                        ETXvlie = new string[] { "SR->BUS,BUS->Rd" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            //寄存器
                            BUSBOX1.Text = SRBOX1.Text;
                            //同步
                            TongBu();
                            //控存
                            QuanBai();
                            KongZhiCunChu.Items[11].BackColor = Color.SkyBlue;
                            Pause();
                        }
                        break;
                    case "INC":
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                RD = R0BOX.Text;
                                RD_int = System.Int32.Parse(R0BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R0BOX.Text = RD_Hex;
                                break;
                            case "R1":
                                RD = R1BOX.Text;
                                RD_int = System.Int32.Parse(R1BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R1BOX.Text = RD_Hex;
                                break;
                            case "R2":
                                RD = R2BOX.Text;
                                RD_int = System.Int32.Parse(R2BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R2BOX.Text = RD_Hex;
                                break;
                            case "R3":
                                RD = R3BOX.Text;
                                RD_int = System.Int32.Parse(R3BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R3BOX.Text = RD_Hex;
                                break;
                            case "R4":
                                RD = R4BOX.Text;
                                RD_int = System.Int32.Parse(R4BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R4BOX.Text = RD_Hex;
                                break;
                            case "R5":
                                RD = R5BOX.Text;
                                RD_int = System.Int32.Parse(R5BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R5BOX.Text = RD_Hex;
                                break;
                            case "R6":
                                RD = R6BOX.Text;
                                RD_int = System.Int32.Parse(R6BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R6BOX.Text = RD_Hex;
                                break;
                            case "R7":
                                RD = R7BOX.Text;
                                RD_int = System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) + 1;//rd自增1
                                RD_Hex = RD_int.ToString("X4");
                                R7BOX.Text = RD_Hex;
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        if (RD_int == 0)
                        {
                            PSW_Z.Text = "1";
                        }
                        else if (RD_int <0)
                        {
                            PSW_N.Text = "1";
                        }
                        else if (RD_int > 65535)
                        {
                            PSW_C.Text = "1";
                            PSW_V.Text = "1";
                        }
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器可视
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                            break;
                        }
                        //ET
                        ETXvlie = new string[] { "INC" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            //寄存器可视
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[12].BackColor = Color.SkyBlue;
                        }
                        break;
                    case "DEC":
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                RD = R0BOX.Text;
                                RD_int = System.Int32.Parse(R0BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R0BOX.Text = RD_Hex;
                                break;
                            case "R1":
                                RD = R1BOX.Text;
                                RD_int = System.Int32.Parse(R1BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R1BOX.Text = RD_Hex;
                                break;
                            case "R2":
                                RD = R2BOX.Text;
                                RD_int = System.Int32.Parse(R2BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R2BOX.Text = RD_Hex;
                                break;
                            case "R3":
                                RD = R3BOX.Text;
                                RD_int = System.Int32.Parse(R3BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R3BOX.Text = RD_Hex;
                                break;
                            case "R4":
                                RD = R4BOX.Text;
                                RD_int = System.Int32.Parse(R4BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R4BOX.Text = RD_Hex;
                                break;
                            case "R5":
                                RD = R5BOX.Text;
                                RD_int = System.Int32.Parse(R5BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R5BOX.Text = RD_Hex;
                                break;
                            case "R6":
                                RD = R6BOX.Text;
                                RD_int = System.Int32.Parse(R6BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R6BOX.Text = RD_Hex;
                                break;
                            case "R7":
                                RD = R7BOX.Text;
                                RD_int = System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber) - 1;//rd自减1
                                RD_Hex = RD_int.ToString("X4");
                                R7BOX.Text = RD_Hex;
                                break;
                        }
                        BinaryBox.Text += "\r\n";
                        if (RD_int == 0)
                        {
                            PSW_Z.Text = "1";
                        }
                        else if (RD_int > 0)
                        {
                            PSW_N.Text = "1";
                        }
                        else if (RD_int > 65535)
                        {
                            PSW_C.Text = "1";
                            PSW_V.Text = "1";
                        }
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int=PC_int+2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器可视
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                            break;
                        }
                        //ET
                        ETXvlie = new string[] { "DEC" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            //寄存器可视
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[13].BackColor = Color.SkyBlue;
                        }
                        break;
                    case "NEG":
                        switch (SplitStr[1])//目的寄存器
                        {
                            case "R0":
                                RD = R0BOX.Text;
                                RD_int = System.Int32.Parse(R0BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R0BOX.Text = RD_Hex;
                                break;
                            case "R1":
                                RD = R1BOX.Text;
                                RD_int = System.Int32.Parse(R1BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R1BOX.Text = RD_Hex;
                                break;
                            case "R2":
                                RD = R2BOX.Text;
                                RD_int = System.Int32.Parse(R2BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R2BOX.Text = RD_Hex;
                                break;
                            case "R3":
                                RD = R3BOX.Text;
                                RD_int = System.Int32.Parse(R3BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R3BOX.Text = RD_Hex;
                                break;
                            case "R4":
                                RD = R4BOX.Text;
                                RD_int = System.Int32.Parse(R4BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R4BOX.Text = RD_Hex;
                                break;
                            case "R5":
                                RD = R5BOX.Text;
                                RD_int = System.Int32.Parse(R5BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R5BOX.Text = RD_Hex;
                                break;
                            case "R6":
                                RD = R6BOX.Text;
                                RD_int = System.Int32.Parse(R6BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R6BOX.Text = RD_Hex;
                                break;
                            case "R7":
                                RD = R7BOX.Text;
                                RD_int = System.Int32.Parse(R7BOX.Text, System.Globalization.NumberStyles.HexNumber);//rd转十进制
                                Azx.Text = RD_int.ToString();//转字符串粗存入文本框AZX
                                ZhunaHuan_Click(sender, e);//调用转换事件
                                RD_int = System.Convert.ToInt32(txtNumOne.Text, 2);//二进制转十进制
                                RD_Hex = RD_int.ToString("X4");//十进制转十六进制
                                R7BOX.Text = RD_Hex;
                                break;
                        }
                        //打印微命令序列
                        //TF
                        TFXvlie = new string[]
                        {
                            "PC->IBUS, IBUS->IMAR, IREAD",
                            " PC->BUS,CLEAR LA,1->C0,ADD,ALU->LT",
                            "LT->IBUS,IBUS->PC,IWAIT",
                            "IMDR->IBUS,IBUS->IR"
                        };
                        for (int j = 0; j < 4; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = TFXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            switch (j)//4条序列填入的同时可视化
                            {
                                case 0:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IMARBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化  注意同步
                                    IBUSBOX1.Text = PCBOX1.Text;
                                    IMARBOX1.Text = PCBOX.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 1:
                                    //高亮可视化
                                    BUSBOX1.BackColor = Color.SkyBlue;
                                    LABOX1.BackColor = Color.SkyBlue;
                                    LTBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    BUSBOX1.Text = PCBOX1.Text;
                                    LABOX1.Text = "";
                                    IMARBOX1.Text = PCBOX1.Text;
                                    int PC_int = int.Parse(PCBOX1.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    PC_int = PC_int + 2;
                                    string PC_Bin = Convert.ToString(PC_int, 2);
                                    LTBOX1.Text = PC_int.ToString("X4");
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[0].BackColor = Color.SkyBlue;
                                    break;
                                case 2:
                                    //高亮可视化
                                    QuanHui();
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    PCBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视化
                                    IBUSBOX1.Text = LTBOX1.Text;
                                    PCBOX1.Text = IBUSBOX1.Text;
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[1].BackColor = Color.SkyBlue;
                                    break;
                                case 3:
                                    //高亮可视化
                                    IBUSBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    PCBOX1.BackColor = Color.FromArgb(229, 229, 229);//恢复灰色
                                    IBUSBOX1.BackColor = Color.SkyBlue;
                                    IRBOX1.BackColor = Color.SkyBlue;
                                    //寄存器可视
                                    IR_Bin = BinaryBox.Lines[i + 1];
                                    if (IR_Bin != "")
                                    {
                                        IR_Bin = IR_Bin.Replace(" ", "");
                                        IR_int = Convert.ToInt32(IR_Bin, 2);
                                        IR_Hex = IR_int.ToString("X4");
                                        IBUSBOX1.Text = IR_Hex;
                                        IRBOX1.Text = IR_Hex;
                                    }
                                    //寄存器同步
                                    TongBu();
                                    //控存可视化
                                    QuanBai();
                                    KongZhiCunChu.Items[2].BackColor = Color.SkyBlue;
                                    break;
                            }
                            Pause();
                        }
                        //DT
                        DTXvlie = new string[] { "Rd->BUS,BUS->LA" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = DTXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            BUSBOX1.BackColor = Color.SkyBlue;
                            LABOX1.BackColor = Color.SkyBlue;
                            //寄存器可视
                            BUSBOX1.Text = RD;
                            LABOX1.Text = BUSBOX1.Text;
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[4].BackColor = Color.SkyBlue;
                            break;
                        }
                        //ET
                        ETXvlie = new string[] { "DEC" };
                        for (int j = 0; j < 1; j++)   //添加
                        {
                            ListViewItem Lv_XvLie = new ListViewItem();
                            Lv_XvLie.Text = ETXvlie[j];
                            this.WeiZhiLinXvLie.Items.Add(Lv_XvLie);
                            //高亮可视化
                            QuanHui();
                            //寄存器可视
                            //寄存器同步
                            TongBu();
                            //控存可视化
                            QuanBai();
                            KongZhiCunChu.Items[13].BackColor = Color.SkyBlue;
                        }
                        break;
                    case "NOP":
                        break;
                    case "":
                        break;
                }
                BufferBox.Text += (32 + i*2).ToString("X4");//十六进制;
                BufferBox.Text += "  ";
                BufferBox.Text += "汇编指令:";
                BufferBox.Text += AsmBox.Lines[i];
                BufferBox.Text += "  ";
                BufferBox.Text += "机器指令:";
                BufferBox.Text += BinaryBox.Lines[i];
                BufferBox.Text += "\r\n";
                BufferBox.Text += "R0:" + R0BOX.Text + " 	 " + "R1:" + R1BOX.Text + " 	    " + "R2:" + R2BOX.Text + "       " + "R3:" + R3BOX.Text + "        " + "R4:" + R4BOX.Text + "    " + "R5:" + R5BOX.Text + "        " + "R6:" + R6BOX.Text + "     " + "R7:" + R7BOX.Text;
                BufferBox.Text += "\r\n";
                BufferBox.Text += "PC:" + PCBOX.Text + "    " + "MAR:" + MARBOX.Text + "    " + "BUS:"+BUSBOX.Text + "    " + "MDR:" + MDRBOX.Text + "    " + "SR:" + SRBOX.Text + "    " + "IMAR:" + IMARBOX.Text + "    " + "DR:" + DRBOX.Text + "    " + "IMDR:" + IMDRBOX.Text + "    ";
                BufferBox.Text += "\r\n";
                BufferBox.Text += "PSW:" + PSW_N.Text + PSW_Z.Text + PSW_V.Text + PSW_C.Text;
                BufferBox.Text += "\r\n";
                BufferBox.Text += "\r\n";
                BufferBox.Text += "***********************************************************************************************";
                BufferBox.Text += "\r\n";
                BufferBox.Text += "\r\n";
            }
            //保存文件
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            saveFileDialog.FileName = "文件" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
            //将日期时间作为文件名
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, true);
                streamWriter.Write(this.BufferBox.Text);
                streamWriter.Close();
            }
        }
        /// <summary>
        /// The ResetBT_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void ResetBT_Click(object sender, EventArgs e)
        {
            R0BOX.Text = "0000";
            R1BOX.Text = "0000";
            R2BOX.Text = "0000";
            R3BOX.Text = "0000";
            R4BOX.Text = "0000";
            R5BOX.Text = "0000";
            R6BOX.Text = "0000";
            R7BOX.Text = "0000";
            PCBOX.Text = "0020";
            MARBOX.Text = "0000";
            BUSBOX.Text = "0000";
            MDRBOX.Text = "0000";
            SRBOX.Text = "0000";
            IMARBOX.Text = "0000";
            DRBOX.Text = "0000";
            IMDRBOX.Text = "0000";
            PSW_N.Text = "0";
            PSW_Z.Text = "0";
            PSW_V.Text = "0";
            PSW_C.Text = "0";
            BinaryBox.Text = "";
            BUSBOX1.Text = "0000";
            LTBOX1.Text = "0000";
            IMARBOX1.Text = "0000";
            MARBOX1.Text = "0000";
            LABOX1.Text = "0000";
            IMDRBOX1.Text = "0000";
            MDRBOX1.Text = "0000";
            IRBOX1.Text = "0000";
            IBUSBOX1.Text = "0000";
            SRBOX1.Text = "0000";
            DRBOX1.Text = "0000";
            PCBOX1.Text = "0020";
            Azx.Text = "";
            Bzx.Text = "";
            txtNumOne.Text = "";
            txtNumTwo.Text = "";
            ShowACC.Text = "";
            ShowMQ .Text= "";
            ShowX.Text = "";
            ShowBox.Text = "";
            YunSuanJIeGuo_Bin.Text = "";
            YunSuanJieGuo_Int.Text = "";
            BufferBox.Text = "";
            this.ZhuCunBox.Items.Clear();  //只移除所有的项。
            this.KongZhiCunChu.Items.Clear();  //只移除所有的项。
            this.WeiZhiLinXvLie.Items.Clear();  //只移除所有的项。
            this.ZhiLinCunChuList.Items.Clear();  //只移除所有的项。
        }
        private void ImportBT_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = " 请选择您要导入的文件：";
            openFileDialog.Filter = "All files (*.*)|*.*|TextDocument(*.txt)|*.txt|TextDocument(*.ini)|*.ini|TextDocument(*.data)|*.data";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")//判断文件名是否为空
            {
                StreamReader streamReader = new StreamReader(openFileDialog.FileName, Encoding.Default);
                this.AsmBox.Text = streamReader.ReadToEnd();
            }
        }
        private void SpeedBT_Click(object sender, EventArgs e)
        {
            Speed_string = SpeedBOX.Text;
            Speed_int = Convert.ToInt32(Speed_string);
            times = Speed_int;
            MessageBox.Show("改变成功!现在间隔为:"+times);
        }
    }
}
