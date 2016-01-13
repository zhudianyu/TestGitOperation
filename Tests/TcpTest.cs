using UnityEngine;
using System.Collections;
using System.Net;
using Cmd;
using System.Text;
using GX.Net;

public class TcpTest : MonoBehaviour
{
#if false
	public static uint Version { get { return 20201008; } }
	/// <summary>
	/// 客户端对外的IP
	/// </summary>
	public static string ClientIP { get; private set; }
	/// <summary>
	/// 游戏的运营区域
	/// </summary>
	public static int GameRegion { get; private set; }
	/// <summary>
	/// 网络消息的编码
	/// </summary>
	public static Encoding CmdEncoding { get; private set; }

	public static ushort GameID {get; private set;}
	public static ushort ZoneID { get; private set; }

	public static string LoginServerIP { get; private set; }
	public static int LoginServerPort { get; private set; }

	public static string GatewayServerIP { get; private set; }
	public static int GatewayServerPort { get; private set; }

	public static uint UserID { get; private set; }
	public static uint LoginTempID { get; private set; }

	public static uint Accid { get; set; }
	public static uint Channel { get; set; }
	public static string Account { get; set; }

	public TcpTest()
	{
		CmdEncoding = ConstDefine.Encoding;
		GameID = 22;
		ZoneID = 12;
		LoginServerIP = "192.168.86.58";
		LoginServerPort = 7000;
	}

	void Start()
	{
		Net.Instance.Start(LoginServerIP, LoginServerPort);
		Debug.Log(Net.Instance.NetService.CommandSerializer);
		Debug.Log("Start OK");

		Net.Instance.Send(new stUserVerifyVerCmd() { version = Version});
		Net.Instance.Send(new stRequestClientIP() { });
	}

	[Execute]
	public static void Execute(stReturnClientIP cmd)
	{
		ClientIP = MyConvert.ToString(cmd.pstrIP);

		var send = new stUserRequestLoginCmd()
		{
			pstrName = MyConvert.ToBytesOfAccountString("100"),
			game = GameID,
			zone = ZoneID,
			wdNetType = 1,
			userType = 1, // ChannelType_ZQB
		};
		send.pstrPassword = MyConvert.ToBytes("111111", send.pstrPassword.Length, ConstDefine.Encoding);
		Net.Instance.Send(send);
	}

	[Execute]
	static void Execute(stSetServerLangLogonUserCmd cmd)
	{
		GameRegion = cmd.gameRegion;
		var lang = MyConvert.ToString(cmd.lang).ToLowerInvariant();
		CmdEncoding = (lang.Contains("utf8") || lang.Contains("utf-8")) ? new UTF8Encoding(false) : Encoding.GetEncoding("GBK");
	}

	[Execute]
	static void Execute(stServerReturnLoginSuccessCmd cmd)
	{
		Debug.Log("帐号验证成功，准备连接到网关");
		ZoneID = (ushort)cmd.zoneid;
		GatewayServerIP = MyConvert.ToString(cmd.pstrIP);
		GatewayServerPort = cmd.wdPort;
		UserID = cmd.dwUserID;
		LoginTempID = cmd.loginTempID;

		Net.Instance.Close();
		Net.Instance.Start(GatewayServerIP, GatewayServerPort);
		Net.Instance.Send(new stUserVerifyVerCmd()
		{
			default_charid = cmd.charid,
			version = Version,
		});
	}

	/// <summary>
	/// 设置加密方式
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(stSetEncdecLogonUserCmd cmd)
	{
		Debug.Log("网关要求的加密方式: " + cmd.enctype);
		Net.Instance.Send(new stPasswdLogonUserCmd()
		{
			dwUserID = UserID,
			loginTempID = LoginTempID,
			pstrName = MyConvert.ToBytesOfNameString("wanghaijun"),
			pstrPassword = MyConvert.ToBytesOfAccountString("bwgame.org"),
		});
	}

	[Execute]
	public static void Execute(stReqPassword2SelectUserCmd cmd)
	{
	}

	[Execute]
	public static void Execute(stSetChannelAndAccidSelectUserCmd cmd)
	{
		Accid = cmd.accid;
		Channel = cmd.channel;
		Account = MyConvert.ToString(cmd.account);
	}

	[Execute]
	public static void Execute(stSetServerNameLogonUserCmd cmd)
	{
	}

	[Execute]
	public static void Execute(stIsServerDebugLogonUserCmd cmd)
	{
	}

	[Execute]
	public static void Execute(stRequestCharListLogonUserCmd cmd)
	{
	}

	[Execute]
	public static void Execute(t_ZipCmdPackNullCmd cmd)
	{
		foreach (var c in cmd.Parse(Net.Instance.NetService.CommandSerializer))
		{
			Net.Instance.SendToMe(c);
		}
	}
#endif
}
