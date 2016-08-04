using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;

namespace TeraLibrary
{
	public class Work_Data
	{
		public string str_Name = "", str_Status = "";
		public DateTime dt_Start = DateTime.Now, dt_Now = DateTime.Now, dt_End = DateTime.Now;

		public AB_Data ab_Data = null;

		public Dictionary<string, AB_Data> dic_AB = null;


		public Work_Data(string a_path, string b_base_path)
		{
			str_Status = "Normal";

			if (Directory.Exists(a_path) == false)
			{
				str_Status = "Abend";
				return;
			}

			var di_a = new DirectoryInfo(a_path);

			if (Directory.Exists(b_base_path) == false)
			{
				str_Status = "Abend";
				return;
			}

			var di_b = new DirectoryInfo(b_base_path + @"\" + di_a.Name);

			dic_AB = new Dictionary<string, AB_Data>();


			str_Name = di_a.Name;

			ab_Data = new AB_Data(dic_AB, di_a, di_b);
		}

		public bool Copy()
		{
			ab_Data.Copy();

			return false;
		}

	}

	public class AB_Data
	{
		public string type = "";
		public string Name = "", a_FullName = "", b_FullName = "";
		public long a_Length = 0, b_Length = 0;
		public bool b_Flag = false, copyEnd = false;

		public DirectoryInfo di_B = null;
		public FileInfo fi_A = null, fi_B = null;

		public List<AB_Data> ab_Datas = null;

 
		public AB_Data(Dictionary<string, AB_Data> dic_AB, DirectoryInfo di_a, DirectoryInfo di_b)
		{
			type = "Dir";

			Name = di_a.Name;

			a_FullName = di_a.FullName;
			b_FullName = di_b.FullName;

			a_Length = b_Length = 0;

			di_B = di_b;

			ab_Datas = new List<AB_Data>();

			foreach (var fi_c_a in di_a.GetFiles())
			{
				var fi_c_b = new FileInfo(b_FullName + @"\" + fi_c_a.Name);

				var ab_data = new AB_Data(dic_AB, fi_c_a, fi_c_b);

				ab_Datas.Add(ab_data);
			}

			foreach (var di_c_a in di_a.GetDirectories())
			{
				var di_c_b = new DirectoryInfo(b_FullName + @"\" + di_c_a.Name);

				var ab_data = new AB_Data(dic_AB, di_c_a, di_c_b);

				ab_Datas.Add(ab_data);
			}

			dic_AB.Add(a_FullName, this);
		}

		public AB_Data(Dictionary<string, AB_Data> dic_AB, FileInfo fi_a, FileInfo fi_b)
		{
			type = "File";

			Name = fi_a.Name;

			a_FullName = fi_a.FullName;
			b_FullName = fi_b.FullName;

			a_Length = fi_a.Length;
			if (fi_b.Exists)
			{
				b_Length = fi_b.Length;
			}

			fi_A = fi_a;
			fi_B = fi_b;

			//Console.WriteLine("Length = " + a_Length);

			dic_AB.Add(a_FullName, this);
		}

		public bool Copy()
		{
			loop:
		
			switch (type)
			{
				case "Dir":
 
					if (di_B.Exists == false)
					{
						di_B.Create();

						di_B = new DirectoryInfo(b_FullName);

						goto loop;
					}

					foreach (var ab_data in ab_Datas)
					{
						ab_data.Copy();
					}

					break;
				
				case "File":

					if (copyEnd == false)
					{
						if (fi_B.Exists == false)
						{
							fi_A.CopyTo(b_FullName, true);

							fi_B = new FileInfo(b_FullName);

							if (fi_B.Exists)
							{
								b_Length = fi_B.Length;
							}

							goto loop;
						}
						else
						{
							if (a_Length == b_Length)
							{
								if (file_compare(a_FullName, b_FullName) == false)
								{
									fi_A.CopyTo(b_FullName, true);

									fi_B = new FileInfo(b_FullName);

									if (fi_B.Exists)
									{
										b_Length = fi_B.Length;
									}

									goto loop;
								}
								else
								{
									copyEnd = true;
								}
							}
							else
							{
								fi_A.CopyTo(b_FullName, true);

								fi_B = new FileInfo(b_FullName);

								if (fi_B.Exists)
								{
									b_Length = fi_B.Length;
								}

								goto loop;
							}
						}
					}

					break;
			}

			return false;
		}

		private bool file_compare(string file_name1, string file_name2)
		{
			FileStream reader1 = new FileStream(file_name1,
												FileMode.Open,
												FileAccess.Read);

			FileStream reader2 = new FileStream(file_name2,
												FileMode.Open,
												FileAccess.Read);

			int X1, X2;
			bool ret_val = false;
			while (true)
			{
				X1 = reader1.ReadByte();
				X2 = reader2.ReadByte();
				if (X1 != X2) break;    // 比較
				if (X1 == -1) { ret_val = true; break; }
			}
			reader1.Close();
			reader2.Close();

			return ret_val;
		}

		private long SetFilesSize(DirectoryInfo di)
		{
			var size = 0L;

			/*if (di.Name.StartsWith("$"))
			{
				return size;
			}*/

			try
			{
				foreach (var file_c in di.GetFiles())
				{
					size += file_c.Length;
				}
			}
			catch (System.UnauthorizedAccessException e)
			{
				Console.WriteLine(e.Message);
				return size;
			}

			foreach (var dic_c in di.GetDirectories())
			{
				size += SetFilesSize(dic_c);
			}

			return size;
		}
	}

	[Serializable]
	public class Lite_動向調査
	{
		public string
			社員番号, 漢字氏名, 性別, 生年月日, 年齢_年数値, レベル０２名称, レベル０３名称, レベル０４名称,
			所属略称, 駐在, 建家名称, 建家順位, 役職, 担当１名称, 留・勤区分, 留・勤期間開始日, 留・勤機関名,
			社員区分名, 社員区分変更日, 職責ランク名称, 職責ランク順位, 入社経路, グループ入社日, 入社日付,
			退職区分, 退職日付, 退職事由;
	}

	public class Lite_Flag
	{
		public Boolean Flag = true;
	}

	public class Dic_Hantosi
	{
		public Lite_動向調査 l_前 = null;
		public Lite_動向調査 l_後 = null;

		public string str_Basyo = null;


		public Dic_Hantosi()
		{
		}
	}

	public class Dic_SyainNo : Dictionary<string, Dic_Hantosi>
	{
	}

	public static class Util
	{
		/// <summary>
		/// 指定されたインスタンスの深いコピーを生成し返します
		/// </summary>
		/// <typeparam name="T">コピーするインスタンスの型</typeparam>
		/// <param name="original">コピー元のインスタンス</param>
		/// <returns>指定されたインスタンスの深いコピー</returns>
		public static T DeepCopy<T>(T original)
		{
			// シリアル化した内容を保持するメモリーストリームを生成
			MemoryStream stream = new MemoryStream();
			try
			{
				// バイナリ形式でシリアライズするためのフォーマッターを生成
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				// コピー元のインスタンスをシリアライズ
				formatter.Serialize(stream, original);
				// メモリーストリームの現在位置を先頭に設定
				stream.Position = 0L;
				// メモリーストリームの内容を逆シリアル化
				return (T)formatter.Deserialize(stream);
			}
			finally
			{
				stream.Close();
			}
		}
	}

	public class DanJyo
	{
		public string[] str_Sex = { "男性", "女性" };

		public int[] int_Nasi, int_Ari, int_Kei;

		public int int_GoKei;

		public DanJyo()
		{
			int_Nasi = new int[2];
			int_Ari  = new int[2];
			int_Kei  = new int[2];

			int_Nasi[0] = int_Ari[0] = int_Kei[0] = int_Nasi[1] = int_Ari[1] = int_Kei[1] = int_GoKei = 0;
		}

		public void Soukei()
		{
			int_Kei[0] = int_Nasi[0] + int_Ari[0];
			int_Kei[1] = int_Nasi[1] + int_Ari[1];

			int_GoKei = int_Kei[0] + int_Kei[1];
		}
	}
}
