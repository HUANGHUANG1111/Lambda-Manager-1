﻿using System;
using System.Text;
using System.Text.Json.Serialization;

namespace ACE.Global
{

    /// <summary>
    /// 注册信息
    /// </summary>
    public class RegisterInfo : ViewModelBase
    {

        public void SetValue(RegisterInfo registerInfo)
        {
            this.UserName = registerInfo.UserName;
            this.RegistrationDate = registerInfo.RegistrationDate;
            this.RegisteredAddress = registerInfo.RegisteredAddress;
            this.ExpirationDate =registerInfo.ExpirationDate;
            this.Email = registerInfo.Email;
            this.PhoneNumber = registerInfo.PhoneNumber;
        }

        private string userName = string.Empty;
        /// <summary>
        /// 用户名
        /// </summary>
       [JsonPropertyName("name")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; NotifyPropertyChanged(); }
        }

        private string registrationDate = string.Empty;

        
        /// <summary>
        /// 注册日期
        /// </summary>
        [JsonPropertyName("create_date")]
        public string RegistrationDate
        {
            get { return registrationDate; }
            set { registrationDate = value; NotifyPropertyChanged(); }
        }


        private string registeredAddress = string.Empty;

        /// <summary>
        /// 注册地址
        /// </summary>
        [JsonPropertyName("legal_address")]
        public string RegisteredAddress
        {
            get { return registeredAddress; }
            set { registeredAddress = value; NotifyPropertyChanged(); }
        }

        private string expirationDate = string.Empty;
        /// <summary>
        /// 失效日期
        /// </summary>
        [JsonPropertyName("expire_date")]
        public string ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; NotifyPropertyChanged(); }
        }

        private string email = string.Empty;
        /// <summary>
        /// 注册邮件地址
        /// </summary>
        [JsonPropertyName("email_address")]
        public string Email
        {
            get { return email; }
            set { email = value; NotifyPropertyChanged(); }
        }

        private string phoneNumber = string.Empty;
        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonPropertyName("contact_number")]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; NotifyPropertyChanged(); }
        }

        public override string ToString()
        {
            return $"{UserName},{RegistrationDate},{RegisteredAddress},{ExpirationDate},{Email},{PhoneNumber}";
        }


        public string MD5()
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.ToString());
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
   
    }
}
