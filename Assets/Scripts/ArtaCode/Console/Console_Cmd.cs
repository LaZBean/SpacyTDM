using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class Console_Cmd : MonoBehaviour{


	public void GetAllCommands(){
		System.Reflection.MethodInfo[] prop = GetType().GetMethods();
		for(int i=0; i<prop.Length; i++){
			Console.HandleLog(prop[i].Name,0);
		}
	}

	void help(string[] args){
		//GetAllCommands();
		Console.HandleLog("Avaliable Commands:",0);
		Console.HandleLog("-- spawn [name] - creation of an object instance of the Resources folder",0);
		Console.HandleLog("-- timescale [value] - change time scale",0);
		Console.HandleLog("-- timeset [value] - change time",0);
		Console.HandleLog("-- select/deselect - control the selected actor",0);
		Console.HandleLog("-- systemInfo - show your system info",0);

	}

	void clear(string[] args){
		Console.i.Clear();
	}

	/*void adress(string[] args){
		NetworkManager.singleton.networkAddress = args[1];
	}


	
	void spawn(string[] args){
		float d = 3;
		if(args.Length>2) float.TryParse(args[2],out d);
		Transform cam = CameraManager.i.camera.transform;
		GameObject obj =(GameObject) Instantiate(Resources.Load(args[1]),cam.position+cam.forward*d,Quaternion.identity);
		Console.HandleLog(args[1]+" spawned",1);
	}
	
	void timescale(string[] args){
		Time.timeScale = float.Parse(args[1]);
		Console.HandleLog("Time scale = " + args[1],1);
	}
	
	void timeset(string[] args){
		int t;
		int.TryParse(args[1],out t);
		WorldTime.i.Hour = t;

		if(t>=24){ t-=Mathf.FloorToInt(t/24)*24; }
		string s = (t>=6 && t<12)? "Morning" : (t>=12 && t<18)? "Day" : (t>=18 && t<24)? "Evening" : "Night";
		Console.HandleLog("Time set on " + t + ":00 hours" + "\t-\t" + s,1);
	}
	void timespeed(string[] args){
		int t;
		int.TryParse(args[1],out t);
		WorldTime.i.timeSpeed = t;
		Console.HandleLog("Time speed = " + t + "\tDefault value = 60",1);
	}

	void select(string[] args){
		if(Player.my==null){
			return;
		}

		Entity e = (Console.i.target_obj!=null)? Console.i.target_obj.GetComponent<Entity>() : null;

		if(e!=null){
			Player.my.SelectEntity(e);
			Console.HandleLog("Take Control",1);
		}
		else{
			Console.HandleLog("Actor is null!",3);
		}
	}

	void deselect(string[] args){
		if(Player.my!=null){
			Player.my.DropControl();
			Player.my.avatar = null;
			Console.HandleLog("Deselect Actor",1);
		}
		else{
			Console.HandleLog("Actor is null!",3);
		}
	}





	void superhot(string[] args){
		Game.i.options.SuperHot = !Game.i.options.SuperHot;
	}
	
	/*void eval(string[] args){
			SendMessageUpwards("Eval",args[1],SendMessageOptions.RequireReceiver);
	}*/




	/*void UIColor(string[] args){
		if(args.Length<4){
			Console.HandleLog("Not all args!",3);
		}
		int t;
		int.TryParse(args[1],out t);
		Game.i.settings.uiR = t * 1/255.0f;
		int.TryParse(args[2],out t);
		Game.i.settings.uiG = t * 1/255.0f;
		int.TryParse(args[3],out t);
		Game.i.settings.uiB = t * 1/255.0f;
		Game.i.settings.Apply();
	}

	void nickname(string[] args){
		
		if(args.Length>1){
			string n = "";
			for(int i=1; i<args.Length; i++){
				n += args[i] + " ";
			}
			Game.i.settings.nickname = n;
			Game.i.settings.Apply();
		}

	}*/



	void systemInfo(string[] args){
		Console.HandleLog("=============================SYSTEM=============================",2);
		Console.HandleLog("-- Core --",2);
		Console.HandleLog(" deviceModel:        " + SystemInfo.deviceModel,2);
		Console.HandleLog(" operatingSystem:    " + SystemInfo.operatingSystem,2);
		Console.HandleLog(" deviceName:         " + SystemInfo.deviceName,2);
		Console.HandleLog(" deviceType:         " + SystemInfo.deviceType,2);
		Console.HandleLog(" deviceUniqueIdentifier:    " + SystemInfo.deviceUniqueIdentifier,2);
		Console.HandleLog("",0);

		Console.HandleLog("-- Graphics --",2);
		Console.HandleLog(" ID:             " + SystemInfo.graphicsDeviceID,2);
		Console.HandleLog(" Name:           " + SystemInfo.graphicsDeviceName,2);
		Console.HandleLog(" Type:           " + SystemInfo.graphicsDeviceType,2);
		Console.HandleLog(" Vendor:         " + SystemInfo.graphicsDeviceVendor,2);
		Console.HandleLog(" VendorID:       " + SystemInfo.graphicsDeviceVendorID,2);
		Console.HandleLog(" Version:        " + SystemInfo.graphicsDeviceVersion,2);
		Console.HandleLog(" MemorySize:     " + SystemInfo.graphicsMemorySize,2);
		Console.HandleLog(" MultiThreaded:  " + SystemInfo.graphicsMultiThreaded,2);
		Console.HandleLog(" ShaderLevel:    " + SystemInfo.graphicsShaderLevel,2);
		Console.HandleLog("",0);

		Console.HandleLog("-- CPU --",2);
		Console.HandleLog(" processorType:    " + SystemInfo.processorType,2);
		Console.HandleLog(" processorCount:    " + SystemInfo.processorCount,2);
		Console.HandleLog(" processorFrequency:    " + SystemInfo.processorFrequency,2);
		Console.HandleLog(" systemMemorySize:    " + SystemInfo.systemMemorySize,2);

		Console.HandleLog("",0);

	}
}
