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


		public Work_Data(string a_path, string b_path)
		{
			str_Status = "Normal";

			if (Directory.Exists(a_path) == false)
			{
				str_Status = "Abend";
				return;
			}

			var di_a = new DirectoryInfo(a_path);

			if (Directory.Exists(b_path) == false)
			{
				str_Status = "Abend";
				return;
			}

			var di_b = new DirectoryInfo(b_path);


			dic_AB = new Dictionary<string, AB_Data>();


			str_Name = di_a.Name;

			ab_Data = new AB_Data(dic_AB, di_a);
		}
	}

	public class AB_Data
	{
		public string type = "";
		public string Name = "", a_FullName = "", b_FullName = "";
		public long a_Length = 0, b_Length = 0;

		public List<AB_Data> ab_Datas = null;

 
		public AB_Data(Dictionary<string, AB_Data> dic_AB, DirectoryInfo ab)
		{
			type = "Dir";

			Name = ab.Name;

			a_FullName = ab.FullName;
			a_Length = 0;

			ab_Datas = new List<AB_Data>();

			foreach (var fi_c in ab.GetFiles())
			{
				var ab_data = new AB_Data(dic_AB, fi_c);

				ab_Datas.Add(ab_data);
			}

			foreach (var di_c in ab.GetDirectories())
			{
				var ab_data = new AB_Data(dic_AB, di_c);

				ab_Datas.Add(ab_data);
			}

			dic_AB.Add(a_FullName, this);
		}

		public AB_Data(Dictionary<string, AB_Data> dic_AB, FileInfo ab)
		{
			type = "File";

			Name = ab.Name;

			a_FullName = ab.FullName;
			a_Length = ab.Length;

			//Console.WriteLine("Length = " + a_Length);

			dic_AB.Add(a_FullName, this);
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
