using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class User
    {
        public User()
        {
            this.U_IsDelete = 0;
            this.U_Province = 0;
            this.U_City = 0;
            this.U_Region = 0;
            this.U_Is_Enable = 1;
            this.U_Is_Check = 0;
            this.U_CreateTime = DateTime.Now;
            this.Referrer = 0;
            this.U_Gender = 0;
            this.U_DepartmentID = 0;
            this.U_StudyCount = 0;
            this.U_CommentCount = 0;
            this.U_DemandCount = 0;
            this.U_CourseVideoCount = 0;
            this.U_CoursePPTCount = 0;
            this.U_CourseJiangYiCount = 0;
        }
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "用户名不少于5个字符")]
        [Display(Name = "用户名")]
        public string U_UserName { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "姓名由5-20个字符组成")]
        [Display(Name = "姓名")]
        public string U_RealName { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "昵称由5-20个字符组成")]
        [Display(Name = "昵称")]
        public string U_NickName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50)]
        [Display(Name = "密码")]
        public string U_Pwd { get; set; }

        [StringLength(50)]
        [Display(Name = "邮箱")]
        public string U_Email { get; set; }

        [StringLength(50)]
        [Display(Name = "手机号码")]
        public string U_Phone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(500)]
        [Display(Name = "头像")]
        public string U_Thumbnail { get; set; }
        /// <summary>
        /// 删除 1是，0否
        /// </summary>
        public int U_IsDelete { get; set; }

        [Display(Name = "省份")]
        public int U_Province { get; set; }

        [Display(Name = "城市")]
        public int U_City { get; set; }

        [Display(Name = "地区")]
        public int U_Region { get; set; }

        [StringLength(500)]
        [Display(Name = "地址")]
        public string U_Address { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        [Display(Name = "账户余额")]
        public decimal U_Amount { get; set; }

        /// <summary>
        /// 可提现金额（提现额度）
        /// </summary>
        [Display(Name = "可提现额度")]
        public decimal U_TiXianAmount { get; set; }

        /// <summary>
        /// 锁定账户余额（不可用余额）
        /// </summary>
        [Display(Name = "不可用余额")]
        public decimal U_LockAmount { get; set; }

        /// <summary>
        /// 账户积分
        /// </summary>
        [Display(Name = "账户积分")]
        public int U_Score { get; set; }

        /// <summary>
        /// 锁定账户积分（不可用积分）
        /// </summary>
        [Display(Name = "不可用积分")]
        public int U_LockScore { get; set; }

        [Display(Name = "医院名称")]
        [StringLength(50)]
        public string U_Company { get; set; }
        /// <summary>
        /// 门店信息
        /// </summary>
        [Display(Name = "门店")]
        [StringLength(50)]
        public string U_ShopName { get; set; }

        [Display(Name = "QQ")]
        [StringLength(50)]
        public string U_QQ { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        [Display(Name = "身高")]
        [StringLength(50)]
        public string U_Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        [Display(Name = "体重")]
        [StringLength(50)]
        public string U_Weight { get; set; }

        /// <summary>
        /// 目标体重
        /// </summary>
        [Display(Name = "目标体重")]
        [StringLength(50)]
        public string U_HopeWeight { get; set; }

        /// <summary>
        /// 绑定微信openid
        /// </summary>
        [Display(Name = "绑定微信openid")]
        [StringLength(50)]
        public string U_OpenId { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        [StringLength(50)]
        public string U_Birthday { get; set; }


        /// <summary>
        /// 0未启用 1已启用
        /// </summary>
        [Display(Name = "是否启用")]
        public int U_Is_Enable { get; set; }

        /// <summary>
        /// 默认0未开光 1已开光
        /// </summary>
        [Display(Name = "是否开光")]
        public int U_Is_Check { get; set; }

        public DateTime U_CreateTime { get; set; }

        /// <summary>
        /// 注册所在门店
        /// </summary>
        [Display(Name = "所属门店")]
        public int UserShopID { get; set; }

        [Display(Name = "会员角色")]
        public int UserRoleID { get; set; }

        public virtual UserRole UserRole { get; set; }

        [Display(Name = "会员等级")]
        public int UserLevelID { get; set; }

        /// <summary>
        /// 推荐人ID
        /// </summary>
        [Display(Name = "推荐人")]
        public int Referrer { get; set; }

        /// <summary>
        /// 0未设置 1男 2女 3保密
        /// </summary>
        [Display(Name = "性别")]
        public int U_Gender { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [Display(Name = "学历")]
        [StringLength(50)]
        public string U_Education { get; set; }

        /// <summary>
        /// 所属科室 ,默认0未设置
        /// </summary>
        [Display(Name = "所属科室")]
        public int U_DepartmentID { get; set; }

        /// <summary>
        /// 学习热度
        /// </summary>
        [Display(Name = "学习热度")]
        public int U_StudyCount { get; set; }

        /// <summary>
        /// 评论跟帖数
        /// </summary>
        [Display(Name = "评论跟帖")]
        public int U_CommentCount { get; set; }

        /// <summary>
        /// 培训需求提交次数（经销商使用）
        /// </summary>
        [Display(Name = "培训需求提交次数")]
        public int U_DemandCount { get; set; }

        /// <summary>
        /// 学习视频数
        /// </summary>
        [Display(Name = "学习视频数")]
        public int U_CourseVideoCount { get; set; }

        /// <summary>
        /// 学习PPT数
        /// </summary>
        [Display(Name = "学习PPT数")]
        public int U_CoursePPTCount { get; set; }
        /// <summary>
        /// 学习讲义数
        /// </summary>
        [Display(Name = "学习讲义数")]
        public int U_CourseJiangYiCount { get; set; }



        public virtual UserLevel UserLevel { get; set; }

        public virtual ICollection<UserPayInfo> PayInfos { get; set; }
    }


    //登录
    public class UserLoginVModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "手机号")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住密码")]
        public bool IsRememberPwd { get; set; }

    }

    #region 注册

    /// <summary>
    /// 用户名注册
    /// </summary>
    public class UserRegisterVModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "用户名不少于5个字符")]
        [Display(Name = "用户名")]
        public string U_UserName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码由6-20个字符组成")]
        [Display(Name = "密码")]
        public string U_Pwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("U_Pwd", ErrorMessage = "输入密码不一致")]
        public string U_Pwd2 { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "手机号码")]
        public string U_Phone { get; set; }

        [Display(Name = "会员角色")]
        public int Role { get; set; }
    }

    /// <summary>
    /// 邮箱注册
    /// </summary>
    public class UserEmailRegisterVModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50)]
        [RegularExpression(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "邮箱")]
        public string U_Email { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码由6-20个字符组成")]
        [Display(Name = "密码")]
        public string U_Pwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("U_Pwd", ErrorMessage = "输入密码不一致")]
        public string U_Pwd2 { get; set; }

        [Display(Name = "验证码")]
        public string U_Code { get; set; }

        [Display(Name = "会员角色")]
        public int Role { get; set; }
    }

    /// <summary>
    /// 手机号码注册
    /// </summary>
    public class UserMobileRegisterVModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "手机号码")]
        public string U_Phone { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码由6-20个字符组成")]
        [Display(Name = "密码")]
        public string U_Pwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("U_Pwd", ErrorMessage = "输入密码不一致")]
        public string U_Pwd2 { get; set; }

        [Display(Name = "验证码")]
        public string U_Code { get; set; }

        [Display(Name = "会员角色")]
        public int Role { get; set; }
    }

    #endregion

    #region 修改资料

    //修改资料
    public class EditUserVModel
    {
        public EditUserVModel()
        {
        }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "昵称")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        [Display(Name = "公司名称")]
        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "手机号码")]
        public string Phone { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^\d{4,20}$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "QQ")]
        public string QQ { get; set; }

        /// <summary>
        /// 可提现金额（提现额度）
        /// </summary>
        [Display(Name = "可提现金额")]
        public decimal U_TiXianAmount { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        [Display(Name = "身高")]
        [StringLength(50)]
        public string Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        [Display(Name = "体重")]
        [StringLength(50)]
        public string Weight { get; set; }

        /// <summary>
        /// 目标体重
        /// </summary>
        [Display(Name = "目标体重")]
        [StringLength(50)]
        public string HopeWeight { get; set; }

        /// <summary>
        /// 绑定微信openid
        /// </summary>
        [Display(Name = "绑定微信openid")]
        [StringLength(50)]
        public string OpenId { get; set; }

        /// <summary>
        /// 默认0未开光 1已开光
        /// </summary>
        [Display(Name = "是否开光")]
        public int IsCheck { get; set; }

        /// <summary>
        /// 0不启用 1启用
        /// </summary>
        [Display(Name = "是否启用")]
        public int IsEnable { get; set; }


        [Display(Name = "省份")]
        public int Province { get; set; }

        [Display(Name = "城市")]
        public int City { get; set; }

        [Display(Name = "地区")]
        public int Region { get; set; }

        [StringLength(500)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 注册所在门店
        /// </summary>
        [Display(Name = "所属门店")]
        public int UserShopID { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        [Display(Name = "推荐人")]
        public int Referrer { get; set; }

        /// <summary>
        /// 0保密 1男 2女
        /// </summary>
        [Display(Name = "性别")]
        public int Gender { get; set; }

        [Display(Name = "所属部门")]
        public int DepartmentID { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [Display(Name = "学历")]
        [StringLength(50)]
        public string Education { get; set; }

        /// <summary>
        /// 门店
        /// </summary>
        [Display(Name = "门店")]
        [StringLength(50)]
        public string ShopName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        public string U_Birthday { get; set; }


        #region 结算收款/支付账号信息

        [Display(Name = "微信号")]
        [StringLength(50)]
        public string Wechat { get; set; }

        [Display(Name = "支付宝账号", ShortName = "账号")]
        [StringLength(50)]
        public string AlipayNO { get; set; }

        [Display(Name = "卡号")]
        [StringLength(50)]
        public string BankNO { get; set; }

        [Display(Name = "开户行")]
        [StringLength(50)]
        public string BankName { get; set; }

        [Display(Name = "开户地")]
        [StringLength(200)]
        public string BankAddress { get; set; }

        #endregion
    }
    #endregion

    #region 修改密码
    //修改密码
    public class ChangePwdVModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "原密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码由6-20个字符组成")]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "输入密码不一致")]
        public string NewPassword2 { get; set; }
    }
    #endregion

    #region 会员重置密码

    /// <summary>
    /// 会员重置密码
    /// </summary>
    public class UserForgetPwdVModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "手机号码")]
        public string U_Phone { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码由6-20个字符组成")]
        [Display(Name = "新密码")]
        public string U_Pwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("U_Pwd", ErrorMessage = "输入密码不一致")]
        public string U_Pwd2 { get; set; }

        [Display(Name = "验证码")]
        public string U_Code { get; set; }
    }
    #endregion

    #region 主页统计数量实体

    /// <summary>
    /// 主页统计数量实体
    /// </summary>
    public class TongJiCountVModel
    {
        /// <summary>
        /// 总视频
        /// </summary>
        public int VideoCount { get; set; }
        /// <summary>
        /// 总PPT
        /// </summary>
        public int PPTCount { get; set; }
        /// <summary>
        /// 总讲议
        /// </summary>
        public int JiangYiCount { get; set; }
        /// <summary>
        /// 总培训活动
        /// </summary>
        public int PeiXunCount { get; set; }
        /// <summary>
        /// 总作品
        /// </summary>
        public int DesignWorkCount { get; set; }
        /// <summary>
        /// 总问卷调查
        /// </summary>
        public int QuestionnaireCount { get; set; }
    }
    #endregion

    #region 用户统计数据
    public class UserTongJiVModel
    {
        public UserTongJiVModel()
        {
        }
        public User User { get; set; }
        public Department Department { get; set; }
        public int TestCount { get; set; }
        public int TestVideoCount { get; set; }
        /// <summary>
        /// 合格数
        /// </summary>
        public int TestHegeCount { get; set; }
        /// <summary>
        /// 学习视频个数
        /// </summary>
        public int VideoCount { get; set; }
        /// <summary>
        /// 学习课件个数
        /// </summary>
        public int PPTCount { get; set; }
    }

    #endregion


}
