using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.Networking;

public class Record : MonoBehaviour
{
      
	public GameObject card;
    public Button record;
	private bool recording = false;
	public AudioSource audio;
	public Sprite rec_doing;
	public Sprite recorded;
	private int duration = 3;
	private bool transition = false;
	private bool transited = false;
	private Vector3 card_pos0;
	private Vector3 card_dim0;
	private float width;
	private float height;
	private float speed = 0.5f;
	private float acceleration = 0f;
	public Camera face_cam;
	private float sensitivity = 0.2f;
	//public GameObject canvas;
    void Start()
    {

		//if(GameLogic.ParseMode(GameLogic.user.mode) == GameLogic.Mode.AVT && GameLogic.current_level > 60 && GameLogic.current_level <= 180){
			//canvas.SetActive(false);
		//}
		
			//tmpgo = Instantiate(Resources.Load<GameObject>("Prefabs/Level"), new Vector2(i,i), Quaternion.identity);
		if(GameLogic.mode == GameLogic.Mode.AV){
			sensitivity = 0.5f;
		}
		if(card != null){
			card_pos0 = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z);
		}else if(face_cam != null){
			card_pos0 = new Vector3(face_cam.rect.x, face_cam.rect.y,0);
			card_dim0 = new Vector3(face_cam.rect.width, face_cam.rect.height,0);
		}
		record.onClick.AddListener(Recd);
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
		
    }

    // Update is called once per frame
    void Update()
    {
		 if(GameLogic.stimulated){
            record.interactable = true;
        }else{
            record.interactable = false;
        }

        /*if(audio.time >= duration){ //here 10 is the *10
			audio.Stop();
		}*/

		if(transition && card != null){
			speed += acceleration;
			if( Math.Abs(card.transform.position.x - card_pos0.x) < sensitivity){
				if(!transited){
					card.transform.position -= new Vector3(speed, 0, 0);
				}else{
					transition = false;
					transited = false;
				}
			}
			else if(card.transform.position.x < card_pos0.x){
				if(card.transform.position.x < -width/1.5f){
					card.transform.position = new Vector3(width/1.5f, card_pos0.y, card_pos0.z);
				}else{
					card.transform.position -= new Vector3(speed, 0, 0);
				}
				
			}
			else if(card.transform.position.x > card_pos0.x){
				transited = true;
				card.transform.position -= new Vector3(speed, 0, 0);
				
			}
		}
		
		if(transition && face_cam != null && GameLogic.mode == GameLogic.Mode.AVT){

			speed += acceleration;
			if( Math.Abs(face_cam.rect.x - card_pos0.x) < 0.01f){
				Debug.Log("Case a");
				if(!transited){
					Debug.Log("Case a");
					face_cam.rect = new Rect(face_cam.rect.x - speed/40f, face_cam.rect.y,face_cam.rect.width, face_cam.rect.height);
					//card.transform.position -= new Vector3(speed, 0, 0);
				}else{
					transition = false;
					transited = false;
				}
			}
			else if(face_cam.rect.x < card_pos0.x){
				if(face_cam.rect.x < -0.5){
					face_cam.rect = new Rect(1f, face_cam.rect.y,face_cam.rect.width, face_cam.rect.height);
					//card.transform.position = new Vector3(width/1.5f, card_pos0.y, card_pos0.z);
				}else{
					Debug.Log("Case b");
					face_cam.rect = new Rect(face_cam.rect.x - speed/40f, face_cam.rect.y,face_cam.rect.width, face_cam.rect.height);
				}
				
			}
			else if(face_cam.rect.x > card_pos0.x){
				transited = true;
				Debug.Log("Case c");
				face_cam.rect = new Rect(face_cam.rect.x - speed/40f, face_cam.rect.y,face_cam.rect.width, face_cam.rect.height);				
			}
		}
	}

    public void Recd()
    {
		if(!GameLogic.dressing){
			Debug.Log(Microphone.devices[0]);
		
			if(recording){
				GameLogic.stimuli_play_count = 0;
        		GameLogic.stimulated = false;
				transition = true;
				Microphone.End("Default Input Device");
				record.image.sprite = recorded;
				//Debug.Log(audio.clip.length);
				//store recorded audio
				SavWav.Save("audiofile", audio.clip);
				Upload();
				//audio.Play();
				recording = false;
				RedirectFeedback();
				
			}else{
				audio.clip = Microphone.Start(Microphone.devices[0], true, duration, 16000); //*10
				audio.loop = false;
				recording = true;
				record.image.sprite = rec_doing;
			}
		}
    }

	public void Upload()
    {
        string path = Path.Combine(Application.persistentDataPath, "audiofile.wav");
        FileInfo fi = new FileInfo(path);
        string fileName = fi.Name;
        byte[] fileContents = File.ReadAllBytes(fi.FullName);
		string s1 = GameLogic.current_stimulis.Dequeue();
        GameLogic.user.stimuli_history += s1;
		int l = GameLogic.user.stimuli_history.Length;
		string cr_stimuli = "" + GameLogic.user.stimuli_history[l - 4] +
							GameLogic.user.stimuli_history[l - 3] +
							GameLogic.user.stimuli_history[l - 2] +
						    GameLogic.user.stimuli_history[l - 1];
        Uri webService = new Uri(@"http://explora-app-api.herokuapp.com/uploadfile?user_name=" + GameLogic.user.user_name 
																				+ "&current_stimuli=" + cr_stimuli);
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
        //Debug.Log(requestMessage.Content);
		requestMessage.Headers.ExpectContinue = false;

        MultipartFormDataContent multiPartContent = new MultipartFormDataContent("----MyGreatBoundary");
        ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
        byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
        multiPartContent.Add(byteArrayContent, "file", fileName);
        requestMessage.Content = multiPartContent;

        HttpClient httpClient = new HttpClient();
        Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
        HttpResponseMessage httpResponse = httpRequest.Result;
		string content = httpResponse.Content.ReadAsStringAsync().Result;
    	Debug.Log("Response to pronunciation: " + content);
		GameLogic.user.answers += content;
		GameLogic.user.score += int.Parse("" + content[2]);
		//Debug.Log("" + httpResponse.Content.Headers);
    }

	
  private void RedirectFeedback()
    {
		GameLogic.current_stimulis = new Queue<string>();
        GameLogic.answering = true;
		GameLogic.update_stimuli = true;
        GameLogic.stimuli_change_count = 0;
        if(GameLogic.feedback_count == 5){
           GameLogic.dressing = true;
		    /*if(GameLogic.mode != GameLogic.Mode.AVT){
            	 SceneManager.LoadScene("Feedback", LoadSceneMode.Additive);//,  LoadSceneMode.Additive);
        	}else{
				if(GameLogic.current_level > 61 && GameLogic.current_level <= 180){
					face_cam.SetActive(false);
					canvas.SetActive(true);
				}else{
					SceneManager.LoadScene("Feedback", LoadSceneMode.Additive);//,  LoadSceneMode.Additive);
				}
				
			}*/
            //SceneManager.LoadScene("Feedback", LoadSceneMode.Additive);//,  LoadSceneMode.Additive);
            
			GameLogic.feedback_count = 0;
			SceneManager.LoadScene("Feedback");//,  LoadSceneMode.Additive);

        }else{
            GameLogic.feedback_count++;
			GameLogic.user.score++;
			GameLogic.user.lvl++;
        }
       
        
    }
}