using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Console : MonoBehaviour {

	public static Console i;

	public Console_Cmd _CMD;

	//AWAKE
	void Awake(){																
		i = this;
		GUI_Style_Init();

		_CMD = GetComponent<Console_Cmd>();
	}


	//START
	void Start(){
		HandleLog("Type \"help\" to get additional help",2);
	}


	//UPDATE
	public GameObject target_obj;
	public Vector3 target_pos;

	void Update () {
		if(i==null) i=this;


		//For optional console
		if(Input.GetMouseButtonDown(1)){
			optOpen = !optOpen;
			optPos = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
		}

		//target
		if(Input.GetMouseButtonDown(0)){
			/*CameraManager cam = CameraManager.i;
			if(cam.GetHitFromMouse(100f).collider!=null){
				if(target_obj!=cam.GetHitFromMouse(100f).collider.gameObject)
					target_obj = cam.GetHitFromMouse(100f).collider.gameObject;
				else 
					target_obj = null;
				target_pos = cam.GetHitFromMouse(100f).point;
			}*/
		}

		if(!show && Input.GetKeyDown(KeyCode.BackQuote)){ 
			ToggleShow();
		}
		t_showtoggle-=Time.deltaTime;
	}


	//TOGGLE SHOW
	private float t_showtoggle;
	public bool show;

	void ToggleShow(){
		if(t_showtoggle<=0){
			show = !show;
			t_showtoggle = 0.2f;

			//if(!show)
				//CameraManager.i.mode = CameraManager.Mode.FreeFly;
		}
	}


	//LOGS
	private string cmd = "";
	private List<Log> logs = new List<Log>();
	public int story_indx = 0;
	public List<string> story = new List<string>();

	struct Log{
		public string message;
		public Log(string msg){
			message = msg;
		}
	}

	//CLEAR LOGS
	public void Clear(){
		logs.Clear();
	}




	//EVENTS
	void Events(){											
		
		Event e = Event.current;
		if(e!=null){
			if(e.character == '\n' && cmd!= ""){
				CompileCommand();
			}
			else if(show && e.character == '`'){
				ToggleShow();
			}
			else if(e.type == EventType.KeyDown && e.keyCode  == KeyCode.DownArrow && story.Count>0){
				if(story_indx>0) story_indx--;
				cmd = story[story_indx];
			}
			else if(e.type == EventType.KeyDown && e.keyCode  == KeyCode.UpArrow && story.Count>0){
				if(story_indx!=story.Count-1){
					if(story_indx<story.Count-1) story_indx++;
					cmd = story[story_indx];
				}
			}

		}
	}



	//+++++++++++++++++++++++++++++++++++++ GUI +++++++++++++++++++++++++++++++++++++++++
	GUIStyle textStyle = new GUIStyle();
	Vector2 scroll_log = new Vector2(0,0);

	Rect box_r = new Rect(0,(Screen.height/5)*3,Screen.width,(Screen.height/5)*2);
	Rect log_r = new Rect(10,(Screen.height-(Screen.height/5)*2)+10,Screen.width-20,((Screen.height/5)*2)-50);
	//OnGUI
	void OnGUI(){																
		if(show){
			Events();

			box_r = new Rect(0,(Screen.height/5)*3,Screen.width,(Screen.height/5)*2);
			log_r = new Rect(10,(Screen.height-(Screen.height/5)*2)+10,Screen.width-20,((Screen.height/5)*2)-50);

			GUI.Box(box_r,"");

			GUI_Info();
			GUI_Field();
			GUI_Log();

			OptionalCommandButtons();

		}
	}



	//INFO
	void GUI_Info(){																//ГУИ ИНФО
		float xp = box_r.width-(box_r.width/8);
		float yp = box_r.y+25;
		if(target_obj!=null){
			GUI.Label(new Rect(xp, yp, (box_r.width/8), 200), target_obj.name);
		}

		float x = (int)(target_pos.x*1000)/1000f;
		float y = (int)(target_pos.y*1000)/1000f;
		float z = (int)(target_pos.z*1000)/1000f;
		GUI.Label(new Rect(xp, yp+20, (box_r.width/8), 200), "("+x+" , "+y+" , "+z+")");
	}

	//LOG
	void GUI_Log(){																	//ГУИ ПОЛЕ ЛОГА
		Rect log_area = new Rect(10,0,Screen.width-40,15*logs.Count);
		//GUI.Box(log_r,"");
		scroll_log = GUI.BeginScrollView(log_r,scroll_log,log_area);

		for(int i=0;i<logs.Count;i++){
			Rect log_field = new Rect(10,15*i,log_area.width-60,15);
			GUI.Label(log_field,logs[i].message,textStyle);
		}
		

		GUI.EndScrollView();
	}

	//FIELD
	void GUI_Field(){																	//ГУИ ПОЛЕ ВВОДА
		GUI.SetNextControlName("Command_Field");
		Rect field_r = new Rect(10,Screen.height-30,Screen.width-30,25);
		cmd = GUI.TextField(field_r,cmd,textStyle);
		GUI.FocusControl("Command_Field");
	}

	//STYLE INIT
	void GUI_Style_Init(){																//ИНИЦИАЛИЗАЦИЯ СТИЛЯ ГУИ
		textStyle = new GUIStyle();
		textStyle.normal.textColor = Color.white;
		textStyle.wordWrap = true;
		textStyle.richText = true;
	}

	//OPTIONAL BUTTONS
	bool optOpen;
	Vector2 optPos;

	void OptionalCommandButtons(){

		/*if(target_obj!=null && target_obj.GetComponent<Entity>()){
			Event e = Event.current;

			if(optOpen){

				if(GUI.Button(new Rect(optPos.x-10,optPos.y-10,200,20), "select")){
					string[] args = new string[]{"select"};
					_CMD.SendMessage(args[0],args,SendMessageOptions.DontRequireReceiver);
					optOpen = false;
				}
			}
		}*/
	}


	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	//HANDLE LOG
	public static void HandleLog(string txt,int m){	//default - 0 ,success - 2 ,warning - 2, error - 3
		txt = ((m==3)? "<color=red>" : (m==2)? "<color=yellow>" : (m==1)? "<color=green>" : "<color=white>")+txt;
		txt = txt+"</color>";
		Log l = new Log(txt);
		i.logs.Add(l);
	}
	//Compile Command
	void CompileCommand(){			
		HandleLog(" > " + cmd,0);
		string[] args;
		args = cmd.Split(" "[0]);
		SendCommand(args);
		scroll_log = new Vector2(0,15*logs.Count);
		story.Add(cmd);
		story_indx = story.Count-1;
		cmd = "";
	}
	//Send Command
	void SendCommand(string[] args){
		_CMD.SendMessage(args[0],args,SendMessageOptions.DontRequireReceiver);
		//_CMD.CallCommand(args[0],args);
		//_CMD.GetAllCommands();
	}
}
