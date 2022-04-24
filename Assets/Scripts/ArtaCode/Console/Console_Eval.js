#pragma strict
public class Console_Eval extends MonoBehaviour{

	public function Eval (cmd : String):void {
		eval(cmd);
		
	}
}
