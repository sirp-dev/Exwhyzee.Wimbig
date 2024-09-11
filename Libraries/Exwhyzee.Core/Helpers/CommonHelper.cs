using PhoneNumbers;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Exwhyzee.Core.Helpers
{
    public static class CommonHelper
    {
        public static string GetSha512(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes_sha512_in = Encoding.UTF8.GetBytes(text);
                byte[] bytes_sha512_out = sha512.ComputeHash(bytes_sha512_in);
                string str_sha512_out = BitConverter.ToString(bytes_sha512_out);
                str_sha512_out = str_sha512_out.Replace("-", "");
                return str_sha512_out;
            }
        }

        public static string CreateKey(int size = 32)
        {
            string key;
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[size]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                key = Convert.ToBase64String(secretKeyByteArray);
            }
            return key;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str = string.Concat(str, random.Next(10).ToString());
            }

            return str;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <typeparam name="T">The Type to convert the object to</typeparam>
        /// <param name="myobj">>The value to convert.</param>
        /// <param name="keepType">Converts all Properties to the same Type, else all properties are converted to string</param>
        /// <returns>The converted value.</returns>
        public static T To<T>(object myobj, bool keepType = false)
        {
            var objectType = myobj.GetType();
            var targetType = typeof(T);
            var targetInstance = Activator.CreateInstance(targetType, false);

            // Find common members by name
            var sourceMembers = from source in objectType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;
            var targetMembers = from source in targetType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;
            var commonMembers = targetMembers.Where(memberInfo => sourceMembers.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();

            foreach (var memberInfo in commonMembers)
            {
                if (!((PropertyInfo)memberInfo).CanWrite)
                {
                    continue;
                }

                var targetProperty = typeof(T).GetProperty(memberInfo.Name);
                if (targetProperty == null)
                {
                    continue;
                }

                var sourceProperty = myobj.GetType().GetProperty(memberInfo.Name);
                if (sourceProperty == null)
                {
                    continue;
                }

                // Commented out to pervent type checking

                // Check source and target types are the same
                //if (sourceProperty.PropertyType.Name != targetProperty.PropertyType.Name) continue;

                var value = myobj.GetType().GetProperty(memberInfo.Name)?.GetValue(myobj, null);
                if (value == null)
                {
                    continue;
                }

                // Set the value
                targetProperty.SetValue(targetInstance, keepType == true ? value : Convert.ToString(value), null);
            }
            return (T)targetInstance;
        }

        public static bool IsValidPhoneNumber(string phoneNumber, string countryCode = "NG")
        {
            bool valid = false;

            try
            {
                PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

                var number = phoneNumberUtil.Parse(phoneNumber, countryCode);
                valid = phoneNumberUtil.IsValidNumber(number);
            }
            catch (NumberParseException ex)
            {
                throw ex;
            }

            return valid;
        }

        public static string FormatPhoneNumber(string phoneNumber, bool includePlusSign = false, string countryCode = "NG")
        {
            try
            {
                PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
                if (!IsValidPhoneNumber(phoneNumber, countryCode))
                {
                    return phoneNumber;
                }

                var number = phoneNumberUtil.Parse(phoneNumber, countryCode);
                var phone = phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
                if (includePlusSign)
                {
                    return phone;
                }
                return phone.StartsWith("+") ? phone.Substring(1, phone.Length - 1) : phone;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
