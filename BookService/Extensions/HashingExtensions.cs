using System;
using System.Security.Cryptography;
using System.Text;

namespace FinanceManager.WebApi.Infrastructure.Extensions
{
    public static class HashingExtensions
    {
        public static string ComputeHash(string login, string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(login))
            {
                return "";
            }
            Span<char> span = stackalloc char[login.Length + password.Length];
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            while (num3 < span.Length)
            {
                if (num2 % 2 == 0)
                {
                    if (num >= password.Length)
                    {
                        span[num3++] = login[num];
                    }
                    else
                    {
                        span[num3++] = password[num];
                    }
                }
                else
                {
                    if (num >= login.Length)
                    {
                        span[num3++] = password[num];
                    }
                    else
                    {
                        span[num3++] = login[num];
                    }

                    num++;
                }
                num2++;
            }
            int charsWritten = Encoding.UTF8.GetMaxByteCount(span.Length);
            Span<byte> span2 = stackalloc byte[charsWritten];
            span2 = span2[..Encoding.UTF8.GetBytes(span, span2)];
            Span<byte> destination = stackalloc byte[48];
            SHA384.HashData(span2, destination);
            Span<char> span3 = stackalloc char[96];
            for (int i = 0; i < 48; i++)
            {
                destination[i].TryFormat(span3.Slice(i * 2), out charsWritten, "x2");
            }
            return span3.ToString();
        }
    }
}
